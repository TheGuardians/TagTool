using TagTool.Cache;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using TagTool.Shaders;
using System.IO;

namespace TagTool.Commands.Shaders
{
    public class DisassembleCommand<T> : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private T Definition { get; }

        public DisassembleCommand(GameCache cache, CachedTag tag, T definition) :
            base(true,

                "Disassemble",
                "Disassembles a VertexShader at the specified index.",

                "Disassemble <index>",

                "<index> - index into the VertexShaders tagblock.")
        {
            Cache = cache;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1 && args.Count != 2)
                return false;

            var disassemblies = new List<string> { };

            if (args[0] == "*")
            {
                for (var i = 0; ; i++)
                {
                    var disassembly = Disassemble(i);
                    if (disassembly == null)
                        break;
                    else
                        disassemblies.Add(disassembly);
                }
            }
            else
            {
                if (int.TryParse(args[0], out int shaderIndex))
                    disassemblies.Add(Disassemble(shaderIndex));
            }

            if (args.Count == 2)
                for (var i = 0; i < disassemblies.Count; i++)
                    using (var writer = File.CreateText(Path.Combine(args[1], $"{i}.{Tag.Group}")))
                        writer.WriteLine(disassemblies[i]);
            else
                foreach (var disassembly in disassemblies)
                    Console.WriteLine(disassembly);

            return true;
        }

        private string Disassemble(int shaderIndex)
        {
            string disassembly = null;

            if (typeof(T) == typeof(PixelShader) || typeof(T) == typeof(GlobalPixelShader))
            {
                PixelShaderBlock shader_block = null;
                if (typeof(T) == typeof(PixelShader))
                {
                    var _definition = Definition as PixelShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return null;
                }

                if (typeof(T) == typeof(GlobalPixelShader))
                {
                    var _definition = Definition as GlobalPixelShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return null;
                }

                var pc_shader = shader_block.PCShaderBytecode;
                disassembly = D3DCompiler.Disassemble(pc_shader);
                if (pc_shader == null)
                    disassembly = null;
            }

            if (typeof(T) == typeof(VertexShader) || typeof(T) == typeof(GlobalVertexShader))
            {
                VertexShaderBlock shader_block = null;
                if (typeof(T) == typeof(VertexShader))
                {
                    var _definition = Definition as VertexShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return null;
                }

                if (typeof(T) == typeof(GlobalVertexShader))
                {
                    var _definition = Definition as GlobalVertexShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return null;
                }

                var pc_shader = shader_block.PCShaderBytecode;
                disassembly = D3DCompiler.Disassemble(pc_shader);
                if (pc_shader == null)
                    disassembly = null;
            }

            return disassembly;
        }
    }
}
