using TagTool.Cache;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using TagTool.Shaders;

namespace TagTool.Commands.Shaders
{
    public class DisassembleCommand<T> : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private T Definition { get; }

        public DisassembleCommand(GameCacheContext cacheContext, CachedTagInstance tag, T definition) :
            base(true,

                "Disassemble",
                "Disassembles a VertexShader at the specified index.",

                "Disassemble <index>",

                "<index> - index into the VertexShaders tagblock.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            if (typeof(T) == typeof(PixelShader) || typeof(T) == typeof(GlobalPixelShader))
            {
                PixelShaderBlock shader_block = null;
                if (typeof(T) == typeof(PixelShader))
                {
                    var _definition = Definition as PixelShader;
                    shader_block = _definition.Shaders[int.Parse(args[0])];
                }

                if (typeof(T) == typeof(GlobalPixelShader))
                {
                    var _definition = Definition as GlobalPixelShader;
                    shader_block = _definition.Shaders[int.Parse(args[0])];
                }

                var pc_shader = shader_block.PCShaderBytecode;
                var disassembly = D3DCompiler.Disassemble(pc_shader);
				if (pc_shader != null) Console.WriteLine(disassembly);
                else Console.WriteLine("Failed to disassemble shader");
            }

            if (typeof(T) == typeof(VertexShader) || typeof(T) == typeof(GlobalVertexShader))
            {
                VertexShaderBlock shader_block = null;
                if (typeof(T) == typeof(VertexShader))
                {
                    var _definition = Definition as VertexShader;
                    shader_block = _definition.Shaders[int.Parse(args[0])];
                }

                if (typeof(T) == typeof(GlobalVertexShader))
                {
                    var _definition = Definition as GlobalVertexShader;
                    shader_block = _definition.Shaders[int.Parse(args[0])];
                }

                var pc_shader = shader_block.PCShaderBytecode;
                var disassembly = D3DCompiler.Disassemble(pc_shader);
                if (pc_shader != null) Console.WriteLine(disassembly);
                else Console.WriteLine("Failed to disassemble shader");
            }


            return true;
        }
    }
}
