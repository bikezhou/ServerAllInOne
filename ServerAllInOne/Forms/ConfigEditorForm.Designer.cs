namespace ServerAllInOne.Forms
{
    partial class ConfigEditorForm
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
            listConfig = new ListBox();
            richEditor = new RichTextBox();
            splitter1 = new Splitter();
            panel1 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // listConfig
            // 
            listConfig.BorderStyle = BorderStyle.FixedSingle;
            listConfig.Dock = DockStyle.Left;
            listConfig.FormattingEnabled = true;
            listConfig.ItemHeight = 17;
            listConfig.Location = new Point(0, 0);
            listConfig.Name = "listConfig";
            listConfig.Size = new Size(169, 461);
            listConfig.TabIndex = 0;
            listConfig.SelectedIndexChanged += listConfig_SelectedIndexChanged;
            // 
            // richEditor
            // 
            richEditor.AcceptsTab = true;
            richEditor.BorderStyle = BorderStyle.None;
            richEditor.Dock = DockStyle.Fill;
            richEditor.Location = new Point(5, 5);
            richEditor.Name = "richEditor";
            richEditor.Size = new Size(660, 451);
            richEditor.TabIndex = 1;
            richEditor.Text = "";
            richEditor.WordWrap = false;
            richEditor.KeyPress += richEditor_KeyPress;
            // 
            // splitter1
            // 
            splitter1.Location = new Point(169, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(5, 461);
            splitter1.TabIndex = 2;
            splitter1.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(richEditor);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(174, 0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(5);
            panel1.Size = new Size(670, 461);
            panel1.TabIndex = 3;
            // 
            // ConfigEditorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(844, 461);
            Controls.Add(panel1);
            Controls.Add(splitter1);
            Controls.Add(listConfig);
            KeyPreview = true;
            Name = "ConfigEditorForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "配置编辑器";
            KeyDown += ConfigEditorForm_KeyDown;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ListBox listConfig;
        private RichTextBox richEditor;
        private Splitter splitter1;
        private Panel panel1;
    }
}