namespace OGIS.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tsbOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbNull = new System.Windows.Forms.ToolStripButton();
            this.tsbZoomIn = new System.Windows.Forms.ToolStripButton();
            this.tsbRoomOut = new System.Windows.Forms.ToolStripButton();
            this.tsbMapFull = new System.Windows.Forms.ToolStripButton();
            this.tsbMapPan = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbImportShp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tpgLeft = new System.Windows.Forms.TabControl();
            this.tpgSet = new System.Windows.Forms.TabPage();
            this.tpgToc = new System.Windows.Forms.TabPage();
            this.pnlTop.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tpgLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.toolStrip1);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1266, 52);
            this.pnlTop.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNew,
            this.tsbOpen,
            this.tsbSave,
            this.tsbSaveAs,
            this.toolStripSeparator1,
            this.tsbNull,
            this.tsbZoomIn,
            this.tsbRoomOut,
            this.tsbMapFull,
            this.tsbMapPan,
            this.toolStripSeparator2,
            this.tsbImportShp,
            this.toolStripSeparator3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1266, 39);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbNew
            // 
            this.tsbNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNew.Image = global::OGIS.UI.Properties.Resources.新建;
            this.tsbNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNew.Name = "tsbNew";
            this.tsbNew.Size = new System.Drawing.Size(36, 36);
            this.tsbNew.Text = "toolStripButton1";
            this.tsbNew.ToolTipText = "新建工程";
            this.tsbNew.Click += new System.EventHandler(this.tsbNew_Click);
            // 
            // tsbOpen
            // 
            this.tsbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpen.Image = global::OGIS.UI.Properties.Resources.打开;
            this.tsbOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpen.Name = "tsbOpen";
            this.tsbOpen.Size = new System.Drawing.Size(36, 36);
            this.tsbOpen.Text = "打开工程";
            this.tsbOpen.Click += new System.EventHandler(this.tsbOpen_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Image = global::OGIS.UI.Properties.Resources.保存;
            this.tsbSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(36, 36);
            this.tsbSave.Text = "保存工程";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbSaveAs
            // 
            this.tsbSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSaveAs.Image = global::OGIS.UI.Properties.Resources.另存;
            this.tsbSaveAs.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSaveAs.Name = "tsbSaveAs";
            this.tsbSaveAs.Size = new System.Drawing.Size(36, 36);
            this.tsbSaveAs.Text = "另存工程";
            this.tsbSaveAs.Click += new System.EventHandler(this.tsbSaveAs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // tsbNull
            // 
            this.tsbNull.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNull.Image = ((System.Drawing.Image)(resources.GetObject("tsbNull.Image")));
            this.tsbNull.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbNull.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNull.Name = "tsbNull";
            this.tsbNull.Size = new System.Drawing.Size(36, 36);
            this.tsbNull.Text = "无功能";
            this.tsbNull.Click += new System.EventHandler(this.tsbNull_Click);
            // 
            // tsbZoomIn
            // 
            this.tsbZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbZoomIn.Image = global::OGIS.UI.Properties.Resources.地图放大;
            this.tsbZoomIn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbZoomIn.Name = "tsbZoomIn";
            this.tsbZoomIn.Size = new System.Drawing.Size(36, 36);
            this.tsbZoomIn.Text = "地图放大";
            this.tsbZoomIn.Click += new System.EventHandler(this.tsbZoomIn_Click);
            // 
            // tsbRoomOut
            // 
            this.tsbRoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRoomOut.Image = global::OGIS.UI.Properties.Resources.地图缩小;
            this.tsbRoomOut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbRoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRoomOut.Name = "tsbRoomOut";
            this.tsbRoomOut.Size = new System.Drawing.Size(36, 36);
            this.tsbRoomOut.Text = "地图缩小";
            this.tsbRoomOut.Click += new System.EventHandler(this.tsbRoomOut_Click);
            // 
            // tsbMapFull
            // 
            this.tsbMapFull.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMapFull.Image = global::OGIS.UI.Properties.Resources.地图全图;
            this.tsbMapFull.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbMapFull.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMapFull.Name = "tsbMapFull";
            this.tsbMapFull.Size = new System.Drawing.Size(36, 36);
            this.tsbMapFull.Text = "全图";
            this.tsbMapFull.Click += new System.EventHandler(this.tsbMapFull_Click);
            // 
            // tsbMapPan
            // 
            this.tsbMapPan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMapPan.Image = global::OGIS.UI.Properties.Resources.地图平移;
            this.tsbMapPan.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbMapPan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMapPan.Name = "tsbMapPan";
            this.tsbMapPan.Size = new System.Drawing.Size(31, 36);
            this.tsbMapPan.Text = "地图平移";
            this.tsbMapPan.Click += new System.EventHandler(this.tsbMapPan_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // tsbImportShp
            // 
            this.tsbImportShp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbImportShp.Enabled = false;
            this.tsbImportShp.Image = global::OGIS.UI.Properties.Resources.选择shp;
            this.tsbImportShp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbImportShp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImportShp.Name = "tsbImportShp";
            this.tsbImportShp.Size = new System.Drawing.Size(36, 37);
            this.tsbImportShp.Text = "添加图层";
            this.tsbImportShp.Visible = false;
            this.tsbImportShp.Click += new System.EventHandler(this.tsbImportShp_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 39);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 697);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1266, 42);
            this.pnlBottom.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 52);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tpgLeft);
            this.splitContainer1.Size = new System.Drawing.Size(1266, 645);
            this.splitContainer1.SplitterDistance = 507;
            this.splitContainer1.TabIndex = 2;
            // 
            // tpgLeft
            // 
            this.tpgLeft.Controls.Add(this.tpgSet);
            this.tpgLeft.Controls.Add(this.tpgToc);
            this.tpgLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpgLeft.Location = new System.Drawing.Point(0, 0);
            this.tpgLeft.Name = "tpgLeft";
            this.tpgLeft.SelectedIndex = 0;
            this.tpgLeft.Size = new System.Drawing.Size(507, 645);
            this.tpgLeft.TabIndex = 0;
            // 
            // tpgSet
            // 
            this.tpgSet.Location = new System.Drawing.Point(4, 25);
            this.tpgSet.Name = "tpgSet";
            this.tpgSet.Padding = new System.Windows.Forms.Padding(3);
            this.tpgSet.Size = new System.Drawing.Size(499, 616);
            this.tpgSet.TabIndex = 0;
            this.tpgSet.Text = "Setting";
            this.tpgSet.UseVisualStyleBackColor = true;
            // 
            // tpgToc
            // 
            this.tpgToc.Location = new System.Drawing.Point(4, 25);
            this.tpgToc.Name = "tpgToc";
            this.tpgToc.Padding = new System.Windows.Forms.Padding(3);
            this.tpgToc.Size = new System.Drawing.Size(499, 616);
            this.tpgToc.TabIndex = 1;
            this.tpgToc.Text = "Maplayers";
            this.tpgToc.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1266, 739);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Name = "MainForm";
            this.Text = "MapWindow V1.0";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tpgLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripButton tsbOpen;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton tsbSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbZoomIn;
        private System.Windows.Forms.ToolStripButton tsbRoomOut;
        private System.Windows.Forms.ToolStripButton tsbMapFull;
        private System.Windows.Forms.ToolStripButton tsbMapPan;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbImportShp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbNull;
        private System.Windows.Forms.TabControl tpgLeft;
        private System.Windows.Forms.TabPage tpgSet;
        private System.Windows.Forms.TabPage tpgToc;
    }
}