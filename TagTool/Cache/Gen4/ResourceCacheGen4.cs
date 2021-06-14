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

namespace TagTool.Cache.Gen4
{
    public class ResourceCacheGen4 : ResourceCache
    {
        public bool isLoaded;
        public ResourceGestalt ResourceGestalt;
        public ResourceLayoutTable ResourceLayoutTable;
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
                    ResourceGestalt = Cache.Deserialize<ResourceGestalt>(cacheStream, Cache.TagCacheGen4.GlobalInstances["zone"]);
                    ResourceLayoutTable = Cache.Deserialize<ResourceLayoutTable>(cacheStream, Cache.TagCacheGen4.GlobalInstances["play"]);
                }
            }
            isLoaded = true;
        }

        public ResourceData GetTagResourceFromReference(TagResourceReference resourceReference)
        {
            if (!isLoaded)
                LoadResourceCache();

            if (resourceReference == null)
                return null;

            return ResourceGestalt.TagResources[resourceReference.Gen3ResourceID.Index];
        }

        public bool IsResourceValid(ResourceData tagResource)
        {
            if (tagResource == null || tagResource.ResourceTypeIndex == -1)
                return false;
            else
                return true;
        }

        public override BinkResource GetBinkResource(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public override BitmapTextureInteropResource GetBitmapTextureInteropResource(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (!IsResourceValid(tagResource) || GetResourceTypeName(tagResource) != "bitmap_texture_interop_resource")
                return null;
            return GetResourceDefinition<BitmapTextureInteropResource>(resourceReference);
        }

        public override BitmapTextureInterleavedInteropResource GetBitmapTextureInterleavedInteropResource(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (!IsResourceValid(tagResource) || GetResourceTypeName(tagResource) != "bitmap_texture_interleaved_interop_resource")
                return null;
            return GetResourceDefinition<BitmapTextureInterleavedInteropResource>(resourceReference);
        }

        public override RenderGeometryApiResourceDefinition GetRenderGeometryApiResourceDefinition(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (!IsResourceValid(tagResource) || GetResourceTypeName(tagResource) != "render_geometry_api_resource_definition")
                return null;
            return GetResourceDefinition<RenderGeometryApiResourceDefinition>(resourceReference);
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
            foreach(var subPage in primarySubPageTable.Subpages)
            {
                Array.Copy(primaryResourceData, primaryRunningOffset, data, subPage.Offset, subPage.Size);
                primaryRunningOffset += subPage.Size;
            }

            if(secondarySubPageTable != null && secondaryResourceData.Length > 0)
            {
                var secondaryRunningOffset = 0;
                foreach (var subPage in secondarySubPageTable.Subpages)
                {
                    Array.Copy(secondaryResourceData, secondaryRunningOffset, data, subPage.Offset, subPage.Size);
                    secondaryRunningOffset += subPage.Size;
                }
            }

            // does not exist in Gen4, create one.
            var resourceDef = new SoundResourceDefinition
            {
                Data = new TagData(data)
            };
            return resourceDef;
        }

        public override ModelAnimationTagResource GetModelAnimationTagResource(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (!IsResourceValid(tagResource) || GetResourceTypeName(tagResource) != "model_animation_tag_resource")
                return null;
            return GetResourceDefinition<ModelAnimationTagResource>(resourceReference);
        }

        public override StructureBspTagResources GetStructureBspTagResources(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (!IsResourceValid(tagResource) || GetResourceTypeName(tagResource) != "structure_bsp_tag_resources")
                return null;
            // extra step for bsp resources
            if (ResourceLayoutTable.Sections[tagResource.SegmentIndex].RequiredPageIndex == -1)
                return null;
            return GetResourceDefinition<StructureBspTagResources>(resourceReference);
        }

        public override StructureBspCacheFileTagResources GetStructureBspCacheFileTagResources(TagResourceReference resourceReference)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);
            if (!IsResourceValid(tagResource) || GetResourceTypeName(tagResource) != "structure_bsp_cache_file_tag_resources")
                return null;
            // extra step for bsp resources
            if (ResourceLayoutTable.Sections[tagResource.SegmentIndex].RequiredPageIndex == -1)
                return null;
            return GetResourceDefinition<StructureBspCacheFileTagResources>(resourceReference);
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

        private string GetResourceTypeName(ResourceData tagResource)
        {
            return Cache.StringTable.GetString(ResourceGestalt.ResourceDefinitions[tagResource.ResourceTypeIndex].Name);
        }

        private void ApplyResourceDefinitionFixups(ResourceData tagResource, byte[] resourceDefinitionData)
        {
            using (var resourceDefinitionStream = new MemoryStream(resourceDefinitionData))
            using (var fixupWriter = new EndianWriter(resourceDefinitionStream, EndianFormat.BigEndian))
            {
                for (int i = 0; i < tagResource.FixupLocations.Count; i++)
                {
                    var fixup = tagResource.FixupLocations[i];
                    fixupWriter.Seek((int)fixup.BlockOffset, SeekOrigin.Begin);
                    fixupWriter.Write(fixup.Address.Value);
                }
            }
        }

        public override object GetResourceDefinition(TagResourceReference resourceReference, Type definitionType)
        {
            var tagResource = GetTagResourceFromReference(resourceReference);

            byte[] resourceDefinitionData = new byte[tagResource.DefinitionDataLength];
            Array.Copy(ResourceGestalt.DefinitionData, tagResource.DefinitionDataOffset, resourceDefinitionData, 0, tagResource.DefinitionDataLength);

            ApplyResourceDefinitionFixups(tagResource, resourceDefinitionData);

            byte[] primaryResourceData = GetPrimaryResource(resourceReference.Gen3ResourceID);
            byte[] secondaryResourceData = GetSecondaryResource(resourceReference.Gen3ResourceID);

            if (primaryResourceData == null && secondaryResourceData == null && (definitionType ==  typeof(BitmapTextureInteropResource) || definitionType == typeof(BitmapTextureInterleavedInteropResource)))
                return null;

            if (primaryResourceData == null)
                primaryResourceData = new byte[0];

            if (secondaryResourceData == null)
                secondaryResourceData = new byte[0];

            using (var definitionDataStream = new MemoryStream(resourceDefinitionData))
            using (var dataStream = new MemoryStream(primaryResourceData))
            using (var secondaryDataStream = new MemoryStream(secondaryResourceData))
            using (var definitionDataReader = new EndianReader(definitionDataStream, EndianFormat.BigEndian))
            using (var dataReader = new EndianReader(dataStream, EndianFormat.BigEndian))
            using (var secondaryDataReader = new EndianReader(secondaryDataStream, EndianFormat.BigEndian))
            {
                var context = new ResourceDefinitionSerializationContext(dataReader, secondaryDataReader, definitionDataReader, tagResource.DefinitionAddress.Type);
                var deserializer = new ResourceDeserializer(Cache.Version);
                definitionDataReader.SeekTo(tagResource.DefinitionAddress.Offset);
                return deserializer.Deserialize(context, definitionType);
            }
        }

        private byte[] ReadSegmentData(ResourceData resource, int pageIndex, int offset, int sizeIndex)
        {
            var page = ResourceLayoutTable.Pages[pageIndex];
            var decompressed = ReadPageData(resource, page);

            int length;
            if (sizeIndex != -1)
                length = ResourceLayoutTable.SubpageTables[sizeIndex].TotalSize;
            else
                length = decompressed.Length - offset;

            var data = new byte[length];
            Array.Copy(decompressed, offset, data, 0, length);
            return data;
        }

        private ResourceSubpageTable GetPrimarySubpageTable(DatumHandle ID)
        {
            var resource = ResourceGestalt.TagResources[ID.Index];

            if (resource.SegmentIndex == -1)
                return null;

            var segment = ResourceLayoutTable.Sections[resource.SegmentIndex];

            if (segment.RequiredSizeIndex == -1)
                return null;
            else
                return ResourceLayoutTable.SubpageTables[segment.RequiredSizeIndex];
        }

        private ResourceSubpageTable GetSecondarySubpageTable(DatumHandle ID)
        {
            var resource = ResourceGestalt.TagResources[ID.Index];

            if (resource.SegmentIndex == -1)
                return null;

            var segment = ResourceLayoutTable.Sections[resource.SegmentIndex];

            if (segment.OptionalSizeIndex == -1)
                return null;
            else
                return ResourceLayoutTable.SubpageTables[segment.OptionalSizeIndex];
        }

        private byte[] GetPrimaryResource(DatumHandle ID)
        {
            var resource = ResourceGestalt.TagResources[ID.Index];

            if (resource.SegmentIndex == -1)
                return null;

            var segment = ResourceLayoutTable.Sections[resource.SegmentIndex];

            if (segment.RequiredPageIndex == -1 || segment.RequiredSegmentOffset == -1)
                return null;

            if (ResourceLayoutTable.Pages[segment.RequiredPageIndex].CrcChecksum == -1)
                return null;

            return ReadSegmentData(resource, segment.RequiredPageIndex, segment.RequiredSegmentOffset, segment.RequiredSizeIndex);
        }

        private byte[] GetSecondaryResource(DatumHandle ID)
        {
            var resource = ResourceGestalt.TagResources[ID.Index];

            if (resource.SegmentIndex == -1)
                return null;

            var segment = ResourceLayoutTable.Sections[resource.SegmentIndex];

            if (segment.OptionalPageIndex == -1 || segment.OptionalSegmentOffset == -1)
                return null;

            if (ResourceLayoutTable.Pages[segment.OptionalPageIndex].CrcChecksum == -1)
                return null;

            return ReadSegmentData(resource, segment.OptionalPageIndex, segment.OptionalSegmentOffset, segment.OptionalSizeIndex);
        }

        private byte[] ReadPageData(ResourceData resource, ResourcePage page)
        {
            string cacheFilePath;

            var cache = Cache;

            if (page.SharedCacheIndex != -1)
            {
                cacheFilePath = ResourceLayoutTable.SharedFiles[page.SharedCacheIndex].MapPath;
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

            var decompressed = new byte[page.UncompressedBlockSize];

            using (var cacheStream = cache.OpenCacheRead())
            using (var reader = new EndianReader(cacheStream, EndianFormat.BigEndian))
            {
                var sectionTable = ((CacheFileHeaderGen4)cache.BaseMapFile.Header).SectionTable;
                var blockOffset = sectionTable.GetOffset(CacheFileSectionType.ResourceSection, page.BlockAddress);

                reader.SeekTo(blockOffset);
                var compressed = reader.ReadBytes(BitConverter.ToInt32(BitConverter.GetBytes(page.CompressedBlockSize), 0));

                if (resource.ResourceTypeIndex != -1 && GetResourceTypeName(resource) == "sound_resource_definition")
                    return compressed;

                if (page.CompressionCodecIndex == -1)
                    reader.BaseStream.Read(decompressed, 0, BitConverter.ToInt32(BitConverter.GetBytes(page.UncompressedBlockSize), 0));
                else
                    using (var readerDeflate = new DeflateStream(new MemoryStream(compressed), CompressionMode.Decompress))
                        readerDeflate.Read(decompressed, 0, BitConverter.ToInt32(BitConverter.GetBytes(page.UncompressedBlockSize), 0));
            }

            return decompressed;
        }
    }
}