using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Biaoqingbao
{
    public partial class Form1 : Form
    {
        private string _baseUrl
        {
            get
            {
                return string.Format(
                    "http://pic.sogou.com/pics/json.jsp?query={0}&st=5&start=0&xml_len={1}&callback=dataCallback&reqFrom=wap_result"
                    , CbOnlyEmotion.Checked ? "{0}%20表情" : "{0}"
                    , TbResultCount.Text);
            }
        }
        private HttpClient _client;
        private Font _labelFont;
        private string _tempFolder;
        private bool _copyGif { get { return CbCopyGif.Checked; } }
        private string _lastClickedPicbox = "";

        public Form1()
        {
            InitializeComponent();
            BindEvents();

            ShowInTaskbar = false;
            _client = new HttpClient();
            _client.Timeout = new TimeSpan(0, 0, 3);
            FlowLayout.AutoScroll = true;
            //FlowLayoutHints.AutoScroll = true;

            ni.Visible = true;
            KeyPreview = true;

            _labelFont = new Font(DefaultFont.Name, DefaultFont.Size, FontStyle.Underline);
            //TempFolder
            var tempPath = Path.GetTempPath();
            _tempFolder = Path.Combine(tempPath, "biaoqingbao");
            if (!Directory.Exists(_tempFolder))
            {
                Directory.CreateDirectory(_tempFolder);
            }
        }

        private void BindEvents()
        {
            BtnSearch.Click += async (sender, args) => await SearchAsync();
            ni.Click += (s, e) =>
            {
                var me = e as MouseEventArgs;
                if (me.Button == MouseButtons.Left)
                {
                    Show();
                    WindowState = FormWindowState.Normal;
                    Activate();
                }
            };
            SizeChanged += (s, e) =>
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    ni.Visible = true;
                    Hide();
                }
            };
            Load += (s, e) =>
            {
                var size = Screen.PrimaryScreen.WorkingArea.Size;
                Location = new Point(size.Width - Width, size.Height - Height);
                TbKeyword.Focus();
            };
            KeyDown += async (s, e) =>
            {
                if (e.KeyCode == Keys.Enter && TbKeyword.Focused)
                {
                    await SearchAsync();
                }
            };
            FormClosed += (s, e) =>
            {
                if (Directory.Exists(_tempFolder))
                {
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();

                    //向cmd窗口发送输入信息
                    p.StandardInput.WriteLine(string.Format("rmdir /s/q {0} &exit", _tempFolder));
                }
            };
            TbResultCount.KeyUp += (s, e) =>
            {
                var c = 0;
                if (int.TryParse(TbResultCount.Text, out c))
                {
                    if (c > 100)
                        c = 100;
                    if (c < 1)
                        c = 1;
                    TbResultCount.Text = c.ToString();
                }
                else
                {
                    TbResultCount.Text = "";
                }
            };
            CtxItemQuit.Click += (s, e) =>
            {
                Close();
            };
            CtxItemCopy.Click += (s, e) =>
            {
                if (imgList.ContainsKey(_lastClickedPicbox))
                {
                    var sender = FlowLayout.Controls.Find(_lastClickedPicbox, true).FirstOrDefault() as PictureBox;
                    if (sender != null)
                    {
                        PicBoxDbClick(sender, null);
                        return;
                    }
                }
                MessageBox.Show("there was something wrong...");
            };
            CtxItemSave.Click += (s, e) =>
            {
                if (imgList.ContainsKey(_lastClickedPicbox))
                {
                    var img = imgList[_lastClickedPicbox];
                    if (!File.Exists(img))
                    {
                        MessageBox.Show("there was something wrong...");
                        return;
                    }
                    var fileInfo = new FileInfo(img);
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = string.Format("Image files(*{0})|*{0}|All files(*.*)|*.*", fileInfo.Extension);
                    sfd.FileName = DateTime.Now.ToString("yyMMddHHmmssfff");
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        fileInfo.CopyTo(sfd.FileName);
                    }
                }
                else
                {
                    MessageBox.Show("there was something wrong...");
                }
            };
        }

        private async Task SearchAsync()
        {
            var keyword = TbKeyword.Text.Trim();
            if (keyword.Length > 0)
            {
                foreach (Control c in FlowLayout.Controls)
                {
                    if (c is PictureBox)//清除背景图 释放内存
                    {
                        ((PictureBox)c).BackgroundImage.Dispose();
                    }
                }
                FlowLayout.Controls.Clear();//图片框
                FlowLayoutHints.Controls.Clear();//相关词
                imgList.Clear();
                BtnSearch.Enabled = false;
                CancellationTokenSource source = new CancellationTokenSource(30000);
                var task = Task.Run(() =>
                {
                    Search(keyword);
                }, source.Token);
                try
                {
                    await task;
                }
                catch (Exception ex) { }
                finally
                {
                    source.Dispose();
                    BtnSearch.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("请输入关键词");
            }
        }

        private void PicBoxDbClick(object sender, EventArgs e)
        {
            var picbox = sender as PictureBox;
            var sc = new StringCollection();
            var filename = imgList[picbox.Name];
            if (_copyGif && !filename.EndsWith(".gif"))
            {
                int width = 128;
                int height = width;
                int stride = width / 8;
                byte[] pixels = new byte[height * stride];
                // Define the image palette
                BitmapPalette myPalette = BitmapPalettes.WebPalette;

                // Creates a new empty image with the pre-defined palette
                BitmapSource image = BitmapSource.Create(
                    width,
                    height,
                    96,
                    96,
                    PixelFormats.Indexed1,
                    myPalette,
                    pixels,
                    stride);
                using (FileStream stream = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    using (FileStream stream2 = new FileStream(filename + ".gif", FileMode.OpenOrCreate))
                    {
                        GifBitmapEncoder encoder = new GifBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(stream));
                        encoder.Save(stream2);
                        sc = new StringCollection();
                        sc.Add(filename + ".gif");
                        Clipboard.SetFileDropList(sc);
                    }
                }
            }
            else
            {
                sc.Add(imgList[picbox.Name]);
                Clipboard.SetFileDropList(sc);
            }
        }

        private async Task LableClick(object sender, EventArgs e)
        {
            var label = sender as Label;
            TbKeyword.Text = label.Text;
            await SearchAsync();
        }

        private Dictionary<string, string> imgList = new Dictionary<string, string>();

        private void Search(string keyword)
        {
            var url = string.Format(_baseUrl, keyword);
            var jsonp = _client.GetStringAsync(url).Result;
            if (!string.IsNullOrEmpty(jsonp))
            {
                var match = Regex.Match(jsonp, @"dataCallback\((.*)\);");
                if (match.Success)
                {
                    jsonp = match.Groups[1].Value;
                }
                else
                {
                    return;
                }
                JavaScriptSerializer jss = new JavaScriptSerializer();
                var data = jss.Deserialize<TempData>(jsonp);
                foreach (var word in data.hintWords)
                {
                    Label l = new Label();
                    l.Text = word.Replace("表情包", "");
                    l.AutoSize = true;
                    l.Font = _labelFont;
                    l.ForeColor = System.Drawing.Color.Blue;
                    l.Margin = new Padding(1);
                    l.Cursor = Cursors.Hand;

                    l.Click += async (s, e) =>
                    {
                        await LableClick(s, e);
                    };
                    Invoke(new Action(() =>
                    {
                        FlowLayoutHints.Controls.Add(l);
                    }));
                }
                Parallel.For(0, data.items.Count, new ParallelOptions { MaxDegreeOfParallelism = 5 }, i =>
                {
                    try
                    {
                        var picUrl = data.items[i].picUrl;
                        var result = _client.GetAsync(picUrl).Result;
                        if (result.IsSuccessStatusCode)
                        {
                            using (var fs = result.Content.ReadAsStreamAsync().Result)
                            {
                                Invoke(new Action(() =>
                                {
                                    var pb = new PictureBox();
                                    pb.Name = Guid.NewGuid().ToString();
                                    pb.Cursor = Cursors.Hand;
                                    pb.BackgroundImageLayout = ImageLayout.Stretch;
                                    pb.ContextMenuStrip = CtxMenuPicbox;
                                    Bitmap img = Image.FromStream(fs) as Bitmap;
                                    var fileName = Path.Combine(_tempFolder, Guid.NewGuid().ToString() + GetImgExt(img.RawFormat));
                                    img.Save(fileName);
                                    imgList.Add(pb.Name, fileName);
                                    var tImg = img.GetThumbnailImage(100, 100, () => { return false; }, IntPtr.Zero);
                                    pb.BackgroundImage = tImg;
                                    if (img.RawFormat.Guid == ImageFormat.Gif.Guid)
                                    {
                                        using (var g = Graphics.FromImage(tImg))
                                        {
                                            g.DrawImage(Resource.gif, new Point(1, 1));
                                        }
                                    }
                                    img.Dispose();

                                    pb.Size = new Size(100, 100);
                                    pb.DoubleClick += (s, e) =>
                                    {
                                        try
                                        {
                                            PicBoxDbClick(s, e);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("复制失败, 请重试");
                                        }
                                    };
                                    pb.MouseUp += (s, e) =>
                                    {
                                        if (e.Button == MouseButtons.Right)
                                        {
                                            _lastClickedPicbox = pb.Name;
                                        }
                                    };
                                    FlowLayout.Controls.Add(pb);
                                }));
                            }
                        }
                    }
                    catch { }
                });
            }
        }

        private string GetImgExt(ImageFormat fmt)
        {
            if (fmt.Guid == ImageFormat.Bmp.Guid) { return ".bmp"; }
            if (fmt.Guid == ImageFormat.Emf.Guid) { return ".emp"; }
            if (fmt.Guid == ImageFormat.Exif.Guid) { return ".exif"; }
            if (fmt.Guid == ImageFormat.Gif.Guid) { return ".gif"; }
            if (fmt.Guid == ImageFormat.Icon.Guid) { return ".ico"; }
            if (fmt.Guid == ImageFormat.Jpeg.Guid) { return ".jpg"; }
            if (fmt.Guid == ImageFormat.MemoryBmp.Guid) { return ".bmp"; }
            if (fmt.Guid == ImageFormat.Png.Guid) { return ".png"; }
            if (fmt.Guid == ImageFormat.Tiff.Guid) { return ".tiff"; }
            if (fmt.Guid == ImageFormat.Wmf.Guid) { return ".wmf"; }
            return ".jpg";
        }
    }

    class TempData
    {
        public IList<TempItem> items { get; set; }
        public IList<string> hintWords { get; set; }
    }

    class TempItem
    {
        public string picUrl { get; set; }
    }
}
