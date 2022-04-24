using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Shaders;
using TagTool.Tags.Definitions;
using RenderMethodTemplate = TagTool.Tags.Definitions.RenderMethodTemplate;
using ShaderGen2 = TagTool.Tags.Definitions.Gen2.Shader;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        public Shader ConvertShader(ShaderGen2 gen2Shader, Stream stream, string shader_template)
        {
            // Temporary tag
            TagTool.Cache.CachedTag tag;
            short i = 1;

            var shader = new Shader()
            {
                Material = gen2Shader.MaterialName,
                ShaderProperties = new List<Shader.RenderMethodPostprocessBlock>(),
                Options = new List<Shader.RenderMethodOptionIndex>()
            };

            // Default shader arguments
            var args = new List<string> {
                "shader",
                "0",
                "0",
                "0",
                "0",
                "0",
                "0",
                "0",
                "0",
                "0",
                "0",
                "0"
            };

            // Declare string lists that contain the order and contents for each of these tagblocks
            var h2_bitmap_order = new List<string> {
                "\0",
                "\0",
                "\0",
                "\0",
                "\0",
                "\0",
                "\0",
                "\0"
            };
            var h2_pixel_constants = new List<string> {
                "\0",
                "\0",
                "\0",
                "\0",
                "\0",
                "\0",
                "\0",
                "\0"
            };
            var h2_vertex_constants = new List<string>
            {
                "\0",
                "\0",
                "\0",
                "\0",
                "\0",
                "\0",
                "\0",
                "\0"
            };

            // Change the contents of lists depending on the h2 template used
            switch (shader_template)
            {
                case "tex_bump":
                case "tex_bump_active_camo":
                case "tex_bump_shiny":
                    {
                        args[2] = "1";
                        args[5] = "2";

                        h2_vertex_constants[0] = "bump_map";
                        h2_vertex_constants[1] = "base_map";
                        h2_vertex_constants[2] = "detail_map";
                        h2_vertex_constants[3] = "\0";


                        h2_pixel_constants[0] = "normal_specular_tint";
                        h2_pixel_constants[1] = "glancing_specular_tint";
                        h2_pixel_constants[2] = "\0";

                        h2_bitmap_order[0] = "bump_map";
                        h2_bitmap_order[1] = "alpha_test_map";
                        h2_bitmap_order[2] = "base_map";
                        h2_bitmap_order[3] = "detail_map";
                        h2_bitmap_order[4] = "\0";
                    }
                    break;
                case "tex_bump_env":
                    {
                        args[2] = "1";
                        args[5] = "2";
                        args[6] = "1";

                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_vertex_constants[0] = "bump_map";
                            h2_vertex_constants[1] = "base_map";
                            h2_vertex_constants[2] = "detail_map";
                            h2_vertex_constants[3] = "\0";
                        }
                        else
                        {
                            h2_vertex_constants[0] = "detail_map";
                            h2_vertex_constants[1] = "base_map";
                            h2_vertex_constants[2] = "bump_map";
                            h2_vertex_constants[3] = "\0";
                        }

                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_pixel_constants[0] = "normal_specular_tint";
                            h2_pixel_constants[1] = "glancing_specular_tint";
                            h2_pixel_constants[2] = "environment_map_specular_contribution";
                            h2_pixel_constants[3] = "";                     // Env Glancing Tint Brightness dosent exist in h3
                            h2_pixel_constants[4] = "env_tint_color";
                            h2_pixel_constants[5] = "\0";                   // Env Glancing Tint Colour dosent exist in h3
                        }
                        else
                        {
                            h2_pixel_constants[0] = "";                     // Blank value
                            h2_pixel_constants[1] = "normal_specular_tint";
                            h2_pixel_constants[2] = "glancing_specular_tint";
                            h2_pixel_constants[3] = "environment_map_specular_contribution";
                            h2_pixel_constants[4] = "";                     // Env Glancing Tint Brightness dosent exist in h3
                            h2_pixel_constants[5] = "env_tint_color";
                            h2_pixel_constants[6] = "\0";                   // Env Glancing Tint Colour dosent exist in h3
                        }

                        h2_bitmap_order[0] = "bump_map";
                        h2_bitmap_order[1] = "alpha_test_map";
                        h2_bitmap_order[2] = "base_map";
                        h2_bitmap_order[3] = "detail_map";
                        h2_bitmap_order[4] = "environment_map";
                        h2_bitmap_order[5] = "\0";
                        break;
                    }
                case "tex_bump_env_two_change_color_indexed":
                    {
                        args[1] = "3";
                        args[2] = "1";
                        args[5] = "2";
                        args[6] = "1";

                        if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                        {
                            h2_vertex_constants[0] = "bump_map";
                            h2_vertex_constants[1] = "base_map";
                            h2_vertex_constants[2] = "detail_map";
                            h2_vertex_constants[3] = "change_color_map";
                            h2_vertex_constants[4] = "\0";
                        }
                        else
                        {
                            h2_vertex_constants[0] = "detail_map";
                            h2_vertex_constants[1] = "base_map";
                            h2_vertex_constants[2] = "bump_map";
                            h2_vertex_constants[3] = "change_color_map";
                            h2_vertex_constants[4] = "\0";
                        }

                        h2_pixel_constants[0] = "normal_specular_tint";
                        h2_pixel_constants[1] = "glancing_specular_tint";
                        h2_pixel_constants[2] = "environment_map_specular_contribution";
                        h2_pixel_constants[3] = "";                     // Env Midrange Tint Colour dosent exist in h3
                        h2_pixel_constants[4] = "env_tint_color";
                        h2_pixel_constants[5] = "\0";                   // Env Glancing Tint Colour dosent exist in h3

                        h2_bitmap_order[0] = "bump_map";
                        h2_bitmap_order[1] = "base_map";
                        h2_bitmap_order[2] = "detail_map";
                        h2_bitmap_order[3] = "change_color_map";
                        h2_bitmap_order[4] = "environment_map";
                        h2_bitmap_order[5] = "\0";
                        break;
                    }
                default:
                    break;
            }
            string shader_path = @"shaders\shader_templates\_" + args[1] + "_" + args[2] + "_" + args[3] + "_" + args[4] + "_"
                + args[5] + "_" + args[6] + "_" + args[7] + "_" + args[8] + "_" + args[9] + "_" + args[10] + "_" + args[11];

            // Generate if the template dosent exist yet
            if (!Cache.TagCacheGenHO.TryGetTag(shader_path + ".rmt2", out tag))
            {
                new GenerateShaderCommand(Cache).ExecuteWithStream(args, stream);
                i--;
            }

            shader.BaseRenderMethod = Cache.TagCacheGenHO.GetTag(@"shaders\shader.rmdf");

            // Get Tag of the used render method template and deserialize
            var UsedRenderMethod = Cache.TagCacheGenHO.GetTag<RenderMethodTemplate>(shader_path);
            RenderMethodTemplate UsedRenderMethodDef = Cache.Deserialize<RenderMethodTemplate>(stream, UsedRenderMethod);

            // Add postprocessblock
            Shader.RenderMethodPostprocessBlock newPostprocessBlock = new Shader.RenderMethodPostprocessBlock
            {
                Template = Cache.TagCache.GetTag<RenderMethodTemplate>(shader_path),
                TextureConstants = new List<Shader.RenderMethodPostprocessBlock.TextureConstant>(),
                RealConstants = new List<RenderMethod.RenderMethodPostprocessBlock.RealConstant>()
            };
            shader.ShaderProperties.Add(newPostprocessBlock);

            // Populate options block
            for (; i < 11; i++)
            {
                Shader.RenderMethodOptionIndex option = new Shader.RenderMethodOptionIndex
                {
                    OptionIndex = Convert.ToInt16(args[i])
                };
                shader.Options.Add(option);
            }

            // Add all the texture maps
            foreach (var shadermap in UsedRenderMethodDef.TextureParameterNames)
            {
                var h2_texture_reference = gen2Shader.PredictedResources;
                string current_type = Cache.StringTable[((int)shadermap.Name.Value)];   // Gets the current type of bitmap in the template

                // If the string in the bitmap order list matches the current_type in the rmt2,
                // Then set current_type to the bitmap path
                for (i = 0; h2_bitmap_order[i] != "\0"; i++)
                {
                    if (h2_bitmap_order[i] == current_type)
                    {
                        current_type = Gen2Cache.TagCacheGen2.Tags[(short)h2_texture_reference[i].TagIndex].ToString();
                        break;
                    }
                }

                Shader.RenderMethodPostprocessBlock.TextureConstant newTextureConstant = new Shader.RenderMethodPostprocessBlock.TextureConstant
                {
                    Bitmap = Cache.TagCacheGenHO.GetTag(current_type)
                };
                newPostprocessBlock.TextureConstants.Add(newTextureConstant);
            }

            // Declare Real Parameters and "try" to convert the similar counterparts in h2 to h3 equivalents
            foreach (var floatconstant in UsedRenderMethodDef.RealParameterNames)
            {
                bool found = false;
                var h2resource = gen2Shader.PostprocessDefinition;
                var h2pixel_constant = h2resource[0].PixelConstants;
                var h2vertex_constant = h2resource[0].VertexConstants;

                string current_type = Cache.StringTable[((int)floatconstant.Name.Value)];   // Gets the current type of bitmap in the template

                // Set albedo_color to 1 default for all shaders (fixup when setting up templates in h2 which use this field)
                if (current_type == "albedo_color")
                {
                    found = true;
                    Shader.RenderMethodPostprocessBlock.RealConstant newfloatconstant = new Shader.RenderMethodPostprocessBlock.RealConstant
                    {
                        Arg0 = 1,
                        Arg1 = 1,
                        Arg2 = 1,
                        Arg3 = 1
                    };
                    newPostprocessBlock.RealConstants.Add(newfloatconstant);
                }

                // Set diffuse_coefficient to 1 default for all shaders
                if (current_type == "diffuse_coefficient")
                {
                    found = true;
                    Shader.RenderMethodPostprocessBlock.RealConstant newfloatconstant = new Shader.RenderMethodPostprocessBlock.RealConstant
                    {
                        Arg0 = 1,
                        Arg1 = 1,
                        Arg2 = 1,
                        Arg3 = 1
                    };
                    newPostprocessBlock.RealConstants.Add(newfloatconstant);
                }

                // Convert Pixel Constants to Float Constants if matches are found
                for (i = 0; h2_pixel_constants[i] != "\0"; i++)
                {
                    if (h2_pixel_constants[i] == current_type)
                    {
                        found = true;
                        float r = -1, g = -1, b = -1, a = -1;

                        // Convert values in h2 pixel constant block to the layout and values it expects in h3
                        // (For numeric values in shader)
                        // Set (rgba) to value of (a) in h3 shader if only (a) has a value and (rgb) dosent in h2
                        if
                        (h2pixel_constant[i].Color.Red == 0 && h2pixel_constant[i].Color.Green == 0 && h2pixel_constant[i].Color.Blue == 0 && h2pixel_constant[i].Color.Alpha > 0)
                        {
                            r = (float)h2pixel_constant[i].Color.Alpha / (float)255;
                            g = (float)h2pixel_constant[i].Color.Alpha / (float)255;
                            b = (float)h2pixel_constant[i].Color.Alpha / (float)255;
                            a = (float)h2pixel_constant[i].Color.Alpha / (float)255;
                        }

                        // (For colours in shader)
                        // Set (a) to value of 1 if any (rgb) values are above 0 and (a) is 0
                        else if
                        (h2pixel_constant[i].Color.Red > 0 || h2pixel_constant[i].Color.Green > 0 || h2pixel_constant[i].Color.Blue > 0 && h2pixel_constant[i].Color.Alpha == 0)
                        {
                            r = (float)h2pixel_constant[i].Color.Red / (float)255;
                            g = (float)h2pixel_constant[i].Color.Green / (float)255;
                            b = (float)h2pixel_constant[i].Color.Blue / (float)255;
                            a = 1;
                        }

                        Shader.RenderMethodPostprocessBlock.RealConstant newfloatconstant = new Shader.RenderMethodPostprocessBlock.RealConstant
                        {
                            Arg0 = r,
                            Arg1 = g,
                            Arg2 = b,
                            Arg3 = a
                        };
                        newPostprocessBlock.RealConstants.Add(newfloatconstant);
                    }
                }

                // If any Pixel Constants with matching types aren't found
                // try to find and convert Vertex Constants to Float Constants
                if (found == false)
                {
                    for (i = 0; h2_vertex_constants[i] != "\0"; i++)
                    {
                        if (h2_vertex_constants[i] == current_type)
                        {
                            found = true;
                            Shader.RenderMethodPostprocessBlock.RealConstant newfloatconstant = new Shader.RenderMethodPostprocessBlock.RealConstant
                            {
                                Arg0 = h2vertex_constant[i].Vector3.I,
                                Arg1 = h2vertex_constant[i].Vector3.J,
                                Arg2 = h2vertex_constant[i].Vector3.K,
                                Arg3 = h2vertex_constant[i].W
                            };
                            newPostprocessBlock.RealConstants.Add(newfloatconstant);
                        }
                    }
                }

                // Set all to 0 if no matches are found
                if (found == false)
                {
                    Shader.RenderMethodPostprocessBlock.RealConstant newfloatconstant = new Shader.RenderMethodPostprocessBlock.RealConstant
                    {
                        Arg0 = 0,
                        Arg1 = 0,
                        Arg2 = 0,
                        Arg3 = 0
                    };
                    newPostprocessBlock.RealConstants.Add(newfloatconstant);
                }
            }


            return shader;
        }
    }
}