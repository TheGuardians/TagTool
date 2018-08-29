using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Tags;
using TagTool.Audio;

namespace TagTool.Commands.Sounds
{
    class UseAudioCacheCommand : Command
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
                return false;

            if(Directory.Exists(args[0]))
            {
                AudioCacheDirectory = new DirectoryInfo(args[0]);
                Console.WriteLine("Audio cache directory set successfully");
                return true;
            }
            else
            {
                Console.WriteLine("Failed to find directory");
                return false;
            }
        }
    }
}