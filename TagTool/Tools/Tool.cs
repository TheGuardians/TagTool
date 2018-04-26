using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Tools
{
	public class Tool
	{
		public string Output;
		public string Error;

		public Tool(string args, string toolName)
		{
			ProcessStartInfo info = new ProcessStartInfo($@"Tools\{toolName}")
			{
				Arguments = args,
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Hidden,
				UseShellExecute = false,
				RedirectStandardError = false,
				RedirectStandardOutput = false,
				RedirectStandardInput = false
			};
			Process process = Process.Start(info);
			process.WaitForExit();
		}


		public Tool(string args, string toolName, bool createWindow, bool redirectError, 
													bool redirectOutput, bool redirectInput)
		{
			ProcessStartInfo info = new ProcessStartInfo($@"Tools\{toolName}")
			{
				Arguments = args,
				CreateNoWindow = createWindow,
				WindowStyle = createWindow ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden,
				UseShellExecute = false,
				RedirectStandardError = redirectError,
				RedirectStandardOutput = redirectOutput,
				RedirectStandardInput = redirectInput
			};
			Process process = Process.Start(info);

			Output = process.StandardOutput.ReadToEnd();
			Error = process.StandardError.ReadToEnd();

			process.WaitForExit();
		}
	}
}
