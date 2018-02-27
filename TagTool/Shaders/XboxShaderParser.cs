using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Shaders.SM3;
using TagTool.Shaders.SM3Ext;
using TagTool.Tags.Definitions;

namespace TagTool.Shaders
{
    class XboxShaderParser
    {
        public UPDBParser UPDB;
        public byte[] ShaderData => IsVertexShader ? (Block as VertexShaderBlock).XboxShaderReference?.ShaderData : (Block as PixelShaderBlock).XboxShaderReference?.ShaderData;
        public byte[] DebugData => IsVertexShader ? (Block as VertexShaderBlock).XboxShaderReference?.DebugData : (Block as PixelShaderBlock).XboxShaderReference?.DebugData;
        public byte[] ConstantData => IsVertexShader ? (Block as VertexShaderBlock).XboxShaderReference?.ConstantData : (Block as PixelShaderBlock).XboxShaderReference?.ConstantData;

        public bool IsVertexShader => Block?.GetType() == typeof(VertexShaderBlock);
        public bool IsPixelShader => Block?.GetType() == typeof(PixelShaderBlock);
        public bool IsValid => !(CacheContext == null || Tag == null || Block == null || ShaderData == null || ShaderData.Length == 0);

        private GameCacheContext CacheContext { get; }

        public object Block { get; set; }
        public object Tag { get; set; }

        public XboxShaderParser(VertexShader tag, VertexShaderBlock block, GameCacheContext gamecachecontext, UPDBParser updb_parser = null) : this((object)tag, (object)block, gamecachecontext, updb_parser) { }
        public XboxShaderParser(PixelShader tag, PixelShaderBlock block, GameCacheContext gamecachecontext, UPDBParser updb_parser = null) : this((object)tag, (object)block, gamecachecontext, updb_parser) { }
        public XboxShaderParser(GlobalVertexShader tag, VertexShaderBlock block, GameCacheContext gamecachecontext, UPDBParser updb_parser = null) : this((object)tag, (object)block, gamecachecontext, updb_parser) { }
        public XboxShaderParser(GlobalPixelShader tag, PixelShaderBlock block, GameCacheContext gamecachecontext, UPDBParser updb_parser = null) : this((object)tag, (object)block, gamecachecontext, updb_parser) { }
        public XboxShaderParser(object tag, object block, GameCacheContext gamecachecontext, UPDBParser updb_parser = null)
        {
            CacheContext = gamecachecontext;
            Block = block;
            Tag = tag;
            if (!IsValid) return;
            WriteOutput(OutputFile.ShaderData);
            WriteOutput(OutputFile.ConstantData);
            WriteOutput(OutputFile.DebugData);


            //UPDB = updb_parser != null ? updb_parser : new UPDBParser(DebugData); //TODO: It appears that the current format is for Vertex uPDB's only...

            //if (typeof(T) == typeof(GlobalVertexShader)) ProcessShader(Tag as GlobalVertexShader, Block as VertexShaderBlock);
            //if (typeof(T) == typeof(VertexShader)) ProcessShader(Tag as VertexShader, Block as VertexShaderBlock);
            //if (typeof(T) == typeof(GlobalPixelShader)) ProcessShader(Tag as GlobalPixelShader, Block as PixelShaderBlock);
            //if (typeof(T) == typeof(PixelShader)) ProcessShader(Tag as PixelShader, Block as PixelShaderBlock);

        }

        private List<ShaderParameter> GetParameters()
        {
            if (Tag.GetType() == typeof(PixelShader)) return GetParameters(Tag as PixelShader);
            throw new NotImplementedException();
        }
        private List<ShaderParameter> GetParameters(PixelShader Tag)
        {
            var Block = this.Block as PixelShaderBlock;
            return Block.XboxParameters.ToList();
        }
        private string GetShaderParameterTypeString(ShaderParameter.RType type)
        {
            if (type == ShaderParameter.RType.Boolean) return "b";
            if (type == ShaderParameter.RType.Float) return "c";
            if (type == ShaderParameter.RType.Integer) return "i";
            if (type == ShaderParameter.RType.Sampler) return "s";
            throw new NotImplementedException();
        }
        private string GetShaderParameterNameString(ShaderParameter.RType type)
        {
            if (type == ShaderParameter.RType.Boolean) return "bool";
            if (type == ShaderParameter.RType.Float) return "float4";
            if (type == ShaderParameter.RType.Integer) return "integer";
            if (type == ShaderParameter.RType.Sampler) return "sampler";
            throw new NotImplementedException();
        }

        public string GetParametersBlock()
        {
            StringBuilder sb = new StringBuilder();
            var parameters = GetParameters();
            foreach (var parameter in parameters)
            {
                sb.AppendLine($"//   {GetShaderParameterNameString(parameter.RegisterType)} {CacheContext.GetString(parameter.ParameterName)};");
            }
            return sb.ToString();

        }

        private string FormatRegistersBlockEntry(string _name, string _reg, string _size, int max_name_length, int reg_length, int size_length)
        {
            string name_col = $"{_name}{new String(' ', max_name_length - _name.Length + 1)}";
            string reg_col = $"{_reg}{new String(' ', reg_length - _reg.Length + 1)}";
            string size_col = $"{new String(' ', size_length - _size.Length)}{_size}";
            return name_col + reg_col + size_col;
        }

        public string GetRegistersBlock()
        {
            int max_name_length = 14;
            int reg_length = 5;
            int size_length = 4;

            var registers = GetParameters();
            foreach (var register in registers)
            {
                var name = CacheContext.GetString(register.ParameterName);
                max_name_length = Math.Max(max_name_length, name.Length);
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"//   {FormatRegistersBlockEntry("Name", "Reg", "Size", max_name_length, reg_length, size_length)}");
            sb.AppendLine($"//   {FormatRegistersBlockEntry(new String('-', max_name_length), new String('-', reg_length), new String('-', size_length), max_name_length, reg_length, size_length)}");
            foreach (var register in registers)
            {
                string name = CacheContext.GetString(register.ParameterName);
                string reg = $"{GetShaderParameterTypeString(register.RegisterType)}{register.RegisterIndex}";
                string size = register.RegisterCount.ToString();
                sb.AppendLine($"//   {FormatRegistersBlockEntry(name, reg, size, max_name_length, reg_length, size_length)}");
            }
            return sb.ToString();
        }

        public string Disassemble(string _raw_shader_code = null)
        {
            var raw_shader_code = _raw_shader_code == null ? SM3Ext.SM3ExtShaderParser.XSDDisassemble(IsVertexShader ? SM3ExtShaderParser.ShaderType.Vertex : SM3ExtShaderParser.ShaderType.Pixel, ShaderData) : _raw_shader_code;
            var formatted_raw_block = CommentStringBlock(raw_shader_code, true);
            var converted_shader_code = ConvertXboxShader(raw_shader_code);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("//");
            sb.AppendLine($"// Generated by Tag Tool {Environment.Version}");
            sb.AppendLine("//");
            sb.AppendLine("// Parameters:");
            sb.Append(GetParametersBlock());
            sb.AppendLine("//");
            sb.AppendLine("//");
            sb.AppendLine("// Registers:");
            sb.AppendLine("//");
            sb.Append(GetRegistersBlock());
            sb.AppendLine("//");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("// Converted PC Shader Assembly");
            sb.AppendLine(converted_shader_code);
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("// Raw Xbox 360 Shader Assembly");
            sb.AppendLine(formatted_raw_block);
            sb.AppendLine();
            sb.AppendLine();

            return sb.ToString();
        }

        class ShaderConstant
        {
            public string Name { get; set; } = null;
            public int Index { get; }
            public float[] Data { get; } = new float[4];
            public float X { get => Data[0]; set => Data[0] = value; }
            public float Y { get => Data[1]; set => Data[1] = value; }
            public float Z { get => Data[2]; set => Data[2] = value; }
            public float W { get => Data[3]; set => Data[3] = value; }


            public ShaderConstant(int index, string name = null)
            {
                Name = name;
                Index = index;
                X = 0;
                Y = 0;
                Z = 0;
                W = 0;
            }

            public ShaderConstant(int index, float value, string name = null)
            {
                Name = name;
                Index = index;
                X = value;
                Y = value;
                Z = value;
                W = value;
            }

            public ShaderConstant(int index, float x, float y, float z, float w, string name = null)
            {
                Name = name;
                Index = index;
                X = x;
                Y = y;
                Z = z;
                W = w;
            }
        }

        class PixelDecalTemplatesFixups : SM3ShaderConverter
        {
            public PixelDecalTemplatesFixups(SM3ExtShaderParser parser, GameCacheContext context, List<ShaderParameter> shader_parameters) : base(parser, context, shader_parameters)
            {
            }

            public override void PostProcess()
            {
                base.PostProcess();

                //TODO: There may be more exports to come, testing is required


                // Vertex -> Pixel Declarations
                {
                    var inputs = new List<SM3Instruction>
                    {
                        new SM3Instruction("dcl_texcoord", new List<string> { $"v0" }),
                        new SM3Instruction("dcl_texcoord1", new List<string> { $"v1.x" }),
                        new SM3Instruction("dcl_texcoord2", new List<string> { $"v2.xyz" }),
                        new SM3Instruction("dcl_texcoord3", new List<string> { $"v3.xyz" }),
                        new SM3Instruction("dcl_texcoord4", new List<string> { $"v4.xyz" }),
                    };
                    Instructions.InsertRange(0, inputs);
                }

                // replace registers until changes
                //r0 => v0
                //r1 => v1
                //r2 => v2
                //r3 => v3
                //r4 => v4
                ReplaceInputRegister("r0", "v0"); // Confirmed


                /*100% this order
                 * 
                 * mul r3.xyz, r0.y, v3.xzy
                    mad r0.xyz, r0.x, v2.xzy, r3.xyz
                    mad r0.yzw, r0.w, v4.xxyz, r0.xxzy
                 * 
                 * 
                 * */

                ReplaceInputRegister("r3", "v2");
                ReplaceInputRegister("r2", "v4");
                ReplaceInputRegister("r1", "v3");


                // The worlds shittest fix that probbaly doesn't work for ever shader
                // But hey, how about you try better for you judge this fair effort

                //TODO:
                // Convert Normal Map using (x * 2) - 1 formula for normals
                // DATA: def c0, 2.00787401, -1.00787401, 0, 0
                // Example. mad r0.xy, r1, c0.x, c0.y
                // This needs to be done on the normals.

                var bump_map = this.Parameters.Where(param => CacheContext.GetString(param.ParameterName) == "bump_map").FirstOrDefault();
                if (bump_map != null)
                {
                    // This fixup is 200% shit. Butt fuck it, I'm tired.

                    var rA = AllocateRegister(); // Processing Register
                    var rB = AllocateRegister(); // Final Normal Register
                    var c0 = AllocateConstant(false);
                    var c1 = AllocateConstant(false);

                    var exists = RegisterExists("c0");

                    var constants_block = new List<SM3Instruction>
                    {
                        new SM3Instruction("def", new List<string> { $"{c0}", $"{2.0 * 256.0 / 255.0}", $"{-128.0 / 127.0}", "0", "0" }) {Comment="Used for the normal  x * 2.0 - 1  calculation" },
                        new SM3Instruction("def", new List<string> { $"{c1}", "1", "-1", "0", "0.5" }),
                    };
                    Instructions.InsertRange(0, constants_block);

                    var input_register = TextureTemporaryRegister;
                    var target_sampler = bump_map.RegisterIndex;

                    var normal_texture_sample = this.Instructions.Where(instruction =>
                    {
                        if (instruction.Operation != "texld") return false;
                        //var register0 = instruction.Args[0].Split('.')[0];
                        var register2 = instruction.Args[2].Split('.')[0];
                        //return register0 == input_register && register2 == $"s{target_sampler}";
                        return register2 == $"s{target_sampler}";

                    }).FirstOrDefault();

                    if(normal_texture_sample == null)
                    {
                        throw new Exception("There should always be a normal sampler with the bump map?");
                    }
                    var normal_sampler_requires_extra_mov_op = normal_texture_sample.Args[0].Split('.')[0] == input_register;

                    var normal_texture_sample_index = Instructions.IndexOf(normal_texture_sample) + 1;
                    if (normal_sampler_requires_extra_mov_op) normal_texture_sample_index++;


                    /*
                    mad r0.xy, r1, c0.x, c0.y // x * 2.0 - 1
                    mul r1.xy, r0, r0         // Square of the value
                    add_sat r1.x, r1.y, r1.x  // Some saturate function on x^2 and y^2
                    add r1.x, -r1.x, c1.x     // 1 - saturated x^2   (pretty sure r1 is reconstructed z^2 now)
                    rsq r1.x, r1.x            // First part of square root
                    rcp r0.z, r1.x            // z0.z is the square root of z. Basically sqrt(x^2 + y^2 + z^2) = 1 and solve for z
                    nrm r1.xyz, r0            // Normalize r0 which contains the original X,Y and a reconstructed Z
                    mov r12, r1
                    //mul r0.xyz, r1.y, v3      // This converts the texture into world space normals
                    //mad r0.xyz, r1.x, v2, r0  // This converts the texture into world space normals
                    //mad r0.xyz, r1.z, v4, r0  // This converts the texture into world space normals
                    //nrm r1.xyz, r0            // Normalizes the output
                    */
                    var normal_calculation_block = new List<SM3Instruction>
                    {
                        new SM3Instruction("mad", new List<string> { $"{rA}.xy", $"{input_register}", $"{c0}.x", $"{c0}.y" }) {Comment="x * 2.0 - 1" },
                        new SM3Instruction("mul", new List<string> { $"{rB}.xy", $"{rA}", $"{rA}" }) {Comment="Square of the value" },
                        new SM3Instruction("add_sat", new List<string> { $"{rB}.x", $"{rB}.y", $"{rB}.x" }) {Comment="saturate function on x^2 and y^2" },
                        new SM3Instruction("add", new List<string> { $"{rB}.x", $"-{rB}.x", $"{c1}.x" }) {Comment="1 - saturated x^2 (pretty sure {rB} is reconstructed z^2 now)" },
                        new SM3Instruction("rsq", new List<string> { $"{rB}.x", $"{rB}.x" }) {Comment="First part of square root" },
                        new SM3Instruction("rcp", new List<string> { $"{rA}.z", $"{rB}.x" }) {Comment="z0.z is the square root of z. Basically sqrt(x^2 + y^2 + z^2) = 1 and solve for z" },
                        new SM3Instruction("nrm", new List<string> { $"{rB}.xyz", $"{rA}" }) {Comment="Normalize {rA} which contains the original X,Y and a reconstructed Z" },
                        new SM3Instruction("mul", new List<string> { $"{rA}.xyz", $"{rB}.y", "v3" }) {Comment="Converts the normal into world space normals" },
                        new SM3Instruction("mad", new List<string> { $"{rA}.xyz", $"{rB}.x", "v2", $"{rA}" }) {Comment="Converts the normal into world space normals" },
                        new SM3Instruction("mad", new List<string> { $"{rA}.xyz", $"{rB}.z", "v4", $"{rA}" }) {Comment="Converts the normal into world space normals" },
                        new SM3Instruction("nrm", new List<string> { $"{rB}.xyz", $"{rA}" }) {Comment="Normalizes the output" },
                    };
                    Instructions.InsertRange(normal_texture_sample_index, normal_calculation_block);

                    var normal_color_output = this.Instructions.Where(instruction =>
                    {
                        if (instruction.Operation != "mov") return false;
                        var register2 = instruction.Args[0].Split('.')[0];
                        return register2 == $"oC1";
                    }).LastOrDefault();

                    if(normal_color_output == null)
                    {
                        throw new Exception("There should always be a normal output the fuk?");
                    }

                    var last_normal_color_output_index = Instructions.IndexOf(normal_color_output) + 1;

                    var normal_assignment_block = new List<SM3Instruction>
                    {
                        //new SM3Instruction("mov", new List<string> { $"oC1.xyz", $"{rB}" }) {Comment="Override the normal output" },
                        // This instruction is very very important due to differences in H3 to HO engine. It converts normals in the 0-1 space
                        new SM3Instruction("mad", new List<string> { $"oC1.xyz", $"{rB}", $"{c1}.w", $"{c1}.w" }) {Comment="Override the normal output" },
                    };
                    Instructions.InsertRange(last_normal_color_output_index, normal_assignment_block);

                    // We can fix the innefficiency of all of this down the road by creating an assembly processor to optimize all of the stuff that this makes redundant.
                    // For this, have fun with your melting GPU's while my Titan V tanks it ;)
                }














                {
                    // Insert a new instruction at the end of the shader
                    // this will output the texcoord 1 value fo the correct sampler
                    //mov oC2, v1.x

                    var instruction = new SM3Instruction("mov", new List<string> { "oC2", "v1.x" });
                    instruction.Comment = "PixelDecal Unknown Output Fix";
                    Instructions.Add(instruction);
                }
            }
        }

        private string ConvertXboxShader(string raw_shader_code)
        {
            if (IsVertexShader) throw new NotImplementedException();

            var instructions = raw_shader_code.Split(new[] { "\r\n", "\r", "\n", Environment.NewLine }, StringSplitOptions.None).ToList();
            for (var instruction_index = instructions.Count - 1; instruction_index >= 0; instruction_index--)
            {
                var instruction = instructions[instruction_index];
                instruction = instruction.Replace(" + ", "");
                instruction = instruction.Trim();

                var original_instruction = instruction;

                // Take the left side of any comments
                instruction = instruction.Split(new[] { "//" }, StringSplitOptions.None)[0];

                if (String.IsNullOrWhiteSpace(instruction))
                {
                    instructions.RemoveAt(instruction_index);
                    continue;
                }

                var assembly_codes = instruction.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);

                var assembly_code = assembly_codes.Length > 0 ? assembly_codes[0] : null;
                if (assembly_code != null)
                {
                    if (
                        assembly_code == "exec" ||
                        assembly_code == "exece" ||
                        assembly_code == "alloc" ||
                        assembly_code == "kill_eq" ||
                        assembly_code == "kill_ge" ||
                        assembly_code == "kill_gt" ||
                        assembly_code == "kill_ne"
                        )
                    {
                        instruction = "//" + instruction;

                        instructions[instruction_index] = instruction.Trim();
                        continue;
                    }
                    if (assembly_code == "sqrt")
                    {
                        instruction = "";
                        instruction = $"rsq {assembly_codes[1]}, {assembly_codes[2]} // {original_instruction}";
                        instruction += $"\nrcp {assembly_codes[1]}, {assembly_codes[1]} // 1/(1/sqrt)";

                        instructions[instruction_index] = instruction.Trim();
                        continue;
                    }
                    if (assembly_code == "movs")
                    {
                        instruction = $"mov {assembly_codes[1]}, {assembly_codes[2]} // {original_instruction}";

                        instructions[instruction_index] = instruction.Trim();
                        continue;
                    }
                    if (assembly_code == "mulsc")
                    {
                        instruction = $"mul {assembly_codes[1]}, {assembly_codes[2]}, {assembly_codes[3]} // {original_instruction}";

                        instructions[instruction_index] = instruction.Trim();
                        continue;
                    }
                    if (assembly_code == "addsc")
                    {
                        instruction = $"add {assembly_codes[1]}, {assembly_codes[2]}, {assembly_codes[3]} // {original_instruction}";

                        instructions[instruction_index] = instruction.Trim();
                        continue;
                    }
                    if (assembly_code == "tfetch2D")
                    {
                        // TODO: Find the highest register number and allocate a new one for this fix instead of defaulting to 31
                        instruction = $"texld r31.xyzw, {assembly_codes[2]}, {assembly_codes[3].Replace("tf", "s")} // {original_instruction} (copy to register)";
                        instruction += "\n";

                        var destination = assembly_codes[1];
                        if (destination.Contains(".") && destination.Contains("_")) // Has a mask
                        {
                            var destinationA = assembly_codes[1].Split('.')[0];
                            var destinationB = assembly_codes[1].Split('.')[1];
                            const string coords = "xyzw";

                            //TODO: Bunching up these commands would speed it up a bit
                            for (var destination_index = 0; destination_index < 4; destination_index++)
                            {
                                var destination_coordinate_char = destinationB[destination_index];
                                if (destination_coordinate_char == '_') continue;
                                if (destination_coordinate_char == '1') throw new NotImplementedException();

                                var destination_coordinate = new String(destination_coordinate_char, 1);
                                var source_coordinate = new String(coords[destination_index], 1);

                                instruction += $"mov {destinationA}.{destination_coordinate}, r31.{source_coordinate} // {original_instruction} (masking {destination_coordinate})\n";
                            }





                        }
                        else instruction += $"mov {assembly_codes[1]}, r31.xyzw // {original_instruction} (default masking)";

                        instructions[instruction_index] = instruction.Trim();
                        continue;
                    }

                }

                instructions[instruction_index] = instruction.Trim();
            }

            List<string> new_instructions = new List<string>();
            for (var instruction_index = 0; instruction_index < instructions.Count; instruction_index++)
            {
                var instruction = instructions[instruction_index];
                var instruction_instructions = instruction.Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.None);
                for (var instruction_instructions_index = 0; instruction_instructions_index < instruction_instructions.Length; instruction_instructions_index++)
                {
                    new_instructions.Add(instruction_instructions[instruction_instructions_index]);
                }
            }
            instructions = new_instructions;

            // Register Fixups
            for (var instruction_index = instructions.Count - 1; instruction_index >= 0; instruction_index--)
            {
                var instruction = instructions[instruction_index];
                instruction = instruction.Replace(" + ", "");
                instruction = instruction.Trim();

                var original_instruction = instruction;

                // Take the left side of any comments
                instruction = instruction.Split(new[] { "//" }, StringSplitOptions.None)[0];

                if (String.IsNullOrWhiteSpace(instruction))
                {
                    instructions.RemoveAt(instruction_index);
                    continue;
                }

                var assembly_codes = instruction.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);

                var assembly_code = assembly_codes.Length > 0 ? assembly_codes[0] : null;
                if (assembly_code != null)
                {

                    // : Anti Fuck Cheatcodes for 1337 kids :
                    if (IsPixelShader) // Pixel Shader 
                    {
                        // Fixup Output Colors
                        if (assembly_codes.Length >= 2)
                            for (var assembly_code_index = 1; assembly_code_index < assembly_codes.Length; assembly_code_index++)
                                if (assembly_codes[assembly_code_index][0] == 'o')
                                {
                                    var index_str = assembly_codes[assembly_code_index].Substring(1);
                                    var arr = index_str.Split('.');
                                    index_str = arr[0];
                                    var args_str = arr.Length > 1 ? arr[1] : null;

                                    if (Int32.TryParse(index_str, out Int32 index))
                                    {
                                        if (args_str == null) assembly_codes[assembly_code_index] = $"oC{index}";
                                        else assembly_codes[assembly_code_index] = $"oC{index}.{args_str}";
                                        var assembly_code_args = assembly_codes.Skip(1);
                                        instruction = $"{assembly_code} " + string.Join(", ", assembly_code_args);
                                    }
                                }

                    }

                    if (IsVertexShader) // Vertex Shader
                    {

                    }

                    // Fixup Register Differences Top Down
                    if (assembly_codes.Length >= 2)
                        for (var assembly_code_index = 1; assembly_code_index < assembly_codes.Length; assembly_code_index++)
                            if (assembly_codes[assembly_code_index][0] == 'c')
                            {
                                var index_str = assembly_codes[assembly_code_index].Substring(1);
                                var arr = index_str.Split('.');
                                index_str = arr[0];
                                var args_str = arr.Length > 1 ? arr[1] : null;

                                if (Int32.TryParse(index_str, out Int32 index) && index >= (255 - 32))
                                {
                                    if (args_str == null) assembly_codes[assembly_code_index] = $"c{index - 32}";
                                    else assembly_codes[assembly_code_index] = $"c{index - 32}.{args_str}";
                                    var assembly_code_args = assembly_codes.Skip(1);
                                    instruction = $"{assembly_code} " + string.Join(", ", assembly_code_args);
                                }
                            }

                }

                instructions[instruction_index] = instruction.Trim();
            }

            List<string> header = new List<string>();
            if (IsVertexShader) header.Insert(0, "vs_3_0");
            if (IsPixelShader) header.Insert(0, "ps_3_0");

            var registers = GetParameters();
            foreach (var register in registers)
            {
                if (register.RegisterType == ShaderParameter.RType.Sampler)
                {
                    header.Add($"dcl_2d {GetShaderParameterTypeString(register.RegisterType)}{register.RegisterIndex} // {CacheContext.GetString(register.ParameterName)}");
                }
            }

            instructions.InsertRange(0, header);

            return String.Join("\n", instructions);
        }

        private byte[] ProcessVertexShader(VertexShaderBlock Block)
        {
            throw new NotImplementedException();
        }

        private byte[] ProcessPixelShader(PixelShaderBlock Block)
        {
            if (IsPixelShader)
            {
                var parser = new SM3ExtShaderParser(SM3ExtShaderParser.ShaderType.Pixel, this.ShaderData, this.ConstantData);
                var converter = new PixelDecalTemplatesFixups(parser, CacheContext, Block.XboxParameters);
                string converted_shader_code = converter.Convert();
                Console.WriteLine(converted_shader_code);
                var shader_bytecode = Utilities.DirectXUtilities.AssembleShader(converted_shader_code);
                //TODO Add different converters and get the name of a tag (or a better method) to determine the converter type to use
                return shader_bytecode;
            }
            else throw new NotImplementedException();






            //var raw_shader_code = XSDDisassemble();
            //var converted_shader_code = ConvertXboxShader(raw_shader_code);
            //converted_shader_code = File.ReadAllText("shader.txt");

            //

            //
            //return shader_bytecode;
        }

        public byte[] ProcessShader()
        {
            if (IsVertexShader) return ProcessVertexShader(Block as VertexShaderBlock);
            if (IsPixelShader) return ProcessPixelShader(Block as PixelShaderBlock);
            return null;
        }

        private string CommentStringBlock(string input, bool trim)
        {
            var linesInText = input.Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.None);

            StringBuilder stringWithRowNumbers = new StringBuilder();

            foreach (var line in linesInText)
            {
                var str = line;
                if (trim) str = line.Trim();

                stringWithRowNumbers.AppendLine($"// {str}");
            }
            string result = stringWithRowNumbers.ToString();
            return result;
        }

        public enum OutputFile
        {
            ShaderData,
            DebugData,
            ConstantData
        }

        public void WriteOutput(OutputFile file)
        {
            Directory.CreateDirectory(@"Temp");

            if (file == OutputFile.ShaderData)
            {
                WriteOutput(@"Temp\permutation.shader", ShaderData);
            }

            if (file == OutputFile.ConstantData)
            {
                WriteOutput(@"Temp\permutation.shader.updb", DebugData);
            }

            if (file == OutputFile.ShaderData)
            {
                WriteOutput(@"Temp\permutation.shader.cbin", ConstantData);
            }
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
