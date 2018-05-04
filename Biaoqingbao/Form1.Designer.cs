namespace Biaoqingbao
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.BtnSearch = new System.Windows.Forms.Button();
            this.TbKeyword = new System.Windows.Forms.TextBox();
            this.FlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.ni = new System.Windows.Forms.NotifyIcon(this.components);
            this.FlowLayoutHints = new System.Windows.Forms.FlowLayoutPanel();
            this.CtxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CtxItemQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.CbCopyGif = new System.Windows.Forms.CheckBox();
            this.CtxMenuPicbox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CtxItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.CtxItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.CtxMenu.SuspendLayout();
            this.CtxMenuPicbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(301, 7);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 23);
            this.BtnSearch.TabIndex = 2;
            this.BtnSearch.Text = "搜 索";
            this.BtnSearch.UseVisualStyleBackColor = true;
            // 
            // TbKeyword
            // 
            this.TbKeyword.Location = new System.Drawing.Point(7, 8);
            this.TbKeyword.Name = "TbKeyword";
            this.TbKeyword.Size = new System.Drawing.Size(290, 21);
            this.TbKeyword.TabIndex = 1;
            // 
            // FlowLayout
            // 
            this.FlowLayout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.FlowLayout.Location = new System.Drawing.Point(0, 80);
            this.FlowLayout.Name = "FlowLayout";
            this.FlowLayout.Size = new System.Drawing.Size(468, 208);
            this.FlowLayout.TabIndex = 2;
            // 
            // ni
            // 
            this.ni.ContextMenuStrip = this.CtxMenu;
            this.ni.Icon = ((System.Drawing.Icon)(resources.GetObject("ni.Icon")));
            this.ni.Text = "notifyIcon1";
            this.ni.Visible = true;
            // 
            // FlowLayoutHints
            // 
            this.FlowLayoutHints.Location = new System.Drawing.Point(0, 36);
            this.FlowLayoutHints.Name = "FlowLayoutHints";
            this.FlowLayoutHints.Size = new System.Drawing.Size(468, 45);
            this.FlowLayoutHints.TabIndex = 3;
            // 
            // CtxMenu
            // 
            this.CtxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CtxItemQuit});
            this.CtxMenu.Name = "CtxMenu";
            this.CtxMenu.Size = new System.Drawing.Size(101, 26);
            // 
            // CtxItemQuit
            // 
            this.CtxItemQuit.Name = "CtxItemQuit";
            this.CtxItemQuit.Size = new System.Drawing.Size(100, 22);
            this.CtxItemQuit.Text = "退出";
            // 
            // CbCopyGif
            // 
            this.CbCopyGif.AutoSize = true;
            this.CbCopyGif.Location = new System.Drawing.Point(379, 15);
            this.CbCopyGif.Name = "CbCopyGif";
            this.CbCopyGif.Size = new System.Drawing.Size(90, 16);
            this.CbCopyGif.TabIndex = 5;
            this.CbCopyGif.Text = "复制Gif格式";
            this.CbCopyGif.UseVisualStyleBackColor = true;
            // 
            // CtxMenuPicbox
            // 
            this.CtxMenuPicbox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CtxItemCopy,
            this.CtxItemSave});
            this.CtxMenuPicbox.Name = "CtxMenuPicbox";
            this.CtxMenuPicbox.Size = new System.Drawing.Size(153, 70);
            // 
            // CtxItemSave
            // 
            this.CtxItemSave.Name = "CtxItemSave";
            this.CtxItemSave.Size = new System.Drawing.Size(152, 22);
            this.CtxItemSave.Text = "另存为";
            // 
            // CtxItemCopy
            // 
            this.CtxItemCopy.Name = "CtxItemCopy";
            this.CtxItemCopy.Size = new System.Drawing.Size(152, 22);
            this.CtxItemCopy.Text = "复制";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 288);
            this.Controls.Add(this.CbCopyGif);
            this.Controls.Add(this.FlowLayoutHints);
            this.Controls.Add(this.FlowLayout);
            this.Controls.Add(this.TbKeyword);
            this.Controls.Add(this.BtnSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "表情包搜索";
            this.CtxMenu.ResumeLayout(false);
            this.CtxMenuPicbox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.TextBox TbKeyword;
        private System.Windows.Forms.FlowLayoutPanel FlowLayout;
        private System.Windows.Forms.NotifyIcon ni;
        private System.Windows.Forms.FlowLayoutPanel FlowLayoutHints;
        private System.Windows.Forms.ContextMenuStrip CtxMenu;
        private System.Windows.Forms.ToolStripMenuItem CtxItemQuit;
        private System.Windows.Forms.CheckBox CbCopyGif;
        private System.Windows.Forms.ContextMenuStrip CtxMenuPicbox;
        private System.Windows.Forms.ToolStripMenuItem CtxItemSave;
        private System.Windows.Forms.ToolStripMenuItem CtxItemCopy;
    }
}

