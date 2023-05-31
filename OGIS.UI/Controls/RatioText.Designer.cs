namespace Richway.Ocean.Common.UserControls
{
    partial class RatioText
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtNumerator = new System.Windows.Forms.TextBox();
            this.lblSymble = new System.Windows.Forms.Label();
            this.txtDenominator = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtNumerator
            // 
            this.txtNumerator.Location = new System.Drawing.Point(4, 4);
            this.txtNumerator.Name = "txtNumerator";
            this.txtNumerator.Size = new System.Drawing.Size(78, 25);
            this.txtNumerator.TabIndex = 0;
            this.txtNumerator.Text = "1";
            this.txtNumerator.TextChanged += new System.EventHandler(this.txtNumerator_TextChanged);
            // 
            // lblSymble
            // 
            this.lblSymble.AutoSize = true;
            this.lblSymble.Location = new System.Drawing.Point(89, 4);
            this.lblSymble.Name = "lblSymble";
            this.lblSymble.Size = new System.Drawing.Size(15, 15);
            this.lblSymble.TabIndex = 1;
            this.lblSymble.Text = ":";
            // 
            // txtDenominator
            // 
            this.txtDenominator.Location = new System.Drawing.Point(111, 4);
            this.txtDenominator.Name = "txtDenominator";
            this.txtDenominator.Size = new System.Drawing.Size(100, 25);
            this.txtDenominator.TabIndex = 2;
            this.txtDenominator.Text = "1";
            this.txtDenominator.TextChanged += new System.EventHandler(this.txtDenominator_TextChanged);
            // 
            // RatioText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtDenominator);
            this.Controls.Add(this.lblSymble);
            this.Controls.Add(this.txtNumerator);
            this.Name = "RatioText";
            this.Size = new System.Drawing.Size(221, 31);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNumerator;
        private System.Windows.Forms.Label lblSymble;
        private System.Windows.Forms.TextBox txtDenominator;
    }
}
