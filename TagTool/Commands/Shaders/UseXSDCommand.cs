using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Commands.Common;

namespace TagTool.Commands.Shaders
{
    class UseXSDCommand : Command
    {
        public static FileInfo XSDFileInfo = null;

        public UseXSDCommand() :
            base(true,

                "UseXSD",
                "Specify the directory to your xsd.exe for xbox 360 shader decompiler",

                "UseXSD <Directory>",
                "Specify the directory to your xsd.exe for xbox 360 shader decompiler")
        {
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            var directory = args[0];

            if (Directory.Exists(directory))
            {
                var file = Path.Combine(directory, "xsd.exe");
                if (File.Exists(file))
                {
                    XSDFileInfo = new FileInfo(file);
                    Console.WriteLine("Stored location of xsd.exe successfully!");
                    return true;
                }
            }

            return new TagToolError(CommandError.FileNotFound, $"Failed to locate xsd.exe in \"{directory}\"");
        }
    }
}