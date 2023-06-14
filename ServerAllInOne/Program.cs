using System.Diagnostics;
using System.Management;
using System.Text.RegularExpressions;

namespace ServerAllInOne
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (TryShowExistsApplication())
            {
                return;
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm()
            {
                ShowServerList = args.Contains("--list")
            });
        }

        /// <summary>
        /// 打开已启动的应用程序（单例应用程序）
        /// </summary>
        /// <returns></returns>
        private static bool TryShowExistsApplication()
        {
            var currentProcessId = Process.GetCurrentProcess().Id;
            var currentCommandLine = GetProcessCommandLine(currentProcessId);
            var currentLocation = GetProcessLocation(currentCommandLine);

            var procs = Process.GetProcessesByName("ServerAllInOne");
            foreach (var proc in procs)
            {
                if (proc.Id == currentProcessId)
                    continue;

                var commandLine = GetProcessCommandLine(proc.Id);
                if (currentLocation == GetProcessLocation(commandLine))
                {
                    var hWnd = Win32Api.GetProcessWindowHandle(proc.Id);

                    if (hWnd == IntPtr.Zero)
                    {
                        continue;
                    }

                    Win32Api.ShowWindow(hWnd, 1);

                    return true;
                }
            }

            return false;
        }

        private static string GetProcessCommandLine(int processId)
        {
            using var searcher = new ManagementObjectSearcher(
                    "SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + processId);
            using var objects = searcher.Get();
            var obj = objects.Cast<ManagementBaseObject>().SingleOrDefault();

            return obj?["CommandLine"]?.ToString() ?? "";
        }

        private static string GetProcessLocation(string commandLine)
        {
            var pattern = "^\"(.*?)\" ?.*$";
            var match = Regex.Match(commandLine, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return string.Empty;
        }
    }
}