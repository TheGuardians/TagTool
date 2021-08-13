using System;
using System.IO;
using System.IO.Compression;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Resources;
using TagTool.BlamFile;
using TagTool.Cache.Resources;
using TagTool.Tags.Definitions.Gen4;
using static TagTool.Tags.Definitions.Gen4.CacheFileResourceGestalt;

namespace TagTool.Cache.Gen4
{
    public class ResourceCacheGen4 : ResourceCache
    {
        public bool isLoaded;
        public CacheFileResourceGestalt ResourceGestalt;
        public CacheFileResourceLayoutTable ResourceLayoutTable;
        public GameCacheGen4 Cache;

        public ResourceCacheGen4(GameCacheGen4 cache, bool load = false)
        {
            isLoaded = false;
            Cache = cache;

            if (load)
                LoadResourceCache();
        }

        public void LoadResourceCache()
        {
            var Gen4Header = (CacheFileHeaderGen4)Cache.BaseMapFile.Header;

            // means no resources
            if (Cache.Version > CacheVersion.Halo3Beta && Gen4Header.SectionTable.Sections[(int)CacheFileSectionType.ResourceSection].Size == 0)
                return;
            // means resources but no tags, campaign.map for example. The resource section only contains pages for resources
            else if (Cache.Version > CacheVersion.Halo3Beta &&Gen4Header.SectionTable.Sections[(int)CacheFileSectionType.TagSection].Size == 0)
                return;
            else
            {
                using (var cacheStream = Cache.OpenCacheRead())
                {
                    ResourceGestalt = Cache.Deserialize<CacheFileResourceGestalt>(cacheStream, Cache.TagCacheGen4.GlobalInstances["zone"]);
                    ResourceLayoutTable = Cache.Deserialize<CacheFileResourceLayoutTable>(cacheStream, Cache.TagCacheGen4.GlobalInstances["play"]);
                }
            }
            isLoaded = true;
        }

        public CacheFileResourceDataBlock GetTagResourceFromReference(TagResourceReference resourceReference)
        {
            if (!isLoaded)
                LoadResourceCache();

            if (resourceReference == null)
                return null;

            return ResourceGestalt.Resources[resourceReference.Gen3ResourceID.Index];
        }

        public bool IsResourceValid(CacheFileResourceDataBlock tagResource)
        {
            if (tagResource == null || tagResource.ResourceTypeIndex == -1)
                return false;
            else
                return true;
        }

        public override SoundResourceDefinition GetSoundResourceDefinition(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (!IsResourceValid(tagResource))
                return null;

            byte[] primaryResourceData = GetPrimaryResource(resourceReference.Gen3ResourceID);
            byte[] secondaryResourceData = GetSecondaryResource(resourceReference.Gen3ResourceID);

            if (primaryResourceData == null)
                primaryResourceData = new byte[0];

            if (secondaryResourceData == null)
                secondaryResourceData = new byte[0];


            //
            // Reorganize primary and secondary resources into a single contiguous data[]
            //

            byte[] data = new byte[primaryResourceData.Length + secondaryResourceData.Length];

            if (data.Length == 0)
                return null;

            var primarySubPageTable = GetPrimarySubpageTable(resourceReference.Gen3ResourceID);
            var secondarySubPageTable = GetSecondarySubpageTable(resourceReference.Gen3ResourceID);

            if (primarySubPageTable == null)
                return null;

            var primaryRunningOffset = 0;
            foreach(var subPage in primarySubPageTable.StreamingSubpages)
            {
                Array.Copy(primaryResourceData, primaryRunningOffset, data, subPage.MemoryOffset, subPage.MemorySize);
                primaryRunningOffset += subPage.MemorySize;
            }

            if(secondarySubPageTable != null && secondaryResourceData.Length > 0)
            {
                var secondaryRunningOffset = 0;
                foreach (var subPage in secondarySubPageTable.StreamingSubpages)
                {
                    Array.Copy(secondaryResourceData, secondaryRunningOffset, data, subPage.MemoryOffset, subPage.MemorySize);
                    secondaryRunningOffset += subPage.MemorySize;
                }
            }

            // does not exist in Gen4, create one.
            var resourceDef = new SoundResourceDefinition
            {
                Data = new TagData(data)
            };
            return resourceDef;
        }

        // TODO: create an interface/adapter for BitmapTextureInteropResource

        public Tags.Resources.Gen4.BitmapTextureInteropResource GetBitmapTextureInteropResourceGen4(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (!IsResourceValid(tagResource) || GetResourceTypeName(tagResource) != "bitmap_texture_interop_resource")
                return null;
            return GetResourceDefinition<Tags.Resources.Gen4.BitmapTextureInteropResource>(resourceReference);
        }

        public Tags.Resources.Gen4.ModelAnimationTagResource GetModelAnimationTagResourceGen4(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (!IsResourceValid(tagResource) || GetResourceTypeName(tagResource) != "model_animation_tag_resource")
                return null;
            return GetResourceDefinition<Tags.Resources.Gen4.ModelAnimationTagResource>(resourceReference);
        }

        public override BinkResource GetBinkResource(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public override BitmapTextureInteropResource GetBitmapTextureInteropResource(TagResourceReference resourceReference)
        {
            // Use GetBitmapTextureInteropResourceGen4() for now
            throw new NotImplementedException();
        }

        public override BitmapTextureInterleavedInteropResource GetBitmapTextureInterleavedInteropResource(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public override RenderGeometryApiResourceDefinition GetRenderGeometryApiResourceDefinition(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public override ModelAnimationTagResource GetModelAnimationTagResource(TagResourceReference resourceReference)
        {
            // Use GetModelAnimationTagResourceGen4() for now
            throw new NotImplementedException();
        }

        public override StructureBspTagResources GetStructureBspTagResources(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public override StructureBspCacheFileTagResources GetStructureBspCacheFileTagResources(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateBinkResource(BinkResource binkResourceDefinition)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateRenderGeometryApiResource(RenderGeometryApiResourceDefinition renderGeometryDefinition)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateModelAnimationGraphResource(ModelAnimationTagResource modelAnimationGraphDefinition)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateBitmapInterleavedResource(BitmapTextureInterleavedInteropResource bitmapResourceDefinition)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateStructureBspResource(StructureBspTagResources sbspResource)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateStructureBspCacheFileResource(StructureBspCacheFileTagResources sbspCacheFileResource)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateSoundResource(SoundResourceDefinition soundResourceDefinition)
        {
            return null;
        }

        public override TagResourceReference CreateBitmapResource(BitmapTextureInteropResource bitmapResourceDefinition)
        {
            return null;
        }

        private string GetResourceTypeName(CacheFileResourceDataBlock tagResource)
        {
            return Cache.StringTable.GetString(ResourceGestalt.ResourceTypeIdentifiers[tagResource.ResourceTypeIndex].Name);
        }

        private void ApplyResourceDefinitionFixups(CacheFileResourceDataBlock tagResource, byte[] resourceDefinitionData)
        {
            using (var resourceDefinitionStream = new MemoryStream(resourceDefinitionData))
            using (var fixupWriter = new EndianWriter(resourceDefinitionStream, EndianFormat.BigEndian))
            {
                for (int i = 0; i < tagResource.ControlFixups.Count; i++)
                {
                    var fixup = tagResource.ControlFixups[i];
                    fixupWriter.Seek((int)fixup.BlockOffset, SeekOrigin.Begin);
                    fixupWriter.Write(fixup.Address.Value);
                }
            }
        }

        private int GetResourceDefinitionDataOffset(CacheFileResourceDataBlock tagResource)
        {
            return tagResource.PriorityLevelData[0].NaiveDataOffset;
        }

        public override object GetResourceDefinition(TagResourceReference resourceReference, Type definitionType)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);

            var resourceDefinitionDataOffset = GetResourceDefinitionDataOffset(tagResource);
            byte[] resourceDefinitionData = new byte[tagResource.ControlSize];
            Array.Copy(ResourceGestalt.NaiveResourceControlData, resourceDefinitionDataOffset, resourceDefinitionData, 0, resourceDefinitionData.Length);

            ApplyResourceDefinitionFixups(tagResource, resourceDefinitionData);

            byte[] primaryResourceData = GetPrimaryResource(resourceReference.Gen3ResourceID);
            byte[] secondaryResourceData = GetSecondaryResource(resourceReference.Gen3ResourceID);
            byte[] tertiaryResourceData = GetTertiaryResource(resourceReference.Gen3ResourceID);

            if (primaryResourceData == null && secondaryResourceData == null && (definitionType ==  typeof(BitmapTextureInteropResource) || definitionType == typeof(BitmapTextureInterleavedInteropResource)))
                return null;

            if (primaryResourceData == null)
                primaryResourceData = new byte[0];

            if (secondaryResourceData == null)
                secondaryResourceData = new byte[0];

            if (tertiaryResourceData == null)
                tertiaryResourceData = new byte[0];

            using (var definitionDataStream = new MemoryStream(resourceDefinitionData))
            using (var dataStream = new MemoryStream(primaryResourceData))
            using (var secondaryDataStream = new MemoryStream(secondaryResourceData))
            using (var tertiaryDataStream = new MemoryStream(tertiaryResourceData))
            using (var definitionDataReader = new EndianReader(definitionDataStream, EndianFormat.BigEndian))
            using (var dataReader = new EndianReader(dataStream, EndianFormat.BigEndian))
            using (var secondaryDataReader = new EndianReader(secondaryDataStream, EndianFormat.BigEndian))
            using (var tertiaryDataReader = new EndianReader(tertiaryDataStream, EndianFormat.BigEndian))
            {
                var context = new ResourceDefinitionSerializationContext(dataReader, secondaryDataReader, tertiaryDataReader, definitionDataReader, tagResource.RootFixup.Type);
                var deserializer = new ResourceDeserializer(Cache.Version, Cache.Platform);
                definitionDataReader.SeekTo(tagResource.RootFixup.Offset);
                return deserializer.Deserialize(context, definitionType);
            }
        }

        private byte[] ReadSegmentData(CacheFileResourceDataBlock resource, int pageIndex, int offset, int sizeIndex)
        {
            var page = ResourceLayoutTable.FilePages[pageIndex];
            var decompressed = ReadPageData(resource, page);

            int length;
            if (sizeIndex != -1)
                length = ResourceLayoutTable.StreamingSubpageTables[sizeIndex].TotalMemorySize;
            else
                length = decompressed.Length - offset;

            var data = new byte[length];
            Array.Copy(decompressed, offset, data, 0, length);
            return data;
        }

        private CacheFileResourceLayoutTable.CacheFileResourceStreamingSubpageTableBlock GetSubpageTable(DatumHandle ID, int type)
        {
            var resource = ResourceGestalt.Resources[ID.Index];

            if (resource.Page == -1)
                return null;

            var segment = ResourceLayoutTable.Sections[resource.Page];

            if (segment.SubpageTableIndexes[type].SubpageTableIndex == -1)
                return null;
            else
                return ResourceLayoutTable.StreamingSubpageTables[segment.SubpageTableIndexes[type].SubpageTableIndex];
        }

        private CacheFileResourceLayoutTable.CacheFileResourceStreamingSubpageTableBlock GetPrimarySubpageTable(DatumHandle ID)
        {
            return GetSubpageTable(ID, 0);
        }

        private CacheFileResourceLayoutTable.CacheFileResourceStreamingSubpageTableBlock GetSecondarySubpageTable(DatumHandle ID)
        {
            return GetSubpageTable(ID, 1);
        }

        private CacheFileResourceLayoutTable.CacheFileResourceStreamingSubpageTableBlock GetTertiarySubpageTable(DatumHandle ID)
        {
            return GetSubpageTable(ID, 2);
        }

        private byte[] GetResourceData(DatumHandle ID, int type)
        {
            var resource = ResourceGestalt.Resources[ID.Index];

            if (resource.Page == -1)
                return null;

            var segment = ResourceLayoutTable.Sections[resource.Page];

            if (segment.FilePageIndexes[type].PageIndex == -1 || segment.PageOffsets[type].Offset == -1)
                return null;

            if (ResourceLayoutTable.FilePages[segment.FilePageIndexes[type].PageIndex].Checksum.Checksum == -1)
                return null;

            return ReadSegmentData(resource, segment.FilePageIndexes[type].PageIndex, /*segment.PageOffsets[type].Offset*/ 0, segment.SubpageTableIndexes[type].SubpageTableIndex);
        }

        private byte[] GetPrimaryResource(DatumHandle ID)
        {
            return GetResourceData(ID, 0);
        }

        private byte[] GetSecondaryResource(DatumHandle ID)
        {
            return GetResourceData(ID, 1);
        }

        private byte[] GetTertiaryResource(DatumHandle ID)
        {
            return GetResourceData(ID, 2);
        }

        private byte[] ReadPageData(CacheFileResourceDataBlock resource, CacheFileResourceLayoutTable.CacheFileResourcePageStruct page)
        {
            string cacheFilePath;

            var cache = Cache;

            if (page.SharedFile != -1)
            {
                cacheFilePath = ResourceLayoutTable.SharedFiles[page.SharedFile].DvdRelativePath;
                var pathComponent = cacheFilePath.Split('\\');
                cacheFilePath = pathComponent[pathComponent.Length - 1];
                cacheFilePath = Path.Combine(Cache.Directory.FullName, cacheFilePath);

                if (cacheFilePath != Cache.CacheFile.FullName)
                {
                    if (Cache.SharedCacheFiles.ContainsKey(cacheFilePath))
                        cache = Cache.SharedCacheFiles[cacheFilePath];
                    else
                    {
                        var newCache = new FileInfo(cacheFilePath);
                        using (var newCacheStream = newCache.OpenRead())
                        using (var newCacheReader = new EndianReader(newCacheStream))
                        {
                            var newMapFile = new MapFile();
                            newMapFile.Read(newCacheReader);
                            cache = Cache.SharedCacheFiles[cacheFilePath] = new GameCacheGen4(newMapFile, newCache);
                        }
                    }
                }
            }

            var decompressed = new byte[page.UncompressedSize];
            
            using (var cacheStream = cache.OpenCacheRead())
            using (var reader = new EndianReader(cacheStream, EndianFormat.BigEndian))
            {
                var sectionTable = ((CacheFileHeaderGen4)cache.BaseMapFile.Header).SectionTable;
                var blockOffset = sectionTable.GetOffset(CacheFileSectionType.ResourceSection, (uint)page.FileOffset);
            
                reader.SeekTo(blockOffset);
                var compressed = reader.ReadBytes(page.CompressedSize);
            
                if (resource.ResourceTypeIndex != -1 && GetResourceTypeName(resource) == "sound_resource_definition")
                    return compressed;

                if (page.Codec == -1)
                {
                    return compressed;
                }
                else
                {
                    IntPtr decompressionContext = IntPtr.Zero;
                    int outSize = decompressed.Length;
                    int inSize = compressed.Length;
                    XCompress.XMemCreateDecompressionContext(XCompress.XMemCodecType.LZX, IntPtr.Zero, 0, ref decompressionContext);
                    XCompress.XMemResetDecompressionContext(decompressionContext);
                    XCompress.XMemDecompressStream(decompressionContext, decompressed, ref outSize, compressed, ref inSize);
                    XCompress.XMemDestroyDecompressionContext(decompressionContext);
                }
            }

            return decompressed;
        }
    }
}