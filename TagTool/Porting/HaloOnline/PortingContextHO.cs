using System.Collections.Generic;
using System.IO;
using TagTool.Animations;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Porting.HaloOnline
{
    public partial class PortingContextHO : PortingContext
    {
        private readonly record struct ResourceDesc(ResourceLocation Location, int Index);
        private readonly Dictionary<ResourceDesc, ResourceDesc> ConvertedResources = [];

        private GameCacheHaloOnlineBase BlamCacheHO => (GameCacheHaloOnlineBase)BlamCache;

        public PortingContextHO(GameCacheHaloOnlineBase cacheContext, GameCache blamCache) : base(cacheContext, blamCache)
        {
        }

        protected override object ConvertDefinition(Stream cacheStream, Stream blamCacheStream, CachedTag blamTag, CachedTag edTag, object blamDefinition)
        {
            blamDefinition = ConvertData(cacheStream, blamCacheStream, blamDefinition, blamDefinition, blamTag.Name);

            switch (blamDefinition)
            {
                case Scenario scnr:
                    blamDefinition = ConvertScenario(cacheStream, blamCacheStream, scnr);
                    break;

                case ModelAnimationGraph jmad:
                    AnimationSorter.Sort(jmad);
                    break;
            }

            return blamDefinition;
        }

        public override object ConvertData(Stream cacheStream, Stream blamCacheStream, object data, object definition, string blamTagName)
        {
            switch (data)
            {
                case PageableResource pageable:
                    return ConvertPageableResource(pageable);
            }

            return base.ConvertData(cacheStream, blamCacheStream, data, definition, blamTagName);
        }

        private PageableResource ConvertPageableResource(PageableResource pageable)
        {
            if (pageable == null || pageable.Page.Index < 0 || pageable.Page.CompressedBlockSize == 0)
                return pageable;

            ResourceLocation location = pageable.GetLocation();
            ResourceDesc key = new(location, pageable.Page.Index);

            if (ConvertedResources.TryGetValue(key, out ResourceDesc resourceDesc))
            {
                // Resource has already been added, take a reference to it 
                pageable.Page.Index = resourceDesc.Index;
                pageable.ChangeLocation(resourceDesc.Location);
            }
            else
            {
                byte[] data = BlamCacheHO.ResourceCaches.ExtractRawResource(pageable);
                ResourceLocation newLocation = GetPreferredResourceLocation(pageable.Resource.ResourceType);
                pageable.ChangeLocation(newLocation);
                CacheContext.ResourceCaches.AddRawResource(pageable, data);
                ConvertedResources.Add(key, new ResourceDesc(newLocation, pageable.Page.Index));
            }

            return pageable;
        }

        private ResourceLocation GetPreferredResourceLocation(TagResourceTypeGen3 resourceType)
        {
            if (CacheContext is GameCacheModPackage)
                return ResourceLocation.Mods;

            switch (resourceType)
            {
                case TagResourceTypeGen3.Bitmap:
                    return ResourceLocation.Textures;
                case TagResourceTypeGen3.BitmapInterleaved:
                    return ResourceLocation.TexturesB;
                case TagResourceTypeGen3.RenderGeometry:
                    return ResourceLocation.Resources;
                case TagResourceTypeGen3.Sound:
                    return ResourceLocation.Audio;
                default:
                    return ResourceCachesHaloOnline.GetOverflowResourceLocation(CacheContext.Version);
            }
        }
    }
}
