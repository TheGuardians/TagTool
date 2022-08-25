using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Tags.Definitions;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using System.Numerics;

namespace TagTool.Geometry.Jms
{
    public class JmsModeExporter
    {
        GameCache Cache { get; set; }
        JmsFormat Jms { get; set; }

        public JmsModeExporter(GameCache cacheContext, JmsFormat jms)
        {
            Cache = cacheContext;
            Jms = jms;
        }

        public void Export(RenderModel mode)
        {
            
        }


    }
}
