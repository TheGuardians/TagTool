using TagTool.Cache;
using TagTool.Geometry;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using System.Collections.Generic;
using Direct3D = Microsoft.DirectX.Direct3D;

namespace Sentinel.Render
{
    public class RenderMaterial
    {
        public RenderMethod RenderMethod { get; }
        public Dictionary<string, RenderTexture> Textures { get; }
        public Direct3D.Effect Effect { get; }

        public RenderMaterial(Direct3D.Device device, GameCache cache, TagTool.Geometry.RenderMaterial material)
        {
            if (material.RenderMethod == null)
                return;

            using (var cacheStream = cache.OpenCacheRead())
            {
                var renderMethod = cache.Deserialize<RenderMethod>(cacheStream, material.RenderMethod);

                var template = cache.Deserialize<RenderMethodTemplate>(cacheStream, renderMethod.ShaderProperties[0].Template);

                Textures = new Dictionary<string, RenderTexture>();

                for (var shaderMapIndex = 0; shaderMapIndex < renderMethod.ShaderProperties[0].TextureConstants.Count; shaderMapIndex++)
                {
                    var shaderMapName = cache.StringTable.GetString(template.TextureParameterNames[shaderMapIndex].Name);

                    if (Textures.ContainsKey(shaderMapName))
                        continue;

                    var shaderMap = renderMethod.ShaderProperties[0].TextureConstants[shaderMapIndex];
                    var shaderMapDefinition = cache.Deserialize<Bitmap>(cacheStream, shaderMap.Bitmap);

                    Textures[shaderMapName] = new RenderTexture(device, cache, shaderMapDefinition, shaderMap.BitmapIndex);
                }
            }
        }
    }
}