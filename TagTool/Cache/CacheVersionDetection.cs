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
                case "11091.07.05.11.1104.main": // This is another version of the Vista Release of Halo 2, Added this to allow for detection of this version and extraction and porting of the tags.
                    version = CacheVersion.Halo2Vista;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "11122.07.08.24.1808.main":
                    version = CacheVersion.Halo2Vista;
                    cachePlatform = CachePlatform.Original;
                    break;
		case "06481.06.11.17.1330.alpha_relea": // The Fabled Holy Grail, Pimps at Sea
                case "08117.07.03.07.1702.delta": // This is the March 7th Build of the Halo 3 Beta, I have added support for this build to allow the addition of cut content such as a player and enemy models that are not present in retail.
                case "08172.07.03.08.2240.delta": // This is the March 8th Build of the Halo 3 Beta, not much is diffrent much March 7th. but for version detection purposes this is needed as some Tag offsets are diffrent.
                case "Mar  9 2007 22:22:32": // This version is unique in which is uses the "343i" Style of Timestamping builds as opposed to the Bungie Style of Timestamping builds. This is the March 9th Build of the Halo 3 Beta, this build is the first to have the new build naming style.
                case "Mar 10 2007 16:16:44": // This is the March 10th Build of the Halo 3 Beta, this build is the second to have the new build naming style.
                case "09699.07.05.01.1534.delta":
                   version = CacheVersion.Halo3Beta;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "11729.07.08.10.0021.main": // This is the Halo 3 Expo Build shown at E3 2007.
                case "11855.07.08.20.2317.halo3_ship":
                case "11856.07.08.20.2332.release": // This is the Halo 3 Epsilon Build.
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
                case "11.1.601838 Live":
                    version = CacheVersion.HaloOnline604673;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "12.1.700123 cert_ms30_oct19":
                    version = CacheVersion.HaloOnline700123;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "09449.10.03.25.1545.omaha_beta": // This is the Halo Reach Pre-Beta Build. This was used to internally Test the game before the Beta was released.
                case "09730.10.04.09.1309.omaha_delta": // This is the Halo Reach Beta Build. This was used to publically Test the game before the it was released.
                    version = CacheVersion.HaloReachBeta; // This is the Added Version Detection for the Halo Reach Beta Build, I have had to manually map out Tag Offsets for this build as they are not present in the Halo Reach Retail Build. This is due to the fact that the Halo Reach Beta Build is a heavily modified version of the Halo 3 Engine and requires some special handling.
                    cachePlatform = CachePlatform.Original;
                    break;
                case "00095.11.04.09.1509.demo": // This is the Halo Reach Demo, which ran on a seprate build of the Halo Reach Engine. This is the Added Version Detection for the Halo Reach Demo Build, I have had to manually map out Tag Offsets for this build as they are not present in the Halo Reach Retail Build. This is due to the fact that the Halo Reach Demo Build is a modified version of the Halo Reach Retail Build and requires some special handling.
                case "11860.10.07.24.0147.omaha_relea":
                    version = CacheVersion.HaloReach;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "07556.11.09.18.2200.ca": // This is the very first alpha build of Halo 4, this build is very similar to the Halo Reach Build but requires some special handling. which appears to be running on the "ca" Build of the Halo Reach Engine which was created by Certain Affinity for the Halo Reach DLC's.
                case "07831.11.10.03.0231.main": // This is the second Halo 4 Alpha Build which appears to be running on a more typical main build of the Halo Reach Engine.
                case "07866.11.10.04.1335.horse": // This is the third Halo 4 Alpha Build which appears to be running on the "horse" branch of the engine. which is the first branch to be created by 343 Industries.
                case "07867.11.10.04.1338.horse": // This is the fourth Halo 4 Alpha Build which appears to be running on the "horse" branch of the engine which has replaced the main engine branch for this period of development.
                case "08028.11.10.11.1702.horse": // This is the fifth Halo 4 Alpha Build which appears to be running on the "horse" branch of the engine.
                case "08257.11.10.20.0230.main": // this is the sixth Halo 4 Alpha Build which appears to be running on the "main" branch of the engine. this marks this first return to the main branch since the third Alpha Build.
                case "08912.11.11.14.2334.jmalbert": // This is the seventh Halo 4 Alpha Build which appears to be running on the "jmalbert" branch of the engine. this is the first branch to be created by 343 Industries since the "horse" branch.
                case "09550.11.12.21.0300.main": // This is the eighth Halo 4 Alpha Build which appears to be running on the "main" branch of the engine. this is also the final Alpha Build.
                    version = CacheVersion.Halo4Alpha; // This is the Added Version Detection for the Halo 4 Alpha Build, I have had to manually map out Tag Offsets for this build as they are not present in the Halo 4 Retail or Beta Builds. This is due to the fact that the Halo 4 Alpha Build is a heavily modified version of the Halo Reach Engine and requires some special handling.
                    cachePlatform = CachePlatform.Original;
                    break;
                case "14064.12.05.05.1011.beta": // This is the Halo 4 Network Beta Build, which was used to internally Test the game on Xbox Live before the it was released.
                    version = CacheVersion.Halo4Beta; // This is the Added Version Detection for the Halo 4 Beta Build, I have had to manually map out Tag Offsets for this build as they are not present in the Halo 4 Retail Build. This is due to the fact that the Halo 4 Beta Build is a heavily modified version of the Halo Reach Engine 343 Industries had been developing during the Alpha phase of development and requires some special handling.
                    cachePlatform = CachePlatform.Original;
                    break;
                case "15119.12.05.31.0400.e3m60": // This build was specifically created for the E3 2012 Demo of Halo 4, it would showcase the campaign map m60 as it couldn't be run at the same time as other portions of the game due to it being in an unfinished state.
                case "15117.12.05.31.0300.e3": // This build was specifically created for the E3 2012 Demo of Halo 4, it would showcase the Spartan Ops as it couldn't be run at the same time as other portions of the game due to it being in an unfinished state.
                case "15116.12.05.31.0300.e3": // This build was specifically created for the E3 2012 Demo of Halo 4, it would showcase the multiplayer as it couldn't be run at the same time as other portions of the game due to it being in an unfinished state.
                    version = CacheVersion.Halo4E3; // This is the Added Version Detection for the Halo 4 E3 Build, I have had to manually map out Tag Offsets for this build as they are not present in the Halo 4 Retail Build. This is due to the fact that the Halo 4 E3 Build is a heavily modified version of the Halo Reach Engine 343 Industries had been developing during the Alpha phase of development and requires some special handling.
                    cachePlatform = CachePlatform.Original;
                    break;
                case "19057.12.08.19.0400.main": 
                    version = CacheVersion.Halo4PreRelease; // This is the Added Version Detection for the Halo 4 Pre-Release Build, I have had to manually map out Tag Offsets for this build as they are not present in the Halo 4 Retail Build. This is due to the fact that the Halo 4 Pre-Release Build is a modifed build that 343 Had been using to test the game internally.
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
                case "01.00.01.0563":
                case "01.03.43.0000":
                    version = CacheVersion.Halo1A;
                    cachePlatform = CachePlatform.Original;
                    break;
                case "Oct 12 2020 08:13:40": // MCC Flight 1
                case "Oct 26 2020 11:43:08": // MCC Day 1
                case "Mar 20 2021 04:23:02": // MCC Update 1
                case "May 16 2021 10:41:44": // MCC Flight 2
                case "May 27 2021 15:23:34": // MCC Update 2
                case "Sep 10 2021 06:54:23": // MCC Update 3
                case "Feb  3 2022 19:02:31": // MCC Update 4
                case "Aug  8 2022 17:28:57": // MCC Update 5
                    version = CacheVersion.Halo4;
                    cachePlatform = CachePlatform.MCC; // I have added a complete MCC Version History for the Cache Detection System, this is the MCC Version Detection for Halo 4.
                    break;
                case "Apr  9 2020 01:36:04": // MCC Flight 1
                case "Apr 13 2020 02:24:30": // MCC Day 1
                case "May 10 2020 21:14:00": // MCC Update 1
                case "May 12 2020 12:18:21": // MCC Update 2
                case "Jul 25 2020 22:24:58": // MCC Update 3
                case "Sep 30 2020 20:30:41": // MCC Update 4
                case "Dec 25 2020 16:05:40": // MCC Update 5
                case "Sep 17 2021 14:26:28": // MCC Update 6
                case "Sep 23 2021 02:46:19": // MCC Update 7
                case "Feb 14 2022 19:18:28": // MCC Update 8
                case "Jun 13 2022 13:44:24": // MCC Update 9
                    version = CacheVersion.Groundhog; // This is the Added Version Detection for the Groundhog Build. not much is known of about this Build but it was confirmed as being a heavily modified Halo 4 Multiplayer Build.
                    cachePlatform = CachePlatform.MCC;
                    break;
                case "May 29 2019 00:44:52": // MCC Internal Test Build
                case "Jun 24 2019 00:36:03": // MCC Insider Build 1
                case "Jul 30 2019 14:17:16": // MCC Insider Build 2
                case "Oct 24 2019 15:56:32": // MCC Day One Build
                case "Jan 30 2020 16:55:25": // MCC Update 1
                case "Mar 24 2020 12:10:36": // MCC Update 2
                    version = CacheVersion.HaloReach; // I have added a complete MCC Version History for the Cache Detection System, this is the MCC Version Detection for Halo Reach.
                    cachePlatform = CachePlatform.MCC;
                    break;
                case "Jun  4 2020 20:29:31": // MCC Flight 1
                case "Jun 21 2020 16:34:20": // MCC Flight 2
                case "Jun 25 2020 15:02:49": // MCC Day 1
                case "Aug 11 2020 23:34:41": // MCC Flight 3
                case "Aug 26 2020 21:24:11": // MCC Update 1
                case "Oct  7 2020 03:55:07": // MCC Flight 4 Main Menu Map
                case "Oct 21 2020 09:24:30": // MCC Update 2
                case "Nov 24 2020 15:47:48": // MCC Update 3
                case "Feb 19 2021 11:19:43": // MCC Flight 4
                case "Mar  5 2021 08:45:13": // MCC Flight 5
                case "Mar 14 2021 03:19:55": // MCC Update 4
                case "May 19 2021 16:19:54": // MCC Flight 6
                case "Jun  9 2021 09:25:41": // MCC Update 5
                case "Aug 25 2021 03:43:53": // MCC Flight 7
                case "Sep 29 2021 09:17:56": // MCC Update 6
                case "Oct 11 2021 04:58:42": // MCC Update 7
                case "Oct 11 2021 04:56:39": // MCC Mod Tools Release
                case "Nov  1 2021 06:16:14": // MCC Update 8
                case "Nov  1 2021 06:14:30": // MCC Mod Tools Update 1
                case "Feb 15 2022 01:35:34": // MCC Update 9
                case "Apr  6 2022 10:48:27": // MCC Mod Tools Update 2
                case "Apr  6 2022 10:48:46": // MCC Mod Tools Update 2 (Tool_Fast)
                case "Apr 20 2022 11:55:21": // MCC Mod Tools Update 3
                case "Apr 20 2022 11:56:23": // MCC Mod Tools Update 3 (Tool_Fast)
                case "May  3 2022 12:38:41": // MCC Update 10
                case "Jun  8 2022 16:05:04": // MCC Mod Tools Update 4
                case "Jun  8 2022 16:06:30": // MCC Mod Tools Update 4 (Tool_Fast)
                case "Jul 21 2022 21:15:27": // MCC Update 11
                case "Aug 21 2022 17:23:29": // MCC Mod Tools Update 5
                case "Aug 21 2022 17:24:39": // MCC Mod Tools Update 5 (Tool_Fast)
                    version = CacheVersion.Halo3Retail; // I have added a complete MCC Version History for the Cache Detection System, this is the MCC Version Detection for Halo 3.
                    cachePlatform = CachePlatform.MCC;
                    break;
                case "Aug 11 2020 06:58:27": // MCC Insider Build 1
                case "Aug 17 2020 01:12:27": // MCC Insider Build 2
                case "Aug 24 2020 08:37:26": // MCC Insider Build 3
                case "Aug 28 2020 08:43:36": // MCC Day One Build
                case "Sep 29 2020 10:59:04": // MCC Update 1
                case "Dec  4 2020 18:24:06": // MCC Update 2
                case "Aug 25 2021 03:35:05": // MCC Insider Build 4
                case "Sep 18 2021 21:15:55": // MCC Update 3
                case "Feb 15 2022 01:59:47": // MCC Update 4
                case "Apr  5 2022 14:16:52": // MCC Mod Tools Day One
                case "Apr  5 2022 14:16:35": // MCC Mod Tools Day One (Tool Fast Enabled Build)
                case "Apr 20 2022 11:53:15": // MCC Mod Tools Update 1 / MCC Update 5
                case "Apr 20 2022 11:52:14": // MCC Mod Tools Update 1 (Tool Fast Enabled Build)
                case "May  3 2022 12:27:56": // MCC Mod Tools Update 2
                case "May  3 2022 12:27:08": // MCC Mod Tools Update 2 (Tool Fast Enabled Build)
                case "Jul 22 2022 05:55:30": // MCC Update 6
                case "Aug 21 2022 17:29:59": // MCC Mod Tools Update 3
                case "Aug 21 2022 17:30:28": // MCC Mod Tools Update 3 (Tool Fast Enabled Build)
                    version = CacheVersion.Halo3ODST; // I have added a complete MCC Version History for the Cache Detection System, this is the MCC Version Detection for Halo 3 ODST. Which isn't present in the current dev branch of TagTool.
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
                    case CacheVersion.Halo1A:
                        return "01.00.01.0563";
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
                    case CacheVersion.HaloOnline604673:
                        return "11.1.604673 cert_ms29 Live";
                    case CacheVersion.HaloOnline700123:
                        return "12.1.700123 cert_ms30_oct19";
                    case CacheVersion.HaloReachBeta:
                        return "09730.10.04.09.1309.omaha_delta";
                    case CacheVersion.HaloReach:
                        return "11860.10.07.24.0147.omaha_relea";
                    case CacheVersion.HaloReach11883:
                        return "11883.10.10.25.1227.dlc_1_ship__tag_test";
                    case CacheVersion.Halo4Alpha:
                        return "07556.11.09.18.2200.ca";
                    case CacheVersion.Halo4Beta:
                        return "14064.12.05.05.1011.beta";
                    case CacheVersion.Halo4E3:
                        return "15119.12.05.31.0400.e3m60";
                    case CacheVersion.Halo4PreRelease:
                        return "19057.12.08.19.0400.main";
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
                case CacheVersion.HaloReachBeta:
                case CacheVersion.HaloReach11883:
                case CacheVersion.Halo4:
                case CacheVersion.Halo4Alpha:
                case CacheVersion.Halo4Beta:
                case CacheVersion.Halo4E3:
                case CacheVersion.Halo4PreRelease:
					return false;

                case CacheVersion.HaloXbox:
                case CacheVersion.HaloPC:
                case CacheVersion.HaloCustomEdition:
                case CacheVersion.Halo1A:
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
                case CacheVersion.HaloOnline604673:
                case CacheVersion.HaloOnline700123:
                case CacheVersion.Groundhog:
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
                case CacheVersion.Halo1A:
                    return CacheGeneration.First;

                case CacheVersion.Halo2Vista:
                case CacheVersion.Halo2Xbox:
                case CacheVersion.Halo2Beta:
                    return CacheGeneration.Second;

                case CacheVersion.Halo3Beta:
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                case CacheVersion.HaloReachBeta:
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
                case CacheVersion.HaloOnline604673:
                case CacheVersion.HaloOnline700123:
                    return CacheGeneration.HaloOnline;

                case CacheVersion.Halo4Alpha:
                case CacheVersion.Halo4Beta:
                case CacheVersion.Halo4:
                case CacheVersion.Groundhog:
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
                case CacheVersion.Halo1A:
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
                case CacheVersion.HaloOnline604673:
                case CacheVersion.HaloOnline700123:
                    return GameTitle.HaloOnline;
                case CacheVersion.HaloReachDemo:
                case CacheVersion.HaloReach:
                case CacheVersion.HaloReach11883:
                    return GameTitle.HaloReach;
                case CacheVersion.Groundhog:
                    return GameTitle.Groundhog;
                case CacheVersion.Halo4Alpha:
                case CacheVersion.Halo4Beta:
                case CacheVersion.Halo4E3:
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
            [130893802351772672] = CacheVersion.HaloOnline604673,
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
            -1, // Halo 1A
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
            130893802351772672, // HaloOnline604673
            130930071628935939, // HaloOnline700123
            -1, // HaloReachBeta
            -1, // HaloReachDemo
            -1, // HaloReach
            -1, // HaloReach11883
            -1, // Halo4Alpha
            -1, // Halo4Beta
            -1, // Halo4E3
            -1, // Halo4PreRelease
            -1, // Halo 4
            -1 // Groundhog

        };
    }

    public enum CacheVersion : int
    {
        Unknown = -1,
        HaloXbox,
        HaloPC,
        HaloCustomEdition,
        Halo1A,
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
        HaloOnline604673,
        HaloOnline700123,
        HaloReachBeta,
        HaloReachDemo,
        HaloReach,
        HaloReach11883,
        Halo4,
        Halo4Alpha,
        Halo4Beta,
        Halo4E3,
        Halo4PreRelease,
        Groundhog
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
