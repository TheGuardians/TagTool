using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Commands.Common
{
	class BeginScriptCommand : Command
	{
		private CommandContextStack Context;

		public BeginScriptCommand(CommandContextStack context) : base(true,
			
			"BeginScript",
			"Use before running a script to count errors and warnings.",
			"BeginScript [path-to-cmds]",
			"Use before running a script to count errors and warnings.")
		{
			Context = context;
		}

		public override object Execute(List<string> args)
		{
			Program.ErrorCount = 0;
			Program.WarningCount = 0;

			Program._stopWatch.Start();

			if (args.Count > 0 && File.Exists(args[0]))
			{
				Console.WriteLine("Timing execution of commands from file. Errors and warnings will be counted.");
				var runner = new RunCommands(Context);
				runner.Execute(args);

				var endScript = new EndScriptCommand();
				endScript.Execute(args);
			}
			else
				Console.WriteLine("\t\nTimer has started. Errors and warnings will be counted.");
			
			return true;
		}
	}
}
