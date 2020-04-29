using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using TagTool.IO;
using TagTool.Cache;
using TagTool.Serialization;
using System.Runtime.InteropServices;
using TagTool.Cache.HaloOnline;

namespace TagTool.Commands.Editing
{
    class PokeTagChangesCommand : Command
    {
        private GameCacheHaloOnlineBase Cache { get; }
        private CachedTagHaloOnline Tag { get; }
        private object Value { get; }

        public PokeTagChangesCommand(GameCacheHaloOnlineBase cache, CachedTagHaloOnline tag, object value)
            : base(true,

                  "PokeTagChanges",
                  $"Pokes changes made to the current {tag.Group} definition to a running game's memory.",

                  "PokeTagChanges [process id]",

                  $"Pokes changes made to the current {tag.Group} definition to a running game's memory.")
        {
            Cache = cache;
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
                if(address != 0)
                {
                    var runtimeContext = new RuntimeSerializationContext(Cache, processStream, address, Tag.Offset, Tag.CalculateHeaderSize(), Tag.TotalSize);

                    //pause the process during poking to prevent race conditions
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    process.Suspend();
                    Cache.Serializer.Serialize(runtimeContext, Value);
                    process.Resume();
                    stopWatch.Stop();

                    Console.WriteLine($"Poked tag at 0x{address.ToString("X8")} in {stopWatch.ElapsedMilliseconds / 1000.0f} seconds");
                }  
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

    public static class ProcessExtension
    {
        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);

        public static void Suspend(this Process process)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }
                SuspendThread(pOpenThread);
            }
        }
        public static void Resume(this Process process)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }
                ResumeThread(pOpenThread);
            }
        }
    }
}