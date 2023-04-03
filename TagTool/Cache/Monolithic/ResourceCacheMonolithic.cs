using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache.Resources;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Resources;

namespace TagTool.Cache.Monolithic
{
    public class ResourceCacheMonolithic : ResourceCache
    {
        public MonolithicTagFileBackend Backend;
        public ResourceDeserializer Deserializer;

        public ResourceCacheMonolithic(GameCacheMonolithic cache)
        {
            Backend = cache.Backend;
            Deserializer = new ResourceDeserializer(cache.Version, cache.Platform);
        }

        public override TagResourceReference CreateBinkResource(BinkResource binkResourceDefinition)
            => throw new NotImplementedException();

        public override TagResourceReference CreateBitmapInterleavedResource(BitmapTextureInterleavedInteropResource bitmapResourceDefinition)
            => throw new NotImplementedException();

        public override TagResourceReference CreateBitmapResource(BitmapTextureInteropResource bitmapResourceDefinition)
            => throw new NotImplementedException();

        public override TagResourceReference CreateModelAnimationGraphResource(ModelAnimationTagResource modelAnimationGraphDefinition)
            => throw new NotImplementedException();

        public override TagResourceReference CreateRenderGeometryApiResource(RenderGeometryApiResourceDefinition renderGeometryDefinition)
            => throw new NotImplementedException();

        public override TagResourceReference CreateSoundResource(SoundResourceDefinition soundResourceDefinition)
            => throw new NotImplementedException();

        public override TagResourceReference CreateStructureBspCacheFileResource(StructureBspCacheFileTagResources sbspCacheFileResource)
            => throw new NotImplementedException();

        public override TagResourceReference CreateStructureBspResource(StructureBspTagResources sbspResource)
            => throw new NotImplementedException();

        public override BinkResource GetBinkResource(TagResourceReference resourceReference)
            => GetTagResource<BinkResource>(resourceReference);

        public override BitmapTextureInterleavedInteropResource GetBitmapTextureInterleavedInteropResource(TagResourceReference resourceReference)
            => GetTagResource<BitmapTextureInterleavedInteropResource>(resourceReference);

        public override BitmapTextureInteropResource GetBitmapTextureInteropResource(TagResourceReference resourceReference)
            => GetTagResource<BitmapTextureInteropResource>(resourceReference);

        public override ModelAnimationTagResource GetModelAnimationTagResource(TagResourceReference resourceReference)
            => GetTagResource<ModelAnimationTagResource>(resourceReference);

        public override RenderGeometryApiResourceDefinition GetRenderGeometryApiResourceDefinition(TagResourceReference resourceReference)
            => GetTagResource<RenderGeometryApiResourceDefinition>(resourceReference);

        public override SoundResourceDefinition GetSoundResourceDefinition(TagResourceReference resourceReference)
            => GetTagResource<SoundResourceDefinition>(resourceReference);

        public override StructureBspCacheFileTagResources GetStructureBspCacheFileTagResources(TagResourceReference resourceReference)
            => GetTagResource<StructureBspCacheFileTagResources>(resourceReference);

        public override StructureBspTagResources GetStructureBspTagResources(TagResourceReference resourceReference)
            => GetTagResource<StructureBspTagResources>(resourceReference);

        public override Tags.Resources.Gen4.BitmapTextureInteropResource GetBitmapTextureInteropResourceGen4(TagResourceReference resourceReference)
            => GetTagResource<Tags.Resources.Gen4.BitmapTextureInteropResource>(resourceReference);
        public override Tags.Resources.Gen4.ModelAnimationTagResource GetModelAnimationTagResourceGen4(TagResourceReference resourceReference)
            => GetTagResource<Tags.Resources.Gen4.ModelAnimationTagResource>(resourceReference);
        public override Tags.Resources.Gen4.CollisionModelResource GetCollisionModelResourceGen4(TagResourceReference resourceReference)
            => GetTagResource<Tags.Resources.Gen4.CollisionModelResource>(resourceReference);

        public override object GetResourceDefinition(TagResourceReference resourceReference, Type definitionType)
            => GetTagResource(resourceReference, definitionType);

        public T GetTagResource<T>(TagResourceReference resourceReference)
            => (T)GetTagResource(resourceReference, typeof(T));

        public object GetTagResource(TagResourceReference resourceReference, Type definitionType)
        {
            var xsyncState = resourceReference.XSyncState;
            if (xsyncState == null)
                return null;

            var entryIndex = Backend.TagFileIndex.Index.FindFileEntry(xsyncState.ResourceOwner);
            if (entryIndex == -1)
                return null;

            if (!Backend.TagFileIndex.GetCacheFilePartitionBlock(entryIndex, out PartitionBlock partitionBlock))
                return null;

            byte[] primaryData, secondaryData;
            ExtractResourceRaw(xsyncState, partitionBlock, out primaryData, out secondaryData);

            var definitionData = xsyncState.ControlData.ToArray();
            ApplyResourceDefinitionFixups(xsyncState.ControlFixups, definitionData);

            var definitionReader = new EndianReader(new MemoryStream(definitionData), Backend.Format);
            var primaryDataReader = new EndianReader(new MemoryStream(primaryData), Backend.Format);
            var secondaryDataReader = new EndianReader(new MemoryStream(secondaryData), Backend.Format);

            var dataContext = new ResourceDefinitionSerializationContext(primaryDataReader, secondaryDataReader, definitionReader, xsyncState.Header.RootAddress.Type);
            definitionReader.SeekTo(xsyncState.Header.RootAddress.Offset);
            return Deserializer.Deserialize(dataContext, definitionType);
        }

        public void ExtractResourceRaw(TagResourceXSyncState xsyncState, PartitionBlock partitionBlock, out byte[] primaryData, out byte[] secondaryData)
        {
            var cacheFile = Backend.GetCachePartitionFile(partitionBlock.PartitionIndex);

            primaryData = new byte[0];
            secondaryData = new byte[0];
            using (var reader = new EndianReader(cacheFile.OpenRead(), Backend.Format))
            {
                reader.SeekTo(partitionBlock.Offset + xsyncState.Header.CacheLocationOffset);
                primaryData = reader.ReadBytes((int)xsyncState.Header.CacheLocationSize);
                if (xsyncState.Header.OptionalLocationSize > 0)
                {
                    reader.SeekTo(partitionBlock.Offset + xsyncState.Header.OptionalLocationOffset);
                    secondaryData = reader.ReadBytes((int)xsyncState.Header.OptionalLocationSize);
                }
            }
        }

        private void ApplyResourceDefinitionFixups(List<ResourceFixupLocation> fixups, byte[] resourceDefinitionData)
        {
            using (var fixupWriter = new EndianWriter(new MemoryStream(resourceDefinitionData), Backend.Format))
            {
                for (int i = 0; i < fixups.Count; i++)
                {
                    var fixup = fixups[i];
                    fixupWriter.Seek((int)fixup.BlockOffset, SeekOrigin.Begin);
                    fixupWriter.Write(fixup.Address.Value);
                }
            }
        }
    }
}
