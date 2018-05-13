using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Shaders.SM3;
using TagTool.Shaders.SM3Ext;

namespace TagTool.Shaders.Converters
{
    class PixelDecalTemplatesConverter : SM3ShaderConverter
    {
        public PixelDecalTemplatesConverter(SM3ExtShaderParser parser, HaloOnlineCacheContext context, List<ShaderParameter> shader_parameters) : base(parser, context, shader_parameters)
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
            

            ConverterGlobalFunctions.FixBumpMapNormalExport(this);













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
}
