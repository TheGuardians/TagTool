using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Geometry;
using TagTool.Tags.Definitions;

namespace TagTool.Shaders
{
    class XboxShaderParser<T, Tx>
    {
        UPDBParser UPDB;
        readonly string ShaderCode = null;
        readonly byte[] ShaderData = null;
        readonly T Tag;
        readonly Tx Block;
        public static bool IsVertexShader => typeof(T) == typeof(GlobalVertexShader) || typeof(T) == typeof(VertexShader);
        public static bool IsPixelShader => typeof(T) == typeof(GlobalPixelShader) || typeof(T) == typeof(PixelShader);

        public XboxShaderParser(T tag, Tx block, byte[] shader_data, UPDBParser updb_parser)
        {
            UPDB = updb_parser;
            ShaderData = shader_data;
            Tag = tag;
            Block = block;

            if (!File.Exists(@"Tools\xsd.exe"))
            {
                Console.WriteLine("Missing tools, please install xsd.exe before porting shaders.");
                return;
            }

            Directory.CreateDirectory(@"Temp");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"Tools\xsd.exe",
                    Arguments = IsVertexShader ? "/rawvs permutation.shader" : "/rawps permutation.shader",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "temp")
                }
            };
            process.Start();

            ShaderCode = process.StandardOutput.ReadToEnd();
            Console.WriteLine(ShaderCode);
            string err = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!String.IsNullOrWhiteSpace(err))
            {
                Console.WriteLine(err);
                return;
            }

            if (typeof(T) == typeof(GlobalVertexShader)) ProcessShader(Tag as GlobalVertexShader, Block as VertexShaderBlock);
            if (typeof(T) == typeof(VertexShader)) ProcessShader(Tag as VertexShader, Block as VertexShaderBlock);
            if (typeof(T) == typeof(GlobalPixelShader)) ProcessShader(Tag as GlobalPixelShader, Block as PixelShaderBlock);
            if (typeof(T) == typeof(PixelShader)) ProcessShader(Tag as PixelShader, Block as PixelShaderBlock);

        }

        void ProcessShader(GlobalVertexShader Tag, VertexShaderBlock Block)
        {
            var instructions = ShaderCode.Split(new[] { "\r\n", "\r", "\n", Environment.NewLine }, StringSplitOptions.None).ToList();
            for (var i = instructions.Count - 1; i >= 0; i--)
            {
                var instruction = instructions[i].Trim();

                // Take the left side of any comments
                instruction = instruction.Split(new[] { "//" }, StringSplitOptions.None)[0];

                if (String.IsNullOrWhiteSpace(instruction))
                {
                    instructions.RemoveAt(i);
                    continue;
                }

            }
            
            var code = "vs_3_0\n" + String.Join("\n", instructions);
            var bytecode = ShaderCompiler.Compile(code, "main", "vs_3_0", out string errors);

            if (ShaderCompiler.PrintError(errors))
            {
                Console.WriteLine("There was some errors. Shame. SHAME. SHAME!!!");
            }
                


            Console.WriteLine("Processed GlobalVertexShader");
        }

        void ProcessShader(VertexShader Tag, VertexShaderBlock Block)
        {


            Console.WriteLine("Processed GlobalVertexShader");
        }

        void ProcessShader(GlobalPixelShader Tag, PixelShaderBlock Block)
        {


            Console.WriteLine("Processed GlobalVertexShader");
        }

        void ProcessShader(PixelShader Tag, PixelShaderBlock Block)
        {


            Console.WriteLine("Processed GlobalVertexShader");
        }
    }
}
