using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ServerAllInOne
{
    public static class ChildProcessTracker
    {
        private static readonly nint s_jobHandle;

        static ChildProcessTracker()
        {
            if (Environment.OSVersion.Version < new Version(6, 2))
                return; // Windows 8 或更高版本支持此功能

            string jobName = "ServerAllInOne" + Process.GetCurrentProcess().Id;
            s_jobHandle = CreateJobObject(nint.Zero, jobName);

            var info = new JOBOBJECT_BASIC_LIMIT_INFORMATION
            {
                LimitFlags = JOBOBJECTLIMIT.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE
            };

            var extendedInfo = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION
            {
                BasicLimitInformation = info
            };

            int length = Marshal.SizeOf(typeof(JOBOBJECT_EXTENDED_LIMIT_INFORMATION));
            nint extendedInfoPtr = Marshal.AllocHGlobal(length);
            try
            {
                Marshal.StructureToPtr(extendedInfo, extendedInfoPtr, false);
                if (!SetInformationJobObject(s_jobHandle, JobObjectInfoType.ExtendedLimitInformation, extendedInfoPtr, (uint)length))
                {
                    throw new Win32Exception();
                }
            }
            finally
            {
                Marshal.FreeHGlobal(extendedInfoPtr);
            }
        }

        public static void AddProcess(Process process)
        {
            if (s_jobHandle != nint.Zero)
            {
                bool success = AssignProcessToJobObject(s_jobHandle, process.Handle);
                if (!success && !process.HasExited)
                    throw new Win32Exception();
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern nint CreateJobObject(nint lpJobAttributes, string name);

        [DllImport("kernel32.dll")]
        private static extern bool SetInformationJobObject(nint job, JobObjectInfoType infoType, nint lpJobObjectInfo, uint cbJobObjectInfoLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AssignProcessToJobObject(nint job, nint process);
    }

    public enum JobObjectInfoType
    {
        ExtendedLimitInformation = 9
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JOBOBJECT_BASIC_LIMIT_INFORMATION
    {
        public long PerProcessUserTimeLimit;
        public long PerJobUserTimeLimit;
        public JOBOBJECTLIMIT LimitFlags;
        public nint MinimumWorkingSetSize;
        public nint MaximumWorkingSetSize;
        public uint ActiveProcessLimit;
        public nint Affinity;
        public uint PriorityClass;
        public uint SchedulingClass;
    }

    [Flags]
    public enum JOBOBJECTLIMIT : uint
    {
        JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x2000
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IO_COUNTERS
    {
        public ulong ReadOperationCount;
        public ulong WriteOperationCount;
        public ulong OtherOperationCount;
        public ulong ReadTransferCount;
        public ulong WriteTransferCount;
        public ulong OtherTransferCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
    {
        public JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;
        public IO_COUNTERS IoInfo;
        public nint ProcessMemoryLimit;
        public nint JobMemoryLimit;
        public nint PeakProcessMemoryUsed;
        public nint PeakJobMemoryUsed;
    }
}