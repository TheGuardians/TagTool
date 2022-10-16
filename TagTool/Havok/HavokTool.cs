using System.Diagnostics;
using System.IO;
using TagTool.Commands;

namespace TagTool.Havok
{
    public static class HavokTool
    {
        public static int ExecuteCommand(string command, string args)
        {
            var pi = new ProcessStartInfo()
            {
                FileName = Path.GetFullPath(Path.Combine(Program.TagToolDirectory, "Tools", "mopp.exe")),
                Arguments = $"{command} {args}",
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            var process = Process.Start(pi);
            process.WaitForExit();
            return process.ExitCode;
        }
    }
}
