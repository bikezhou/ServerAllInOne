using ServerAllInOne.Configs;
using System.Diagnostics;
using System.Runtime.InteropServices;

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

        private Process? process;
        private bool running;

        private readonly static SemaphoreSlim slim = new(1);

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
        public Server ServerConfig { get; set; }

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
            WriteTextLine("服务未启动");
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
                if (!File.Exists(ServerConfig.ExePath))
                {
                    WriteTextLine($"服务文件不存在：{ServerConfig.ExePath}");
                    return;
                }

                process = Process.Start(new ProcessStartInfo(ServerConfig.ExePath, ServerConfig.Arguments)
                {
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                });

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

                    WriteTextLine($"服务已启动[进程ID：{process.Id}]");
                    Invoke(new MethodInvoker(() =>
                    {
                        ServerStateChanged?.Invoke(this, EventArgs.Empty);
                    }));
                }
            }
            catch (Exception ex)
            {
                WriteTextLine($"启动服务异常：{ex.Message}");
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

        public void Stop()
        {
            if (running && process != null)
            {
                // 未成功
                var handler = new ConsoleCtrlDelegate(c =>
                {
                    return c == CtrlTypes.CTRL_C_EVENT;
                });

                if (AttachConsole((uint)process.Id))
                {
                    // 设置自己的ctrl+c处理，防止自己被终止
                    SetConsoleCtrlHandler(handler, true);
                    try
                    {
                        // 发送ctrl+c（注意：这是向所有共享该console的进程发送）
                        if (!GenerateConsoleCtrlEvent((uint)CtrlTypes.CTRL_C_EVENT, 0))
                            return;

                        process.CancelErrorRead();
                        process.CancelOutputRead();

                        // 等待100ms
                        process.WaitForExit(100);
                    }
                    finally
                    {
                        // 重置此参数
                        SetConsoleCtrlHandler(handler, false);
                        FreeConsole();
                    }
                }

                if (running)
                {
                    WriteTextLine("服务停止失败");
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
            Invoke(new MethodInvoker(() =>
            {
                ServerStateChanged?.Invoke(this, EventArgs.Empty);
            }));
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
            WriteText(text + Environment.NewLine);
        }

        private void WriteText(string? text)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() =>
                {
                    WriteText(text);
                }));
                return;
            }

            if (string.IsNullOrEmpty(text))
                return;

            richTextBox.SuspendLayout();
            var start = richTextBox.SelectionStart;
            var length = richTextBox.SelectionLength;
            var autoScroll = start == richTextBox.TextLength;

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

            richTextBox.ResumeLayout(true);
        }

        private void ClearText()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() =>
                {
                    ClearText();
                }));
                return;
            }

            richTextBox.Clear();
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var input = txtInput.Text;
                try
                {
                    if (input == ":clear")
                    {
                        ClearText();
                        return;
                    }
                    if (ServerConfig?.CanInput ?? false)
                    {
                        WriteTextLine(input);
                        if (process != null)
                        {
                            process.StandardInput.WriteLine(input);
                            process.StandardInput.Flush();
                        }
                    }
                }
                finally
                {
                    txtInput.Clear();
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
            richTextBox.Clear();
        }
    }
}
