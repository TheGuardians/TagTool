using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Scripting;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Commands.Modding
{
    class ApplyModPackageCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        private Dictionary<int, int> TagMapping = new Dictionary<int, int>();

        private Stream CacheStream;

        public ApplyModPackageCommand(HaloOnlineCacheContext cacheContext) :
            base(false,

                "ApplyModPackage",
                "Apply a mod package to the current cache. \n",

                "ApplyModPackage <File>",

                "Apply a mod package to the current cache. \n")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File {filePath} does not exist!");
                return false;
            }

            CacheStream = CacheContext.OpenTagCacheReadWrite();

            var modPackage = new ModPackageExtended(new FileInfo(filePath));

            for (int i = 0; i < modPackage.Tags.Index.Count; i++)
            {
                var modTag = modPackage.Tags.Index[i];

                if (modTag != null)
                {
                    if (!TagMapping.ContainsKey(modTag.Index))
                        ConvertCachedTagInstance(modPackage, modTag);
                }
            }

            // fixup map files

            foreach (var mapFile in modPackage.MapFileStreams)
            {
                using (var reader = new EndianReader(mapFile))
                {
                    MapFile map = new MapFile();
                    map.Read(reader);

                    var modIndex = map.Header.GetScenarioTagIndex();
                    TagMapping.TryGetValue(modIndex, out int newScnrIndex);
                    map.Header.SetScenarioTagIndex(newScnrIndex);
                    var mapName = map.Header.GetName();

                    var mapPath = $"{CacheContext.Directory.FullName}\\{mapName}.map";
                    var file = new FileInfo(mapPath);
                    var fileStream = file.OpenWrite();
                    using (var writer = new EndianWriter(fileStream, map.EndianFormat))
                    {
                        map.Write(writer);
                    }
                }
            }

            // apply .campaign file

            var campaignFilepath = $"{CacheContext.Directory.FullName}\\halo3.campaign";
            var campaignFile = new FileInfo(campaignFilepath);
            var campaignFileStream = campaignFile.OpenWrite();
            modPackage.CampaignFileStream.CopyTo(campaignFileStream);
            campaignFileStream.Close();

            CacheStream.Close();
            CacheStream.Dispose();
            CacheContext.SaveTagNames();
            return true;
        }


        private CachedTagInstance ConvertCachedTagInstance(ModPackageExtended modPack, CachedTagInstance modTag)
        {
            // tag has already been converted
            if (TagMapping.ContainsKey(modTag.Index))
                return CacheContext.TagCache.Index[TagMapping[modTag.Index]];   // get the matching tag in the destination package

            // Determine if tag requires conversion
            if (modTag.HeaderOffset == modTag.TotalSize)
            {
                //modtag references a base tag, figure out which one is it and add it to the mapping
                CachedTagInstance baseTag = null;
                if (modTag.Index < CacheContext.TagCache.Index.Count)
                    baseTag = CacheContext.GetTag(modTag.Index);

                // mod tag has a name, first check if baseTag name is null, else if the names don't match or group don't match
                if (baseTag != null && baseTag.Name != null && baseTag.Name == modTag.Name && baseTag.Group == modTag.Group)
                {
                    TagMapping[modTag.Index] = baseTag.Index;
                    return baseTag;
                }
                else
                {
                    // tag name/group doesn't match base tag, try to look for it
                    foreach (var cacheTag in CacheContext.TagCache.Index)
                    {
                        if (cacheTag.Group == modTag.Group && cacheTag.Name == modTag.Name)
                        {
                            TagMapping[modTag.Index] = cacheTag.Index;
                            return cacheTag;
                        }
                    }
                    // Failed to find tag in base cache
                    Console.Error.WriteLine($"Failed to find {modTag.Name}.{modTag.Group.ToString()} in the base cache, returning null tag reference.");
                    return null;
                }
            }     
            else
            {
                CachedTagInstance newTag;
                if (!CacheContext.TryGetTag($"{modTag.Name}.{modTag.Group}", out newTag))
                {
                    newTag = CacheContext.TagCache.AllocateTag(modTag.Group);
                    newTag.Name = modTag.Name;
                }

                TagMapping.Add(modTag.Index, newTag.Index);
                var definitionType = TagDefinition.Find(modTag.Group.Tag);
                var tagDefinition = CacheContext.Deserialize(new ModPackageTagSerializationContext(modPack.TagsStream, CacheContext, modPack, modTag), definitionType);
                tagDefinition = ConvertData(modPack, tagDefinition);

                if (definitionType == typeof(ForgeGlobalsDefinition))
                {
                    tagDefinition = ConvertForgeGlobals((ForgeGlobalsDefinition)tagDefinition);
                }
                else if (definitionType == typeof(Scenario))
                {
                    tagDefinition = ConvertScenario(modPack, (Scenario)tagDefinition);
                }
                CacheContext.Serialize(CacheStream, newTag, tagDefinition);

                foreach (var resourcePointer in modTag.ResourcePointerOffsets)
                {
                    newTag.AddResourceOffset(resourcePointer);
                }
                return newTag;
            }
        }

        private object ConvertData(ModPackageExtended modPack, object data)
        {

            var type = data.GetType();

            switch (data)
            {
                case null:
                case string _:
                case ValueType _:
                    return data;
                case PageableResource resource:
                    return ConvertPageableResource(modPack, resource);
                case TagStructure structure:
                    return ConvertStructure(modPack, structure);
                case IList collection:
                    return ConvertCollection(modPack, collection);
                case CachedTagInstance tag:
                    return ConvertCachedTagInstance(modPack, tag);
            }

            return data;
        }

        private PageableResource ConvertPageableResource(ModPackageExtended modPack, PageableResource resource)
        {
            if (resource.Page.Index == -1)
                return resource;

            var resourceStream = new MemoryStream();
            modPack.Resources.Decompress(modPack.ResourcesStream, resource.Page.Index, resource.Page.CompressedBlockSize, resourceStream);
            resourceStream.Position = 0;
            resource.ChangeLocation(ResourceLocation.ResourcesB);
            resource.Page.OldFlags &= ~OldRawPageFlags.InMods;
            CacheContext.AddResource(resource, resourceStream);

            return resource;
        }

        private IList ConvertCollection(ModPackageExtended modPack, IList collection)
        {
            // return early where possible
            if (collection is null || collection.Count == 0)
                return collection;

            for (var i = 0; i < collection.Count; i++)
            {
                var oldValue = collection[i];
                var newValue = ConvertData(modPack, oldValue);
                collection[i] = newValue;
            }

            return collection;
        }

        private T ConvertStructure<T>(ModPackageExtended modPack, T data) where T : TagStructure
        {
            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(data.GetType(), CacheContext.Version))
            {
                var oldValue = tagFieldInfo.GetValue(data);

                if (oldValue is null)
                    continue;

                var newValue = ConvertData(modPack, oldValue);
                tagFieldInfo.SetValue(data, newValue);
            }

            return data;
        }

        private ForgeGlobalsDefinition ConvertForgeGlobals(ForgeGlobalsDefinition forg)
        {
            var currentForgTag = CacheContext.GetTag<ForgeGlobalsDefinition>("multiplayer\\forge_globals");
            var currentForg = (ForgeGlobalsDefinition)CacheContext.Deserialize(CacheStream, currentForgTag);

            // hardcoded base indices:
            int[] baseBlockCounts = new int[] { 0, 15, 173, 6, 81, 468, 9, 12 };

            for (int i = baseBlockCounts[0]; i < forg.ReForgeMaterialTypes.Count; i++)
                currentForg.ReForgeMaterialTypes.Add(forg.ReForgeMaterialTypes[i]);

            for (int i = baseBlockCounts[1]; i < forg.ReForgeMaterialTypes.Count; i++)
                currentForg.ReForgeMaterialTypes.Add(forg.ReForgeMaterialTypes[i]);

            for (int i = baseBlockCounts[2]; i < forg.ReForgeObjects.Count; i++)
                currentForg.ReForgeObjects.Add(forg.ReForgeObjects[i]);

            for (int i = baseBlockCounts[3]; i < forg.Descriptions.Count; i++)
                currentForg.Descriptions.Add(forg.Descriptions[i]);

            for (int i = baseBlockCounts[4]; i < forg.PaletteCategories.Count; i++)
                currentForg.PaletteCategories.Add(forg.PaletteCategories[i]);

            for (int i = baseBlockCounts[5]; i < forg.Palette.Count; i++)
                currentForg.Palette.Add(forg.Palette[i]);

            for (int i = baseBlockCounts[6]; i < forg.WeatherEffects.Count; i++)
                currentForg.WeatherEffects.Add(forg.WeatherEffects[i]);

            for (int i = baseBlockCounts[7]; i < forg.Skies.Count; i++)
                currentForg.Skies.Add(forg.Skies[i]);

            // move over the rest of the definition
            currentForg.InvisibleRenderMethod = forg.InvisibleRenderMethod;
            currentForg.DefaultRenderMethod = forg.DefaultRenderMethod;
            currentForg.PrematchCameraObject = forg.PrematchCameraObject;
            currentForg.ModifierObject = forg.ModifierObject;
            currentForg.KillVolumeObject = forg.KillVolumeObject;
            currentForg.GarbageVolumeObject = forg.GarbageVolumeObject;

            return currentForg;
        }

        private Scenario ConvertScenario(ModPackageExtended modPack, Scenario scnr)
        {

            foreach (var expr in scnr.ScriptExpressions)
            {
                ConvertScriptExpression(modPack, expr);
            }

            return scnr;
        }

        public void ConvertScriptExpression(ModPackageExtended modPack, HsSyntaxNode expr)
        {
            ConvertScriptExpressionData(modPack, expr);
        }

        public void ConvertScriptExpressionData(ModPackageExtended modPack, HsSyntaxNode expr)
        {
            if (expr.Flags == HsSyntaxNodeFlags.Expression)
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
                        ConvertScriptTagReferenceExpressionData(modPack, expr);
                        return;
                    default:
                        break;
                }

        }

        public void ConvertScriptTagReferenceExpressionData(ModPackageExtended modPack, HsSyntaxNode expr)
        {
            var tagIndex = BitConverter.ToInt32(expr.Data.ToArray(), 0);

            if (tagIndex == -1)
                return;

            var tag = ConvertCachedTagInstance(modPack, modPack.Tags.Index[tagIndex]);
            expr.Data = BitConverter.GetBytes(tag.Index).ToArray();
        }
    }
}
