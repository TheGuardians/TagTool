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
                        var miliseconds = _stopWatch.ElapsedMilliseconds;
                        if (miliseconds <= 1000)
                        {
                            Console.WriteLine($"{miliseconds}ms");
                        }
                        else
                        {
                            Console.WriteLine("{0:0.000}s", (double)_stopWatch.ElapsedMilliseconds / 1000.0f);
                        }
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