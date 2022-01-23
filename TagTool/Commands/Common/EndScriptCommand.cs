using System;
using System.Collections.Generic;

namespace TagTool.Commands.Common
{
	class EndScriptCommand : Command
	{
		public EndScriptCommand() : base(true,

			"EndScript",
			"Print time elapsed, error and warning counts when a script ends.",
			"EndScript",
			"Print time elapsed, error and warning counts when a script ends.")
		{
		}

		public override object Execute(List<string> args)
		{
			Program._stopWatch.Stop();
			var timeMs = Program._stopWatch.ElapsedMilliseconds;
			int seconds = 0;
			int minutes = 0;

			string timeDisplay = $"{timeMs} milliseconds";

			if (timeMs > 10000)
			{
				minutes = ((int)timeMs / 1000) / 60;
				seconds = ((int)timeMs / 1000) % 60;

				timeDisplay = $"{minutes} minute(s) and {seconds} seconds";
			}

			if (Program.ErrorCount == 0 && Program.WarningCount == 0)
				Console.ForegroundColor = ConsoleColor.Green;

			Console.Write($"\n\tScript finished in {timeDisplay} with ");

			if (Program.ErrorCount > 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write($"{Program.ErrorCount} errors and ");
				Console.ResetColor();
			}
			else
				Console.Write("no errors ");

			Console.Write("and ");

			if (Program.WarningCount > 0)
			{
				Console.ForegroundColor = ConsoleColor.DarkYellow;
				Console.Write($"{Program.WarningCount} warnings");
				Console.ResetColor();
			}
			else
				Console.Write("no warnings");

			Console.Write(".\n");
			Console.ResetColor();

			Program.ErrorCount = 0;
			Program.WarningCount = 0;
			Program._stopWatch.Reset();

			return true;
		}
	}
}
