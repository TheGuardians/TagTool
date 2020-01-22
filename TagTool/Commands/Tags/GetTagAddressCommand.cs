using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using TagTool.IO;

namespace TagTool.Commands.Tags
{
    class GetTagAddressCommand : Command
    {
        public GetTagAddressCommand()
            : base(true,

                  "GetTagAddress",
                  "Get the address of a tag in memory",

                  "GetTagAddress <tag> [process id]",

                  "Gets the address of the given tag in memory.\n" +
                  "By default, this will read the memory of the first eldorado.exe process found.\n" +
                  "Specify a process ID in hexadecimal to read the memory of a specific process.\n")
        {
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 2)
                return false;


            if (!int.TryParse(args[0], NumberStyles.HexNumber, null, out int tagIndex) || tagIndex < 0)
                return false;

            Process process;

            if (args.Count == 2)
            {

                if (!int.TryParse(args[1], NumberStyles.HexNumber, null, out int processId) || processId < 0)
                    return false;

                try
                {
                    process = Process.GetProcessById(processId);
                }
                catch (ArgumentException)
                {
                    Console.Error.WriteLine("Unable to find a process with an ID of 0x{0:X}", processId);
                    return true;
                }
            }
            else
            {
                var processes = Process.GetProcessesByName("eldorado");

                if (processes.Length == 0)
                {
                    Console.Error.WriteLine("Unable to find any eldorado.exe processes.");
                    return true;
                }

                process = processes[0];
            }

            using (var stream = new ProcessMemoryStream(process))
            {
                var exeBase = (uint)stream.BaseProcess.MainModule.BaseAddress - 0x400000;
                var address = GetTagAddress(stream, tagIndex, exeBase);

                if (address != 0)
                    Console.WriteLine("Tag 0x{0:X} is loaded at 0x{1:X8} in process 0x{2:X}.", tagIndex, address, process.Id);
                else
                    Console.Error.WriteLine("Tag 0x{0:X} is not loaded in process 0x{1:X}.", tagIndex, process.Id);
            }

            return true;
        }

        private static uint GetTagAddress(ProcessMemoryStream stream, int tagIndex, uint exeBase)
        {
            // Read the tag count and validate the tag index
            var reader = new BinaryReader(stream);
            reader.BaseStream.Position = 0x22AB008 + exeBase;
            var maxIndex = reader.ReadInt32();
            if (tagIndex >= maxIndex)
                return 0;

            // Read the tag index table to get the index of the tag in the address table
            reader.BaseStream.Position = 0x22AAFFC + exeBase;
            var tagIndexTableAddress = reader.ReadUInt32();
            if (tagIndexTableAddress == 0)
                return 0;
            reader.BaseStream.Position = tagIndexTableAddress + tagIndex * 4;
            var addressIndex = reader.ReadInt32();
            if (addressIndex < 0)
                return 0;

            // Read the tag's address in the address table
            reader.BaseStream.Position = 0x22AAFF8 + exeBase;
            var tagAddressTableAddress = reader.ReadUInt32();
            if (tagAddressTableAddress == 0)
                return 0;
            reader.BaseStream.Position = tagAddressTableAddress + addressIndex * 4;
            return reader.ReadUInt32();
        }
    }
}
