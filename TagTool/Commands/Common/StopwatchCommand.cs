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
				  "Stopwatch stop - Stops the Stopwatch.\n",

				  "Utility command for timing other commands.")
		{
		}

		private static Stopwatch _stopWatch = new Stopwatch();
		public override object Execute(List<string> args)
		{
			if (args.Count == 0)
				return false;

			foreach (var arg in args)
			{
				switch (arg)
				{
					case "print":
                        var r = _stopWatch.ElapsedMilliseconds;

						// days
						var d = r / 86_400_000;
						r -= d * 86_400_000;
						
						// hours
						var h = r / 3_600_000;
						r -= h * 3_600_000;

						// minutes
						var m = r / 60_000;
						r -= m * 60_000;

						// seconds
						var s = r / 1_000;
						r -= s * 1_000;

						var output = "";
						output += d > 0 ? $"{d}d " : "";
						output += h > 0 ? $"{h}h " : "";
						output += m > 0 ? $"{m}m " : "";
						output += s > 0 ? $"{s}s " : "";
						output += r > 0 ? $"{r}ms" : "";
						output = output != "" ? output : "GOOD GOLLY THAT WAS FAST";

						var startColor = Console.ForegroundColor;
						Console.ForegroundColor = ConsoleColor.DarkCyan;
						Console.WriteLine(output);
						Console.ForegroundColor = startColor;
						break;
					case "reset":
						_stopWatch.Reset();
						break;
					case "restart":
						_stopWatch.Restart();
						break;
					case "start":
						_stopWatch.Start();
						break;
					case "stop":
						_stopWatch.Stop();
						break;
					default:
						return false;
				}
			}

			return true;
		}
	}
}