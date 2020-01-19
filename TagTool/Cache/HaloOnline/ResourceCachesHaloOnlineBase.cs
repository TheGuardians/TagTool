using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Resources;

namespace TagTool.Cache.HaloOnline
{
    // Wrapper for multiple ResourceCacheHaloOnline instances (one per .dat), implements the abstract class and refers to the individual class
    public abstract class ResourceCachesHaloOnlineBase : ResourceCache
    {
        public GameCacheHaloOnlineBase Cache;

        private Dictionary<ResourceLocation, LoadedResourceCache> LoadedResourceCaches { get; } = new Dictionary<ResourceLocation, LoadedResourceCache>();

        public PageableResource GetPageableResource(TagResourceReference resourceReference)
        {
            return resourceReference.HaloOnlinePageableResource;
        }

        public abstract LoadedResourceCache GetResourceCache(ResourceLocation location);

        private LoadedResourceCache GetResourceCache(PageableResource resource)
        {
            if (!resource.GetLocation(out var location))
                return null;

            return GetResourceCache(location);
        }

        /// <summary>
        /// Adds a new pageable_resource to the current cache.
        /// </summary>
        /// <param name="resource">The pageable_resource to add.</param>
        /// <param name="dataStream">The stream to read the resource data from.</param>
        /// <exception cref="System.ArgumentNullException">resource</exception>
        /// <exception cref="System.ArgumentException">The input stream is not open for reading;dataStream</exception>
        public void AddResource(PageableResource resource, Stream dataStream)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");
            if (!dataStream.CanRead)
                throw new ArgumentException("The input stream is not open for reading", "dataStream");

            var cache = GetResourceCache(resource);
            using (var stream = cache.File.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var dataSize = (int)(dataStream.Length - dataStream.Position);
                var data = new byte[dataSize];
                dataStream.Read(data, 0, dataSize);
                resource.Page.Index = cache.Cache.Add(stream, data, out uint compressedSize);
                resource.Page.CompressedBlockSize = compressedSize;
                resource.Page.UncompressedBlockSize = (uint)dataSize;
                resource.DisableChecksum();
            }
        }

        /// <summary>
        /// Adds raw, pre-compressed resource data to a cache.
        /// </summary>
        /// <param name="resource">The resource reference to initialize.</param>
        /// <param name="data">The pre-compressed data to store.</param>
        public void AddRawResource(PageableResource resource, byte[] data)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");

            resource.DisableChecksum();
            var cache = GetResourceCache(resource);
            using (var stream = cache.File.Open(FileMode.Open, FileAccess.ReadWrite))
                resource.Page.Index = cache.Cache.AddRaw(stream, data);
        }

        /// <summary>
        /// Extracts and decompresses the data for a resource from the current cache.
        /// </summary>
        /// <param name="pageable">The resource.</param>
        /// <param name="outStream">The stream to write the extracted data to.</param>
        /// <exception cref="System.ArgumentException">Thrown if the output stream is not open for writing.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the file containing the resource has not been loaded.</exception>
        public void ExtractResource(PageableResource pageable, Stream outStream)
        {
            if (pageable == null)
                throw new ArgumentNullException("resource");
            if (!outStream.CanWrite)
                throw new ArgumentException("The output stream is not open for writing", "outStream");

            var cache = GetResourceCache(pageable);
            using (var stream = cache.File.OpenRead())
                cache.Cache.Decompress(stream, pageable.Page.Index, pageable.Page.CompressedBlockSize, outStream);
        }

        /// <summary>
        /// Extracts and decompresses the data for a resource from the current cache.
        /// </summary>
        /// <param name="inStream"></param>
        /// <param name="pageable">The resource.</param>
        /// <param name="outStream">The stream to write the extracted data to.</param>
        /// <exception cref="System.ArgumentException">Thrown if the output stream is not open for writing.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the file containing the resource has not been loaded.</exception>
        public void ExtractResource(Stream inStream, PageableResource pageable, Stream outStream)
        {
            if (pageable == null)
                throw new ArgumentNullException("resource");
            if (!outStream.CanWrite)
                throw new ArgumentException("The output stream is not open for writing", "outStream");

            var cache = GetResourceCache(pageable);
            cache.Cache.Decompress(inStream, pageable.Page.Index, pageable.Page.CompressedBlockSize, outStream);
        }

        /// <summary>
        /// Extracts raw, compressed resource data.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns>The raw, compressed resource data.</returns>
        public byte[] ExtractRawResource(PageableResource resource)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");

            var cache = GetResourceCache(resource);
            using (var stream = cache.File.OpenRead())
                return cache.Cache.ExtractRaw(stream, resource.Page.Index, resource.Page.CompressedBlockSize);
        }

        /// <summary>
        /// Compresses and replaces the data for a resource.
        /// </summary>
        /// <param name="resource">The resource whose data should be replaced. On success, the reference will be adjusted to account for the new data.</param>
        /// <param name="dataStream">The stream to read the new data from.</param>
        /// <exception cref="System.ArgumentException">Thrown if the input stream is not open for reading.</exception>
        public void ReplaceResource(PageableResource resource, Stream dataStream)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");
            if (!dataStream.CanRead)
                throw new ArgumentException("The input stream is not open for reading", "dataStream");

            var cache = GetResourceCache(resource);
            using (var stream = cache.File.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var dataSize = (int)(dataStream.Length - dataStream.Position);
                var data = new byte[dataSize];
                dataStream.Read(data, 0, dataSize);
                var compressedSize = cache.Cache.Compress(stream, resource.Page.Index, data);
                resource.Page.CompressedBlockSize = compressedSize;
                resource.Page.UncompressedBlockSize = (uint)dataSize;
                resource.DisableChecksum();
            }
        }

        /// <summary>
        /// Replaces a resource with raw, pre-compressed data.
        /// </summary>
        /// <param name="resource">The resource whose data should be replaced. On success, the reference will be adjusted to account for the new data.</param>
        /// <param name="data">The raw, pre-compressed data to use.</param>
        public void ReplaceRawResource(PageableResource resource, byte[] data)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");

            resource.DisableChecksum();
            var cache = GetResourceCache(resource);
            using (var stream = cache.File.Open(FileMode.Open, FileAccess.ReadWrite))
                cache.Cache.ImportRaw(stream, resource.Page.Index, data);
        }

        public FileStream OpenResourceCacheRead(ResourceLocation location) => LoadedResourceCaches[location].File.OpenRead();

        public FileStream OpenResourceCacheWrite(ResourceLocation location) => LoadedResourceCaches[location].File.OpenWrite();

        public FileStream OpenResourceCacheReadWrite(ResourceLocation location) => LoadedResourceCaches[location].File.Open(FileMode.Open, FileAccess.ReadWrite);

        //
        // Overrides
        //

        public byte[] GetResourceData(TagResourceReference resourceReference)
        {
            var pageableResource = GetPageableResource(resourceReference);
            var cache = GetResourceCache(pageableResource);

            if (pageableResource.Page == null || pageableResource.Page.UncompressedBlockSize < 0)
                return null;

            byte[] result = new byte[pageableResource.Page.UncompressedBlockSize];
            using (var cacheStream = cache.File.OpenRead())
            using (var dataStream = new MemoryStream(result))
            {
                ExtractResource(cacheStream, pageableResource, dataStream);
            }
            return result;
        }

        public bool IsResourceReferenceValid(TagResourceReference resourceReference)
        {
            if (resourceReference == null || resourceReference.HaloOnlinePageableResource == null)
                return false;
            var page = resourceReference.HaloOnlinePageableResource.Page;
            var resource = resourceReference.HaloOnlinePageableResource.Resource;
            if (page == null || resource == null)
                return false;

            return true;
        }

        private void ApplyResourceDefinitionFixups(TagResourceGen3 tagResource, byte[] resourceDefinitionData)
        {
            using (var resourceDefinitionStream = new MemoryStream(resourceDefinitionData))
            using (var fixupWriter = new EndianWriter(resourceDefinitionStream, EndianFormat.LittleEndian))
            {
                for (int i = 0; i < tagResource.ResourceFixups.Count; i++)
                {
                    var fixup = tagResource.ResourceFixups[i];
                    fixupWriter.Seek((int)fixup.BlockOffset, SeekOrigin.Begin);
                    fixupWriter.Write(fixup.Address.Value);
                }
            }
        }

        private T GetResourceDefinition<T>(TagResourceReference resourceReference)
        {
            var tagResource = GetPageableResource(resourceReference).Resource;

            T result;
            byte[] resourceDefinitionData = tagResource.DefinitionData;
            ApplyResourceDefinitionFixups(tagResource, resourceDefinitionData);

            byte[] resourceRawData = GetResourceData(resourceReference);
            if (resourceRawData == null)
                resourceRawData = new byte[0];

            // deserialize the resource definition again
            using (var definitionDataStream = new MemoryStream(resourceDefinitionData))
            using (var definitionDataReader = new EndianReader(definitionDataStream, EndianFormat.LittleEndian))
            using (var dataStream = new MemoryStream(resourceRawData))
            using (var dataReader = new EndianReader(dataStream, EndianFormat.LittleEndian))
            {
                var context = new ResourceDefinitionSerializationContext(dataReader, definitionDataReader, tagResource.DefinitionAddress.Type);
                var deserializer = new ResourceDeserializer(Cache.Version);
                // deserialize without access to the data
                definitionDataReader.SeekTo(tagResource.DefinitionAddress.Offset);
                result = deserializer.Deserialize<T>(context);
            }
            return result;
        }

        public override BinkResource GetBinkResource(TagResourceReference resourceReference)
        {
            if (!IsResourceReferenceValid(resourceReference))
                return null;
            var resource = GetPageableResource(resourceReference).Resource;
            if (resource.ResourceType != TagResourceTypeGen3.Bink)
                return null;
            return GetResourceDefinition<BinkResource>(resourceReference);
        }

        public override BitmapTextureInteropResource GetBitmapTextureInteropResource(TagResourceReference resourceReference)
        {
            if (!IsResourceReferenceValid(resourceReference))
                return null;
            var resource = GetPageableResource(resourceReference).Resource;
            if (resource.ResourceType != TagResourceTypeGen3.Bitmap)
                return null;
            return GetResourceDefinition<BitmapTextureInteropResource>(resourceReference);
        }

        public override BitmapTextureInterleavedInteropResource GetBitmapTextureInterleavedInteropResource(TagResourceReference resourceReference)
        {
            if (!IsResourceReferenceValid(resourceReference))
                return null;
            var resource = GetPageableResource(resourceReference).Resource;
            if (resource.ResourceType != TagResourceTypeGen3.BitmapInterleaved)
                return null;
            return GetResourceDefinition<BitmapTextureInterleavedInteropResource>(resourceReference);
        }

        public override RenderGeometryApiResourceDefinition GetRenderGeometryApiResourceDefinition(TagResourceReference resourceReference)
        {
            if (!IsResourceReferenceValid(resourceReference))
                return null;
            var resource = GetPageableResource(resourceReference).Resource;
            if (resource.ResourceType != TagResourceTypeGen3.RenderGeometry)
                return null;
            return GetResourceDefinition<RenderGeometryApiResourceDefinition>(resourceReference);
        }

        public override ModelAnimationTagResource GetModelAnimationTagResource(TagResourceReference resourceReference)
        {
            if (!IsResourceReferenceValid(resourceReference))
                return null;
            var resource = GetPageableResource(resourceReference).Resource;
            if (resource.ResourceType != TagResourceTypeGen3.Animation)
                return null;
            return GetResourceDefinition<ModelAnimationTagResource>(resourceReference);
        }

        public override SoundResourceDefinition GetSoundResourceDefinition(TagResourceReference resourceReference)
        {
            if (!IsResourceReferenceValid(resourceReference))
                return null;
            var resource = GetPageableResource(resourceReference).Resource;
            if (resource.ResourceType != TagResourceTypeGen3.Sound)
                return null;
            return GetResourceDefinition<SoundResourceDefinition>(resourceReference);
        }

        private TagResourceReference CreateResource<T>(T resourceDefinition, ResourceLocation location, TagResourceTypeGen3 resourceType)
        {
            var resourceReference = new TagResourceReference();
            var pageableResource = new PageableResource();

            pageableResource.Page = new RawPage();
            pageableResource.Resource = new TagResourceGen3();
            pageableResource.ChangeLocation(location);
            pageableResource.Resource.Unknown2 = 1;
            pageableResource.Resource.ResourceType = resourceType;

            resourceReference.HaloOnlinePageableResource = pageableResource;

            var definitionStream = new MemoryStream();
            var dataStream = new MemoryStream();

            using (var definitionWriter = new EndianWriter(definitionStream, EndianFormat.LittleEndian))
            using (var dataWriter = new EndianWriter(dataStream, EndianFormat.LittleEndian))
            {
                var context = new ResourceDefinitionSerializationContext(dataWriter, definitionWriter, CacheAddressType.Definition);
                var serializer = new ResourceSerializer(Cache.Version);
                serializer.Serialize(context, resourceDefinition);

                var data = dataStream.ToArray();
                var definitionData = definitionStream.ToArray();
                dataStream.Position = 0;

                pageableResource.DisableChecksum();

                dataStream.Position = 0;
                AddResource(pageableResource, dataStream);

                // add resource definition and fixups
                pageableResource.Resource.DefinitionData = definitionData;
                pageableResource.Resource.ResourceFixups = context.ResourceFixups;
                pageableResource.Resource.DefinitionAddress = context.MainStructOffset;
                pageableResource.Resource.D3DFixups = context.D3DFixups;
            }
            return resourceReference;
        }

        public override TagResourceReference CreateSoundResource(SoundResourceDefinition soundResourceDefinition)
        {
            return CreateResource(soundResourceDefinition, ResourceLocation.Audio, TagResourceTypeGen3.Sound);
        }

        public override TagResourceReference CreateBitmapResource(BitmapTextureInteropResource bitmapResourceDefinition)
        {
            return CreateResource(bitmapResourceDefinition, ResourceLocation.Textures, TagResourceTypeGen3.Bitmap);
        }

        public override TagResourceReference CreateBitmapInterleavedResource(BitmapTextureInterleavedInteropResource bitmapResourceDefinition)
        {
            return CreateResource(bitmapResourceDefinition, ResourceLocation.Textures, TagResourceTypeGen3.BitmapInterleaved);
        }

        public override TagResourceReference CreateBinkResource(BinkResource binkResourceDefinition)
        {
            return CreateResource(binkResourceDefinition, ResourceLocation.Resources, TagResourceTypeGen3.Bink);
        }

        public override TagResourceReference CreateRenderGeometryApiResource(RenderGeometryApiResourceDefinition renderGeometryDefinition)
        {
            return CreateResource(renderGeometryDefinition, ResourceLocation.Resources, TagResourceTypeGen3.RenderGeometry);
        }

        public override TagResourceReference CreateModelAnimationGraphResource(ModelAnimationTagResource modelAnimationGraphDefinition)
        {
            return CreateResource(modelAnimationGraphDefinition, ResourceLocation.Resources, TagResourceTypeGen3.Animation);
        }

        public override TagResourceReference CreateStructureBspResource(StructureBspTagResources sbspResource)
        {
            return CreateResource(sbspResource, ResourceLocation.Resources, TagResourceTypeGen3.Collision);
        }

        public override TagResourceReference CreateStructureBspCacheFileResource(StructureBspCacheFileTagResources sbspCacheFileResource)
        {
            return CreateResource(sbspCacheFileResource, ResourceLocation.Resources, TagResourceTypeGen3.Pathfinding);
        }

        public override StructureBspTagResources GetStructureBspTagResources(TagResourceReference resourceReference)
        {
            if (!IsResourceReferenceValid(resourceReference))
                return null;
            var resource = GetPageableResource(resourceReference).Resource;
            if (resource.ResourceType != TagResourceTypeGen3.Collision)
                return null;
            return GetResourceDefinition<StructureBspTagResources>(resourceReference);
        }

        public override StructureBspCacheFileTagResources GetStructureBspCacheFileTagResources(TagResourceReference resourceReference)
        {
            if (!IsResourceReferenceValid(resourceReference))
                return null;
            var resource = GetPageableResource(resourceReference).Resource;
            if (resource.ResourceType != TagResourceTypeGen3.Pathfinding)
                return null;
            return GetResourceDefinition<StructureBspCacheFileTagResources>(resourceReference);
        }

        //
        // Utilities
        //

        public class LoadedResourceCache
        {
            public ResourceCacheHaloOnline Cache { get; set; }
            public FileInfo File { get; set; }
        }
    }
}
