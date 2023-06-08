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

        Process? process;

        public ServerConsole()
        {
            InitializeComponent();
            Load += ServerConsole_Load;
            Disposed += ServerConsole_Disposed;
        }

        private void ServerConsole_Load(object? sender, EventArgs e)
        {
            WriteText("服务未启动");
            if (ServerConfig != null)
            {
                txtInput.Visible = ServerConfig.CanInput;
            }
        }

        private void ServerConsole_Disposed(object? sender, EventArgs e)
        {
            Stop();
        }

        public bool Running { get; set; }

        public Server ServerConfig { get; set; }

        public void Start()
        {
            if (Running)
                return;

            ClearText();

            try
            {
                if (!File.Exists(ServerConfig.ExePath))
                {
                    WriteText($"可执行程序文件不存在：{ServerConfig.ExePath}");
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
                    process.Exited += Process_Exited;
                    process.OutputDataReceived += Process_OutputDataReceived;
                    process.BeginOutputReadLine();
                    process.ErrorDataReceived += Process_ErrorDataReceived;
                    process.BeginErrorReadLine();

                    txtInput.Visible = ServerConfig.CanInput;

                    Running = true;
                }
            }
            catch (Exception ex)
            {
                WriteText($"启动应用程序异常：{ex.Message}");
            }
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                WriteText("[Console Error]: " + e.Data + Environment.NewLine);
            }
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            WriteText("Server exit.");
        }

        public void Stop()
        {
            if (Running && process != null)
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

                        process.WaitForExit(200);
                    }
                    finally
                    {
                        // 重置此参数
                        SetConsoleCtrlHandler(handler, false);
                        FreeConsole();
                    }
                }
                else
                {
                    return;
                }
                WriteText("服务已停止");
                process = null;
                Running = false;
            }
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            WriteText(e.Data + Environment.NewLine);
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

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && (ServerConfig?.CanInput ?? false))
            {
                var input = txtInput.Text;
                WriteText(input);
                if (input == ":clear")
                {
                    ClearText();
                }
                if (process != null)
                {
                    process.StandardInput.WriteLine(input);
                    process.StandardInput.Flush();
                }
                txtInput.Clear();
            }
        }
    }
}
