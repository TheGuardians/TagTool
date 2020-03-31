using System;
using System.Collections.Generic;
using System.IO;

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
                "")
        {
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var directory = args[0];

            if (Directory.Exists(directory))
            {
                var file = Path.Combine(directory, "xsd.exe");
                if (File.Exists(file))
                {
                    XSDFileInfo = new FileInfo(file);
                    Console.WriteLine("Stored location of xsd.exe sucessfully!");
                    return true;
                }


            }
            Console.WriteLine("Failed to locate xsd.exe at the specified location");
            return false;
        }
    }
}