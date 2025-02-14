using Microsoft.VisualBasic.ApplicationServices;
using ServerAllInOne.Configs;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ServerAllInOne.Controls
{
    public partial class ServerConsole : UserControl
    {
        #region win32 api
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll")]
        static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);

        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate? handlerRoutine, bool add);

        // Delegate type to be used as the Handler Routine for SCCH
        delegate bool ConsoleCtrlDelegate(CtrlTypes CtrlType);

        enum CtrlTypes : uint
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }
        #endregion

        private readonly static SemaphoreSlim slim = new(1);

        private Process? process;
        private bool running;
        private Server config;
        private List<string> writeCache = new List<string>();

        /// <summary>
        /// 进程ID
        /// </summary>
        public int ProcessId
        {
            get => process?.Id ?? 0;
        }

        /// <summary>
        /// 运行中
        /// </summary>
        public bool Running => running;

        /// <summary>
        /// 服务配置
        /// </summary>
        public Server ServerConfig
        {
            get => config;
            set
            {
                config = value;
                if (cmsRichText.Created)
                {
                    InitContextMenuStrip();
                }
            }
        }

        /// <summary>
        /// 服务执行状态改变
        /// </summary>
        public event EventHandler<EventArgs> ServerStateChanged;

        public ServerConsole()
        {
            InitializeComponent();

            Load += ServerConsole_Load;
            Disposed += ServerConsole_Disposed;
        }

        private void ServerConsole_Load(object? sender, EventArgs e)
        {
            // 日志缓存输出
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(100);
                    if (writeCache.Count == 0)
                        continue;

                    List<string> lines = new List<string>();
                    lock (writeCache)
                    {
                        lines.AddRange(writeCache);
                        writeCache.Clear();
                    }

                    WriteText(string.Join(Environment.NewLine, lines) + Environment.NewLine);
                }
            });

            WriteTextLine("服务未启动");
            InitContextMenuStrip();
        }

        private void ServerConsole_Disposed(object? sender, EventArgs e)
        {
            Stop();
        }

        public void Start()
        {
            if (running)
                return;

            ClearText();

            try
            {
                // 获取完整路径
                string currentDirectory = Path.GetDirectoryName(Application.ExecutablePath) ?? "";
                string exePath = Path.GetFullPath(Path.Combine(currentDirectory, ServerConfig.ExePath));

                if (!File.Exists(exePath))
                {
                    WriteTextLine($"服务文件不存在：{exePath}");
                    return;
                }

                string workingDirectory = Path.GetDirectoryName(exePath) ?? "";
                if (!string.IsNullOrEmpty(ServerConfig.WorkingDirectory))
                {
                    workingDirectory = Path.GetFullPath(Path.Combine(currentDirectory, ServerConfig.WorkingDirectory));
                }

                var startInfo = new ProcessStartInfo(exePath, ServerConfig.Arguments)
                {
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory
                };

                switch (ServerConfig.CodePage)
                {
                    case 65001: // utf-8
                        startInfo.StandardOutputEncoding = Encoding.UTF8;
                        startInfo.StandardErrorEncoding = Encoding.UTF8;
                        startInfo.StandardInputEncoding = Encoding.UTF8;
                        break;
                }

                process = Process.Start(startInfo);

                if (process != null)
                {
                    /**
                     * 开启EnableRaisingEvents以触发Exited事件
                     * 默认为false
                     * 为false时要调用HasExited属性才会触发Exited事件
                     */
                    process.EnableRaisingEvents = true;
                    process.Exited += Process_Exited;
                    process.OutputDataReceived += Process_OutputDataReceived;
                    process.BeginOutputReadLine();
                    process.ErrorDataReceived += Process_ErrorDataReceived;
                    process.BeginErrorReadLine();

                    running = true;

                    switch (ServerConfig.CodePage)
                    {
                        case 65001: // utf-8
                            process.StandardInput.WriteLine("chcp " + ServerConfig.CodePage);
                            process.StandardInput.Flush();
                            break;
                    }

                    WriteTextLine($"服务已启动[进程ID：{process.Id}]");
                    UIInvoke(() =>
                    {
                        ServerStateChanged?.Invoke(this, EventArgs.Empty);
                    });
                }
            }
            catch (Exception ex)
            {
                WriteTextLine($"启动服务异常：{ex.Message}");
            }
        }

        private void InitContextMenuStrip()
        {
            UIInvoke(() =>
            {
                cmsRichText.SuspendLayout();
                cmsRichText.Items.Clear();
                cmsRichText.Items.AddRange(new ToolStripItem[]
                {
                    tsmiCopy,
                    tsmiSeparator,
                    tsmiReturnTop,
                    tsmiReturnBottom,
                    tsmiClear
                });

                AppendQuickMenu();
                cmsRichText.ResumeLayout(false);
            });
        }

        private ToolStripItem CreateMenuItem(ContextMenu item)
        {
            if (item.Name == "-")
            {
                return new ToolStripSeparator();
            }
            var menu = new ToolStripMenuItem(item.Name)
            {
                Tag = item
            };
            var command = item.Command;
            if (!string.IsNullOrEmpty(item.Command))
            {
                menu.Click += (s, e) =>
                {
                    WriteCommand(command);
                };
            }
            return menu;
        }

        private void AppendQuickMenu()
        {
            var contextMenu = ServerConfig.ContextMenu ?? Array.Empty<ContextMenu>();
            if (contextMenu.Length > 0)
            {
                cmsRichText.Items.Add(tsmiSeparator1);

                foreach (var item in contextMenu)
                {
                    var menu = CreateMenuItem(item);
                    if (menu is ToolStripMenuItem toolStripMenuItem)
                    {
                        AppendQuickMenu(toolStripMenuItem, item.Items);
                    }
                    cmsRichText.Items.Add(menu);
                }
            }
        }

        private void AppendQuickMenu(ToolStripMenuItem parent, ContextMenu[] items)
        {
            if (items == null || items.Length == 0)
                return;

            foreach (var item in items)
            {
                var menu = CreateMenuItem(item);
                if (menu is ToolStripMenuItem toolStripMenuItem)
                {
                    AppendQuickMenu(toolStripMenuItem, item.Items);
                }
                parent.DropDownItems.Add(menu);
            }
        }

        public async Task StartAsync()
        {
            await Task.Run(() =>
            {
                slim.Wait();
                try
                {
                    Start();
                }
                finally
                {
                    slim.Release();
                }
            });
        }

        private void WaitForExit()
        {
            if (process == null)
                return;

            var timeout = config.StopConfig?.Timeout ?? 1000;
            if (timeout > 0)
            {
                process.WaitForExit(timeout);
            }
            else
            {
                process.WaitForExit();
            }
        }

        public void Stop()
        {
            if (running && process != null)
            {
                // 未成功
                var handler = new ConsoleCtrlDelegate(c =>
                {
                    return c == CtrlTypes.CTRL_C_EVENT;
                });

                WriteTextLine("服务停止中...");

                try
                {
                    var stopConfig = config.StopConfig ?? new ServerStopConfig
                    {
                        Method = "kill",
                        Timeout = 1000
                    };

                    switch (stopConfig.Method)
                    {
                        case "event":
                            if (AttachConsole((uint)process.Id))
                            {
                                CtrlTypes ctrlEvent;
                                switch (stopConfig.Command)
                                {
                                    case "ctrl+c":
                                    default:
                                        ctrlEvent = CtrlTypes.CTRL_C_EVENT;
                                        break;
                                }

                                // 设置自己的ctrl+c处理，防止自己被终止
                                SetConsoleCtrlHandler(handler, true);
                                try
                                {
                                    if (!GenerateConsoleCtrlEvent((uint)ctrlEvent, (uint)process.SessionId))
                                        return;
                                }
                                finally
                                {
                                    FreeConsole();
                                    // 重置此参数
                                    SetConsoleCtrlHandler(handler, false);
                                }
                            }
                            break;
                        case "input":
                            WriteCommand(stopConfig.Command, true);
                            break;
                        case "execute":
                        case "command":
                            var proc = Process.Start(new ProcessStartInfo
                            {
                                FileName = "cmd.exe",
                                Arguments = $"/c {stopConfig.Command}",
                                WorkingDirectory = Path.GetFullPath((string.IsNullOrEmpty(config.WorkingDirectory) ? Path.GetDirectoryName(config.ExePath) : config.WorkingDirectory) ?? ""),
                            });
                            proc?.WaitForExit(1000);
                            break;
                        case "kill":
                            // 执行kill
                            process.Kill();
                            break;
                    }

                    WaitForExit();
                }
                finally
                {
                    if (running)
                    {
                        WriteTextLine("服务停止失败");
                    }
                }
            }
        }

        public async Task StopAsync()
        {
            await Task.Run(() =>
            {
                slim.Wait();
                try
                {
                    Stop();
                }
                finally
                {
                    slim.Release();
                }
            });
        }


        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                WriteTextLine(e.Data);
            }
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            process = null;
            running = false;

            WriteTextLine("服务已停止");
            UIInvoke(() =>
            {
                ServerStateChanged?.Invoke(this, EventArgs.Empty);
            });
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                WriteTextLine(e.Data);
            }
        }

        private void WriteTextLine(string text)
        {
            // 写入缓存
            lock (writeCache)
            {
                writeCache.Add(text);
            }
        }

        private void WriteText(string? text)
        {
            if (IsDisposed)
                return;

            UIInvoke(() =>
            {
                if (string.IsNullOrEmpty(text))
                    return;

                try
                {
                    var focused = richTextBox.Focused;
                    var start = richTextBox.SelectionStart;
                    var length = richTextBox.SelectionLength;
                    var autoScroll = start == richTextBox.TextLength;

                    if (focused)
                    {
                        txtInput.Focus();
                    }

                    richTextBox.AppendText(text);
                    if (autoScroll)
                    {
                        richTextBox.SelectionStart = richTextBox.TextLength;
                        richTextBox.ScrollToCaret();
                    }
                    else
                    {
                        richTextBox.SelectionStart = start;
                        richTextBox.SelectionLength = length;
                    }
                    if (focused)
                    {
                        richTextBox.Focus();
                    }

                    // 最多保留1000行
                    var lines = richTextBox.Lines;
                    var maxline = 1000;
                    if (lines.Length > maxline)
                    {
                        var count = 0;
                        for (int i = 0; i < lines.Length - maxline; i++)
                        {
                            count += lines[i].Length;
                        }

                        richTextBox.Lines = lines.Skip(lines.Length - maxline).ToArray();
                        if (autoScroll)
                        {
                            richTextBox.SelectionStart = richTextBox.TextLength;
                        }
                        else
                        {
                            richTextBox.SelectionStart = Math.Max(0, start - count);
                        }
                    }
                }
                catch (ObjectDisposedException)
                {

                }
            });
        }

        private void UIInvoke(Action action)
        {
            if (action == null)
                return;

            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        private void ClearText()
        {
            UIInvoke(() =>
            {
                richTextBox.Clear();
            });
        }

        private void ReturnTop()
        {
            UIInvoke(() =>
            {
                richTextBox.SelectionStart = 0;
                richTextBox.SelectionLength = 0;
                richTextBox.ScrollToCaret();
            });
        }

        private void ReturnBottom()
        {
            UIInvoke(() =>
            {
                richTextBox.SelectionStart = richTextBox.TextLength;
                richTextBox.SelectionLength = 0;
                richTextBox.ScrollToCaret();
            });
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var input = txtInput.Text;
                try
                {
                    switch (input)
                    {
                        case ":clear":
                            ClearText();
                            return;
                        case ":top":
                            ReturnTop();
                            return;
                        case ":bottom":
                            ReturnBottom();
                            return;
                    }

                    WriteCommand(input);
                }
                finally
                {
                    txtInput.Clear();
                }
            }
        }

        private void WriteCommand(string command, bool forceWrite = false)
        {
            if (forceWrite || (ServerConfig?.CanInput ?? false))
            {
                WriteTextLine(command);
                if (process != null)
                {
                    process.StandardInput.WriteLine(command);
                    process.StandardInput.Flush();
                }
            }
        }

        private void cmsRichText_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tsmiCopy.Enabled = richTextBox.SelectionLength > 0;
        }

        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            richTextBox.Copy();
        }

        private void tsmiClear_Click(object sender, EventArgs e)
        {
            ClearText();
        }

        private void tsmiReturnTop_Click(object sender, EventArgs e)
        {
            ReturnTop();
        }

        private void tsmiReturnBottom_Click(object sender, EventArgs e)
        {
            ReturnBottom();
        }
    }
}
