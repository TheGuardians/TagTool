using TagTool.Cache;
using System.Collections.Generic;
using TagTool.Cache.HaloOnline;
using System;
using TagTool.Commands.Common;
using TagTool.Commands.Tags;
using System.IO;
using TagTool.Cache.Gen3;
using TagTool.IO;
using System.Threading;
using System.Collections;
using TagTool.Common;
using TagTool.Tags;
using System.Linq;
using static System.Net.WebRequestMethods;
using static TagTool.Tags.Definitions.Model.Variant;

namespace TagTool.Commands.Modding
{
    class UpdateModPackageCommand : Command
    {
        private readonly GameCacheHaloOnline Cache;
        private CommandContextStack ContextStack { get; }
        private GameCacheModPackage oldMod { get; set; }
        private GameCacheModPackage newMod { get; set; }

        public UpdateModPackageCommand(CommandContextStack contextStack, GameCacheHaloOnline cache) :
            base(true,

                "UpdateModPackage",
                "Update a mod package to match the current base cache. Optionally use unmanaged streams for 2gb + resources.",

                "UpdateModPackage [large] <input path> <output path>")
        {
            ContextStack = contextStack;
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            bool useLargeStreams = false;

            if (args.Count > 1 && args[0].ToLower() == "large")
            {
                useLargeStreams = true;
                args.RemoveAt(0);
            }

            var infile = new FileInfo(args[0]);
            var outfile = new FileInfo(args[1]);

            if (!infile.Exists)
                return new TagToolError(CommandError.FileNotFound, $"\"{args[0]}\"");

            Console.WriteLine("Initializing cache...");
            oldMod = new GameCacheModPackage(Cache, infile, largeResourceStream: useLargeStreams);
            newMod = new GameCacheModPackage(Cache, useLargeStreams);
            InitializeModPackage();
            Console.WriteLine("Transferring modded tags...");
            if (!TransferModdedTags())
            {
                Console.WriteLine("Failed to update mod package!");
                return false;
            }
            newMod.SaveTagNames();
            newMod.SaveStrings();

            if (!newMod.SaveModPackage(outfile))
                return new TagToolError(CommandError.OperationFailed, "Failed to save mod package.");

            return true;

        }

        private bool TransferModdedTags()
        {
            bool result = true;
            for (var i = 0; i < oldMod.BaseModPackage.GetTagCacheCount(); i++)
            {
                oldMod.SetActiveTagCache(i);
                newMod.SetActiveTagCache(i);
                var oldStream = oldMod.OpenCacheRead();
                var newStream = newMod.OpenCacheReadWrite();
                List<CachedTag> moddedTags = new List<CachedTag>();
                List<CachedTag> newTags = new List<CachedTag>();
                //allocate modded tags in new mod package
                foreach (var tag in oldMod.TagCacheGenHO.Tags)
                {
                    var modCachedTag = oldMod.TagCache.GetTag(tag.Index) as CachedTagHaloOnline;
                    if (modCachedTag.IsEmpty())
                        continue;
                    if (newMod.TagCache.TryGetCachedTag($"{modCachedTag.Name}.{modCachedTag.Group}", out CachedTag newTag))
                        newTags.Add(newTag);
                    else
                        newTags.Add((CachedTagHaloOnline)newMod.TagCache.AllocateTag(modCachedTag.Group, modCachedTag.Name));
                    moddedTags.Add(modCachedTag);
                }
                //now fixup and copy over
                for(var j = 0; j < moddedTags.Count; j++)
                {
                    object definition = oldMod.Deserialize(oldStream, moddedTags[j]);
                    object newDef = definition;
                    if (!FixTags((TagStructure)definition, (TagStructure)newDef))
                        result = false;
                    newMod.Serialize(newStream, newTags[j], newDef);
                }
            }
            return result;
        }



        public bool FixTags(TagStructure input, TagStructure output)
        {
            if (input == null || output == null)
            {
                return false;
            }

            Type inputtype = input.GetType();
            Type outputtype = output.GetType();

            var inputinfo = TagStructure.GetTagStructureInfo(inputtype, Cache.Version, Cache.Platform);
            var outputinfo = TagStructure.GetTagStructureInfo(inputtype, Cache.Version, Cache.Platform);
            List<TagFieldInfo> outputfieldlist = TagStructure.GetTagFieldEnumerable(outputinfo.Types[0], outputinfo.Version, outputinfo.CachePlatform).ToList();
            List<TagFieldInfo> inputfieldlist = TagStructure.GetTagFieldEnumerable(inputinfo.Types[0], inputinfo.Version, inputinfo.CachePlatform).ToList();

            bool result = true;
            for (var i = 0; i < inputfieldlist.Count; i++)
            {
                var tagFieldInfo = inputfieldlist[i];
                var outputFieldInfo = outputfieldlist[i];

                //don't bother converting the value if it is null, padding, or runtime
                if (tagFieldInfo.Attribute.Flags.HasFlag(TagFieldFlags.Padding) ||
                    tagFieldInfo.Attribute.Flags.HasFlag(TagFieldFlags.Runtime) ||
                    tagFieldInfo.GetValue(input) == null)
                    continue;

                //fixup tagrefs
                if (tagFieldInfo.FieldType == typeof(CachedTag))
                {
                    CachedTag tagRef = (CachedTag)tagFieldInfo.GetValue(input);
                    if (newMod.TagCache.TryGetCachedTag($"{tagRef.Name}.{tagRef.Group}", out CachedTag newRef))
                    {
                        outputFieldInfo.SetValue(output, newRef);
                    }
                    else
                    {
                        new TagToolError(CommandError.CustomError, $"Referenced tag {tagRef.Name}.{tagRef.Group} not found!");
                        result = false;
                    }
                }
                //fixup stringids
                else if(tagFieldInfo.FieldType == typeof(StringId))
                {
                    StringId field = (StringId)tagFieldInfo.GetValue(input);
                    string oldString = oldMod.StringTable.GetString(field);
                    string newString = newMod.StringTable.GetString(field);
                    if (newString != oldString)
                    {
                        StringId newID = newMod.StringTable.AddString(oldString);
                        outputFieldInfo.SetValue(output, newID);
                    }                      
                    else
                        outputFieldInfo.SetValue(output, field);
                }
                //if its a sub-tagstructure, iterate into it
                else if (IsTagStructure(tagFieldInfo.FieldType))
                {
                    var outstruct = Activator.CreateInstance(outputFieldInfo.FieldType);
                    if(!FixTags((TagStructure)tagFieldInfo.GetValue(input), (TagStructure)outstruct))
                        result = false;
                    outputFieldInfo.SetValue(output, outstruct);
                }
                //if its a tagblock, call convertlist to iterate through and convert each one and return a converted list
                else if (tagFieldInfo.FieldType.IsGenericType && tagFieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>) &&
                    outputFieldInfo.FieldType.IsGenericType && tagFieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    object inputlist = tagFieldInfo.GetValue(input);
                    object outputlist = Activator.CreateInstance(outputFieldInfo.FieldType);
                    if(!SearchList(inputlist, outputlist))
                        result = false;
                    outputFieldInfo.SetValue(output, outputlist);
                }
                //otherwise just set the value
                else
                {
                    outputFieldInfo.SetValue(output, tagFieldInfo.GetValue(input));
                }
            }
            return result;
        }

        public bool SearchList(object input, object output)
        {
            if (input == null || output == null)
            {
                return false;
            }

            bool result = true;
            Type outputtype = output.GetType();
            //for simple list types such as List<short>, just assign value and return
            if (outputtype.GenericTypeArguments[0].BaseType == typeof(ValueType))
            {
                output = input;
                return result;
            }               

            IList collection = (IList)input;
            //IEnumerable<object> enumerable = input as IEnumerable<object>;
            //if (enumerable == null)
            //    throw new InvalidOperationException("listData must be enumerable");

            Type outputelementType = outputtype.GenericTypeArguments[0];
            var addMethod = outputtype.GetMethod("Add");
            foreach (object item in collection)
            {              
                var outputelement = Activator.CreateInstance(outputelementType);
                if (!FixTags((TagStructure)item, (TagStructure)outputelement))
                    result = false;
                addMethod.Invoke(output, new[] { outputelement });
            }
            return result;
        }
        private void InitializeModPackage()
        {
            //copy over header
            newMod.BaseModPackage.Header = oldMod.BaseModPackage.Header;

            //copy over metadata
            newMod.BaseModPackage.Metadata = oldMod.BaseModPackage.Metadata;

            //copy over other fields
            newMod.BaseModPackage.MapIds = oldMod.BaseModPackage.MapIds;
            newMod.BaseModPackage.CampaignFileStream = oldMod.BaseModPackage.CampaignFileStream;
            newMod.BaseModPackage.MapFileStreams = oldMod.BaseModPackage.MapFileStreams;
            newMod.BaseModPackage.Files = oldMod.BaseModPackage.Files;
            newMod.BaseModPackage.FontPackage = oldMod.BaseModPackage.FontPackage;
            newMod.BaseModPackage.MapToCacheMapping = oldMod.BaseModPackage.MapToCacheMapping;

            //copy over resources
            newMod.BaseModPackage.ResourcesStream = oldMod.BaseModPackage.ResourcesStream;

            //initialize new tag caches
            newMod.BaseModPackage.CacheNames = new List<string>();
            newMod.BaseModPackage.TagCachesStreams = new List<ExtantStream>();
            newMod.BaseModPackage.TagCacheNames = new List<Dictionary<int, string>>();

            var referenceStream = new MemoryStream(); // will be reused by all base caches
            var writer = new EndianWriter(referenceStream, false);
            var modTagCache = new TagCacheHaloOnline(Cache.Version, referenceStream, newMod.BaseModPackage.StringTable);

            //initialize new mod cache tags
            referenceStream.Seek(0, SeekOrigin.End);
            for (var tagIndex = 0; tagIndex < Cache.TagCache.Count; tagIndex++)
            {
                var srcTag = Cache.TagCache.GetTag(tagIndex);

                if (srcTag == null)
                {
                    modTagCache.AllocateTag(new TagGroupGen3());
                    continue;
                }

                var emptyTag = (CachedTagHaloOnline)modTagCache.AllocateTag(srcTag.Group, srcTag.Name);
                var cachedTagData = new CachedTagData
                {
                    Data = new byte[0],
                    Group = (TagGroupGen3)emptyTag.Group,
                };

                var headerSize = CachedTagHaloOnline.CalculateHeaderSize(cachedTagData);
                var alignedHeaderSize = (uint)((headerSize + 0xF) & ~0xF);
                emptyTag.HeaderOffset = referenceStream.Position;
                emptyTag.Offset = alignedHeaderSize;
                emptyTag.TotalSize = alignedHeaderSize;
                emptyTag.WriteHeader(writer, modTagCache.StringTableReference);
                StreamUtil.Fill(referenceStream, 0, (int)(alignedHeaderSize - headerSize));
            }

            modTagCache.UpdateTagOffsets(writer);

            for (int i = 0; i < oldMod.BaseModPackage.CacheNames.Count; i++)
            {
                string name = oldMod.BaseModPackage.CacheNames[i];

                Dictionary<int, string> tagNames = new Dictionary<int, string>();

                foreach (var tag in Cache.TagCache.NonNull())
                    tagNames[tag.Index] = tag.Name;

                referenceStream.Position = 0;
                var ms = referenceStream;
                if (i > 0)
                {
                    ms = new MemoryStream();
                    referenceStream.CopyTo(ms);
                    ms.Position = 0;
                }

                newMod.BaseModPackage.TagCachesStreams.Add(new ExtantStream(ms));
                newMod.BaseModPackage.CacheNames.Add(name);
                newMod.BaseModPackage.TagCacheNames.Add(tagNames);
            }

            newMod.SetActiveTagCache(0);
        }
        public static bool IsTagStructure(Type type)
        {
            Type originalType = type;
            while (type.BaseType != null)
            {
                if (type == typeof(TagStructure))
                    return true;
                type = type.BaseType;
            }
            return false;
        }
    }
}