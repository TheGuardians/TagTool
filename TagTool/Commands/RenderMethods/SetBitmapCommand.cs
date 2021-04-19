using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.RenderMethods
{
    class SetBitmapCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private RenderMethod Definition { get; }

        public SetBitmapCommand(GameCache cache, CachedTag tag, RenderMethod definition)
            : base(true,

                 "SetBitmap",
                 "Sets the bitmap for the specified texture parameter in the render_method.",

                 "SetBitmap <textureParameterName> <tagname or default bitmap> [transform]",

				 "Sets the bitmap for the specified texture constant in the render_method.")
        {
            Cache = cache;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 2)
                return new TagToolError(CommandError.ArgCount);

            CachedTag tagInstance = null;
            RenderMethodTemplate template = null;
            using (var cacheStream = Cache.OpenCacheRead())
            {
                if (!Cache.TagCache.TryGetCachedTag(args[1], out tagInstance))
                {
                    if (!RasterizerGlobals.DefaultBitmap.ParseDefaultBitmap(args[1], out RasterizerGlobals.DefaultBitmap.RasterizerDefaultBitmap defaultBitmap))
                        return new TagToolError(CommandError.ArgInvalid, $"Bitmap \"{args[1]}\" does not exist");

                    // potentially unsafe but no point editing shaders if no rasg tag is present
                    RasterizerGlobals rasg = Cache.Deserialize<RasterizerGlobals>(cacheStream, Cache.TagCache.FindFirstInGroup("rasg"));

                    tagInstance = rasg.DefaultBitmaps[(int)defaultBitmap].Bitmap;
                }

                template = Cache.Deserialize<RenderMethodTemplate>(cacheStream, Definition.ShaderProperties[0].Template);
            }

            int parameterIndex = -1;
			for (var i = 0; i < template.TextureParameterNames.Count; i++)
            {
                if (Cache.StringTable.GetString(template.TextureParameterNames[i].Name) == args[0])
                {
					parameterIndex = i;
                    break;
                }
            }

			if (parameterIndex < 0)
				return new TagToolError(CommandError.ArgInvalid, $"Invalid texture parameter name: {args[0]}");

            Definition.ShaderProperties[0].TextureConstants[parameterIndex].Bitmap = tagInstance;

            var realConstants = Definition.ShaderProperties[0].RealConstants[0];

            int realIndex = Definition.ShaderProperties[0].TextureConstants[parameterIndex].XFormArgumentIndex;
            if (realIndex == -1)
            {
                for (var i = 0; i < template.RealParameterNames.Count; i++)
                {
                    if (Cache.StringTable.GetString(template.RealParameterNames[i].Name) == args[0])
                    {
                        realIndex = i;
                        break;
                    }
                }
            }

            if (realIndex != -1)
            {
                realConstants = Definition.ShaderProperties[0].RealConstants[realIndex];
                
                switch (args.Count - 2)
                {
                    case 1:
                        realConstants.Arg0 = TryParseFloatString(args[2]);
                        break;
                    case 2:
                        realConstants.Arg0 = TryParseFloatString(args[2]);
                        realConstants.Arg1 = TryParseFloatString(args[3]);
                        break;
                    case 3:
                        realConstants.Arg0 = TryParseFloatString(args[2]);
                        realConstants.Arg1 = TryParseFloatString(args[3]);
                        realConstants.Arg2 = TryParseFloatString(args[4]);
                        break;
                    case 4:
                        realConstants.Arg0 = TryParseFloatString(args[2]);
                        realConstants.Arg1 = TryParseFloatString(args[3]);
                        realConstants.Arg2 = TryParseFloatString(args[4]);
                        realConstants.Arg3 = TryParseFloatString(args[5]);
                        break;
                }
                
                Console.WriteLine($"{args[0]}: {tagInstance.Name}.bitmap, [{realConstants.Arg0}, {realConstants.Arg1}, {realConstants.Arg2}, {realConstants.Arg3}]");
            }
            else
            {
                Console.WriteLine($"{args[0]}: {tagInstance.Name}.bitmap");
            }
            return true;
        }

        float TryParseFloatString(string input)
        {
            if (float.TryParse(input, out float result))
                return result;
            return 0.0f;
        }
    }
}