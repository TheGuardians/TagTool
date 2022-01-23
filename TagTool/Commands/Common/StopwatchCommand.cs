using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TagTool.Commands.Common
{
	class StopwatchCommand : Command
	{
		public StopwatchCommand()
			: base(true,

				  "Stopwatch",
				  "Utility command for timing other commands.",

				  "Stopwatch print - Prints the elapsed time in milliseconds.\n" +
				  "Stopwatch reset - Resets the Stopwatch.\n" +
				  "Stopwatch restart - Restarts the Stopwatch.\n" +
				  "Stopwatch start - Starts the Stopwatch.\n" +
				  "Stopwatch stop - Stops the Stopwatch.",

				  "Utility command for timing other commands.")
		{
		}

		public override object Execute(List<string> args)
		{
			if (args.Count == 0)
				return new TagToolError(CommandError.ArgCount);

			for (var a = 0; a < args.Count; a++)
			{
				var arg = args[a].ToLower();
				switch (arg)
				{
					case "print":
						{
							var milliseconds = Program._stopWatch.ElapsedMilliseconds;
							var output = milliseconds.FormatMilliseconds();
							var startColor = Console.ForegroundColor;
							Console.ForegroundColor = ConsoleColor.DarkCyan;
							Console.WriteLine(output);
							Console.ForegroundColor = startColor;
						}
						break;
					case "reset":
						Program._stopWatch.Reset();
						break;
					case "restart":
						Program._stopWatch.Restart();
						break;
					case "start":
						Program._stopWatch.Start();
						break;
					case "stop":
						Program._stopWatch.Stop();
						break;
					default:
						return new TagToolError(CommandError.ArgInvalid, $"\"{arg}\""); ;
				}
			}

			return true;
		}
	}
}