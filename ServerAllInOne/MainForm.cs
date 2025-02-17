using ServerAllInOne.Configs;
using ServerAllInOne.Controls;
using ServerAllInOne.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace ServerAllInOne
{
    public partial class MainForm : Form
    {
        private readonly AppConfig appConfig;
        private readonly ServerConfigs configs;

        private int runningServerCount = 0;
        private int serverCount = 0;

        public bool ConfirmExit { get; private set; }

        /// <summary>
        /// 显示Server列表
        /// </summary>
        public bool ShowServerList
        {
            get => lstbServers.Visible;
            set => lstbServers.Visible = value;
        }

        public MainForm()
        {
            InitializeComponent();
            Load += Form1_Load;

            configs = ServerConfigs.Default;
            appConfig = AppConfig.Default;

            Text = appConfig.Name;
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            if (DesignMode)
                return;

            lblVersion.Text = $"v{Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString() ?? "1.0"}";

            configs.Servers.Sort((a, b) => a.Sort - b.Sort);
            foreach (var server in configs.Servers)
            {
                AddServerUI(server);
            }

            UpdateNotifyIcon();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (ConfirmExit)
                {
                    if (runningServerCount > 0)
                    {
                        if (MessageBox.Show("服务运行中，是否停止所有运行中的服务并退出程序？", "退出程序", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                    StopAllServer();
                }
                else
                {
                    var exitForm = new ExitConfirmForm();
                    if (exitForm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (exitForm.SimplyExit)
                        {
                            StopAllServer();
                        }
                        else
                        {
                            Hide();
                            e.Cancel = true;
                            notifyIcon.ShowBalloonTip(2000, "提示信息", "程序已最小化到桌面右下角", ToolTipIcon.Info);

                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
            }
            finally
            {
                if (!e.Cancel)
                {
                    notifyIcon.Visible = false;
                    notifyIcon.Dispose();
                }
            }
        }

        private void AddServerUI(Server server)
        {
            var tabPage = new TabPage(server.UUID)
            {
                Text = server.Name
            };
            var serverConsole = new ServerConsole()
            {
                Dock = DockStyle.Fill,
                ServerConfig = server
            };
            serverConsole.ServerStateChanged += ServerConsole_ServerStateChanged;

            var listItem = new ServerListItem
            {
                UUID = server.UUID,
                Name = server.Name,
                Running = serverConsole.Running,
                ProcessId = serverConsole.ProcessId,
            };
            lstbServers.Items.Add(listItem);

            tabPage.Controls.Add(serverConsole);
            tabPage.Tag = listItem;
            tabControl.TabPages.Add(tabPage);

            serverCount++;
        }

        private void RefreshServerConfig(ServerConsole server)
        {
            configs.Refresh(server.ServerConfig.UUID);
            var config = configs.Servers.Find(c => c.UUID == server.ServerConfig.UUID);
            if (config != null)
            {
                server.ServerConfig = config;

                if (server.Parent is TabPage tabPage)
                {
                    tabPage.Text = $"{config.Name}{(server.Running ? $"[{server.ProcessId}]" : "")}";
                    if (tabPage.Tag is ServerListItem listItem)
                    {
                        listItem.Name = config.Name;
                        lstbServers.Refresh();
                    }
                }
            }
        }

        private void ServerConsole_ServerStateChanged(object? sender, EventArgs e)
        {
            if (IsDisposed)
                return;

            // 服务状态改变
            if (sender is ServerConsole server)
            {
                ServerTabForeach(tabPage =>
                {
                    if (tabPage.Controls.Count > 0 && tabPage.Controls[0] == server)
                    {
                        var c = server.ServerConfig;
                        if (server.Running)
                        {
                            tabPage.Text = $"{c.Name}[{server.ProcessId}]";
                            runningServerCount++;
                        }
                        else
                        {
                            tabPage.Text = c.Name;
                            runningServerCount--;
                        }

                        if (tabPage.Tag is ServerListItem item)
                        {
                            item.Running = server.Running;
                            item.ProcessId = server.ProcessId;
                        }
                    }
                });

                lstbServers.Refresh();
            }

            UpdateNotifyIcon();
        }

        private void SortServerTab()
        {
            var selectedTab = tabControl.SelectedTab;

            tabControl.SuspendLayout();

            var tabPages = new List<TabPage>();

            ServerTabForeach(tabPage =>
            {
                tabPages.Add(tabPage);
            });
            foreach (var tabPage in tabPages)
            {
                tabControl.TabPages.Remove(tabPage);
            }

            foreach (var server in configs.Servers)
            {
                var tabPage = tabPages.Find(t => t.Tag is ServerListItem item && item.UUID == server.UUID);
                tabControl.TabPages.Add(tabPage);
            }

            tabControl.SelectedTab = selectedTab;

            tabControl.ResumeLayout(false);
        }

        private void btnAddServer_Click(object sender, EventArgs e)
        {
            var editForm = new EditServerForm();
            if (editForm.ShowDialog(this) == DialogResult.OK)
            {
                if (editForm.Server != null)
                {
                    configs.Servers.Add(editForm.Server);
                    configs.Servers.Sort((a, b) => a.Sort - b.Sort);
                    configs.Save();

                    AddServerUI(editForm.Server);

                    SortServerTab();
                }
            }
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否启动所有服务？", "启动服务", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            StartAllServer();
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            if (runningServerCount == 0)
                return;

            if (MessageBox.Show("是否停止所有服务？", "停止服务", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            StopAllServer();
        }

        private void StartAllServer()
        {
            ServerTabForeach(tabPage =>
            {
                if (tabPage.Controls[0] is ServerConsole server)
                {
                    RefreshServerConfig(server);
                    _ = server.StartAsync();
                }
            });
        }

        private void StopAllServer()
        {
            ServerTabForeach(tabPage =>
            {
                if (tabPage.Controls[0] is ServerConsole server)
                {
                    _ = server.StopAsync();
                }
            });
        }

        private void UpdateNotifyIcon()
        {
            var iconPath = runningServerCount > 0
                    ? "ServerAllInOne.Assets.Icons.server_windows.ico"
                    : "ServerAllInOne.Assets.Icons.server_windows_stop.ico";

            try
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(iconPath);
                notifyIcon.Icon = stream != null ? new Icon(stream) : null;
                notifyIcon.Text = runningServerCount > 0 ? $"{appConfig.Name} - 正在运行：{runningServerCount}/{serverCount}" : $"{appConfig.Name} - 服务未运行";
            }
            catch (Exception)
            {
                // ignore
            }
        }

        private void ServerTabForeach(Action<TabPage> action)
        {
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                if (tabPage == tabHome)
                    continue;

                action?.Invoke(tabPage);
            }
        }

        private void tabControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 1; i < tabControl.TabPages.Count; i++)
                {
                    var tabPage = tabControl.TabPages[i];
                    if (tabControl.GetTabRect(i).Contains(new Point(e.X, e.Y)))
                    {
                        tabControl.SelectedTab = tabPage;
                        tabControl.ContextMenuStrip = cmsTabMenu;
                        break;
                    }
                }
            }
        }

        private void tabControl_MouseLeave(object sender, EventArgs e)
        {
            tabControl.ContextMenuStrip = null;
        }

        private void cmsTabMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (tabControl.SelectedTab.Controls[0] is ServerConsole server)
            {
                tsmiStartServer.Enabled = !server.Running;
                tsmiStopServer.Enabled = server.Running;
                tsmiRestartServer.Enabled = server.Running;

                tsmiEditConfig.Visible = server.ServerConfig.Configs?.Length > 0;
            }
            else
            {
                tsmiStopServer.Enabled
                    = tsmiStartServer.Enabled
                    = tsmiRestartServer.Enabled
                    = false;
            }
        }

        private void tsmiStartServer_Click(object sender, EventArgs e)
        {
            var tabPage = tabControl.SelectedTab;
            if (tabPage.Controls[0] is ServerConsole server)
            {
                RefreshServerConfig(server);
                _ = server.StartAsync();
            }
        }

        private void tsmiStopServer_Click(object sender, EventArgs e)
        {
            var tabPage = tabControl.SelectedTab;
            if (tabPage.Controls[0] is ServerConsole server)
            {
                _ = server.StopAsync();
            }
        }

        private void tsmiRestartServer_Click(object sender, EventArgs e)
        {
            var tabPage = tabControl.SelectedTab;
            if (tabPage.Controls[0] is ServerConsole server)
            {
                Task.Run(async () =>
                {
                    await server.StopAsync();

                    RefreshServerConfig(server);
                    await server.StartAsync();
                });
            }
        }

        private void tsmiOpenFolder_Click(object sender, EventArgs e)
        {
            var tabPage = tabControl.SelectedTab;
            if (tabPage.Controls[0] is ServerConsole server)
            {
                var folder = server.ServerConfig.WorkingDirectory;
                if (string.IsNullOrEmpty(folder))
                {
                    folder = Path.GetDirectoryName(server.ServerConfig.ExePath);
                }
                if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
                {
                    Process.Start(new ProcessStartInfo("explorer.exe", Path.GetFullPath(folder)));
                }
            }
        }

        private void tsmiEditConfig_Click(object sender, EventArgs e)
        {
            var tabPage = tabControl.SelectedTab;
            if (tabPage.Controls[0] is ServerConsole server)
            {
                string currentDirectory = Path.GetDirectoryName(Application.ExecutablePath) ?? "";
                string exePath = Path.GetFullPath(Path.Combine(currentDirectory, server.ServerConfig.ExePath));
                string workingDirectory = Path.GetDirectoryName(exePath) ?? "";
                if (!string.IsNullOrEmpty(server.ServerConfig.WorkingDirectory))
                {
                    workingDirectory = Path.GetFullPath(Path.Combine(currentDirectory, server.ServerConfig.WorkingDirectory));
                }

                var editorForm = new ConfigEditorForm()
                {
                    WorkingDirectory = workingDirectory,
                    Configs = server.ServerConfig.Configs.Select(x => new KeyValuePair<string, string>(x.Name, Path.GetFullPath(Path.Combine(workingDirectory, x.Path)))).ToArray()
                };
                editorForm.Show(this);
            }
        }

        private void tsmiOpenMainForm_Click(object sender, EventArgs e)
        {
            Show();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            ConfirmExit = true;
            Close();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tsmiOpenMainForm_Click(tsmiOpenMainForm, EventArgs.Empty);
        }

        private void tsmiMainStartServer_Click(object sender, EventArgs e)
        {
            btnStartServer_Click(btnStartServer, EventArgs.Empty);
        }

        private void tsmiMainStopServer_Click(object sender, EventArgs e)
        {
            btnStopServer_Click(btnStopServer, EventArgs.Empty);
        }

        private void lstbServers_DrawItem(object sender, DrawItemEventArgs e)
        {
            var rect = e.Bounds;
        }

        private void tsmiEditServer_Click(object sender, EventArgs e)
        {
            var tabPage = tabControl.SelectedTab;
            if (tabPage.Controls[0] is ServerConsole server)
            {
                var editForm = new EditServerForm();
                editForm.Edit(server.ServerConfig);
                if (editForm.ShowDialog(this) == DialogResult.OK)
                {
                    if (editForm.Server != null)
                    {
                        configs.Servers.Sort((a, b) => a.Sort - b.Sort);
                        configs.Save();

                        RefreshServerConfig(server);

                        SortServerTab();
                    }
                }
            }
        }
    }
}