namespace ServerAllInOne.Controls
{
    partial class ServerItem
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            btnStartOrStop = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // btnStartOrStop
            // 
            btnStartOrStop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStartOrStop.Location = new Point(207, 27);
            btnStartOrStop.Name = "btnStartOrStop";
            btnStartOrStop.Size = new Size(50, 40);
            btnStartOrStop.TabIndex = 0;
            btnStartOrStop.Text = "启动";
            btnStartOrStop.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(12, 10);
            label1.Name = "label1";
            label1.Size = new Size(135, 26);
            label1.TabIndex = 1;
            label1.Text = "Server Name";
            // 
            // ServerItem
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label1);
            Controls.Add(btnStartOrStop);
            Name = "ServerItem";
            Size = new Size(278, 93);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStartOrStop;
        private Label label1;
    }
}
