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

        public RenderMaterial(Direct3D.Device device, HaloOnlineCacheContext cacheContext, TagTool.Geometry.RenderMaterial material)
        {
            if (material.RenderMethod == null)
                return;

            using (var cacheStream = cacheContext.OpenTagCacheRead())
            {
                var renderMethod = (RenderMethod)cacheContext.Deserialize(
                    new TagSerializationContext(cacheStream, cacheContext, material.RenderMethod),
                    TagDefinition.Find(material.RenderMethod.Group));

                var template = cacheContext.Deserialize<RenderMethodTemplate>(
                    new TagSerializationContext(cacheStream, cacheContext, renderMethod.ShaderProperties[0].Template));

                Textures = new Dictionary<string, RenderTexture>();

                for (var shaderMapIndex = 0; shaderMapIndex < renderMethod.ShaderProperties[0].ShaderMaps.Count; shaderMapIndex++)
                {
                    var shaderMapName = cacheContext.GetString(template.SamplerArguments[shaderMapIndex].Name);

                    if (Textures.ContainsKey(shaderMapName))
                        continue;

                    var shaderMap = renderMethod.ShaderProperties[0].ShaderMaps[shaderMapIndex];
                    var shaderMapDefinition = cacheContext.Deserialize<Bitmap>(new TagSerializationContext(cacheStream, cacheContext, shaderMap.Bitmap));

                    Textures[shaderMapName] = null;// new RenderTexture(device, cacheContext, shaderMapDefinition, shaderMap.BitmapIndex);
                }
            }
        }
    }
}