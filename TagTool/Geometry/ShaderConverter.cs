using System;
using System.Linq;
using System.Text.RegularExpressions;
using TagTool.Direct3D.Functions;

namespace TagTool.Geometry
{
    /// <summary>
    /// Utility class for converting shader bytecode between rendering pipelines.
    /// </summary>
    public static class ShaderConverter
    {
        /// <summary>
        /// Converts a new (1.235640+) vertex shader to one compatible with the 1.106708 pipeline.
        /// Note that this requires that models have binormals set so that X = Position.W and Y = Tangent.W.
        /// </summary>
        /// <param name="shaderData">The shader bytecode to convert.</param>
        /// <param name="mode">The drawing mode.</param>
        /// <param name="type">The vertex type.</param>
        /// <returns>The new bytecode, or <c>null</c> if conversion failed.</returns>
        public static byte[] ConvertNewVertexShaderToOld(byte[] shaderData, int mode, VertexType type)
        {
            // Only applies to world, rigid, skinned, and dual quat
            if (type != VertexType.World && type != VertexType.Rigid && type != VertexType.Skinned && type != VertexType.DualQuat)
                return shaderData;

            // Disassemble the shader
            new Disassemble(shaderData, out string disassembly);
            if (disassembly == null)
                return null;
            var lines = disassembly.Split('\n').ToList();

            // So, the reason that new vertex shaders don't work in 1.106708
            // is because vertex declarations changed. For world, rigid, and
            // skinned meshes, the normal and binormal components were removed,
            // and the position and tangent vectors were extended to be 4D.
            // (The W components are used for various things.) To convert a new
            // vertex shader to use the old declaration format, we need to do
            // the following:
            //
            // 1. Add an input declaration for the binormal, so we can pull the
            //    position.W and tangent.W values from it (assuming they're
            //    stored that way in the model).
            //
            // 2. Remap the W component of the position to the X component of
            //    the binormal. (The W component will never be used alongside
            //    the other position components because it's technically
            //    completely separate.)
            //
            // 3. The tangent will be unpacked by using a "mad" instruction.
            //    However, in 1.106708, the tangent isn't packed to begin with.
            //    So, we look for an instruction of the form
            //
            //      mad [register], v[tangent], [constant], [constant]
            //
            //    and replace it with instructions to load the components,
            //    with the W component of the tangent in the Y component of the
            //    binormal:
            //
            //      mov [register].xyz, v[tangent]
            //      mov [register].w, v[binormal].y
            //
            // 4. Insert code to handle v_squish_params and v_mesh_squished.
            //    This is mainly needed for first-person models.
            //
            // 5. For albedo shaders only, o2 needs to be made 4D and the W
            //    component must be set to the W component of the position so
            //    that the pixel shader can pass it through to an output. This
            //    appears to be necessary for objects to receive shadows.
            //
            // 6. For default shaders only, a texcoord1 output has to be added
            //    for storing the W component of the position. This means that
            //    the other texcoord outputs need to be remapped.

            var addedDeclarations = false;
            var fixedTangent = false;
            var lastInput = -1;
            var tangentRegister = -1;
            var binormalRegister = -1;
            var storedZ = false;
            var storedW = false;
            var addedSquishParams = false;
            string squishConstant1 = null;
            string squishConstant2 = null;
            var needSquishCheck = false;
            var wOut = -1;
            for (var i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("//"))
                    continue;

                // Find the tangent declaration and keep track of the highest input register
                var match = Regex.Match(lines[i], @"dcl_(\S+) v(\d+)");
                if (match.Success)
                {
                    var name = match.Groups[1].Value;
                    var register = int.Parse(match.Groups[2].Value);
                    if (name == "tangent")
                        tangentRegister = register;
                    lastInput = Math.Max(lastInput, register);
                }

                if (!addedDeclarations)
                {
                    // Once the first output is found, add a definition for the binormal input before it
                    match = Regex.Match(lines[i], @"dcl_(\S+) o\d+");
                    if (match.Success)
                    {
                        binormalRegister = lastInput + 1;
                        lines.Insert(i++, string.Format("    dcl_binormal v{0}", binormalRegister));
                        addedDeclarations = true;
                        continue;
                    }
                }

                if (!fixedTangent && tangentRegister >= 0 && binormalRegister >= 0)
                {
                    // Adjust tangent unpacking
                    match = Regex.Match(lines[i], @"mad r(\d+), v" + tangentRegister);
                    if (match.Success)
                    {
                        var index = int.Parse(match.Groups[1].Value);
                        lines[i++] = string.Format("    mov r{0}.xyz, v{1}", index, tangentRegister);
                        lines.Insert(i, string.Format("    mov r{0}.w, v{1}.y", index, binormalRegister));
                        fixedTangent = true;
                        continue;
                    }
                }

                // When the output Z is set, store it to r31.z instead (for v_squish_params)
                match = Regex.Match(lines[i], @"\S+ (o0)\.z");
                if (match.Success)
                {
                    var group = match.Groups[1];
                    lines[i] = lines[i].Remove(group.Index, group.Length).Insert(group.Index, "r31");
                    storedZ = true;
                }

                // When the output W is set, store it to r30.w instead (for v_squish_params)
                match = Regex.Match(lines[i], @"\S+ (o0)\.w");
                if (match.Success)
                {
                    var group = match.Groups[1];
                    lines[i] = lines[i].Remove(group.Index, group.Length).Insert(group.Index, "r30");
                    storedW = true;
                }

                // If b8 (v_mesh_squished) is checked, we need to remove most
                // of it and transform the previous instruction into a store to
                // r31.z (for the v_squish_params fix later on).
                match = Regex.Match(lines[i], @"if b8");
                if (match.Success)
                {
                    // Check that the previous instruction stores to a register
                    match = Regex.Match(lines[i - 1], @"\S+ (r\d+\..),");
                    if (match.Success)
                    {
                        // Get squish constant 1
                        var constMatch = Regex.Match(lines[i + 1], @"mov r\d+\.., (c\d+\..)");
                        if (constMatch.Success)
                            squishConstant1 = constMatch.Groups[1].Value;

                        // Get squish constant 2
                        constMatch = Regex.Match(lines[i + 3], @"mov r\d+\.., (c\d+\..)");
                        if (constMatch.Success)
                            squishConstant2 = constMatch.Groups[1].Value;

                        if (squishConstant1 != null && squishConstant2 != null)
                        {
                            // Remove the if block
                            lines.RemoveRange(i--, 6);

                            // Transform the previous instruction
                            var group = match.Groups[1];
                            lines[i] = lines[i].Remove(group.Index, group.Length).Insert(group.Index, "r31.z");
                            needSquishCheck = true;
                            storedZ = true;
                        }
                    }
                }

                // v_squish_params fix
                if (storedZ && storedW && !addedSquishParams)
                {
                    lines.Insert(++i, "    mad r30.x, c250.x, r30.w, -c250.y\n" +
                                      "    mad r30.x, r30.x, c250.z, -r31.z");
                    if (needSquishCheck)
                    {
                        // Check v_mesh_squished
                        lines.Insert(++i,
                            string.Format("    mad r31.z, c250.w, r30.x, r31.z\n" +
                                          "    if b8\n" +
                                          "      mov r30.x, {0}\n" +
                                          "    else\n" +
                                          "      mov r30.x, {1}\n" +
                                          "    endif\n" +
                                          "    add o0.z, -r30.x, r31.z",
                                squishConstant1, squishConstant2));
                    }
                    else
                    {
                        lines.Insert(++i, "    mad o0.z, c250.w, r30.x, r31.z");
                    }
                    lines.Insert(++i, "    mov o0.w, r30.w");

                    // Needed for shadow receiving
                    if (wOut >= 0)
                    {
                        if (mode == 0)
                            lines.Insert(++i, string.Format("    mov o{0}.x, r30.w", wOut));
                        else if (mode == 1)
                            lines.Insert(++i, string.Format("    mov o{0}.w, r30.w", wOut));
                    }

                    addedSquishParams = true;
                    continue;
                }

                if (mode == 0)
                {
                    // Default-only: add a texcoord1 output and remap input registers
                    match = Regex.Match(lines[i], @"dcl_texcoord(\d*) o(\d+)");
                    if (match.Success)
                    {
                        var texGroup = match.Groups[1];
                        var outGroup = match.Groups[2];
                        var outIndex = int.Parse(outGroup.Value);
                        if (texGroup.Length == 0)
                        {
                            wOut = outIndex + 1;
                            lines.Insert(++i, string.Format("    dcl_texcoord1 o{0}.x", outIndex + 1));
                            continue;
                        }
                        var texIndex = int.Parse(texGroup.Value);
                        lines[i] = lines[i].Remove(texGroup.Index, texGroup.Length).Insert(texGroup.Index, (texIndex + 1).ToString());
                        lines[i] = lines[i].Remove(outGroup.Index, outGroup.Length).Insert(outGroup.Index, (outIndex + 1).ToString());
                        continue;
                    }

                    if (wOut >= 0)
                    {
                        foreach (Match m in Regex.Matches(lines[i], @" o(\d+)"))
                        {
                            var group = m.Groups[1];
                            var index = int.Parse(group.Value);
                            if (index < wOut)
                                continue;
                            index++;
                            lines[i] = lines[i].Remove(group.Index, group.Length).Insert(group.Index, index.ToString());
                        }
                    }
                }

                // Albedo-only: make o2 be 4D
                if (mode == 1)
                {
                    match = Regex.Match(lines[i], @"dcl_texcoord1 o2\.xyz");
                    if (match.Success)
                    {
                        wOut = 2;
                        lines[i] = lines[i].Remove(match.Index, match.Length).Insert(match.Index, "dcl_texcoord1 o2");
                        continue;
                    }
                }

                if (binormalRegister >= 0)
                {
                    // Change the source of the position W component
                    lines[i] = lines[i].Replace("v0.w", string.Format("v{0}.x", binormalRegister));
                }
            }

            // Reassemble the shader
            var newShader = string.Join("\n", lines);
			new Assemble(newShader, out string errors, out byte[] data);
			return data;
        }

        /// <summary>
        /// Converts a new (1.235640+) pixel shader to one compatible with the 1.106708 pipeline.
        /// </summary>
        /// <param name="shaderData">The shader bytecode to convert.</param>
        /// <param name="mode">The drawing mode.</param>
        /// <returns>The new bytecode, or <c>null</c> if conversion failed.</returns>
        public static byte[] ConvertNewPixelShaderToOld(byte[] shaderData, int mode)
        {
            if (mode != 0 && mode != 1)
                return shaderData; // Only default albedo shaders need to be fixed (?)

            // Disassemble the shader
            new Disassemble(shaderData, out string disassembly);
            if (disassembly == null)
                return null;
            var lines = disassembly.Split('\n').ToList();

            // Default pixel shaders need to have their inputs remapped (see
            // the vertex shader converter for more info).
            //
            // Albedo pixel shaders need to have sRGB color correction added.
            // The code is based off of the shader for the street cone.

            var highestConstant = -1;
            var addedConstants = false;
            var addedColorFix = false;
            var highestTexcoord = -1;
            var highestInput = -1;
            var wIn = -1;
            for (var i = 0; i < lines.Count; i++)
            {
                if (mode == 0)
                {
                    // Default fixes
                    var match = Regex.Match(lines[i], @"dcl_texcoord(\d*)");
                    if (match.Success)
                    {
                        var texGroup = match.Groups[1];
                        var index = (texGroup.Length > 0) ? int.Parse(texGroup.Value) : 0;
                        highestTexcoord = Math.Max(highestTexcoord, index);
                        if (index >= 1)
                            lines[i] = lines[i].Remove(texGroup.Index, texGroup.Length).Insert(texGroup.Index, (index + 1).ToString());
                    }
                    match = Regex.Match(lines[i], @"dcl_\S+ v(\d+)");
                    if (match.Success)
                    {
                        var inGroup = match.Groups[1];
                        highestInput = Math.Max(highestInput, int.Parse(inGroup.Value));
                    }
                    else if (highestTexcoord >= 1 && highestInput >= 0)
                    {
                        wIn = highestInput + 1;
                        lines.Insert(i, string.Format("    dcl_texcoord1 v{0}.x", wIn));
                        lines.Add(string.Format("    mov oC2, v{0}.x", wIn));
                        break;
                    }
                }
                else if (mode == 1)
                {
                    // Albedo fixes
                    var match = Regex.Match(lines[i], @"dcl_texcoord1 v1\.xyz");
                    if (match.Success)
                    {
                        wIn = 1;
                        lines[i] = lines[i].Remove(match.Index, match.Length).Insert(match.Index, "dcl_texcoord1 v1");
                        continue;
                    }

                    // Find the number of the highest-used constant register
                    match = Regex.Match(lines[i], @"def c(\d+)");
                    if (match.Success)
                    {
                        highestConstant = Math.Max(highestConstant, int.Parse(match.Groups[1].Value));
                        continue;
                    }

                    if (!addedConstants)
                    {
                        // Add color correction constants before input/output declarations
                        match = Regex.Match(lines[i], @"dcl_");
                        if (match.Success)
                        {
                            lines.Insert(i,
                                string.Format("    def c{0}, 0.416666657, 1.05499995, -0.0549999997, 0.00313080009\n" +
                                              "    def c{1}, 12.9200001, 0, 0, 0",
                                    highestConstant + 1, highestConstant + 2));
                            addedConstants = true;
                            continue;
                        }
                    }

                    if (!addedColorFix)
                    {
                        // Add color correction code before the output color is set
                        match = Regex.Match(lines[i], @"\S+ (oC0)\.xyz");
                        if (match.Success)
                        {
                            // NOTE: This is really hacky and just assumes that
                            // r29-r31 aren't used. It seems to work OK, but if we
                            // run into issues then some sort of register
                            // allocation may need to be done.
                            var group = match.Groups[1];
                            lines[i] = lines[i].Remove(group.Index, group.Length).Insert(group.Index, "r31");
                            lines.Insert(++i,
                                string.Format("    log r30.x, r31.x\n" +
                                              "    log r30.y, r31.y\n" +
                                              "    log r30.z, r31.z\n" +
                                              "    mul r30.xyz, r30, c{0}.x\n" +
                                              "    exp r29.x, r30.x\n" +
                                              "    exp r29.y, r30.y\n" +
                                              "    exp r29.z, r30.z\n" +
                                              "    mad r30.xyz, r29, c{0}.y, c{0}.z\n" +
                                              "    add r29.xyz, -r31, c{0}.w\n" +
                                              "    mul r31.xyz, r31, c{1}.x\n" +
                                              "    cmp oC0.xyz, r29, r31, r30",
                                    highestConstant + 1, highestConstant + 2));
                            if (wIn >= 0)
                                lines.Insert(++i, string.Format("    mov oC2, v{0}.w", wIn));
                            addedColorFix = true;
                            continue;
                        }
                    }
                }
            }

            // Reassemble the shader
            var newShader = string.Join("\n", lines);
			new Assemble(newShader, out string errors, out byte[] data);
			return data;
        }
    }
}
