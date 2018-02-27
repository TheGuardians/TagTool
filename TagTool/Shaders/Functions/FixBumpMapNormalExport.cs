using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Shaders.SM3;

namespace TagTool.Shaders.Converters
{
    static partial class ConverterGlobalFunctions
    {
        public static void FixBumpMapNormalExport(SM3ShaderConverter @this)
        {
            // The worlds shittest fix that probbaly doesn't work for ever shader
            // But hey, how about you try better for you judge this fair effort

            //TODO:
            // Convert Normal Map using (x * 2) - 1 formula for normals
            // DATA: def c0, 2.00787401, -1.00787401, 0, 0
            // Example. mad r0.xy, r1, c0.x, c0.y
            // This needs to be done on the normals.

            var bump_map = @this.Parameters.Where(param => @this.CacheContext.GetString(param.ParameterName) == "bump_map").FirstOrDefault();
            if (bump_map != null)
            {
                // This fixup is 200% shit. Butt fuck it, I'm tired.

                var rA = @this.AllocateRegister(); // Processing Register
                var rB = @this.AllocateRegister(); // Final Normal Register
                var c0 = @this.AllocateConstant(false);
                var c1 = @this.AllocateConstant(false);

                var exists = @this.RegisterExists("c0");

                var constants_block = new List<SM3Instruction>
                    {
                        new SM3Instruction("def", new List<string> { $"{c0}", $"{2.0 * 256.0 / 255.0}", $"{-128.0 / 127.0}", "0", "0" }) {Comment="Used for the normal  x * 2.0 - 1  calculation" },
                        new SM3Instruction("def", new List<string> { $"{c1}", "1", "-1", "0", "0.5" }),
                    };
                @this.Instructions.InsertRange(0, constants_block);

                var input_register = @this.TextureTemporaryRegister;
                var target_sampler = bump_map.RegisterIndex;

                var normal_texture_sample = @this.Instructions.Where(instruction =>
                {
                    if (instruction.Operation != "texld") return false;
                    //var register0 = instruction.Args[0].Split('.')[0];
                    var register2 = instruction.Args[2].Split('.')[0];
                    //return register0 == input_register && register2 == $"s{target_sampler}";
                    return register2 == $"s{target_sampler}";

                }).FirstOrDefault();

                if (normal_texture_sample == null)
                {
                    throw new Exception("There should always be a normal sampler with the bump map?");
                }
                var normal_sampler_requires_extra_mov_op = normal_texture_sample.Args[0].Split('.')[0] == input_register;

                var normal_texture_sample_index = @this.Instructions.IndexOf(normal_texture_sample) + 1;
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
                @this.Instructions.InsertRange(normal_texture_sample_index, normal_calculation_block);

                var normal_color_output = @this.Instructions.Where(instruction =>
                {
                    //if (instruction.Operation != "mov") return false;
                    if (!instruction.InstructionFirstIndexIsOutput) return false;
                    var dest_register = instruction.Args[0].Split('.')[0];
                    return dest_register == $"oC1";
                }).LastOrDefault();

                if (normal_color_output == null)
                {
                    throw new Exception("There should always be a normal output the fuk?");
                }

                var last_normal_color_output_index = @this.Instructions.IndexOf(normal_color_output) + 1;

                var normal_assignment_block = new List<SM3Instruction>
                    {
                        //new SM3Instruction("mov", new List<string> { $"oC1.xyz", $"{rB}" }) {Comment="Override the normal output" },
                        // This instruction is very very important due to differences in H3 to HO engine. It converts normals in the 0-1 space
                        new SM3Instruction("mad", new List<string> { $"oC1.xyz", $"{rB}", $"{c1}.w", $"{c1}.w" }) {Comment="Override the normal output" },
                    };
                @this.Instructions.InsertRange(last_normal_color_output_index, normal_assignment_block);

                // We can fix the innefficiency of all of this down the road by creating an assembly processor to optimize all of the stuff that this makes redundant.
                // For this, have fun with your melting GPU's while my Titan V tanks it ;)
            }

        }
    }
}
