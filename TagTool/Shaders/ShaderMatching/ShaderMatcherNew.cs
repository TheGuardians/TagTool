using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Shaders.ShaderMatching
{
    public class ShaderMatcherNew
    {
        public bool UseMS30;
        public GameCache BaseCache;
        public GameCache PortingCache;
        public Stream BaseCacheStream;
        public Stream PortingCacheStream;

        public ShaderMatcherNew(GameCache baseCache,  GameCache portingCache, Stream baseCacheStream, Stream portingCacheStream, bool useMS30=false)
        {
            UseMS30 = useMS30;
            BaseCache = baseCache;
            PortingCache = portingCache;
            BaseCacheStream = baseCacheStream;
            PortingCacheStream = portingCacheStream;
            Init();
        }

        private void Init()
        {
            // prepare all rmt2s from the base cache to be ready for matching
        }

        /// <summary>
        /// Find the closest template in the base cache to the input template.
        /// </summary>
        private RenderMethodTemplate FindClosestTemplate(RenderMethod renderMethod, CachedTag templateTag, RenderMethodTemplate template)
        {
            return null;
        }

        /// <summary>
        /// Modifies the input render method to make it work using the matchedTemplate
        /// </summary>
        private RenderMethod MatchRenderMethods(RenderMethod renderMethod, RenderMethodTemplate matchedTemplate, RenderMethodTemplate originalTemplate)
        {

            return renderMethod;
        }

        public RenderMethod MatchShader(RenderMethod renderMethod)
        {
            if (renderMethod.ShaderProperties.Count < 0)
                return renderMethod;

            var property = renderMethod.ShaderProperties[0];
            var originalTemplate = PortingCache.Deserialize<RenderMethodTemplate>(PortingCacheStream, property.Template);
            var matchedTemplate = FindClosestTemplate(renderMethod, property.Template, originalTemplate);
            renderMethod = MatchRenderMethods(renderMethod, matchedTemplate, originalTemplate);
            // might require some special checks if match render method can fail
            return renderMethod;
        }
    }
}
