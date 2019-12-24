using System;
using TagTool.Cache;
using TagTool.Tags;
using TagTool.Tags.Resources;

namespace TagTool.Common
{
    /// <summary>
    /// A reference to a resource used by a tag.
    /// This is treated by the serialization system as a special type of tag element.
    /// </summary>
    [TagStructure(Size = 0x40, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x6C, MaxVersion = CacheVersion.HaloOnline106708)]
    [TagStructure(Size = 0x70, MinVersion = CacheVersion.HaloOnline235640, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x40, MinVersion = CacheVersion.HaloReach)]
    public class PageableResource : TagStructure
	{
        /// <summary>
        /// The <see cref="RawPage"/> of the <see cref="PageableResource"/>.
        /// </summary>
        public RawPage Page = new RawPage
        {
            Index = -1
        };

        /// <summary>
        /// The <see cref="TagResourceGen3"/> of the <see cref="PageableResource"/>.
        /// </summary>
        public TagResourceGen3 Resource = new TagResourceGen3
        {
            ResourceType = TagResourceTypeGen3.None,
            ResourceFixups = new System.Collections.Generic.List<TagResourceGen3.ResourceFixup>(),
            ResourceDefinitionFixups = new System.Collections.Generic.List<TagResourceGen3.ResourceDefinitionFixup>()
        };

        public PageableResource() { }

        public PageableResource(TagResourceTypeGen3 type, CacheVersion version) :
            this()
        {
            uint? size = null;
            switch (type)
            {
                case TagResourceTypeGen3.Collision:
                    size = typeof(StructureBspTagResources).GetSize(version);
                    break;

                case TagResourceTypeGen3.Bitmap:
                    size = typeof(BitmapTextureInteropResource).GetSize(version);
                    break;

                case TagResourceTypeGen3.BitmapInterleaved:
                    size = typeof(BitmapTextureInterleavedInteropResource).GetSize(version);
                    break;
                case TagResourceTypeGen3.Sound:
                    size = typeof(SoundResourceDefinition).GetSize(version);
                    break;

                case TagResourceTypeGen3.Animation:
                    size = typeof(ModelAnimationTagResource).GetSize(version);
                    break;

                case TagResourceTypeGen3.RenderGeometry:
                    size = typeof(RenderGeometryApiResourceDefinition).GetSize(version);
                    break;

                case TagResourceTypeGen3.Bink:
                    size = typeof(BinkResource).GetSize(version);
                    break;

                case TagResourceTypeGen3.Pathfinding:
                    size = typeof(StructureBspCacheFileTagResources).GetSize(version);
                    break;
            }

            Resource.DefinitionData = new byte[size ?? 0];
            Resource.ResourceType = type;
        }

        public PageableResource(TagResourceTypeGen3 type, CacheVersion version, ResourceLocation location) :
            this(type, version)
        {
            ChangeLocation(location);
        }

        /// <summary>
        /// Gets the definition type of the pageable_resource.
        /// </summary>
        /// <returns>The definition type of the pageable_resource, if it is of a valid type.</returns>
        public Type GetDefinitionType()
        {
            switch (Resource.ResourceType)
            {
                case TagResourceTypeGen3.Animation:
                    return typeof(ModelAnimationTagResource);

                case TagResourceTypeGen3.Bink:
                    return typeof(BinkResource);

                case TagResourceTypeGen3.Bitmap:
                    return typeof(BitmapTextureInteropResource);

                case TagResourceTypeGen3.BitmapInterleaved:
                    return typeof(BitmapTextureInterleavedInteropResource);

                case TagResourceTypeGen3.Collision:
                    return typeof(StructureBspTagResources);

                case TagResourceTypeGen3.Pathfinding:
                    return typeof(StructureBspCacheFileTagResources);

                case TagResourceTypeGen3.RenderGeometry:
                    return typeof(RenderGeometryApiResourceDefinition);

                case TagResourceTypeGen3.Sound:
                    return typeof(SoundResourceDefinition);

                default:
                    throw new TypeLoadException(Resource.ResourceType.ToString());
            }
        }

        /// <summary>
        /// Gets the location of the resource by checking its location flags.
        /// </summary>
        /// <returns>The resource's location.</returns>
        /// <exception cref="InvalidOperationException">The resource does not have a location flag set</exception>
        public bool GetLocation(out ResourceLocation location)
        {
            if (Page.OldFlags != 0)
            {
                if (Page.OldFlags.HasFlag(OldRawPageFlags.InResources))
                {
                    location = ResourceLocation.Resources;
                    return true;
                }
                if (Page.OldFlags.HasFlag(OldRawPageFlags.InTextures))
                {
                    location = ResourceLocation.Textures;
                    return true;
                }
                if (Page.OldFlags.HasFlag(OldRawPageFlags.InTexturesB))
                {
                    location = ResourceLocation.TexturesB;
                    return true;
                }
                if (Page.OldFlags.HasFlag(OldRawPageFlags.InAudio))
                {
                    location = ResourceLocation.Audio;
                    return true;
                }
                if (Page.OldFlags.HasFlag(OldRawPageFlags.InResourcesB))
                {
                    location = ResourceLocation.ResourcesB;
                    return true;
                }
            }
            else if (Page.NewFlags != 0)
            {
                // FIXME: haxhaxhax, maybe we should just have separate types for the old and new reference layouts?
                if (Page.NewFlags.HasFlag(NewRawPageFlags.InResources))
                {
                    location = ResourceLocation.Resources;
                    return true;
                }
                if (Page.NewFlags.HasFlag(NewRawPageFlags.InTextures))
                {
                    location = ResourceLocation.Textures;
                    return true;
                }
                if (Page.NewFlags.HasFlag(NewRawPageFlags.InTexturesB))
                {
                    location = ResourceLocation.TexturesB;
                    return true;
                }
                if (Page.NewFlags.HasFlag(NewRawPageFlags.InAudio))
                {
                    location = ResourceLocation.Audio;
                    return true;
                }
                if (Page.NewFlags.HasFlag(NewRawPageFlags.InResourcesB))
                {
                    location = ResourceLocation.ResourcesB;
                    return true;
                }
                if (Page.NewFlags.HasFlag(NewRawPageFlags.InRenderModels))
                {
                    location = ResourceLocation.RenderModels;
                    return true;
                }
                if (Page.NewFlags.HasFlag(NewRawPageFlags.InLightmaps))
                {
                    location = ResourceLocation.Lightmaps;
                    return true;
                }
            }

            location = ResourceLocation.None;
            return false;
        }

        /// <summary>
        /// Changes the location of the resource by changing its location flags.
        /// </summary>
        /// <param name="newLocation">The new location.</param>
        /// <exception cref="System.ArgumentException">Unsupported resource location</exception>
        public void ChangeLocation(ResourceLocation newLocation)
        {
            Page.OldFlags &= ~OldRawPageFlags.LocationMask;
            Page.NewFlags &= ~NewRawPageFlags.LocationMask;
            switch (newLocation)
            {
                case ResourceLocation.Resources:
                    Page.OldFlags |= OldRawPageFlags.InResources;
                    Page.NewFlags |= NewRawPageFlags.InResources;
                    break;
                case ResourceLocation.Textures:
                    Page.OldFlags |= OldRawPageFlags.InTextures;
                    Page.NewFlags |= NewRawPageFlags.InTextures;
                    break;
                case ResourceLocation.TexturesB:
                    Page.OldFlags |= OldRawPageFlags.InTexturesB;
                    Page.NewFlags |= NewRawPageFlags.InTexturesB;
                    break;
                case ResourceLocation.Audio:
                    Page.OldFlags |= OldRawPageFlags.InAudio;
                    Page.NewFlags |= NewRawPageFlags.InAudio;
                    break;
                case ResourceLocation.ResourcesB:
                    Page.OldFlags |= OldRawPageFlags.InResourcesB;
                    Page.NewFlags |= NewRawPageFlags.InResourcesB;
                    break;
                case ResourceLocation.RenderModels:
                    Page.NewFlags |= NewRawPageFlags.InRenderModels;
                    break;
                case ResourceLocation.Lightmaps:
                    Page.NewFlags |= NewRawPageFlags.InLightmaps;
                    break;
                case ResourceLocation.Mods:
                    Page.OldFlags |= OldRawPageFlags.InMods;
                    break;
                default:
                    throw new ArgumentException("Unsupported resource location");
            }
        }

        /// <summary>
        /// Disables the resource's checksum by changing its location flags.
        /// </summary>
        public void DisableChecksum()
        {
            Page.OldFlags &= ~(OldRawPageFlags.UseChecksum | OldRawPageFlags.UseChecksum2);
            Page.NewFlags &= ~NewRawPageFlags.UseChecksum;
        }
    }
    
    /// <summary>
    /// Resource location constants used by <see cref="PageableResource.GetLocation"/>.
    /// </summary>
    public enum ResourceLocation
    {
        /// <summary>
        /// The resource is not cached.
        /// </summary>
        None = -1,

        /// <summary>
        /// The resource is in resources.dat.
        /// </summary>
        Resources,

        /// <summary>
        /// The resource is in textures.dat.
        /// </summary>
        Textures,

        /// <summary>
        /// The resource is in textures_b.dat.
        /// </summary>
        TexturesB,

        /// <summary>
        /// The resource is in audio.dat.
        /// </summary>
        Audio,

        /// <summary>
        /// The resource is in resources_b.dat.
        /// </summary>
        ResourcesB,

        /// <summary>
        /// The resource is in render_models.dat.
        /// </summary>
        RenderModels,

        /// <summary>
        /// The resource is in lightmaps.dat.
        /// </summary>
        Lightmaps,

        /// <summary>
        /// The resource is in mods.dat.
        /// </summary>
        Mods
    }
}
