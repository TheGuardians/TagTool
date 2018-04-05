using TagTool.Cache;
using TagTool.Commands;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagTool.Commands.Porting
{
    public class ListBspMoppCodesCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CacheFile BlamCache { get; }

        public ListBspMoppCodesCommand(GameCacheContext cacheContext, CacheFile blamCache) :
            base(CommandFlags.Inherit,

                "ListBspMoppCodes",
                "Lists bsp mopp codes.",

                "ListBspMoppCodes <Tag Name>",

                "Lists bsp mopp codes.")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            CacheFile.IndexItem blamTag = null;

            var blamTagName = args[0];

            foreach (var tag in BlamCache.IndexItems)
            {
                if (tag.ClassCode == "sbsp" && tag.Filename == blamTagName)
                {
                    blamTag = tag;
                    break;
                }
            }

            if (blamTag == null)
            {
                Console.WriteLine("Blam scenario_structure_bsp tag does not exist: " + blamTagName);
                return false;
            }
            
            var blamContext = new CacheSerializationContext(BlamCache, blamTag);
            var blamSbsp = BlamCache.Deserializer.Deserialize<ScenarioStructureBsp>(blamContext);

            var blamMoppCodes = new List<byte>();

            foreach (var mopp in blamSbsp.CollisionMoppCodes)
                GetMoppCodes(mopp.Data.Select(i => i.Value).ToList(), ref blamMoppCodes);

            var resourceData = BlamCache.GetRawFromID(blamSbsp.ZoneAssetIndex3);

            if (resourceData != null)
            {
                var resourceEntry = BlamCache.ResourceGestalt.TagResources[blamSbsp.ZoneAssetIndex3 & ushort.MaxValue];

                var definitionAddress = new CacheAddress(CacheAddressType.Definition, resourceEntry.DefinitionAddress);
                var definitionData = BlamCache.ResourceGestalt.FixupInformation.Skip(resourceEntry.FixupInformationOffset).Take(resourceEntry.FixupInformationLength).ToArray();

                StructureBspTagResources resourceDefinition = null;

                using (var definitionStream = new MemoryStream(definitionData, true))
                using (var definitionReader = new EndianReader(definitionStream, EndianFormat.BigEndian))
                {
                    definitionStream.Position = definitionAddress.Offset;
                    resourceDefinition = BlamCache.Deserializer.Deserialize<StructureBspTagResources>(
                        new DataSerializationContext(definitionReader, CacheAddressType.Definition));
                }

                using (var blamResourceStream = new MemoryStream(resourceData))
                using (var resourceReader = new EndianReader(blamResourceStream, EndianFormat.BigEndian))
                {
                    var dataContext = new DataSerializationContext(resourceReader);

                    foreach (var instance in resourceDefinition.InstancedGeometry)
                    {
                        foreach (var moppCode in instance.CollisionMoppCodes)
                        {
                            blamResourceStream.Position = moppCode.Data.Address.Offset;
                            var moppData = resourceReader.ReadBytes(moppCode.Data.Count).ToList();
                            GetMoppCodes(moppData, ref blamMoppCodes);
                        }
                    }
                }
            }

            blamMoppCodes.Sort();

            foreach (var blamMoppCode in blamMoppCodes)
                Console.WriteLine($"0x{blamMoppCode:X2}");

            return true;
        }

        private void GetMoppCodes(List<byte> moppData, ref List<byte> moppCodes)
        {
            for (var i = 0; i < moppData.Count; i++)
            {
                var moppCode = moppData[i];

                switch (moppCode)
                {
                    case 0x00: // HK_MOPP_RETURN
                        break;

                    case 0x01: // HK_MOPP_SCALE1
                    case 0x02: // HK_MOPP_SCALE2
                    case 0x03: // HK_MOPP_SCALE3
                    case 0x04: // HK_MOPP_SCALE4
                        i += 3;
                        break;

                    case 0x05: // HK_MOPP_JUMP8
                        i += 1;
                        break;

                    case 0x06: // HK_MOPP_JUMP16
                        i += 2;
                        break;

                    case 0x07: // HK_MOPP_JUMP24
                        i += 3;
                        break;

                    /*case 0x08: // HK_MOPP_JUMP32 (NOT IMPLEMENTED)
                        Array.Reverse(moppData, i + 1, 4);
                        i += 4;
                        break;*/

                    case 0x09: // HK_MOPP_TERM_REOFFSET8
                        i += 1;
                        break;

                    case 0x0A: // HK_MOPP_TERM_REOFFSET16
                        i += 2;
                        break;

                    case 0x0B: // HK_MOPP_TERM_REOFFSET32
                        i += 4;
                        break;

                    case 0x0C: // HK_MOPP_JUMP_CHUNK
                        i += 2;
                        break;

                    case 0x0D: // HK_MOPP_DATA_OFFSET
                        i += 5;
                        break;

                    /*case 0x0E: // UNUSED
                    case 0x0F: // UNUSED
                        break;*/

                    case 0x10: // HK_MOPP_SPLIT_X
                    case 0x11: // HK_MOPP_SPLIT_Y
                    case 0x12: // HK_MOPP_SPLIT_Z
                    case 0x13: // HK_MOPP_SPLIT_YZ
                    case 0x14: // HK_MOPP_SPLIT_YMZ
                    case 0x15: // HK_MOPP_SPLIT_XZ
                    case 0x16: // HK_MOPP_SPLIT_XMZ
                    case 0x17: // HK_MOPP_SPLIT_XY
                    case 0x18: // HK_MOPP_SPLIT_XMY
                    case 0x19: // HK_MOPP_SPLIT_XYZ
                    case 0x1A: // HK_MOPP_SPLIT_XYMZ
                    case 0x1B: // HK_MOPP_SPLIT_XMYZ
                    case 0x1C: // HK_MOPP_SPLIT_XMYMZ
                        i += 3;
                        break;

                    /*case 0x1D: // UNUSED
                    case 0x1E: // UNUSED
                    case 0x1F: // UNUSED
                        break;*/

                    case 0x20: // HK_MOPP_SINGLE_SPLIT_X
                    case 0x21: // HK_MOPP_SINGLE_SPLIT_Y
                    case 0x22: // HK_MOPP_SINGLE_SPLIT_Z
                        i += 2;
                        break;

                    case 0x23: // HK_MOPP_SPLIT_JUMP_X
                    case 0x24: // HK_MOPP_SPLIT_JUMP_Y
                    case 0x25: // HK_MOPP_SPLIT_JUMP_Z
                        i += 6;
                        break;


                    case 0x26: // HK_MOPP_DOUBLE_CUT_X
                    case 0x27: // HK_MOPP_DOUBLE_CUT_Y
                    case 0x28: // HK_MOPP_DOUBLE_CUT_Z
                        i += 2;
                        break;

                    case 0x29: // HK_MOPP_DOUBLE_CUT24_X
                    case 0x2A: // HK_MOPP_DOUBLE_CUT24_Y
                    case 0x2B: // HK_MOPP_DOUBLE_CUT24_Z
                        i += 6;
                        break;

                    /*case 0x2C: // UNUSED
                    case 0x2D: // UNUSED
                    case 0x2E: // UNUSED
                    case 0x2F: // UNUSED
                        break;*/

                    case 0x30: // HK_MOPP_TERM4_0
                    case 0x31: // HK_MOPP_TERM4_1
                    case 0x32: // HK_MOPP_TERM4_2
                    case 0x33: // HK_MOPP_TERM4_3
                    case 0x34: // HK_MOPP_TERM4_4
                    case 0x35: // HK_MOPP_TERM4_5
                    case 0x36: // HK_MOPP_TERM4_6
                    case 0x37: // HK_MOPP_TERM4_7
                    case 0x38: // HK_MOPP_TERM4_8
                    case 0x39: // HK_MOPP_TERM4_9
                    case 0x3A: // HK_MOPP_TERM4_A
                    case 0x3B: // HK_MOPP_TERM4_B
                    case 0x3C: // HK_MOPP_TERM4_C
                    case 0x3D: // HK_MOPP_TERM4_D
                    case 0x3E: // HK_MOPP_TERM4_E
                    case 0x3F: // HK_MOPP_TERM4_F
                    case 0x40: // HK_MOPP_TERM4_10
                    case 0x41: // HK_MOPP_TERM4_11
                    case 0x42: // HK_MOPP_TERM4_12
                    case 0x43: // HK_MOPP_TERM4_13
                    case 0x44: // HK_MOPP_TERM4_14
                    case 0x45: // HK_MOPP_TERM4_15
                    case 0x46: // HK_MOPP_TERM4_16
                    case 0x47: // HK_MOPP_TERM4_17
                    case 0x48: // HK_MOPP_TERM4_18
                    case 0x49: // HK_MOPP_TERM4_19
                    case 0x4A: // HK_MOPP_TERM4_1A
                    case 0x4B: // HK_MOPP_TERM4_1B
                    case 0x4C: // HK_MOPP_TERM4_1C
                    case 0x4D: // HK_MOPP_TERM4_1D
                    case 0x4E: // HK_MOPP_TERM4_1E
                    case 0x4F: // HK_MOPP_TERM4_1F
                               // TODO: Does this function take any operands?
                        break;

                    case 0x50: // HK_MOPP_TERM8
                        i += 1;
                        break;

                    case 0x51: // HK_MOPP_TERM16
                        i += 2;
                        break;

                    case 0x52: // HK_MOPP_TERM24
                        i += 3;
                        break;

                    case 0x53: // HK_MOPP_TERM32
                        i += 4;
                        break;

                    case 0x54: // HK_MOPP_NTERM_8
                        i += 1;
                        break;

                    case 0x55: // HK_MOPP_NTERM_16
                        i += 2;
                        break;

                    case 0x56: // HK_MOPP_NTERM_24
                        i += 3;
                        break;

                    case 0x57: // HK_MOPP_NTERM_32
                        i += 4;
                        break;

                    /*case 0x58: // UNUSED
                    case 0x59: // UNUSED
                    case 0x5A: // UNUSED
                    case 0x5B: // UNUSED
                    case 0x5C: // UNUSED
                    case 0x5D: // UNUSED
                    case 0x5E: // UNUSED
                    case 0x5F: // UNUSED
                        break;*/

                    case 0x60: // HK_MOPP_PROPERTY8_0
                    case 0x61: // HK_MOPP_PROPERTY8_1
                    case 0x62: // HK_MOPP_PROPERTY8_2
                    case 0x63: // HK_MOPP_PROPERTY8_3
                        i += 1;
                        break;

                    case 0x64: // HK_MOPP_PROPERTY16_0
                    case 0x65: // HK_MOPP_PROPERTY16_1
                    case 0x66: // HK_MOPP_PROPERTY16_2
                    case 0x67: // HK_MOPP_PROPERTY16_3
                        i += 2;
                        break;

                    case 0x68: // HK_MOPP_PROPERTY32_0
                    case 0x69: // HK_MOPP_PROPERTY32_1
                    case 0x6A: // HK_MOPP_PROPERTY32_2
                    case 0x6B: // HK_MOPP_PROPERTY32_3
                        i += 4;
                        break;

                    default:
                        throw new NotSupportedException($"Opcode 0x{moppCode:X2}");
                }

                if (!moppCodes.Contains(moppCode))
                    moppCodes.Add(moppCode);
            }
        }
    }
}