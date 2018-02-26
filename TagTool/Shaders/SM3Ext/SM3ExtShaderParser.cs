using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TagTool.IO;

namespace TagTool.Shaders.SM3Ext
{
    class SM3ExtShaderParser
    {
        public enum ShaderType
        {
            Vertex,
            Pixel
        }
        public ShaderType Type { get; }
        public byte[] ShaderConstantsBinary { get; }
        public byte[] ShaderBinary { get; }
        public List<SM3ExtInstruction> Instructions { get; }

        public SM3ExtShaderParser(ShaderType type, byte[] shader_binary, byte[] constant_data)
        {
            Type = type;
            ShaderBinary = shader_binary;
            ShaderConstantsBinary = constant_data;

            if (constant_data != null) ReadConstantData(constant_data);

            var raw_shader_code = XSDDisassemble(Type, ShaderBinary);
            SM3ExtInstruction previous_instruction = null;
            var instruction_codes = raw_shader_code.Split(new[] { "\r\n", "\r", "\n", Environment.NewLine }, StringSplitOptions.None).ToList();

            Instructions = new List<SM3ExtInstruction>();

            foreach (var _instruction_code in instruction_codes)
            {
                // Take the left side of any comments
                var components = _instruction_code.Split(new[] { "//" }, StringSplitOptions.None);
                var instruction_code = components[0];
                var comment = components.Length > 1 ? String.Join("//", components.Skip(1)) : null;
                instruction_code = instruction_code.TrimEnd();

                if (String.IsNullOrWhiteSpace(instruction_code))
                {
                    continue;
                }

                var instruction = new SM3ExtInstruction(instruction_code, previous_instruction);
                instruction.Comment = comment;
                Instructions.Add(instruction);

                previous_instruction = instruction;
            }

            PostProcess();
        }

        public class SM3ExtConstantRegisterData
        {
            public float X, Y, Z, W;

            public SM3ExtConstantRegisterData(EndianReader reader)
            {
                X = reader.ReadSingle();
                Y = reader.ReadSingle();
                Z = reader.ReadSingle();
                W = reader.ReadSingle();
            }

            public SM3ExtConstantRegisterData()
            {
                X = 0.0f;
                Y = 0.0f;
                Z = 0.0f;
                W = 0.0f;
            }

            public SM3ExtConstantRegisterData(float x, float y, float z, float w)
            {
                X = x;
                Y = y;
                Z = z;
                W = w;
            }

            public override string ToString()
            {
                return $"x:{X} y:{Y} z:{Z} w:{W}";
            }
        }

        public Dictionary<int, SM3ExtConstantRegisterData> ConstantDataDefinitions = new Dictionary<int, SM3ExtShaderParser.SM3ExtConstantRegisterData>();

        // Linear
        //private void ReadConstantData(byte[] constant_data)
        //{
        //    using (MemoryStream memoryStream = new MemoryStream(constant_data))
        //    using (EndianReader reader = new EndianReader(memoryStream, EndianFormat.BigEndian))
        //    {
        //        int index = 223;
        //        while (!reader.EOF)
        //        {

        //            SM3ExtConstantRegisterData data0 = new SM3ExtConstantRegisterData(reader);
        //            ConstantDataDefinitions[index++] = data0;
        //        }
        //    }

        //    for (var i = 223; i <= 255; i++)
        //    {
        //        if (ConstantDataDefinitions.ContainsKey(i))
        //        {
        //            Console.WriteLine($"c{i} {ConstantDataDefinitions[i]}");
        //        }
        //    }
        //}


        // Reversed
        private void ReadConstantData(byte[] constant_data)
        {
            using (MemoryStream memoryStream = new MemoryStream(constant_data))
            using (EndianReader reader = new EndianReader(memoryStream, EndianFormat.BigEndian))
            {
                int index = 255;
                while (!reader.EOF)
                {

                    SM3ExtConstantRegisterData data0 = new SM3ExtConstantRegisterData(reader);
                    SM3ExtConstantRegisterData data1 = new SM3ExtConstantRegisterData(reader);
                    SM3ExtConstantRegisterData data2 = new SM3ExtConstantRegisterData(reader);
                    SM3ExtConstantRegisterData data3 = new SM3ExtConstantRegisterData(reader);

                    ConstantDataDefinitions[index--] = data3;
                    ConstantDataDefinitions[index--] = data2;
                    ConstantDataDefinitions[index--] = data1;
                    ConstantDataDefinitions[index--] = data0;
                }
            }

            for (var i = 223; i <= 255; i++)
            {
                if (ConstantDataDefinitions.ContainsKey(i))
                {
                    Console.WriteLine($"c{i} {ConstantDataDefinitions[i]}");
                }
            }
        }

        public virtual void PostProcess()
        {

        }

        public static string XSDDisassemble(ShaderType type, byte[] data)
        {
            if (data == null) return null;

            if (!File.Exists(@"Tools\xsd.exe"))
            {
                Console.WriteLine("Missing tools, please install xsd.exe before porting shaders.");
                return null;
            }

            WriteOutput(OutputFile.ShaderData, data);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"Tools\xsd.exe",
                    Arguments = type == ShaderType.Vertex ? "/rawvs permutation.shader" : "/rawps permutation.shader",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "temp")
                }
            };
            process.Start();

            string shader_code = process.StandardOutput.ReadToEnd();
            string err = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!String.IsNullOrWhiteSpace(err)) throw new Exception(err);

            //var shader_instructionset = type == ShaderType.Vertex ? "xvs_3_0" : "xps_3_0";
            //shader_code = $"   {shader_instructionset}\n{shader_code}";

            return shader_code;
        }

        public enum OutputFile
        {
            ShaderData,
            DebugData,
            ConstantData
        }

        public static void WriteOutput(OutputFile file, byte[] data)
        {
            Directory.CreateDirectory(@"Temp");

            if (file == OutputFile.ShaderData)
            {
                WriteOutput(@"Temp\permutation.shader", data);
            }

            if (file == OutputFile.ConstantData)
            {
                WriteOutput(@"Temp\permutation.shader.updb", data);
            }

            if (file == OutputFile.ShaderData)
            {
                WriteOutput(@"Temp\permutation.shader.cbin", data);
            }
        }

        private static void WriteOutput(string file, byte[] data)
        {
            if (File.Exists(file)) File.Delete(file);
            if (data.Length > 0)
                using (EndianWriter output = new EndianWriter(File.OpenWrite(file), EndianFormat.BigEndian))
                    output.WriteBlock(data);
        }
    }
}
