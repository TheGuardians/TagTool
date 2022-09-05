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
            //build markers
            foreach(var markergroup in mode.MarkerGroups)
            {
                var name = Cache.StringTable.GetString(markergroup.Name);
                foreach(var marker in markergroup.Markers)
                {
                    Jms.Markers.Add(new JmsFormat.JmsMarker
                    {
                        Name = name,
                        NodeIndex = marker.NodeIndex,
                        Rotation = marker.Rotation,
                        Translation = new RealVector3d(marker.Translation.X, marker.Translation.Y, marker.Translation.Z) * 100.0f,
                        Radius = marker.Scale
                    });
                }

            }
        }


    }
}
