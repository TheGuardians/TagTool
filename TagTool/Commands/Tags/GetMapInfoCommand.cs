using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Commands;
using TagTool.Common;

namespace TagTool.Commands
{
    class GetMapInfoCommand : Command
    {
        public GetMapInfoCommand()
            : base(CommandFlags.Inherit,
                  
                  "GetMapInfo",
                  "Get information about a map",
                  
                  "GetMapInfo <filename>",
                  
                  "Loads a .map file and displays information about it.\n" +
                  "Currently only displays the scenario tag index.\n" +
                  "The filename must include the .map extension.\n" +
                  "Put the filename in quotes if it contains spaces.")
        {
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            try
            {
                using (var mapReader = new BinaryReader(File.OpenRead(args[0])))
                {
                    if (mapReader.ReadInt32() != new Tag("head").Value)
                    {
                        Console.Error.WriteLine("Invalid map file");
                        return true;
                    }

                    mapReader.BaseStream.Position = 0x2DF0;
                    var scnrIndex = mapReader.ReadInt32();
                    Console.WriteLine("Scenario tag index: {0:X8}", scnrIndex);
                }
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Unable to open the map file for reading.");
            }

            return true;
        }
    }
}
