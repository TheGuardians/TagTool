using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using TagTool.IO;
using TagTool.Runtime;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using TagTool.Commands;
using TagTool.Tags;

namespace TagTool.Commands.Editing
{
    class PokeTagChangesCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private object Value { get; }

        public PokeTagChangesCommand(GameCacheContext cacheContext, CachedTagInstance tag, object value)
            : base(CommandFlags.Inherit,

                  "PokeTagChanges",
                  $"Pokes changes made to the current {cacheContext.GetString(tag.Group.Name)} definition to a running game's memory.",

                  "PokeTagChanges [process id]",

                  $"Pokes changes made to the current {cacheContext.GetString(tag.Group.Name)} definition to a running game's memory.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Value = value;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1)
                return false;
            
            Process process;

            if (args.Count == 1)
            {
                if (!int.TryParse(args[0], NumberStyles.HexNumber, null, out int processId) || processId < 0)
                    return false;

            #if !DEBUG
                try
                {
            #endif
                    process = Process.GetProcessById(processId);
            #if !DEBUG
                }
                catch (ArgumentException)
                {
                    Console.Error.WriteLine("Unable to find a process with an ID of 0x{0:X}", processId);
                    return true;
                }
            #endif
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

            using (var processStream = new ProcessMemoryStream(process))
            {
                var address = GetTagAddress(processStream, Tag.Index);

                var runtimeContext = new RuntimeSerializationContext(CacheContext, processStream);
                processStream.Position = address + Tag.DefinitionOffset;

                var definition = CacheContext.Deserializer.Deserialize(runtimeContext, TagDefinition.Find(Tag.Group.Tag));
                CacheContext.Serializer.Serialize(runtimeContext, Value);

                if (address != 0)
                    Console.WriteLine("Tag 0x{0:X} is loaded at 0x{1:X8} in process 0x{2:X}.", Tag.Index, address, process.Id);
                else
                    Console.Error.WriteLine("Tag 0x{0:X} is not loaded in process 0x{1:X}.", Tag.Index, process.Id);
            }

            return true;
        }

        private static uint GetTagAddress(ProcessMemoryStream stream, int tagIndex)
        {
            // Read the tag count and validate the tag index
            var reader = new BinaryReader(stream);
            reader.BaseStream.Position = 0x22AB008;
            var maxIndex = reader.ReadInt32();
            if (tagIndex >= maxIndex)
                return 0;

            // Read the tag index table to get the index of the tag in the address table
            reader.BaseStream.Position = 0x22AAFFC;
            var tagIndexTableAddress = reader.ReadUInt32();
            if (tagIndexTableAddress == 0)
                return 0;
            reader.BaseStream.Position = tagIndexTableAddress + tagIndex * 4;
            var addressIndex = reader.ReadInt32();
            if (addressIndex < 0)
                return 0;

            // Read the tag's address in the address table
            reader.BaseStream.Position = 0x22AAFF8;
            var tagAddressTableAddress = reader.ReadUInt32();
            if (tagAddressTableAddress == 0)
                return 0;
            reader.BaseStream.Position = tagAddressTableAddress + addressIndex * 4;
            return reader.ReadUInt32();
        }
    }
}