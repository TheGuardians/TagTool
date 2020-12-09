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

                 "SetBitmap <textureParameterName> <tagname>",

				 "Sets the bitmap for the specified texture constant in the render_method.")
        {
            Cache = cache;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return new TagToolError(CommandError.ArgCount);

			var properties = Definition.ShaderProperties[0];

			RenderMethodTemplate template = null;

			using (var cacheStream = Cache.OpenCacheRead())
				template = Cache.Deserialize<RenderMethodTemplate>(cacheStream, properties.Template);

			object output = null;

			if (!Cache.TagCache.TryGetCachedTag(args[1], out var tagInstance))
				return new TagToolError(CommandError.ArgInvalid, $"Bitmap does not exist in current cache: {args[1]}");

			output = tagInstance;

            var parameterIndex = -1;

			for (var i = 0; i < template.TextureParameterNames.Count; i++)
            {
                if (Cache.StringTable.GetString(template.TextureParameterNames[i].Name) == args[0])
                {
					parameterIndex = i;
                    break;
                }
            }

			if (parameterIndex < 0 || parameterIndex >= properties.TextureConstants.Count)
				return new TagToolError(CommandError.ArgInvalid, $"Invalid texture parameter name: {args[0]}");
			else
			{
				properties.TextureConstants[parameterIndex].Bitmap = tagInstance;
				Console.WriteLine(string.Format("{0}: CachedTag = [0x{1}] {2}", args[0], tagInstance.Index.ToString("X4"), tagInstance.Name));
			}

			//var argument = properties.TextureConstants[parameterIndex];

			Console.WriteLine();

            return true;
        }
    }
}