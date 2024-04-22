using System;
using System.Collections.Generic;

namespace TagTool.Havok
{
    public static class HavokMoppUtil
    {
        public static IList<int> GetAllKeys(IList<byte> code)
        {
            var keys = new List<int>();
            GetAllKeysRecursive(keys, code, 0, 0);
            return keys;
        }

        private static void GetAllKeysRecursive(IList<int> keys, IList<byte> code, int index = 0, int offset = 0)
        {
            int terminal;
            while (true)
            {
                switch (code[index])
                {
                    case 0: // HK_MOPP_RETURN
                        return;
                    case 0x1: // HK_MOPP_SCALE1
                    case 0x2: // HK_MOPP_SCALE2
                    case 0x3: // HK_MOPP_SCALE3
                    case 0x4: // HK_MOPP_SCALE4
                        index += 4;
                        break;
                    case 0x5: // HK_MOPP_JUMP8
                        index += code[index + 1] + 2;
                        break;
                    case 0x6: // HK_MOPP_JUMP16
                        index += 256 * code[index + 1] + code[index + 2] + 3;
                        break;
                    case 0x7: // HK_MOPP_JUMP24
                        index += 256 * (code[index + 2] + (code[index + 1] << 8)) + code[index + 3] + 4;
                        break;
                    case 0x9: // HK_MOPP_TERM_REOFFSET8
                        offset += code[index + 1];
                        index += 2;
                        break;
                    case 0xA: // HK_MOPP_TERM_REOFFSET16
                        offset += code[index + 2] + (code[index + 1] << 8);
                        index += 3;
                        break;
                    case 0xB: // HK_MOPP_TERM_REOFFSET32
                        offset = code[index + 4] + ((code[index + 3] + ((code[index + 2] + (code[index + 1] << 8)) << 8)) << 8);
                        index += 5;
                        break;
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
                        {
                            var v6 = code[index + 3];
                            var v7 = index + 4;
                            GetAllKeysRecursive(keys, code, v7, offset);
                            index = v7 + v6;
                            break;
                        }
                    case 0x20: // HK_MOPP_SINGLE_SPLIT_X
                    case 0x21: // HK_MOPP_SINGLE_SPLIT_Y
                    case 0x22: // HK_MOPP_SINGLE_SPLIT_Z
                        {
                            var v8 = code[index + 2];
                            var v9 = index + 3;
                            GetAllKeysRecursive(keys, code, v9, offset);
                            index = index + 3 + v8;
                            break;
                        }
                    case 0x23: // HK_MOPP_SPLIT_JUMP_X
                    case 0x24: // HK_MOPP_SPLIT_JUMP_Y
                    case 0x25: // HK_MOPP_SPLIT_JUMP_Z
                        {
                            var v10 = code[index + 6];
                            var v11 = code[index + 5];
                            var v12 = code[index + 4] + (code[index + 3] << 8);
                            var v13 = index + 7;
                            var v14 = v10 + (v11 << 8);
                            GetAllKeysRecursive(keys, code, v13 + v12, offset);
                            index = v13 + v14;
                            break;
                        }
                    case 0x26: // HK_MOPP_DOUBLE_CUT_X
                    case 0x27: // HK_MOPP_DOUBLE_CUT_Y
                    case 0x28: // HK_MOPP_DOUBLE_CUT_Z
                        index += 3;
                        break;
                    case 0x29: // HK_MOPP_DOUBLE_CUT24_X
                    case 0x2A: // HK_MOPP_DOUBLE_CUT24_Y
                    case 0x2B: // HK_MOPP_DOUBLE_CUT24_Z
                        index += 7;
                        break;
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
                        terminal = code[index] - 0x30;
                        goto TERM;
                    case 0x50: // HK_MOPP_TERM8
                        terminal = code[index + 1];
                        goto TERM;
                    case 0x51: // HK_MOPP_TERM16
                        terminal = code[index + 2] + (code[index + 1] << 8);
                        goto TERM;
                    case 0x52: // HK_MOPP_TERM24
                        terminal = code[index + 3] + ((code[index + 2] + (code[index + 1] << 8)) << 8);
                        goto TERM;
                    case 0x53: // HK_MOPP_TERM32
                        terminal = ((code[index + 2] + (code[index + 1] << 8)) << 16) + code[index + 4] + (code[index + 3] << 8);
                    TERM:
                        keys.Add(offset + terminal);
                        return;
                    case 0x60: // HK_MOPP_PROPERTY8_0
                    case 0x61: // HK_MOPP_PROPERTY8_1
                    case 0x62: // HK_MOPP_PROPERTY8_2
                    case 0x63: // HK_MOPP_PROPERTY8_3
                        index += 2;
                        break;
                    case 0x64: // HK_MOPP_PROPERTY16_0
                    case 0x65: // HK_MOPP_PROPERTY16_1
                    case 0x66: // HK_MOPP_PROPERTY16_2
                    case 0x67: // HK_MOPP_PROPERTY16_3
                        index += 3;
                        break;
                    case 0x68: // HK_MOPP_PROPERTY32_0
                    case 0x69: // HK_MOPP_PROPERTY32_1
                    case 0x6A: // HK_MOPP_PROPERTY32_2
                    case 0x6B: // HK_MOPP_PROPERTY32_3
                        index += 5;
                        break;
                    default:
                        throw new Exception("Invalid Mopp");
                }
            }
        }
    }
}
