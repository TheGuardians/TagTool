using System;
using System.Collections.Generic;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache
{
    public static class CacheVersionDetection
    {
        /// <summary>
        /// Detects the engine that a tags.dat was built for based on its timestamp.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="closestGuess">On return, the closest guess for the engine's version.</param>
        /// <returns>The engine version if the timestamp matched directly, otherwise <see cref="CacheVersion.Unknown"/>.</returns>
        public static CacheVersion DetectFromTimestamp(long timestamp, out CacheVersion closestGuess)
        {
            if (HaloOnlineTimestampMapping.ContainsKey(timestamp))
            {
                closestGuess = HaloOnlineTimestampMapping[timestamp];
                return closestGuess;
            }

            // (INACCURATE)
            // Match the closest timestamp
            var index = Array.BinarySearch(VersionTimestamps, timestamp);
            index = Math.Max(0, Math.Min(~index - 1, VersionTimestamps.Length - 1));
            closestGuess = (CacheVersion)index;
            return CacheVersion.Unknown;
        }

        /// <summary>
        /// Gets the timestamp for a version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>The timestamp, or -1 for <see cref="CacheVersion.Unknown"/>.</returns>
        public static long GetTimestamp(CacheVersion version)
        {
            if (version == CacheVersion.Unknown)
                return -1;

            return VersionTimestamps[(int)version];
        }

        /// <summary>
        /// Gets the <see cref="CacheVersion"/> associated with the specified build name.
        /// </summary>
        /// <param name="buildName">The build name.</param>
        /// <param name="version"></param>
        /// <param name="cachePlatform"></param>
        /// <returns>The version, or <see cref="CacheVersion.Unknown"/> if not found.</returns>
        public static void GetFromBuildName(string buildName, ref CacheVersion version, ref CachePlatform cachePlatform)
        {
            switch (buildName)
            {
                case "01.09.25.2247":
                case "01.10.12.2276":
                    version = CacheVersion.HaloXbox;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "01.00.00.0564":
                    version = CacheVersion.HaloPC;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "01.00.00.0609":
                    version = CacheVersion.HaloCustomEdition;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "02.06.28.07902":
                    version = CacheVersion.Halo2Beta;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "02.09.27.09809":
                    version = CacheVersion.Halo2Xbox;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "11081.07.04.30.0934.main":
                    version = CacheVersion.Halo2Vista;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "09699.07.05.01.1534.delta":
                   version = CacheVersion.Halo3Beta;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "11855.07.08.20.2317.halo3_ship":
                case "12065.08.08.26.0819.halo3_ship":
                    version = CacheVersion.Halo3Retail;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "13895.09.04.27.2201.atlas_relea":
                    version = CacheVersion.Halo3ODST;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "eldewrito":
                    version = CacheVersion.HaloOnlineED;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "1.106708 cert_ms23":
                    version = CacheVersion.HaloOnline106708;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "1.235640 cert_ms23":
                    version = CacheVersion.HaloOnline235640;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "0.0.1.301003 cert_MS26_new":
                    version = CacheVersion.HaloOnline301003;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "0.4.1.327043 cert_MS26_new":
                    version = CacheVersion.HaloOnline327043;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "8.1.372731 Live":
                    version = CacheVersion.HaloOnline372731;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "0.0.416097 Live":
                    version = CacheVersion.HaloOnline416097;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "10.1.430475 Live":
                    version = CacheVersion.HaloOnline430475;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "10.1.454665 Live":
                    version = CacheVersion.HaloOnline454665;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "10.1.449175 Live":
                    version = CacheVersion.HaloOnline449175;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "11.1.498295 Live":
                    version = CacheVersion.HaloOnline498295;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "11.1.530605 Live":
                    version = CacheVersion.HaloOnline530605;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "11.1.532911 Live":
                    version = CacheVersion.HaloOnline532911;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "11.1.554482 Live":
                    version = CacheVersion.HaloOnline554482;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "11.1.571627 Live":
                    version = CacheVersion.HaloOnline571627;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "12.1.700123 cert_ms30_oct19":
                    version = CacheVersion.HaloOnline700123;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "11860.10.07.24.0147.omaha_relea":
                    version = CacheVersion.HaloReach;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "20810.12.09.22.1647.main":
                case "21122.12.11.21.0101.main":
                case "21165.12.12.12.0112.main":
                case "21339.13.02.05.0117.main":
                case "21391.13.03.13.1711.main":
                case "21401.13.04.23.1849.main":
                case "21501.13.08.06.2311.main":
                case "21522.13.10.17.1936.main":
                    version = CacheVersion.Halo4;
                    cachePlatform = CachePlatform.Original;
                    break;

                case "May 29 2019 00:44:52":
                case "Jun 24 2019 00:36:03":
                case "Jul 30 2019 14:17:16":
                case "Oct 24 2019 15:56:32":
                    version = CacheVersion.HaloReach;
                    cachePlatform = CachePlatform.MCC;
                    break;

                case "Sep 29 2021 09:17:56":
                    version = CacheVersion.Halo3Retail;
                    cachePlatform = CachePlatform.MCC;
                    break;
                default:
                    version = CacheVersion.Unknown;
                    cachePlatform = CachePlatform.All;
                    break;
            }
        }

        /// <summary>
        /// Gets the version string corresponding to an <see cref="CacheVersion"/> value.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="platform"></param>
        /// <returns>The version string.</returns>
        public static string GetBuildName(CacheVersion version, CachePlatform platform)
        {
            if (platform == CachePlatform.MCC)
            {
                switch (version)
                {
                    case CacheVersion.Halo3Retail:
                        return "Sep 29 2021 09:17:56";
                    default:
                        return version.ToString();
                }
            }
            else
            {
                switch (version)
                {
                    case CacheVersion.HaloXbox:
                        return "01.10.12.2276";
                    case CacheVersion.HaloPC:
                        return "01.00.00.0564";
                    case CacheVersion.HaloCustomEdition:
                        return "01.00.00.0609";
                    case CacheVersion.Halo2Beta:
                        return "02.06.28.07902";
                    case CacheVersion.Halo2Xbox:
                        return "02.09.27.09809";
                    case CacheVersion.Halo2Vista:
                        return "11081.07.04.30.0934.main";
                    case CacheVersion.Halo3Retail:
                        return "11855.07.08.20.2317.halo3_ship";
                    case CacheVersion.Halo3ODST:
                        return "13895.09.04.27.2201.atlas_relea";
                    case CacheVersion.HaloOnlineED:
                        return "eldewrito";
                    case CacheVersion.HaloOnline106708:
                        return "1.106708 cert_ms23";
                    case CacheVersion.HaloOnline235640:
                        return "1.235640 cert_ms23";
                    case CacheVersion.HaloOnline301003:
                        return "0.0.1.301003 cert_MS26_new";
                    case CacheVersion.HaloOnline327043:
                        return "0.4.1.327043 cert_MS26_new";
                    case CacheVersion.HaloOnline372731:
                        return "8.1.372731 Live";
                    case CacheVersion.HaloOnline416097:
                        return "0.0.416097 Live";
                    case CacheVersion.HaloOnline430475:
                        return "10.1.430475 Live";
                    case CacheVersion.HaloOnline454665:
                        return "10.1.454665 Live";
                    case CacheVersion.HaloOnline449175:
                        return "10.1.449175 Live";
                    case CacheVersion.HaloOnline498295:
                        return "11.1.498295 Live";
                    case CacheVersion.HaloOnline530605:
                        return "11.1.530605 Live";
                    case CacheVersion.HaloOnline532911:
                        return "11.1.532911 Live";
                    case CacheVersion.HaloOnline554482:
                        return "11.1.554482 Live";
                    case CacheVersion.HaloOnline571627:
                        return "11.1.571627 Live";
                    case CacheVersion.HaloOnline700123:
                        return "12.1.700123 cert_ms30_oct19";
                    case CacheVersion.HaloReach:
                        return "11860.10.07.24.0147.omaha_relea";
                    case CacheVersion.HaloReach11883:
                        return "11883.10.10.25.1227.dlc_1_ship__tag_test";
                    case CacheVersion.Halo4:
                        return "20810.12.09.22.1647.main";
                    default:
                        return version.ToString();
                }
            }
            
        }

        /// <summary>
        /// Checks if a <see cref="CacheVersion"/> is in Little-Endian or Big-Endian.
        /// </summary>
        /// <param name="version">The <see cref="CacheVersion"/> to check the endianness of.</param>
        /// <param name="cachePlatform"></param>
        /// <returns>True if the <see cref="CacheVersion"/> is Little-Endian, false otherwise.</returns>
        public static bool IsLittleEndian(CacheVersion version, CachePlatform cachePlatform)
		{
            if (cachePlatform == CachePlatform.MCC)
                return true;

			switch (version)
			{
				case CacheVersion.Halo3Beta:
				case CacheVersion.Halo3Retail:
				case CacheVersion.Halo3ODST:
				case CacheVersion.HaloReach:
                case CacheVersion.HaloReach11883:
                case CacheVersion.Halo4:
					return false;

                case CacheVersion.HaloXbox:
                case CacheVersion.HaloPC:
                case CacheVersion.HaloCustomEdition:
                case CacheVersion.Halo2Beta:
				case CacheVersion.Halo2Xbox:
				case CacheVersion.Halo2Vista:
				case CacheVersion.HaloOnlineED:
                case CacheVersion.HaloOnline106708:
				case CacheVersion.HaloOnline235640:
				case CacheVersion.HaloOnline301003:
				case CacheVersion.HaloOnline327043:
				case CacheVersion.HaloOnline372731:
				case CacheVersion.HaloOnline416097:
				case CacheVersion.HaloOnline430475:
				case CacheVersion.HaloOnline454665:
				case CacheVersion.HaloOnline449175:
				case CacheVersion.HaloOnline498295:
				case CacheVersion.HaloOnline530605:
				case CacheVersion.HaloOnline532911:
				case CacheVersion.HaloOnline554482:
				case CacheVersion.HaloOnline571627:
				case CacheVersion.HaloOnline700123:
                    return true;

				default:
					throw new NotImplementedException(version.ToString());
			}
		}

        /// <summary>
        /// Determines whether a field exists in the given CacheVersion. Defines a priority : Versions, Gen, Min/Max.
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static bool AttributeInCacheVersion(TagFieldAttribute attr, CacheVersion compare)
        {
            if (attr.Version != CacheVersion.Unknown)
                if (attr.Version != compare)
                    return false;

            if (attr.Gen != CacheGeneration.Unknown)
                if (!IsInGen(attr.Gen, compare))
                    return false;

            if (attr.MinVersion != CacheVersion.Unknown || attr.MaxVersion != CacheVersion.Unknown)
                if (!IsBetween(compare, attr.MinVersion, attr.MaxVersion))
                    return false;

            return true;
        }

        public static bool AttributeInCacheVersion(TagStructureAttribute attr, CacheVersion compare)
        {
            if (attr.Version != CacheVersion.Unknown)
                if (attr.Version != compare)
                    return false;

            if (attr.Gen != CacheGeneration.Unknown)
                if (!IsInGen(attr.Gen, compare))
                    return false;

            if (attr.MinVersion != CacheVersion.Unknown || attr.MaxVersion != CacheVersion.Unknown)
                if (!IsBetween(compare, attr.MinVersion, attr.MaxVersion))
                    return false;

            return true;
        }

        public static bool AttributeInPlatform(TagFieldAttribute attr, CachePlatform compare)
        {
            return ComparePlatform(attr.Platform, compare);
        }

        public static bool AttributeInPlatform(TagStructureAttribute attr, CachePlatform compare)
        {
            return ComparePlatform(attr.Platform, compare);
        }

        public static bool ComparePlatform(CachePlatform attributeCachePlatform, CachePlatform compare)
        {
            if (attributeCachePlatform == CachePlatform.All)
                return true;
            else
                return attributeCachePlatform == compare;
        }

        /// <summary>
        /// Compares two version numbers.
        /// </summary>
        /// <param name="lhs">The left-hand version number.</param>
        /// <param name="rhs">The right-hand version number.</param>
        /// <returns>A positive value if the left version is newer, a negative value if the right version is newer, and 0 if the versions are equivalent.</returns>
        public static int Compare(CacheVersion lhs, CacheVersion rhs)
        {
            // Assume the enum values are in order by release date
            return (int)lhs - (int)rhs;
        }

        /// <summary>
        /// Determines whether a version number is between two other version numbers (inclusive).
        /// </summary>
        /// <param name="compare">The version number to compare. If this is <see cref="CacheVersion.Unknown"/>, this function will always return <c>true</c>.</param>
        /// <param name="min">The minimum version number. If this is <see cref="CacheVersion.Unknown"/>, then the lower bound will be ignored.</param>
        /// <param name="max">The maximum version number. If this is <see cref="CacheVersion.Unknown"/>, then the upper bound will be ignored.</param>
        /// <returns></returns>
        public static bool IsBetween(CacheVersion compare, CacheVersion min, CacheVersion max)
        {
            if (compare == CacheVersion.Unknown)
                return true;

            if (min != CacheVersion.Unknown && Compare(compare, min) < 0)
                return false;

            return (max == CacheVersion.Unknown || Compare(compare, max) <= 0);
        }

        /// <summary>
        /// Determine whether a CacheVersion belongs to a CacheGeneration
        /// </summary>
        /// <param name="gen"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static bool IsInGen(CacheGeneration gen, CacheVersion compare)
        {
            if (compare == CacheVersion.Unknown || gen == CacheGeneration.Unknown)
                return true;
            else
                return GetGeneration(compare) == gen;
        }

        public static bool InCacheBuildType(CacheBuildType buildType, CacheVersion compare)
        {
            if (compare == CacheVersion.Unknown || buildType == CacheBuildType.Unknown)
                return true;
            else
                return GetCacheBuildType(compare) == buildType;
        }

        public static CacheBuildType GetCacheBuildType(CacheVersion version)
        {
            switch (version)
            {
                case CacheVersion.HaloReach11883:
                    return CacheBuildType.TagsBuild;
            }

            return CacheBuildType.ReleaseBuild;
        }

        public static bool TestAttribute(TagFieldAttribute a, CacheVersion version, CachePlatform platform)
        {
            if (!InCacheBuildType(a.BuildType, version))
                return false;
            if (!IsInGen(a.Gen, version))
                return false;
            if (!AttributeInPlatform(a, platform))
                return false;
            if (!AttributeInCacheVersion(a, version))
                return false;

            return true;
        }

        public static bool TestAttribute(TagStructureAttribute a, CacheVersion version, CachePlatform platform)
        {
            if (!InCacheBuildType(a.BuildType, version))
                return false;
           
            if (!AttributeInPlatform(a, platform))
                return false;
            if (!IsInGen(a.Gen, version))
                return false;
            if (!AttributeInCacheVersion(a, version))
                return false;

            return true;
        }

        /// <summary>
        /// Get CacheGeneration from CacheVersion
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static CacheGeneration GetGeneration(this CacheVersion version)
        {
            switch (version)
            {
                case CacheVersion.HaloXbox:
                case CacheVersion.HaloPC:
                case CacheVersion.HaloCustomEdition:
                    return CacheGeneration.First;

                case CacheVersion.Halo2Vista:
                case CacheVersion.Halo2Xbox:
                case CacheVersion.Halo2Beta:
                    return CacheGeneration.Second;

                case CacheVersion.Halo3Beta:
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                case CacheVersion.HaloReach:
                case CacheVersion.HaloReach11883:
                    return CacheGeneration.Third;

                case CacheVersion.HaloOnlineED:
                case CacheVersion.HaloOnline106708:
                case CacheVersion.HaloOnline235640:
                case CacheVersion.HaloOnline301003:
                case CacheVersion.HaloOnline327043:
                case CacheVersion.HaloOnline372731:
                case CacheVersion.HaloOnline416097:
                case CacheVersion.HaloOnline430475:
                case CacheVersion.HaloOnline454665:
                case CacheVersion.HaloOnline449175:
                case CacheVersion.HaloOnline498295:
                case CacheVersion.HaloOnline530605:
                case CacheVersion.HaloOnline532911:
                case CacheVersion.HaloOnline554482:
                case CacheVersion.HaloOnline571627:
                case CacheVersion.HaloOnline700123:
                    return CacheGeneration.HaloOnline;

                case CacheVersion.Halo4:
                    return CacheGeneration.Fourth;

                default:
                    return CacheGeneration.Unknown;
            }
        }

        public static PlatformType GetPlatformType(CachePlatform cachePlatform)
        {
            switch (cachePlatform)
            {
                case CachePlatform.MCC:
                    return PlatformType._64Bit;
                case CachePlatform.Original:
                    return PlatformType._32Bit;
                default:
                    throw new Exception($"Unknown cache platform { cachePlatform}");
            }
        }

        public static GameTitle GetGameTitle(CacheVersion version)
        {
            switch (version)
            {
                case CacheVersion.HaloXbox:
                case CacheVersion.HaloPC:
                case CacheVersion.HaloCustomEdition:
                    return GameTitle.HaloCE;
                case CacheVersion.Halo2Beta:
                case CacheVersion.Halo2Xbox:
                case CacheVersion.Halo2Vista:
                    return GameTitle.Halo2;
                case CacheVersion.Halo3Beta:
                case CacheVersion.Halo3Retail:
                    return GameTitle.Halo3;
                case CacheVersion.Halo3ODST:
                    return GameTitle.Halo3ODST;
                case CacheVersion.HaloOnlineED:
                case CacheVersion.HaloOnline106708:
                case CacheVersion.HaloOnline235640:
                case CacheVersion.HaloOnline301003:
                case CacheVersion.HaloOnline327043:
                case CacheVersion.HaloOnline372731:
                case CacheVersion.HaloOnline416097:
                case CacheVersion.HaloOnline430475:
                case CacheVersion.HaloOnline454665:
                case CacheVersion.HaloOnline449175:
                case CacheVersion.HaloOnline498295:
                case CacheVersion.HaloOnline530605:
                case CacheVersion.HaloOnline532911:
                case CacheVersion.HaloOnline554482:
                case CacheVersion.HaloOnline571627:
                case CacheVersion.HaloOnline700123:
                    return GameTitle.HaloOnline;
                case CacheVersion.HaloReach:
                case CacheVersion.HaloReach11883:
                    return GameTitle.HaloReach;
                case CacheVersion.Halo4:
                    return GameTitle.Halo4;
                default:
                    return GameTitle.Unknown;
            }
        }

        /// <summary>
        /// tags.dat timestamps for each halo online game version.
        /// Timestamps in here map directly to a <see cref="CacheVersion"/> value.
        /// </summary>
        private static readonly Dictionary<long, CacheVersion> HaloOnlineTimestampMapping = new Dictionary<long, CacheVersion>
        {
            [132699675831101597] = CacheVersion.HaloOnlineED,
            [130713360239499012] = CacheVersion.HaloOnline106708,
            [130772932862346058] = CacheVersion.HaloOnline235640,
            [130785901486445524] = CacheVersion.HaloOnline301003,
            [130800445160458507] = CacheVersion.HaloOnline327043,
            [130814318396118255] = CacheVersion.HaloOnline372731,
            [130829123589114103] = CacheVersion.HaloOnline416097,
            [130834294034159845] = CacheVersion.HaloOnline430475,
            [130844512316254660] = CacheVersion.HaloOnline454665,
            [130851642645809862] = CacheVersion.HaloOnline449175,
            [130858473716879375] = CacheVersion.HaloOnline498295,
            [130868891945946004] = CacheVersion.HaloOnline530605,
            [130869644198634503] = CacheVersion.HaloOnline532911,
            [130879952719550501] = CacheVersion.HaloOnline554482,
            [130881889330693956] = CacheVersion.HaloOnline571627,
            [130930071628935939] = CacheVersion.HaloOnline700123
        };

        /// <summary>
        /// tags.dat timestamps for each game version.
        /// Timestamps in here should correspond directly to <see cref="CacheVersion"/> enum values (excluding <see cref="CacheVersion.Unknown"/>).
        /// </summary>
        private static readonly long[] VersionTimestamps =
        {
            -1, // Halo Xbox
            -1, // Halo PC
            -1, // Halo Custom Edition
            -1, // Halo2Beta
            -1, // Halo2Xbox
            -1, // Halo2Vista
            -1, // Halo3Beta
            -1, // Halo3Retail
            -1, // Halo3ODST
            132699675831101597, // HaloOnlineED
            130713360239499012, // HaloOnline106708
            130772932862346058, // HaloOnline235640
            130785901486445524, // HaloOnline301003
            130800445160458507, // V0_4_1_327043_cert_MS26_new
            130814318396118255, // V8_1_372731_Live
            130829123589114103, // V0_0_416097_Live
            130834294034159845, // HaloOnline430475
            130844512316254660, // V10_1_454665_Live
            130851642645809862, // HaloOnline449175
            130858473716879375, // HaloOnline498295
            130868891945946004, // V11_1_530605_Live
            130869644198634503, // V11_1_532911_Live
            130879952719550501, // V11_1_554482_Live
            130881889330693956, // HaloOnline571627
            130930071628935939, // HaloOnline700123
            -1, // HaloReach
            -1  // Halo 4
        };
    }

    public enum CacheVersion : int
    {
        Unknown = -1,
        HaloXbox,
        HaloPC,
        HaloCustomEdition,
        Halo2Beta,
        Halo2Xbox,
        Halo2Vista,
        Halo3Beta,
        Halo3Retail,
        Halo3ODST,
        HaloOnlineED,
        HaloOnline106708,
        HaloOnline235640,
        HaloOnline301003,
        HaloOnline327043,
        HaloOnline372731,
        HaloOnline416097,
        HaloOnline430475,
        HaloOnline454665,
        HaloOnline449175,
        HaloOnline498295,
        HaloOnline530605,
        HaloOnline532911,
        HaloOnline554482,
        HaloOnline571627,
        HaloOnline700123,
        HaloReach,
        HaloReach11883,
        Halo4
    }

    public enum CacheGeneration : int
    {
        Unknown = -1,
        First = 1,
        Second = 2,
        Third = 3,
        HaloOnline = 4,
        Fourth = 5
    }

    public enum CachePlatform : byte
    {
        /// <summary>
        /// Belongs to both the original version and the MCC version
        /// </summary>
        All = 0,

        /// <summary>
        /// Belongs only to the original version (xbox, xbox 360, PC (not MCC))
        /// </summary>
        Original = 1,

        /// <summary>
        /// Belongs only to the MCC version
        /// </summary>
        MCC = 2
    }

    public enum PlatformType : byte
    {
        _32Bit = 0,
        _64Bit = 1
    }

    public enum CacheBuildType
    {
        Unknown,
        TagsBuild,
        ReleaseBuild
    }
}