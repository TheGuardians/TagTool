using System;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags
{
    public static class VersionedEnum
    {
        public static object ImportValue(Type enumType, int value, CacheVersion version, CachePlatform platform)
        {
            var info = TagEnum.GetInfo(enumType, version, platform);
            if (!info.Attribute.IsVersioned)
                throw new InvalidOperationException("Cannot import to an non-versioned enum.");

            var members = TagEnum.GetMemberEnumerable(info).Members;
            if (value == -1)
            {
                if ((int)members[0].Value != -1)
                    throw new ArgumentOutOfRangeException("Expected first member of versioned enum to be -1.");

                return members[0].Value;
            }
            else
            {
                if (value < 0 || value >= members.Count)
                    throw new ArgumentOutOfRangeException(nameof(value), "Value was out of range of the enum members");

                return members[value].Value;
            }
        }

        public static int ExportValue(Type enumType, object enumValue, CacheVersion version, CachePlatform platform)
        {
            var info = TagEnum.GetInfo(enumType, version, platform);
            if (!info.Attribute.IsVersioned)
                throw new InvalidOperationException("Cannot import to an non-versioned enum.");

            var members = TagEnum.GetMemberEnumerable(info).Members;
            for (int i = 0; i < members.Count; i++)
            {
                if (members[i].Value.Equals(enumValue))
                    return i;
            }

            throw new ArgumentOutOfRangeException(nameof(enumValue));
        }

        public static IFlagBits ImportFlags(Type enumType, uint value, CacheVersion version, CachePlatform platform)
        {
            var info = TagEnum.GetInfo(enumType, version, platform);
            if (!info.Attribute.IsVersioned)
                throw new InvalidOperationException("Cannot import to an non-versioned enum.");

            var flagBits = (IFlagBits)Activator.CreateInstance(typeof(FlagBits<>).MakeGenericType(enumType));

            var members = TagEnum.GetMemberEnumerable(info).Members;
            for (int i = 0; i < members.Count; i++)
            {
                uint mask = 1u << i;
                if ((value & mask) != 0)
                {
                    value &= ~mask;
                    flagBits.SetBit((Enum)members[i].Value, true);
                }
            }

            if (value != 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Value had more bits set than enum members.");

            return flagBits;
        }

        public static uint ExportFlags(Type enumType, IFlagBits flagBits, CacheVersion version, CachePlatform platform)
        {
            var info = TagEnum.GetInfo(enumType, version, platform);
            if (!info.Attribute.IsVersioned)
                throw new InvalidOperationException("Cannot import to an non-versioned enum.");

            uint value = 0;
            var members = TagEnum.GetMemberEnumerable(info).Members;
            for (int i = 0; i < members.Count; i++)
            {
                if (flagBits.TestBit((Enum)members[i].Value))
                    value |= 1u << i;
            }

            return value;
        }
    }
}
