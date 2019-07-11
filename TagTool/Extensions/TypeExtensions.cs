using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Resources;

namespace System
{
    public static class TypeExtensions
    {
        private static Dictionary<string, Tag> TypeGroupTags { get; } = new Dictionary<string, Tag>();

        public static Tag GetGroupTag(this Type type, CacheVersion version = CacheVersion.Unknown)
        {
            if (TypeGroupTags.ContainsKey(type.FullName))
                return TypeGroupTags[type.FullName];

            var attr = type.GetCustomAttributes(typeof(TagStructureAttribute), false)
                .Cast<TagStructureAttribute>()
                .FirstOrDefault(a => version == CacheVersion.Unknown || CacheVersionDetection.IsBetween(version, a.MinVersion, a.MaxVersion));

            if (attr == null)
                throw new NotSupportedException(type.FullName);

            return TypeGroupTags[type.FullName] = new Tag(attr.Tag);
        }

        private static Dictionary<string, uint> TypeSizes { get; } = new Dictionary<string, uint>();

        public static uint GetSize(this Type type, CacheVersion version = CacheVersion.Unknown)
        {
            if (TypeSizes.ContainsKey(type.FullName))
                return TypeSizes[type.FullName];

            var attr = type.GetCustomAttributes(typeof(TagStructureAttribute), false)
                .Cast<TagStructureAttribute>()
                .FirstOrDefault(a => version == CacheVersion.Unknown || CacheVersionDetection.IsBetween(version, a.MinVersion, a.MaxVersion));

            if (attr == null)
                throw new NotSupportedException(type.FullName);

            return TypeSizes[type.FullName] = attr.Size;
        }

        public static TagResourceTypeGen3 GetTagResourceTypeGen3(this Type type)
        {
            if (type == typeof(BinkResource))
                return TagResourceTypeGen3.Bink;
            else if (type == typeof(BitmapTextureInterleavedInteropResource))
                return TagResourceTypeGen3.BitmapInterleaved;
            else if (type == typeof(BitmapTextureInteropResource))
                return TagResourceTypeGen3.Bitmap;
            else if (type == typeof(ModelAnimationTagResource))
                return TagResourceTypeGen3.Animation;
            else if (type == typeof(RenderGeometryApiResourceDefinition))
                return TagResourceTypeGen3.RenderGeometry;
            else if (type == typeof(SoundResourceDefinition))
                return TagResourceTypeGen3.Sound;
            else if (type == typeof(StructureBspCacheFileTagResources))
                return TagResourceTypeGen3.Pathfinding;
            else if (type == typeof(StructureBspTagResources))
                return TagResourceTypeGen3.Collision;
            else
                throw new NotSupportedException(type.FullName);
        }
    }
}