using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using TagTool.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
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
                return new TagToolError(CommandError.ArgCount);

            Process process;

            if (args.Count == 1)
            {
                if (!int.TryParse(args[0], NumberStyles.HexNumber, null, out int processId) || processId < 0)
                    return new TagToolError(CommandError.ArgInvalid, $"Invalid ProcessId \"{args[0]}\"");

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
                    return new TagToolError(CommandError.OperationFailed, "Unable to find any eldorado.exe processes");

                process = processes[0];
            }

            using (var processStream = new ProcessMemoryStream(process))
            {
                var address = GetTagAddress(processStream, Tag.Index);
                if(address != 0)
                {
                    //first get a raw copy of the tag in the cache
                    byte[] tagcachedata;
                    using (var stream = Cache.OpenCacheRead())
                    using (var outstream = new MemoryStream())
                    using (EndianWriter writer = new EndianWriter(outstream, EndianFormat.LittleEndian))
                    {
                        //deserialize the cache def then reserialize to a stream
                        var cachedef = Cache.Deserialize(stream, Tag);
                        var dataContext = new DataSerializationContext(writer);                     
                        Cache.Serializer.Serialize(dataContext, cachedef);
                        StreamUtil.Align(outstream, 0x10);
                        tagcachedata = outstream.ToArray();
                    }
                        
                    //then serialize the current version of the tag in the editor
                    byte[] editordata;
                    using (MemoryStream stream = new MemoryStream())
                    using (EndianWriter writer = new EndianWriter(stream, EndianFormat.LittleEndian))
                    {
                        var dataContext = new DataSerializationContext(writer);
                        Cache.Serializer.Serialize(dataContext, Value);
                        StreamUtil.Align(stream, 0x10);
                        editordata = stream.ToArray();
                    }

                    //length should make to make sure the serializer is consistent
                    if(tagcachedata.Length != editordata.Length)
                    {
                        return new TagToolError(CommandError.OperationFailed, $"Error: tag size changed or the serializer failed!");
                    }

                    //some very rare tags have a size that doesn't match our serialized version, need to fix root cause
                    if(tagcachedata.Length != Tag.TotalSize - Tag.CalculateHeaderSize())
                    {
                        return new TagToolError(CommandError.OperationFailed, $"Sorry can't poke this specific tag yet (only happens with very rare specific tags), go bug a dev");
                    }

                    //pause the process during poking to prevent race conditions
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    process.Suspend();

                    //write diffed bytes only
                    int patchedbytes = 0;
                    int headersize = (int)Tag.CalculateHeaderSize();
                    for(var i = 0; i < editordata.Length; i++)
                    {
                        if(editordata[i] != tagcachedata[i])
                        {
                            processStream.Seek(address + headersize + i, SeekOrigin.Begin);
                            processStream.WriteByte(editordata[i]);
                            patchedbytes++;
                        }
                    }
                    processStream.Flush();

                    process.Resume();
                    stopWatch.Stop();

                    Console.WriteLine($"Patched {patchedbytes} bytes in {stopWatch.ElapsedMilliseconds / 1000.0f} seconds");
                }
                else
                    return new TagToolError(CommandError.OperationFailed, $"Tag 0x{Tag.Index:X} is not loaded in process 0x{process.Id:X}.");
            }

            return true;
        }

        public void DumpData(string filename, byte[] data)
        {
            using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                fs.Write(data, 0, data.Length);
            }
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