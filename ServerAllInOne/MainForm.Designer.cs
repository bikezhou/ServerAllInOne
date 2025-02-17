using ServerAllInOne.Controls;

namespace ServerAllInOne
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tabHome = new TabPage();
            tableLayoutPanel = new TableLayoutPanel();
            btnStopServer = new Button();
            btnAddServer = new Button();
            btnStartServer = new Button();
            lstbServers = new ServerList();
            tabControl = new TabControl();
            cmsTabMenu = new ContextMenuStrip(components);
            tsmiStartServer = new ToolStripMenuItem();
            tsmiStopServer = new ToolStripMenuItem();
            tsmiRestartServer = new ToolStripMenuItem();
            tsmiEditServer = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            tsmiOpenFolder = new ToolStripMenuItem();
            tsmiEditConfig = new ToolStripMenuItem();
            notifyIcon = new NotifyIcon(components);
            cmsMain = new ContextMenuStrip(components);
            tsmiOpenMainForm = new ToolStripMenuItem();
            tsmiMainStartServer = new ToolStripMenuItem();
            tsmiMainStopServer = new ToolStripMenuItem();
            tsmiExit = new ToolStripMenuItem();
            panel1 = new Panel();
            lblVersion = new Label();
            panel2 = new Panel();
            tabHome.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            tabControl.SuspendLayout();
            cmsTabMenu.SuspendLayout();
            cmsMain.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // tabHome
            // 
            tabHome.Controls.Add(tableLayoutPanel);
            tabHome.Location = new Point(4, 26);
            tabHome.Name = "tabHome";
            tabHome.Padding = new Padding(3);
            tabHome.Size = new Size(966, 475);
            tabHome.TabIndex = 0;
            tabHome.Text = "主页";
            tabHome.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 5;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.Controls.Add(btnStopServer, 3, 1);
            tableLayoutPanel.Controls.Add(btnAddServer, 1, 1);
            tableLayoutPanel.Controls.Add(btnStartServer, 2, 1);
            tableLayoutPanel.Controls.Add(lstbServers, 1, 2);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(3, 3);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 3;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel.Size = new Size(960, 469);
            tableLayoutPanel.TabIndex = 1;
            // 
            // btnStopServer
            // 
            btnStopServer.Dock = DockStyle.Fill;
            btnStopServer.Font = new Font("Microsoft YaHei UI", 9F);
            btnStopServer.ForeColor = Color.Red;
            btnStopServer.Location = new Point(543, 207);
            btnStopServer.Name = "btnStopServer";
            btnStopServer.Size = new Size(114, 54);
            btnStopServer.TabIndex = 2;
            btnStopServer.Text = "停止服务";
            btnStopServer.UseVisualStyleBackColor = true;
            btnStopServer.Click += btnStopServer_Click;
            // 
            // btnAddServer
            // 
            btnAddServer.Dock = DockStyle.Fill;
            btnAddServer.Location = new Point(303, 207);
            btnAddServer.Name = "btnAddServer";
            btnAddServer.Size = new Size(114, 54);
            btnAddServer.TabIndex = 1;
            btnAddServer.Text = "添加服务";
            btnAddServer.UseVisualStyleBackColor = true;
            btnAddServer.Click += btnAddServer_Click;
            // 
            // btnStartServer
            // 
            btnStartServer.Dock = DockStyle.Fill;
            btnStartServer.Location = new Point(423, 207);
            btnStartServer.Name = "btnStartServer";
            btnStartServer.Size = new Size(114, 54);
            btnStartServer.TabIndex = 0;
            btnStartServer.Text = "启动服务";
            btnStartServer.UseVisualStyleBackColor = true;
            btnStartServer.Click += btnStartServer_Click;
            // 
            // lstbServers
            // 
            tableLayoutPanel.SetColumnSpan(lstbServers, 3);
            lstbServers.Dock = DockStyle.Fill;
            lstbServers.DrawMode = DrawMode.OwnerDrawFixed;
            lstbServers.FormattingEnabled = true;
            lstbServers.ItemHeight = 24;
            lstbServers.Location = new Point(303, 267);
            lstbServers.Name = "lstbServers";
            lstbServers.Size = new Size(354, 199);
            lstbServers.TabIndex = 3;
            lstbServers.DrawItem += lstbServers_DrawItem;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabHome);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(5, 5);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(974, 505);
            tabControl.TabIndex = 0;
            tabControl.MouseDown += tabControl_MouseDown;
            tabControl.MouseLeave += tabControl_MouseLeave;
            // 
            // cmsTabMenu
            // 
            cmsTabMenu.Items.AddRange(new ToolStripItem[] { tsmiStartServer, tsmiStopServer, tsmiRestartServer, tsmiEditServer, toolStripMenuItem1, tsmiOpenFolder, tsmiEditConfig });
            cmsTabMenu.Name = "contextMenuStrip1";
            cmsTabMenu.Size = new Size(125, 142);
            cmsTabMenu.Opening += cmsTabMenu_Opening;
            // 
            // tsmiStartServer
            // 
            tsmiStartServer.Name = "tsmiStartServer";
            tsmiStartServer.Size = new Size(124, 22);
            tsmiStartServer.Text = "启动服务";
            tsmiStartServer.Click += tsmiStartServer_Click;
            // 
            // tsmiStopServer
            // 
            tsmiStopServer.Name = "tsmiStopServer";
            tsmiStopServer.Size = new Size(124, 22);
            tsmiStopServer.Text = "停止服务";
            tsmiStopServer.Click += tsmiStopServer_Click;
            // 
            // tsmiRestartServer
            // 
            tsmiRestartServer.Name = "tsmiRestartServer";
            tsmiRestartServer.Size = new Size(124, 22);
            tsmiRestartServer.Text = "重启服务";
            tsmiRestartServer.Click += tsmiRestartServer_Click;
            // 
            // tsmiEditServer
            // 
            tsmiEditServer.Name = "tsmiEditServer";
            tsmiEditServer.Size = new Size(124, 22);
            tsmiEditServer.Text = "编辑";
            tsmiEditServer.Click += tsmiEditServer_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(121, 6);
            // 
            // tsmiOpenFolder
            // 
            tsmiOpenFolder.Name = "tsmiOpenFolder";
            tsmiOpenFolder.Size = new Size(124, 22);
            tsmiOpenFolder.Text = "打开目录";
            tsmiOpenFolder.Click += tsmiOpenFolder_Click;
            // 
            // tsmiEditConfig
            // 
            tsmiEditConfig.Name = "tsmiEditConfig";
            tsmiEditConfig.Size = new Size(124, 22);
            tsmiEditConfig.Text = "配置编辑";
            tsmiEditConfig.Click += tsmiEditConfig_Click;
            // 
            // notifyIcon
            // 
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon.ContextMenuStrip = cmsMain;
            notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
            notifyIcon.Text = "--";
            notifyIcon.Visible = true;
            notifyIcon.MouseDoubleClick += notifyIcon_MouseDoubleClick;
            // 
            // cmsMain
            // 
            cmsMain.Items.AddRange(new ToolStripItem[] { tsmiOpenMainForm, tsmiMainStartServer, tsmiMainStopServer, tsmiExit });
            cmsMain.Name = "cmsMain";
            cmsMain.Size = new Size(137, 92);
            // 
            // tsmiOpenMainForm
            // 
            tsmiOpenMainForm.Name = "tsmiOpenMainForm";
            tsmiOpenMainForm.Size = new Size(136, 22);
            tsmiOpenMainForm.Text = "打开主界面";
            tsmiOpenMainForm.Click += tsmiOpenMainForm_Click;
            // 
            // tsmiMainStartServer
            // 
            tsmiMainStartServer.Name = "tsmiMainStartServer";
            tsmiMainStartServer.Size = new Size(136, 22);
            tsmiMainStartServer.Text = "启动服务";
            tsmiMainStartServer.Click += tsmiMainStartServer_Click;
            // 
            // tsmiMainStopServer
            // 
            tsmiMainStopServer.Name = "tsmiMainStopServer";
            tsmiMainStopServer.Size = new Size(136, 22);
            tsmiMainStopServer.Text = "停止服务";
            tsmiMainStopServer.Click += tsmiMainStopServer_Click;
            // 
            // tsmiExit
            // 
            tsmiExit.Name = "tsmiExit";
            tsmiExit.Size = new Size(136, 22);
            tsmiExit.Text = "退出";
            tsmiExit.Click += tsmiExit_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(lblVersion);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 515);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(5, 0, 5, 0);
            panel1.Size = new Size(984, 26);
            panel1.TabIndex = 2;
            // 
            // lblVersion
            // 
            lblVersion.Dock = DockStyle.Left;
            lblVersion.Location = new Point(5, 0);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(104, 26);
            lblVersion.TabIndex = 0;
            lblVersion.Text = "1.0.2502.1701";
            lblVersion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            panel2.Controls.Add(tabControl);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(5);
            panel2.Size = new Size(984, 515);
            panel2.TabIndex = 3;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 541);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "服务一键启动";
            FormClosing += MainForm_FormClosing;
            tabHome.ResumeLayout(false);
            tableLayoutPanel.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            cmsTabMenu.ResumeLayout(false);
            cmsMain.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabPage tabHome;
        private TabControl tabControl;
        private Button btnStartServer;
        private TableLayoutPanel tableLayoutPanel;
        private Button btnAddServer;
        private Button btnStopServer;
        private ContextMenuStrip cmsTabMenu;
        private ToolStripMenuItem tsmiStartServer;
        private ToolStripMenuItem tsmiStopServer;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip cmsMain;
        private ToolStripMenuItem tsmiOpenMainForm;
        private ToolStripMenuItem tsmiExit;
        private ToolStripMenuItem tsmiMainStartServer;
        private ToolStripMenuItem tsmiMainStopServer;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem tsmiRestartServer;
        private ServerList lstbServers;
        private ToolStripMenuItem tsmiOpenFolder;
        private ToolStripMenuItem tsmiEditConfig;
        private ToolStripMenuItem tsmiEditServer;
        private Panel panel1;
        private Label lblVersion;
        private Panel panel2;
    }
}