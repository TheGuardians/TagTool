using System;
using System.IO;
using TagTool.Cache.Resources;
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
        public TagDeserializer Deserializer;
        public TagSerializer Serializer;

        public PageableResource GetPageableResource(TagResourceReference resourceReference)
        {
            return resourceReference.HaloOnlinePageableResource;
        }

        public abstract ResourceCacheHaloOnline GetResourceCache(ResourceLocation location);

        public abstract Stream OpenCacheRead(ResourceLocation location);
        public abstract Stream OpenCacheReadWrite(ResourceLocation location);
        public abstract Stream OpenCacheWrite(ResourceLocation location);

        private ResourceCacheHaloOnline GetResourceCache(PageableResource resource, out ResourceLocation location)
        {
            if (!resource.GetLocation(out location))
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
        public virtual void AddResource(PageableResource resource, Stream dataStream)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");
            if (!dataStream.CanRead)
                throw new ArgumentException("The input stream is not open for reading", "dataStream");

            var cache = GetResourceCache(resource, out var location);
            using (var stream = OpenCacheReadWrite(location))
            {
                var dataSize = (int)(dataStream.Length - dataStream.Position);
                var data = new byte[dataSize];
                dataStream.Read(data, 0, dataSize);
                resource.Page.Index = cache.Add(stream, data, out uint compressedSize);
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
            var cache = GetResourceCache(resource, out var location);
            using (var stream = OpenCacheReadWrite(location))
                resource.Page.Index = cache.AddRaw(stream, data);
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

            var cache = GetResourceCache(pageable, out var location);
            using (var stream = OpenCacheRead(location))
                cache.Decompress(stream, pageable.Page.Index, pageable.Page.CompressedBlockSize, outStream);
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

            var cache = GetResourceCache(pageable, out var location);
            cache.Decompress(inStream, pageable.Page.Index, pageable.Page.CompressedBlockSize, outStream);
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

            var cache = GetResourceCache(resource, out var location);
            using (var stream = OpenCacheRead(location))
                return cache.ExtractRaw(stream, resource.Page.Index, resource.Page.CompressedBlockSize);
        }


        /// <summary>
        /// Compresses and replaces the data for a resource.
        /// </summary>
        /// <param name="resource">The resource whose data should be replaced. On success, the reference will be adjusted to account for the new data.</param>
        /// <param name="dataStream">The stream to read the new data from.</param>
        /// <exception cref="System.ArgumentException">Thrown if the input stream is not open for reading.</exception>
        public virtual void ReplaceResource(PageableResource resource, Stream dataStream)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");
            if (!dataStream.CanRead)
                throw new ArgumentException("The input stream is not open for reading", "dataStream");

            var cache = GetResourceCache(resource, out var location);
            using (var stream = OpenCacheReadWrite(location))
            {
                var dataSize = (int)(dataStream.Length - dataStream.Position);
                var data = new byte[dataSize];
                dataStream.Read(data, 0, dataSize);
                resource.Page.Index = cache.Add(stream, data, out uint compressedSize);
                resource.Page.CompressedBlockSize = compressedSize;
                resource.Page.UncompressedBlockSize = (uint)dataSize;
                resource.DisableChecksum();
            }
        }
        
        public void ReplaceResource(PageableResource resource, object resourceDefinition)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");

            var definitionStream = new MemoryStream();
            var dataStream = new MemoryStream();

            using (var definitionWriter = new EndianWriter(definitionStream, EndianFormat.LittleEndian))
            using (var dataWriter = new EndianWriter(dataStream, EndianFormat.LittleEndian))
            {
                var context = new ResourceDefinitionSerializationContext(dataWriter, definitionWriter, CacheAddressType.Definition);
                Serializer.Serialize(context, resourceDefinition);

                var data = dataStream.ToArray();
                var definitionData = definitionStream.ToArray();
                dataStream.Position = 0;

                resource.DisableChecksum();

                dataStream.Position = 0;
                ReplaceResource(resource, dataStream);

                // add resource definition and fixups
                resource.Resource.DefinitionData = definitionData;
                resource.Resource.FixupLocations = context.FixupLocations;
                resource.Resource.DefinitionAddress = context.MainStructOffset;
                resource.Resource.InteropLocations = context.InteropLocations;
            }
        }

        public void ReplaceResource(TagResourceReference resource, object resourceDefinition)
        {
            ReplaceResource(resource.HaloOnlinePageableResource, resourceDefinition);
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
            var cache = GetResourceCache(resource, out var location);
            using (var stream = OpenCacheReadWrite(location))
                cache.ImportRaw(stream, resource.Page.Index, data);
        }

        //
        // Overrides
        //

        public byte[] GetResourceData(TagResourceReference resourceReference)
        {
            var pageableResource = GetPageableResource(resourceReference);
            var cache = GetResourceCache(pageableResource, out var location);

            if (pageableResource.Page == null || pageableResource.Page.UncompressedBlockSize < 0)
                return null;

            byte[] result = new byte[pageableResource.Page.UncompressedBlockSize];
            using (var cacheStream = OpenCacheRead(location))
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

        private void ApplyResourceDefinitionFixups(ResourceData tagResource, byte[] resourceDefinitionData)
        {
            using (var resourceDefinitionStream = new MemoryStream(resourceDefinitionData))
            using (var fixupWriter = new EndianWriter(resourceDefinitionStream, EndianFormat.LittleEndian))
            {
                for (int i = 0; i < tagResource.FixupLocations.Count; i++)
                {
                    var fixup = tagResource.FixupLocations[i];
                    fixupWriter.Seek((int)fixup.BlockOffset, SeekOrigin.Begin);
                    fixupWriter.Write(fixup.Address.Value);
                }
            }
        }

        private new T GetResourceDefinition<T>(TagResourceReference resourceReference)
        {
            return (T)GetResourceDefinition(resourceReference, typeof(T));
        }

        public override object GetResourceDefinition(TagResourceReference resourceReference, Type definitionType)
        {
            var tagResource = GetPageableResource(resourceReference).Resource;

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
                // deserialize without access to the data
                definitionDataReader.SeekTo(tagResource.DefinitionAddress.Offset);
                return Deserializer.Deserialize(context, definitionType);
            }
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
        public override Tags.Resources.Gen4.BitmapTextureInteropResource GetBitmapTextureInteropResourceGen4(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }
        public override Tags.Resources.Gen4.ModelAnimationTagResource GetModelAnimationTagResourceGen4(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }
        public override Tags.Resources.Gen4.CollisionModelResource GetCollisionModelResourceGen4(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }
        public override Tags.Resources.Gen4.RenderGeometryApiResourceDefinition GetRenderGeometryApiResourceDefinitionGen4(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }
        public override Tags.Resources.Gen4.StructureBspTagResources GetStructureBspTagResourcesGen4(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }
        public override Tags.Resources.Gen4.StructureBspCacheFileTagResources GetStructureBspCacheFileTagResourcesGen4(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        private TagResourceReference CreateResource<T>(T resourceDefinition, ResourceLocation location, TagResourceTypeGen3 resourceType)
        {
            var resourceReference = new TagResourceReference();
            var pageableResource = new PageableResource();

            pageableResource.Page = new ResourcePage();
            pageableResource.Resource = new ResourceData();
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
                Serializer.Serialize(context, resourceDefinition);

                var data = dataStream.ToArray();
                var definitionData = definitionStream.ToArray();
                dataStream.Position = 0;

                pageableResource.DisableChecksum();

                dataStream.Position = 0;
                AddResource(pageableResource, dataStream);

                // add resource definition and fixups
                pageableResource.Resource.DefinitionData = definitionData;
                pageableResource.Resource.FixupLocations = context.FixupLocations;
                pageableResource.Resource.DefinitionAddress = context.MainStructOffset;
                pageableResource.Resource.InteropLocations = context.InteropLocations;
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
