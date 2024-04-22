using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Commands.Common;

namespace TagTool.Commands.Sounds
{
    public class UseAudioCacheCommand : Command
    {
        public static DirectoryInfo AudioCacheDirectory = null;

        public UseAudioCacheCommand() :
            base(true,

                "UseAudioCache",
                "Specify a directory to store audio files",

                "UseAudioCache <Directory>",
                "")
        {
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            var directory = args[0];

            if (!Directory.Exists(directory))
            {
                Console.Write("Destination directory does not exist. Create it? [y/n] ");
                var answer = Console.ReadLine().ToLower();

                if (answer.Length == 0 || !(answer.StartsWith("y") || answer.StartsWith("n")))
                    return new TagToolError(CommandError.YesNoSyntax);

                if (answer.StartsWith("y"))
                    Directory.CreateDirectory(directory);
            }

            if (Directory.Exists(directory))
            {
                AudioCacheDirectory = new DirectoryInfo(directory);
                Console.WriteLine("Audio cache directory set successfully");
                return true;
            }
            else
            {
                return new TagToolError(CommandError.DirectoryNotFound, $"\"{directory}\"");
            }
        }
    }
}