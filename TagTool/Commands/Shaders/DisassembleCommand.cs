using TagTool.Cache;
using TagTool.Commands;
using TagTool.Geometry;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;

namespace TagTool.Commands.Shaders
{
    class DisassembleCommand<T> : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private T Definition { get; }

        public DisassembleCommand(GameCacheContext cacheContext, CachedTagInstance tag, T definition) :
            base(CommandFlags.Inherit,

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

            if (typeof(T) == typeof(VertexShader))
            {
                var _definition = Definition as VertexShader;
                var shader = _definition.Shaders[int.Parse(args[0])];
                var pc_shader = shader.PCShaderBytecode;
                var xbox_shader = shader.XboxShaderReference?.ShaderData;
                Console.WriteLine("PC Shader");
                Console.WriteLine(ShaderCompiler.Disassemble(pc_shader));
                //Console.WriteLine("Xbox Shader");
                //Console.WriteLine(ShaderCompiler.Disassemble(xbox_shader));
            }

            if (typeof(T) == typeof(PixelShader))
            {
                var _definition = Definition as PixelShader;
                var shader = _definition.Shaders[int.Parse(args[0])];
                var pc_shader = shader.PCShaderBytecode;
                var xbox_shader = shader.XboxShaderReference?.ShaderData;
                Console.WriteLine("PC Shader");
                Console.WriteLine(ShaderCompiler.Disassemble(pc_shader));
                //Console.WriteLine("Xbox Shader");
                //Console.WriteLine(ShaderCompiler.Disassemble(xbox_shader));
            }

            if (typeof(T) == typeof(GlobalPixelShader))
            {
                var _definition = Definition as GlobalPixelShader;
                var shader = _definition.Shaders[int.Parse(args[0])];
                var pc_shader = shader.PCShaderBytecode;
                var xbox_shader = shader.XboxShaderReference.ShaderData;
                Console.WriteLine("PC Shader");
                Console.WriteLine(ShaderCompiler.Disassemble(pc_shader));
                //Console.WriteLine("Xbox Shader");
                //Console.WriteLine(ShaderCompiler.Disassemble(xbox_shader));
            }

            if (typeof(T) == typeof(GlobalVertexShader))
            {
                var _definition = Definition as GlobalVertexShader;
                var shader = _definition.Shaders[int.Parse(args[0])];
                var pc_shader = shader.PCShaderBytecode;
                var xbox_shader = shader.XboxShaderReference?.ShaderData;
                Console.WriteLine("PC Shader");
                Console.WriteLine(ShaderCompiler.Disassemble(pc_shader));
                //Console.WriteLine("Xbox Shader");
                //Console.WriteLine(ShaderCompiler.Disassemble(xbox_shader));
            }


            return true;
        }
    }
}
