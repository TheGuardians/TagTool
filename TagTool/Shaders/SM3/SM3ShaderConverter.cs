using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Shaders.SM3Ext;

namespace TagTool.Shaders.SM3
{
    class SM3ShaderConverter
    {
        public SM3ExtShaderParser Parser { get; }
        public List<ShaderParameter> Parameters;
        public HaloOnlineCacheContext CacheContext { get; }
        public string TextureTemporaryRegister = null;
        public List<SM3Instruction> Instructions = new List<SM3Instruction>();
        protected List<int> AllocatedRegister = new List<int>();
        protected List<int> AllocatedConstant = new List<int>();

        public SM3ShaderConverter(SM3ExtShaderParser parser, HaloOnlineCacheContext context, List<ShaderParameter> shader_parameters)
        {
            Parser = parser;
            Parameters = shader_parameters;
            CacheContext = context;
        }

        public string Convert()
        {
            PreProcess();

            foreach (var instruction in Parser.Instructions)
            {
                Instructions.Add(new SM3Instruction(instruction));
            }

            PostProcess();

            var shader_version = Parser.Type == SM3ExtShaderParser.ShaderType.Vertex ? "vs_3_0" : "ps_3_0";
            Instructions.Insert(0, new SM3Instruction(shader_version, new List<string>()));

            StringBuilder builder = new StringBuilder();
            foreach (var instruction in Instructions)
            {
                builder.AppendLine(instruction.ToString());
            }

            return builder.ToString();
        }

        public bool ReplaceRegister(string register_old, string register_new)
        {
            bool register_changed = false;
            foreach (var instruction in Instructions)
                for (var i = 0; i < instruction.Args.Count; i++)
                {
                    var arg = instruction.Args[i];

                    if (!arg.StartsWith(register_old)) continue;
                    var components = arg.Split('.');
                    var register = components[0];

                    if (register != register_old) continue;

                    if (components.Length > 1) instruction.Args[i] = $"{register_new}.{String.Join(".", components.Skip(1))}";
                    else instruction.Args[i] = $"{register_new}";

                    register_changed = true;
                }
            return register_changed;
        }

        public int RegisterFirstWrite(string check_register)
        {
            for (var instruction_index = 0; instruction_index < Instructions.Count; instruction_index++)
            {
                var instruction = Instructions[instruction_index];
                if (instruction.InstructionFirstIndexIsOutput)
                    for (var i = 0; i < Math.Min(instruction.Args.Count, 1); i++)
                    {
                        var arg = instruction.Args[i];
                        var components = arg.Split('.');
                        var register = components[0];

                        if (register == check_register) return instruction_index;
                    }
            }

            return -1;
        }

        public void ReplaceInputRegister(string replace_register, string input_register)
        {
            var maximum_index = RegisterFirstWrite(replace_register);

            if (maximum_index == -1)
            {
                ReplaceRegister(replace_register, input_register);
                Console.WriteLine($"Fixup {input_register} -> {replace_register} @<ALL>");
            }
            else
            {
                bool register_written_to = false;

                for (var instruction_index = 0; instruction_index < Instructions.Count; instruction_index++)
                {
                    var instruction = Instructions[instruction_index];

                    if (register_written_to) break;
                    if (instruction.InstructionFirstIndexIsOutput)
                        for (var i = instruction.Args.Count - 1; i >= 0; i--)
                        //for (var i = 0; i < Math.Max(Math.Min(instruction.Args.Count, maximum_index), minimum_index); i++)
                        {
                            var arg = instruction.Args[i];

                            if (!arg.StartsWith(replace_register)) continue;
                            var components = arg.Split('.');
                            var register = components[0];

                            if (instruction_index >= maximum_index && i == 0 && register == replace_register)
                            {
                                register_written_to = true;
                                break;
                            }

                            if (register != replace_register) continue;

                            if (components.Length > 1) instruction.Args[i] = $"{input_register}.{String.Join(".", components.Skip(1))}";
                            else instruction.Args[i] = $"{input_register}";
                        }
                }

                Console.WriteLine($"Fixup {input_register} -> {replace_register} @{maximum_index}");
            }
        }

        public bool RegisterExists(string check_register)
        {
            if (check_register.StartsWith("c"))
                foreach (var param in Parameters)
                    for (var i = 0; i < param.RegisterCount; i++)
                    {
                        var index = param.RegisterIndex + i;
                        switch (param.RegisterType)
                        {
                            case ShaderParameter.RType.Boolean:
                                if ($"b{index}" == check_register) return true;
                                break;
                            case ShaderParameter.RType.Vector:
                                if ($"c{index}" == check_register) return true;
                                break;
                            case ShaderParameter.RType.Integer:
                                if ($"i{index}" == check_register) return true;
                                break;
                            case ShaderParameter.RType.Sampler:
                                if ($"s{index}" == check_register) return true;
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }

            foreach (var instruction in Instructions)
                for (var i = 0; i < instruction.Args.Count; i++)
                {
                    var arg = instruction.Args[i];
                    var components = arg.Split('.');
                    var register = components[0];

                    if (register == check_register) return true;
                }
            return false;
        }
        
        public string AllocateRegister()
        {
            for (var i = 0; i < 32; i++)
            {
                var register_name = $"r{i}";
                if (!RegisterExists(register_name))
                {
                    if (AllocatedRegister.Contains(i)) continue;
                    AllocatedRegister.Add(i);
                    return register_name;
                }
            }
            //TODO: If we ever run into this issue, we should add a range on this function like
            // a start_instruction and an end_instruction instead of trying to keep this reigster
            // free at all times. (It may also optimize the shader, dunno perhaps depends on which
            // gpu is using the code. But most modern GPU's are going to filter out any kind of 
            // reserved register like this anyway
            throw new Exception("Unable to allocate register!");
        }

        public string AllocateConstant(bool reversed)
        {
            if (reversed)
            {
                for (var i = (255 - 32); i >= 0; i--)
                {
                    var register_name = $"c{i}";
                    if (!RegisterExists(register_name))
                    {
                        if (AllocatedConstant.Contains(i)) continue;
                        AllocatedConstant.Add(i);
                        return register_name;
                    }
                }
            }
            else
            {
                for (var i = 0; i < (255 - 32); i++)
                {
                    var register_name = $"c{i}";
                    if (!RegisterExists(register_name))
                    {
                        if (AllocatedConstant.Contains(i)) continue;
                        AllocatedConstant.Add(i);
                        return register_name;
                    }
                }
            }

            // This shouldn't really happen pretty much ever and forever but if it does its entirely your fault for some reason
            // not mine at all.... Yours... got it?
            throw new Exception("Unable to allocate register!");
        }

        public void CreateConstantDefinitions()
        {
            List<int> defined_constant_location = new List<int>();
            List<SM3Instruction> constant_definitions = new List<SM3Instruction>();
            foreach (var instruction in Instructions)
                foreach (var arg in instruction.Args)
                {
                    if (!arg.StartsWith("c")) continue;
                    var components = arg.Split('.');
                    var left = components[0];
                    var register = left.Substring(1);

                    if (!Int32.TryParse(left.Substring(1), out int location)) continue;

                    if (defined_constant_location.Contains(location)) continue;
                    if (!Parser.ConstantDataDefinitions.ContainsKey(location))
                    {
                        if (location >= 223)
                        {
                            Console.WriteLine($"WARNING: Constant index {location} is not found in constant data");
                        }
                        continue;
                    }

                    var constant_data = Parser.ConstantDataDefinitions[location];
                    var constant_data_args = new List<string> {
                        $"c{location}",
                        constant_data.X.ToString(),
                        constant_data.Y.ToString(),
                        constant_data.Z.ToString(),
                        constant_data.W.ToString(),
                    };
                    constant_definitions.Insert(0, new SM3Instruction("def", constant_data_args, false));
                    defined_constant_location.Add(location);
                }
            AllocatedConstant.AddRange(defined_constant_location);
            Instructions.InsertRange(0, constant_definitions);
        }

        public void FixOutputPixelRegisters()
        {
            for (var i = 0; i < 9; i++)
                this.ReplaceRegister($"o{i}", $"oC{i}");

            // There could be more, but iirc there is only 10 on Xbox due to stage 2
        }

        public void FixTextureTemporaryRegister()
        {
            if (RegisterExists("TSO")) // Temporary Output Sampler
            {
                if(TextureTemporaryRegister == null)
                {
                    TextureTemporaryRegister = AllocateRegister();
                }
                ReplaceRegister("TSO", TextureTemporaryRegister);
            }
        }
        public void FixOutOfBoundsRegisters()
        {
            for (var i = (255 - 32); i <= 255; i++)
            {
                var original_register = $"c{i}";
                if (RegisterExists(original_register))
                {
                    var allocated_register = AllocateConstant(false);
                    ReplaceRegister(original_register, allocated_register);
                    Console.WriteLine($"Fixing out of bounds register ${original_register} with ${allocated_register}");
                }
            }
        }

        //TODO: Needs more testing and some extra checks on input format
        public void OptimizeNormalize()
        {
            for(var instruction_index = 0;instruction_index<Instructions.Count;instruction_index++)
            {
                if (instruction_index + 2 >= Instructions.Count) break; // Don't go over the end of the array

                var instructionA = Instructions[instruction_index];
                var instructionB = Instructions[instruction_index+1];
                var instructionC = Instructions[instruction_index+2];

                if (instructionA.Operation != "dp3") continue;
                if (instructionB.Operation != "rsq") continue;
                if (instructionC.Operation != "mul") continue;

                if (instructionA.Args[1] != instructionA.Args[2]) continue;
                if (instructionB.Args[1].IndexOf("_abs") == -1) continue;
                if (instructionA.Args[0] != instructionB.Args[0]) continue;
                if (instructionB.Args[0] != instructionC.Args[2]) continue;

                var registerA0 = instructionA.Args[0].Split('.')[0];
                var registerA1 = instructionA.Args[1].Split('.')[0];
                var registerA2 = instructionA.Args[2].Split('.')[0];
                var registerB1 = instructionB.Args[0].Split('.')[0];
                var registerC0 = instructionC.Args[0].Split('.')[0];
                var registerC1 = instructionC.Args[1].Split('.')[0];
                var registerC2 = instructionC.Args[2].Split('.')[0];

                if (registerA0 != registerA1) continue;
                if (registerA1 != registerA2) continue;
                if (registerA2 != registerB1) continue;
                if (registerB1 != registerC0) continue;
                if (registerC0 != registerC1) continue;
                if (registerC1 != registerC2) continue;
                if (registerC2 != registerA0) continue;

                // This can be optimized out.

                //TODO: Need to organise allocating a register within the range, because NRM src and dst can't be the same register

                //Instructions.RemoveRange(instruction_index, 3);
                //Instructions.Insert(instruction_index, new SM3Instruction("nrm", new List<string> { instructionC.Args[0], instructionC.Args[1] }));
                instructionA.Comment = instructionB.Comment = instructionC.Comment = "NRM instruction expanded";
            }
        }

        public virtual void PostProcess()
        {
            OptimizeNormalize();

            for (var i = Instructions.Count - 1; i >= 0; i--)
            {
                var instruction = Instructions[i];
                if (instruction.ChildInstructions.Count == 0) continue;
                Instructions.InsertRange(i + 1, instruction.ChildInstructions);
            }

            CreateConstantDefinitions();

            if (Parser.Type == SM3ExtShaderParser.ShaderType.Pixel)
            {
                FixOutputPixelRegisters();
            }

            FixTextureTemporaryRegister();
            FixOutOfBoundsRegisters();

            // Inserting Samplers
            for (var i = Parameters.Count - 1; i >= 0; i--)
            {
                var parameter = Parameters[i];

                if (parameter.RegisterType != ShaderParameter.RType.Sampler)
                    continue;
                
                Instructions.Insert(0, new SM3Instruction("dcl_2d", new List<string> { $"s{parameter.RegisterIndex}" })
                {
                    Comment = CacheContext.GetString(parameter.ParameterName)
                });
            }
        }

        public virtual void PreProcess()
        {

        }
    }
}
