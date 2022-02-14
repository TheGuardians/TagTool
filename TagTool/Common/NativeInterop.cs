using System;
using System.Runtime.InteropServices;

namespace TagTool.Common
{
    public static class NativeInterop
    {
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultDllDirectories(uint directoryFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr AddDllDirectory(string NewDirectory);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
    }
}
