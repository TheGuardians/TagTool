using TagTool.Cache;
using TagTool.Havok;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.IO;

namespace TagTool.Commands.ScenarioStructureBSPs
{
    class MoppDataCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private ScenarioStructureBsp Definition { get; }

        public MoppDataCommand(HaloOnlineCacheContext cacheContext, ScenarioStructureBsp bsp) :
            base(true,

                "MoppData",
                "Modify Mopp data",

                "MoppData <export/import/parse> <output/input file>",

                "")
        {
            CacheContext = cacheContext;
            Definition = bsp;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            string mode = args[0].ToLower();
            string file = args[1].ToLower();

            if (mode.Equals("export"))
            {
                ExportMopps(file);
            }
            else if (mode.Equals("import"))
            {
                ImportMopps(file);
            }
            else if(mode.Equals("parse"))
            {
                ParseMopps(file);
            }
            else
            {
                Console.WriteLine($"Invalid mode {mode}");
            }




            
            return true;
        }

        private void ImportMopps(string file)
        {
            Console.WriteLine($"Don't forget to savetagchanges after importing new mopp data!");

            if (!File.Exists(file))
            {
                Console.WriteLine($"File {file} does not exists!");
                return;
            }
                

            List<CollisionMoppCode.Datum> newMoppData = new List<CollisionMoppCode.Datum>();
            using (var reader = new EndianReader(File.Open(file, FileMode.Open)))
            {
                for(int i =0; i < reader.Length; i++)
                {
                    var newMopp = new CollisionMoppCode.Datum
                    {
                        Value = reader.ReadByte()
                    };
                    newMoppData.Add(newMopp);
                }
            }

            var moppData = Definition.CollisionMoppCodes[0];
            moppData.DataSize = newMoppData.Count;
            moppData.DataCapacityAndFlags = (uint)(moppData.DataSize + 0x80000000);
            moppData.Data = newMoppData;


        }

        private void ExportMopps(string file)
        {
            var moppData = Definition.CollisionMoppCodes[0];
            using (var writer = new EndianWriter(File.Create(file), EndianFormat.LittleEndian))
            {
                for(int i = 0; i< moppData.Data.Count; i++)
                {
                    writer.Write(moppData.Data[i].Value);
                }
            }
        }

        private void ParseMopps(string file)
        {
            using (var fileStream = File.Create(file))
            using (var fileWriter = new StreamWriter(fileStream))
            {

                var moppData = Definition.CollisionMoppCodes[0].Data;
                var print = true;
                for (var i = 0; i < moppData.Count; i++)
                {
                    var moppOperator = moppData[i].Value;
                    print = true;

                    var count = 0;
                    switch (moppOperator)
                    {
                        case 0x00: // HK_MOPP_RETURN
                            break;

                        case 0x01: // HK_MOPP_SCALE1
                        case 0x02: // HK_MOPP_SCALE2
                        case 0x03: // HK_MOPP_SCALE3
                        case 0x04: // HK_MOPP_SCALE4
                            count = 3;
                            break;

                        case 0x05: // HK_MOPP_JUMP8
                            count = 1;
                            break;

                        case 0x06: // HK_MOPP_JUMP16
                            count = 2;
                            break;

                        case 0x07: // HK_MOPP_JUMP24
                            count = 3;
                            break;

                        /*case 0x08: // HK_MOPP_JUMP32 (NOT IMPLEMENTED)
                            Array.Reverse(moppData, i + 1, 4);
                            count = 4;
                            break;*/

                        case 0x09: // HK_MOPP_TERM_REOFFSET8
                            count = 1;
                            break;

                        case 0x0A: // HK_MOPP_TERM_REOFFSET16
                            count = 2;
                            break;

                        case 0x0B: // HK_MOPP_TERM_REOFFSET32
                            count = 4;
                            print = true;
                            break;

                        case 0x0C: // HK_MOPP_JUMP_CHUNK
                            count = 2;
                            break;

                        case 0x0D: // HK_MOPP_DATA_OFFSET
                            count = 5;
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
                            count = 3;
                            break;

                        /*case 0x1D: // UNUSED
                        case 0x1E: // UNUSED
                        case 0x1F: // UNUSED
                            break;*/

                        case 0x20: // HK_MOPP_SINGLE_SPLIT_X
                        case 0x21: // HK_MOPP_SINGLE_SPLIT_Y
                        case 0x22: // HK_MOPP_SINGLE_SPLIT_Z
                            count = 2;
                            break;

                        case 0x23: // HK_MOPP_SPLIT_JUMP_X
                        case 0x24: // HK_MOPP_SPLIT_JUMP_Y
                        case 0x25: // HK_MOPP_SPLIT_JUMP_Z
                            count = 6;
                            break;


                        case 0x26: // HK_MOPP_DOUBLE_CUT_X
                        case 0x27: // HK_MOPP_DOUBLE_CUT_Y
                        case 0x28: // HK_MOPP_DOUBLE_CUT_Z
                            count = 2;
                            break;

                        case 0x29: // HK_MOPP_DOUBLE_CUT24_X
                        case 0x2A: // HK_MOPP_DOUBLE_CUT24_Y
                        case 0x2B: // HK_MOPP_DOUBLE_CUT24_Z
                            count = 6;
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
                            count = 1;
                            break;

                        case 0x51: // HK_MOPP_TERM16
                            count = 2;
                            break;

                        case 0x52: // HK_MOPP_TERM24
                            count = 3;
                            break;

                        case 0x53: // HK_MOPP_TERM32
                            count = 4;
                            break;

                        case 0x54: // HK_MOPP_NTERM_8
                            count = 1;
                            break;

                        case 0x55: // HK_MOPP_NTERM_16
                            count = 2;
                            break;

                        case 0x56: // HK_MOPP_NTERM_24
                            count = 3;
                            break;

                        case 0x57: // HK_MOPP_NTERM_32
                            count = 4;
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
                            count = 1;
                            break;

                        case 0x64: // HK_MOPP_PROPERTY16_0
                        case 0x65: // HK_MOPP_PROPERTY16_1
                        case 0x66: // HK_MOPP_PROPERTY16_2
                        case 0x67: // HK_MOPP_PROPERTY16_3
                            count = 2;
                            break;

                        case 0x68: // HK_MOPP_PROPERTY32_0
                        case 0x69: // HK_MOPP_PROPERTY32_1
                        case 0x6A: // HK_MOPP_PROPERTY32_2
                        case 0x6B: // HK_MOPP_PROPERTY32_3
                            count = 4;
                            break;

                        default:
                            throw new NotSupportedException($"Opcode 0x{moppOperator:X2}");
                    }
                    if (print)
                    {
                        string lineIndex = $"0x{i:X8}";
                        string description = GetMoppDescription(moppOperator);
                        int descriptionMaxLength = 40;
                        description = description.PadRight(descriptionMaxLength);
                        string data = GetMoppDataString(moppOperator, moppData, i+1, count);
                        string parsedLine = $"{lineIndex}: 0x{moppOperator:X2}: {description} {data}";

                        fileWriter.Write(parsedLine);

                        //fileWriter.Write($"{moppOperator.ToString("X2")}");
                        //ReadArguments(count, i, moppData, fileWriter);
                        fileWriter.Write(Environment.NewLine);
                    }
                    i += count;

                }
            }
        }

        private void ReadArguments(int count,int offset, List<CollisionMoppCode.Datum> moppData, StreamWriter fileWriter)
        {
            for(int i = 0; i < count; i++)
            {
                fileWriter.Write($":{moppData[offset+i+1].Value.ToString("X2")}");
            }
            return;
        }

        private string GetMoppDescription(int opcode)
        {
            string result = "Undefined";
            switch (opcode)
            {
                case 0x00: // HK_MOPP_RETURN
                    result = "Return";
                    break;

                case 0x01: // HK_MOPP_SCALE1
                    result = "Scale 1";
                    break;

                case 0x02: // HK_MOPP_SCALE2
                    result = "Scale 2";
                    break;
                case 0x03: // HK_MOPP_SCALE3
                    result = "Scale 3";
                    break;
                case 0x04: // HK_MOPP_SCALE4
                    result = "Scale 4";
                    break;

                case 0x05: // HK_MOPP_JUMP8
                    result = "Jump with 8 bits";
                    break;

                case 0x06: // HK_MOPP_JUMP16
                    result = "Jump with 16 bits";
                    break;

                case 0x07: // HK_MOPP_JUMP24
                    result = "Jump with 24 bits";
                    break;

                case 0x08: // HK_MOPP_JUMP32 (NOT IMPLEMENTED)
                    result = "Jump with 32 bits";
                    break;

                case 0x09: // HK_MOPP_TERM_REOFFSET8
                    result = "Reoffset with 8 bits";
                    break;

                case 0x0A: // HK_MOPP_TERM_REOFFSET16
                    result = "Reoffset with 16 bits";
                    break;

                case 0x0B: // HK_MOPP_TERM_REOFFSET32
                    result = "Reoffset with 32 bits";
                    break;

                case 0x0C: // HK_MOPP_JUMP_CHUNK
                    result = "Jump chunk";
                    break;

                case 0x0D: // HK_MOPP_DATA_OFFSET
                    result = "Data offset";
                    break;

                case 0x0E: // UNUSED
                case 0x0F: // UNUSED
                    break;

                case 0x10: // HK_MOPP_SPLIT_X
                    result = "Split X axis";
                    break;
                case 0x11: // HK_MOPP_SPLIT_Y
                    result = "Split Y axis";
                    break;
                case 0x12: // HK_MOPP_SPLIT_Z
                    result = "Split Z axis";
                    break;
                case 0x13: // HK_MOPP_SPLIT_YZ
                    result = "Split YZ axis";
                    break;
                case 0x14: // HK_MOPP_SPLIT_YMZ
                    result = "Split YMZ axis";
                    break;
                case 0x15: // HK_MOPP_SPLIT_XZ
                    result = "Split XZ axis";
                    break;
                case 0x16: // HK_MOPP_SPLIT_XMZ
                    result = "Split XMZ axis";
                    break;
                case 0x17: // HK_MOPP_SPLIT_XY
                    result = "Split XY axis";
                    break;
                case 0x18: // HK_MOPP_SPLIT_XMY
                    result = "Split XMY axis";
                    break;
                case 0x19: // HK_MOPP_SPLIT_XYZ
                    result = "Split XYZ axis";
                    break;
                case 0x1A: // HK_MOPP_SPLIT_XYMZ
                    result = "Split XYMZ axis";
                    break;
                case 0x1B: // HK_MOPP_SPLIT_XMYZ
                    result = "Split XMYZ axis";
                    break;
                case 0x1C: // HK_MOPP_SPLIT_XMYMZ
                    result = "Split XMYMZ axis";
                    break;

                case 0x1D: // UNUSED
                case 0x1E: // UNUSED
                case 0x1F: // UNUSED
                    break;

                case 0x20: // HK_MOPP_SINGLE_SPLIT_X
                    result = "Single split X axis";
                    break;
                case 0x21: // HK_MOPP_SINGLE_SPLIT_Y
                    result = "Single split Y axis";
                    break;
                case 0x22: // HK_MOPP_SINGLE_SPLIT_Z
                    result = "Single split Z axis";
                    break;

                case 0x23: // HK_MOPP_SPLIT_JUMP_X
                    result = "Split jump X axis";
                    break;
                case 0x24: // HK_MOPP_SPLIT_JUMP_Y
                    result = "Split jump Y axis";
                    break;
                case 0x25: // HK_MOPP_SPLIT_JUMP_Z
                    result = "Split jump Z axis";
                    break;


                case 0x26: // HK_MOPP_DOUBLE_CUT_X
                    result = "Double cut X axis";
                    break;
                case 0x27: // HK_MOPP_DOUBLE_CUT_Y
                    result = "Double cut Y axis";
                    break;
                case 0x28: // HK_MOPP_DOUBLE_CUT_Z
                    result = "Double cut Z axis";
                    break;

                case 0x29: // HK_MOPP_DOUBLE_CUT24_X
                    result = "Double cut24 X axis";
                    break;
                case 0x2A: // HK_MOPP_DOUBLE_CUT24_Y
                    result = "Double cut24 Y axis";
                    break;
                case 0x2B: // HK_MOPP_DOUBLE_CUT24_Z
                    result = "Double cut24 Z axis";
                    break;

                case 0x2C: // UNUSED
                case 0x2D: // UNUSED
                case 0x2E: // UNUSED
                case 0x2F: // UNUSED
                    break;

                case 0x30: // HK_MOPP_TERM4_0
                    result = "Term 4 0";
                    break;
                case 0x31: // HK_MOPP_TERM4_1
                    result = "Term 4 1";
                    break;
                case 0x32: // HK_MOPP_TERM4_2
                    result = "Term 4 2";
                    break;
                case 0x33: // HK_MOPP_TERM4_3
                    result = "Term 4 3";
                    break;
                case 0x34: // HK_MOPP_TERM4_4
                    result = "Term 4 4";
                    break;
                case 0x35: // HK_MOPP_TERM4_5
                    result = "Term 4 5";
                    break;
                case 0x36: // HK_MOPP_TERM4_6
                    result = "Term 4 6";
                    break;
                case 0x37: // HK_MOPP_TERM4_7
                    result = "Term 4 7";
                    break;
                case 0x38: // HK_MOPP_TERM4_8
                    result = "Term 4 8";
                    break;
                case 0x39: // HK_MOPP_TERM4_9
                    result = "Term 4 9";
                    break;
                case 0x3A: // HK_MOPP_TERM4_A
                    result = "Term 4 A";
                    break;
                case 0x3B: // HK_MOPP_TERM4_B
                    result = "Term 4 B";
                    break;
                case 0x3C: // HK_MOPP_TERM4_C
                    result = "Term 4 C";
                    break;
                case 0x3D: // HK_MOPP_TERM4_D
                    result = "Term 4 D";
                    break;
                case 0x3E: // HK_MOPP_TERM4_E
                    result = "Term 4 E";
                    break;
                case 0x3F: // HK_MOPP_TERM4_F
                    result = "Term 4 F";
                    break;
                case 0x40: // HK_MOPP_TERM4_10
                    result = "Term 4 10";
                    break;
                case 0x41: // HK_MOPP_TERM4_11
                    result = "Term 4 11";
                    break;
                case 0x42: // HK_MOPP_TERM4_12
                    result = "Term 4 12";
                    break;
                case 0x43: // HK_MOPP_TERM4_13
                    result = "Term 4 13";
                    break;
                case 0x44: // HK_MOPP_TERM4_14
                    result = "Term 4 14";
                    break;
                case 0x45: // HK_MOPP_TERM4_15
                    result = "Term 4 15";
                    break;
                case 0x46: // HK_MOPP_TERM4_16
                    result = "Term 4 16";
                    break;
                case 0x47: // HK_MOPP_TERM4_17
                    result = "Term 4 17";
                    break;
                case 0x48: // HK_MOPP_TERM4_18
                    result = "Term 4 18";
                    break;
                case 0x49: // HK_MOPP_TERM4_19
                    result = "Term 4 19";
                    break;
                case 0x4A: // HK_MOPP_TERM4_1A
                    result = "Term 4 1A";
                    break;
                case 0x4B: // HK_MOPP_TERM4_1B
                    result = "Term 4 1B";
                    break;
                case 0x4C: // HK_MOPP_TERM4_1C
                    result = "Term 4 1C";
                    break;
                case 0x4D: // HK_MOPP_TERM4_1D
                    result = "Term 4 1D";
                    break;
                case 0x4E: // HK_MOPP_TERM4_1E
                    result = "Term 4 1E";
                    break;
                case 0x4F: // HK_MOPP_TERM4_1F
                    result = "Term 4 1F";
                    break;

                case 0x50: // HK_MOPP_TERM8
                    result = "Term 8";
                    break;

                case 0x51: // HK_MOPP_TERM16
                    result = "Term 16";
                    break;

                case 0x52: // HK_MOPP_TERM24
                    result = "Term 24";
                    break;

                case 0x53: // HK_MOPP_TERM32
                    result = "Term 32";
                    break;

                case 0x54: // HK_MOPP_NTERM_8
                    result = "Negative term 8";
                    break;

                case 0x55: // HK_MOPP_NTERM_16
                    result = "Negative term 16";
                    break;

                case 0x56: // HK_MOPP_NTERM_24
                    result = "Negative term 24";
                    break;

                case 0x57: // HK_MOPP_NTERM_32
                    result = "Negative term 32";
                    break;

                case 0x58: // UNUSED
                case 0x59: // UNUSED
                case 0x5A: // UNUSED
                case 0x5B: // UNUSED
                case 0x5C: // UNUSED
                case 0x5D: // UNUSED
                case 0x5E: // UNUSED
                case 0x5F: // UNUSED
                    break;

                case 0x60: // HK_MOPP_PROPERTY8_0
                    result = "Property 8 0";
                    break;
                case 0x61: // HK_MOPP_PROPERTY8_1
                    result = "Property 8 1";
                    break;
                case 0x62: // HK_MOPP_PROPERTY8_2
                    result = "Property 8 2";
                    break;
                case 0x63: // HK_MOPP_PROPERTY8_3
                    result = "Property 8 3";
                    break;

                case 0x64: // HK_MOPP_PROPERTY16_0
                    result = "Property 16 0";
                    break;
                case 0x65: // HK_MOPP_PROPERTY16_1
                    result = "Property 16 1";
                    break;
                case 0x66: // HK_MOPP_PROPERTY16_2
                    result = "Property 16 2";
                    break;
                case 0x67: // HK_MOPP_PROPERTY16_3
                    result = "Property 16 3";
                    break;

                case 0x68: // HK_MOPP_PROPERTY32_0
                    result = "Property 32 0";
                    break;
                case 0x69: // HK_MOPP_PROPERTY32_1
                    result = "Property 32 1";
                    break;
                case 0x6A: // HK_MOPP_PROPERTY32_2
                    result = "Property 32 2";
                    break;
                case 0x6B: // HK_MOPP_PROPERTY32_3
                    result = "Property 32 3";
                    break;

                default:
                    throw new NotSupportedException($"Opcode 0x{opcode:X2}");
            }
            return result;
        }

        private byte[] GetMoppData(List<CollisionMoppCode.Datum> moppData, int index, int count)
        {
            if (count == 0)
                return null;
            else
            {
                byte[] result = new byte[count];
                for(int i = 0; i< count; i++)
                {
                    result[i] = moppData[index + i].Value;
                }
                return result;
            }
        }

        private int GetInt8(byte[] data)
        {
            return data[0];
        }
        private int GetInt16(byte[] data)
        {
            return (data[0] << 8) + data[1];
        }

        private int GetInt24(byte[] data)
        {
            return (((data[0] << 8) + data[1])<<8) + data[2];
        }

        private int GetInt32(byte[] data)
        {
            return (((((data[0] << 8) + data[1]) << 8) + data[2]) << 8 ) + data[3];
        }

        private string GetMoppDataString(int opcode, List<CollisionMoppCode.Datum> moppData, int index, int count)
        {
            string result = "";
            byte[] data = GetMoppData(moppData, index, count);
            int offset;
            int address;
            switch (opcode)
            {
                case 0x00: // HK_MOPP_RETURN
                    break;

                case 0x01: // HK_MOPP_SCALE1
                    break;

                case 0x02: // HK_MOPP_SCALE2
                    break;
                case 0x03: // HK_MOPP_SCALE3
                    break;
                case 0x04: // HK_MOPP_SCALE4
                    break;

                case 0x05: // HK_MOPP_JUMP8
                    offset = GetInt8(data);
                    result = $"Offset = {offset:X8}";
                    break;

                case 0x06: // HK_MOPP_JUMP16
                    offset = GetInt16(data);
                    result = $"Offset = {offset:X8}";
                    break;

                case 0x07: // HK_MOPP_JUMP24
                    offset = GetInt24(data);
                    result = $"Offset = {offset:X8}";
                    break;

                case 0x08: // HK_MOPP_JUMP32 (NOT IMPLEMENTED)
                    offset = GetInt32(data);
                    result = $"Offset = {offset:X8}";
                    break;

                case 0x09: // HK_MOPP_TERM_REOFFSET8
                    break;

                case 0x0A: // HK_MOPP_TERM_REOFFSET16
                    break;

                case 0x0B: // HK_MOPP_TERM_REOFFSET32
                    break;

                case 0x0C: // HK_MOPP_JUMP_CHUNK
                    break;

                case 0x0D: // HK_MOPP_DATA_OFFSET
                    break;

                case 0x0E: // UNUSED
                case 0x0F: // UNUSED
                    break;

                case 0x10: // HK_MOPP_SPLIT_X
                    break;
                case 0x11: // HK_MOPP_SPLIT_Y
                    break;
                case 0x12: // HK_MOPP_SPLIT_Z
                    break;
                case 0x13: // HK_MOPP_SPLIT_YZ
                    break;
                case 0x14: // HK_MOPP_SPLIT_YMZ
                    break;
                case 0x15: // HK_MOPP_SPLIT_XZ
                    break;
                case 0x16: // HK_MOPP_SPLIT_XMZ
                    break;
                case 0x17: // HK_MOPP_SPLIT_XY
                    break;
                case 0x18: // HK_MOPP_SPLIT_XMY
                    break;
                case 0x19: // HK_MOPP_SPLIT_XYZ
                    break;
                case 0x1A: // HK_MOPP_SPLIT_XYMZ
                    break;
                case 0x1B: // HK_MOPP_SPLIT_XMYZ
                    break;
                case 0x1C: // HK_MOPP_SPLIT_XMYMZ
                    break;

                case 0x1D: // UNUSED
                case 0x1E: // UNUSED
                case 0x1F: // UNUSED
                    break;

                case 0x20: // HK_MOPP_SINGLE_SPLIT_X
                    break;
                case 0x21: // HK_MOPP_SINGLE_SPLIT_Y
                    break;
                case 0x22: // HK_MOPP_SINGLE_SPLIT_Z
                    break;

                case 0x23: // HK_MOPP_SPLIT_JUMP_X
                    break;
                case 0x24: // HK_MOPP_SPLIT_JUMP_Y
                    break;
                case 0x25: // HK_MOPP_SPLIT_JUMP_Z
                    break;


                case 0x26: // HK_MOPP_DOUBLE_CUT_X
                    break;
                case 0x27: // HK_MOPP_DOUBLE_CUT_Y
                    break;
                case 0x28: // HK_MOPP_DOUBLE_CUT_Z
                    break;

                case 0x29: // HK_MOPP_DOUBLE_CUT24_X
                    break;
                case 0x2A: // HK_MOPP_DOUBLE_CUT24_Y
                    break;
                case 0x2B: // HK_MOPP_DOUBLE_CUT24_Z
                    break;

                case 0x2C: // UNUSED
                case 0x2D: // UNUSED
                case 0x2E: // UNUSED
                case 0x2F: // UNUSED
                    break;

                case 0x30: // HK_MOPP_TERM4_0
                    break;
                case 0x31: // HK_MOPP_TERM4_1
                    break;
                case 0x32: // HK_MOPP_TERM4_2
                    break;
                case 0x33: // HK_MOPP_TERM4_3
                    break;
                case 0x34: // HK_MOPP_TERM4_4
                    break;
                case 0x35: // HK_MOPP_TERM4_5
                    break;
                case 0x36: // HK_MOPP_TERM4_6
                    break;
                case 0x37: // HK_MOPP_TERM4_7
                    break;
                case 0x38: // HK_MOPP_TERM4_8
                    break;
                case 0x39: // HK_MOPP_TERM4_9
                    break;
                case 0x3A: // HK_MOPP_TERM4_A
                    break;
                case 0x3B: // HK_MOPP_TERM4_B
                    break;
                case 0x3C: // HK_MOPP_TERM4_C
                    break;
                case 0x3D: // HK_MOPP_TERM4_D
                    break;
                case 0x3E: // HK_MOPP_TERM4_E
                    break;
                case 0x3F: // HK_MOPP_TERM4_F
                    break;
                case 0x40: // HK_MOPP_TERM4_10
                    break;
                case 0x41: // HK_MOPP_TERM4_11
                    break;
                case 0x42: // HK_MOPP_TERM4_12
                    break;
                case 0x43: // HK_MOPP_TERM4_13
                    break;
                case 0x44: // HK_MOPP_TERM4_14
                    break;
                case 0x45: // HK_MOPP_TERM4_15
                    break;
                case 0x46: // HK_MOPP_TERM4_16
                    break;
                case 0x47: // HK_MOPP_TERM4_17
                    break;
                case 0x48: // HK_MOPP_TERM4_18
                    break;
                case 0x49: // HK_MOPP_TERM4_19
                    break;
                case 0x4A: // HK_MOPP_TERM4_1A
                    break;
                case 0x4B: // HK_MOPP_TERM4_1B
                    break;
                case 0x4C: // HK_MOPP_TERM4_1C
                    break;
                case 0x4D: // HK_MOPP_TERM4_1D
                    break;
                case 0x4E: // HK_MOPP_TERM4_1E
                    break;
                case 0x4F: // HK_MOPP_TERM4_1F
                    break;

                case 0x50: // HK_MOPP_TERM8
                    break;

                case 0x51: // HK_MOPP_TERM16
                    break;

                case 0x52: // HK_MOPP_TERM24
                    break;

                case 0x53: // HK_MOPP_TERM32
                    break;

                case 0x54: // HK_MOPP_NTERM_8
                    break;

                case 0x55: // HK_MOPP_NTERM_16
                    break;

                case 0x56: // HK_MOPP_NTERM_24
                    break;

                case 0x57: // HK_MOPP_NTERM_32
                    break;

                case 0x58: // UNUSED
                case 0x59: // UNUSED
                case 0x5A: // UNUSED
                case 0x5B: // UNUSED
                case 0x5C: // UNUSED
                case 0x5D: // UNUSED
                case 0x5E: // UNUSED
                case 0x5F: // UNUSED
                    break;

                case 0x60: // HK_MOPP_PROPERTY8_0
                    break;
                case 0x61: // HK_MOPP_PROPERTY8_1
                    break;
                case 0x62: // HK_MOPP_PROPERTY8_2
                    break;
                case 0x63: // HK_MOPP_PROPERTY8_3
                    break;

                case 0x64: // HK_MOPP_PROPERTY16_0
                    break;
                case 0x65: // HK_MOPP_PROPERTY16_1
                    break;
                case 0x66: // HK_MOPP_PROPERTY16_2
                    break;
                case 0x67: // HK_MOPP_PROPERTY16_3
                    break;

                case 0x68: // HK_MOPP_PROPERTY32_0
                    break;
                case 0x69: // HK_MOPP_PROPERTY32_1
                    break;
                case 0x6A: // HK_MOPP_PROPERTY32_2;
                    break;
                case 0x6B: // HK_MOPP_PROPERTY32_3
                    break;

                default:
                    throw new NotSupportedException($"Opcode 0x{opcode:X2}");
            }
            return result;
        }

        
    }
}