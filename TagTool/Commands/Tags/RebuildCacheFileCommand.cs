using TagTool.Cache;
using TagTool.IO;
using System.Collections.Generic;
using System.IO;
using System;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Tags;
using TagTool.Serialization;
using TagTool.Bitmaps;
using TagTool.Tags.Resources;
using TagTool.Bitmaps.Utils;
using TagTool.Bitmaps.DDS;
using TagTool.Geometry;
using TagTool.BlamFile;
using TagTool.Tags.Definitions.Gen1;
using TagTool.Cache.HaloOnline;
using TagTool.Havok;
using System.Linq;
using System.IO.Compression;
using TagTool.Tools.Geometry;
using TagTool.Shaders;
using TagTool.Scripting;
using TagTool.Cache.Gen3;

namespace TagTool.Commands
{
    
    public class RebuildCacheFileCommand : Command
    {
        GameCacheHaloOnline Cache;

        Dictionary<int, CachedTagHaloOnline> ConvertedTags;
        Dictionary<ResourceLocation, Dictionary<int, PageableResource>> CopiedResources;

        Dictionary<ResourceLocation, Stream> SourceResourceStreams;
        Dictionary<ResourceLocation, Stream> DestinationResourceStreams;

        public RebuildCacheFileCommand(GameCacheHaloOnline cache) : base(false, "RebuildCacheFile", "Create new cache files from the current cache.", "RebuildCacheFile [dir]", "")
        {
            Cache = cache;
            ConvertedTags = new Dictionary<int, CachedTagHaloOnline>();
            CopiedResources = new Dictionary<ResourceLocation, Dictionary<int, PageableResource>>();
            DestinationResourceStreams = new Dictionary<ResourceLocation, Stream>();
            SourceResourceStreams = new Dictionary<ResourceLocation, Stream>();
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 2)
                return false;

            string destinationDirectory;

            if(args.Count == 1)
                destinationDirectory = args[0];
            else
                destinationDirectory = @"D:\Halo\NewCache\maps\";
            DirectoryInfo dir = new DirectoryInfo(destinationDirectory);
            if (dir.Exists)
                dir.Delete(true);
            dir.Create();

            var destStringIdPath = Path.Combine(dir.FullName, Cache.StringIdCacheFile.Name);

            var destCacheContext = new GameCacheHaloOnline(dir);
            Cache.StringIdCacheFile.CopyTo(destStringIdPath, true);

            foreach (var value in Enum.GetValues(typeof(ResourceLocation)))
            {
                var location = (ResourceLocation)value;

                if (location == ResourceLocation.None || location == ResourceLocation.RenderModels || location == ResourceLocation.Lightmaps || location == ResourceLocation.Mods)
                    continue;

                CopiedResources[location] = new Dictionary<int, PageableResource>();
                SourceResourceStreams[location] = Cache.ResourceCaches.OpenCacheReadWrite(location);
                DestinationResourceStreams[location] = destCacheContext.ResourceCaches.OpenCacheReadWrite(location);

            }

            using (var srcStream = Cache.OpenCacheRead())
            using (var destStream = destCacheContext.OpenCacheReadWrite())
            {
                // copy cfgt
                CopyTag((CachedTagHaloOnline)Cache.TagCache.GetTag("global_tags", "cfgt"), Cache, srcStream, destCacheContext, destStream);

                // copy rmt2
                foreach (var tag in Cache.TagCache.NonNull())
                {
                    if (tag.Group.Tag == "rmt2" && !ConvertedTags.ContainsKey(tag.Index))
                    {
                        CopyTag((CachedTagHaloOnline)tag, Cache, srcStream, destCacheContext, destStream);
                    }
                }
                // copy scnr
                foreach (var tag in Cache.TagCache.NonNull())
                {
                    if (tag.Group.Tag == "scnr" && !ConvertedTags.ContainsKey(tag.Index))
                    {
                        CopyTag((CachedTagHaloOnline)tag, Cache, srcStream, destCacheContext, destStream);
                    }
                }
            }

            destCacheContext.SaveTagNames();

            using (var nameTagsCommands = File.CreateText($"{destinationDirectory}old_new_cache_mapping.csv"))
            {
                foreach (var kvPair in ConvertedTags)
                {
                    nameTagsCommands.WriteLine($"0x{kvPair.Key.ToString("X8")}, 0x{kvPair.Value.Index.ToString("X8")} ");

                }
            }

            foreach(var entry in SourceResourceStreams)
                entry.Value.Close();

            foreach (var entry in DestinationResourceStreams)
                entry.Value.Close();

            return true;
        }


        private CachedTagHaloOnline CopyTag(CachedTagHaloOnline srcTag, GameCacheHaloOnline srcCacheContext, Stream srcStream, GameCacheHaloOnline destCacheContext, Stream destStream)
        {
            if (srcTag == null)
                return null;

            if (srcTag.Name == "levels\\ui\\mainmenu\\mainmenu" || srcTag.Group.Tag == "cfgt")
                Console.WriteLine("How the hell");

            if (ConvertedTags.ContainsKey(srcTag.Index))
                return ConvertedTags[srcTag.Index];

            var structureType = srcCacheContext.TagCache.TagDefinitions.GetTagDefinitionType(srcTag.Group);
            var srcContext = new HaloOnlineSerializationContext(srcStream, srcCacheContext, srcTag);
            var tagData = srcCacheContext.Deserializer.Deserialize(srcContext, structureType);
            CachedTagHaloOnline destTag = null;

            for (var i = 0; i < destCacheContext.TagCacheGenHO.Tags.Count; i++)
            {
                if (destCacheContext.TagCacheGenHO.Tags[i] == null)
                {
                    destCacheContext.TagCacheGenHO.Tags[i] = destTag = new CachedTagHaloOnline(i, (TagGroupGen3)srcTag.Group);
                    break;
                }
            }

            if (destTag == null)
                destTag = (CachedTagHaloOnline)destCacheContext.TagCacheGenHO.AllocateTag(srcTag.Group);

            ConvertedTags[srcTag.Index] = destTag;

            if (srcTag.Name != null)
                destTag.Name = srcTag.Name;



            tagData = CopyData(tagData, srcCacheContext, srcStream, destCacheContext, destStream);

            if (structureType == typeof(Scenario))
                CopyScenario((Scenario)tagData);

            var destContext = new HaloOnlineSerializationContext(destStream, destCacheContext, destTag);
            destCacheContext.Serializer.Serialize(destContext, tagData);

            return destTag;
        }

        private object CopyData(object data, GameCacheHaloOnline srcCacheContext, Stream srcStream, GameCacheHaloOnline destCacheContext, Stream destStream)
        {
            if (data == null)
                return null;

            var type = data.GetType();

            if (type.IsPrimitive)
                return data;

            if (type == typeof(CachedTagHaloOnline))
                return CopyTag((CachedTagHaloOnline)data, srcCacheContext, srcStream, destCacheContext, destStream);

            if (type == typeof(PageableResource))
                return CopyResource((PageableResource)data, srcCacheContext, destCacheContext);

            if (type.IsArray)
                return CopyArray((Array)data, srcCacheContext, srcStream, destCacheContext, destStream);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return CopyList(data, type, srcCacheContext, srcStream, destCacheContext, destStream);

            if (type.IsSubclassOf(typeof(TagStructure)))
                return CopyStructure((TagStructure)data, type, srcCacheContext, srcStream, destCacheContext, destStream);

            return data;
        }

        private Array CopyArray(Array array, GameCacheHaloOnline srcCacheContext, Stream srcStream, GameCacheHaloOnline destCacheContext, Stream destStream)
        {
            if (array.GetType().GetElementType().IsPrimitive)
                return array;

            for (var i = 0; i < array.Length; i++)
            {
                var oldValue = array.GetValue(i);
                var newValue = CopyData(oldValue, srcCacheContext, srcStream, destCacheContext, destStream);
                array.SetValue(newValue, i);
            }

            return array;
        }

        private object CopyList(object list, Type type, GameCacheHaloOnline srcCacheContext, Stream srcStream, GameCacheHaloOnline destCacheContext, Stream destStream)
        {
            if (type.GenericTypeArguments[0].IsPrimitive)
                return list;

            var count = (int)type.GetProperty("Count").GetValue(list);
            var getItem = type.GetMethod("get_Item");
            var setItem = type.GetMethod("set_Item");

            for (var i = 0; i < count; i++)
            {
                var oldValue = getItem.Invoke(list, new object[] { i });
                var newValue = CopyData(oldValue, srcCacheContext, srcStream, destCacheContext, destStream);

                setItem.Invoke(list, new object[] { i, newValue });
            }

            return list;
        }

        private object CopyStructure(TagStructure data, Type type, GameCacheHaloOnline srcCacheContext, Stream srcStream, GameCacheHaloOnline destCacheContext, Stream destStream)
        {
            foreach (var field in data.GetTagFieldEnumerable(srcCacheContext.Version))
                field.SetValue(data, CopyData(field.GetValue(data), srcCacheContext, srcStream, destCacheContext, destStream));

            return data;
        }

        private PageableResource CopyResource(PageableResource pageable, GameCacheHaloOnline srcCacheContext, GameCacheHaloOnline destCacheContext)
        {
            if (pageable == null || pageable.Page.Index < 0 || !pageable.GetLocation(out var location))
                return null;

            ResourceLocation newLocation;

            switch (pageable.Resource.ResourceType)
            {
                case TagResourceTypeGen3.Bitmap:
                    newLocation = ResourceLocation.Textures;
                    break;

                case TagResourceTypeGen3.BitmapInterleaved:
                    newLocation = ResourceLocation.TexturesB;
                    break;

                case TagResourceTypeGen3.RenderGeometry:
                    newLocation = ResourceLocation.Resources;
                    break;

                case TagResourceTypeGen3.Sound:
                    newLocation = ResourceLocation.Audio;
                    break;

                default:
                    newLocation = ResourceLocation.ResourcesB;
                    break;
            }

            if (pageable.Page.CompressedBlockSize > 0)
            {
                var index = pageable.Page.Index;

                if (CopiedResources[location].ContainsKey(index))
                    return CopiedResources[location][index];

                var srcResCache = srcCacheContext.ResourceCaches.GetResourceCache(location);
                var dstResCache = destCacheContext.ResourceCaches.GetResourceCache(newLocation);

                var data = srcResCache.ExtractRaw(SourceResourceStreams[location], index, pageable.Page.CompressedBlockSize);

                pageable.ChangeLocation(newLocation);
                
                pageable.Page.Index = dstResCache.AddRaw(DestinationResourceStreams[newLocation], data);

                CopiedResources[location][index] = pageable;
            }

            return pageable;
        }

        private bool ScriptExpressionIsValue(HsSyntaxNode expr)
        {
            switch (expr.Flags)
            {
                case HsSyntaxNodeFlags.ParameterReference:
                    return true;

                case HsSyntaxNodeFlags.Expression:
                    if ((int)expr.ValueType.HaloOnline > 0x4)
                        return true;
                    else
                        return false;

                case HsSyntaxNodeFlags.ScriptReference: // The opcode is the tagblock index of the script it uses, so ignore
                case HsSyntaxNodeFlags.GlobalsReference: // The opcode is the tagblock index of the global it uses, so ignore
                case HsSyntaxNodeFlags.Group:
                    return false;

                default:
                    return false;
            }
        }

        private void CopyScriptTagReferenceExpressionData(HsSyntaxNode expr)
        {
            var srcTagIndex = BitConverter.ToInt32(expr.Data, 0);
            var destTagIndex = srcTagIndex == -1 ? -1 : ConvertedTags[srcTagIndex].Index;
            var newData = BitConverter.GetBytes(destTagIndex);
            expr.Data = newData;
        }

        private void CopyScenario(Scenario scnrDefinition)
        {
            foreach (var expr in scnrDefinition.ScriptExpressions)
            {
                if (!ScriptExpressionIsValue(expr))
                    continue;

                switch (expr.ValueType.HaloOnline)
                {
                    case HsType.HaloOnlineValue.Sound:
                    case HsType.HaloOnlineValue.Effect:
                    case HsType.HaloOnlineValue.Damage:
                    case HsType.HaloOnlineValue.LoopingSound:
                    case HsType.HaloOnlineValue.AnimationGraph:
                    case HsType.HaloOnlineValue.DamageEffect:
                    case HsType.HaloOnlineValue.ObjectDefinition:
                    case HsType.HaloOnlineValue.Bitmap:
                    case HsType.HaloOnlineValue.Shader:
                    case HsType.HaloOnlineValue.RenderModel:
                    case HsType.HaloOnlineValue.StructureDefinition:
                    case HsType.HaloOnlineValue.LightmapDefinition:
                    case HsType.HaloOnlineValue.CinematicDefinition:
                    case HsType.HaloOnlineValue.CinematicSceneDefinition:
                    case HsType.HaloOnlineValue.BinkDefinition:
                    case HsType.HaloOnlineValue.AnyTag:
                    case HsType.HaloOnlineValue.AnyTagNotResolving:
                        CopyScriptTagReferenceExpressionData(expr);
                        break;

                    default:
                        break;
                }
            }
        }

    }
}

