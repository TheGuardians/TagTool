using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Cache.Gen2;
using System.IO;
using TagTool.Commands.Common;

namespace TagTool.Commands.Porting.Gen2
{
	partial class PortTagGen2Command : Command
	{
        public object ConvertScenario(object gen2Tag)
        {
            Scenario newScenario = new Scenario();
            TranslateTagStructure((TagTool.Tags.Definitions.Gen2.Scenario)gen2Tag, newScenario, typeof(TagTool.Tags.Definitions.Gen2.Scenario), typeof(Scenario));



            return newScenario;
        }

        public object ConvertStructureBSP(TagTool.Tags.Definitions.Gen2.ScenarioStructureBsp gen2Tag)
        {
            ScenarioStructureBsp newSbsp = new ScenarioStructureBsp();
            TranslateTagStructure(gen2Tag, newSbsp, typeof(TagTool.Tags.Definitions.Gen2.ScenarioStructureBsp), typeof(ScenarioStructureBsp));

            //COLLISION RESOURCE
            //create new collisionresource and populate values from tag
            StructureBspTagResources CollisionResource = new StructureBspTagResources();

            //main collision geometry
            CollisionResource.CollisionBsps = new TagBlock<TagTool.Geometry.BspCollisionGeometry.CollisionGeometry>();
            foreach (var collisiongeo in gen2Tag.CollisionBsp)
                CollisionResource.CollisionBsps.Add(collisiongeo);

            //instanced geometry definitions
            CollisionResource.InstancedGeometry = new TagBlock<TagTool.Geometry.BspCollisionGeometry.InstancedGeometryBlock>();
            foreach(var instanced in gen2Tag.InstancedGeometriesDefinitions)
            {
                CollisionResource.InstancedGeometry.Add(new TagTool.Geometry.BspCollisionGeometry.InstancedGeometryBlock
                {
                    Checksum = instanced.Checksum,
                    BoundingSphereOffset = instanced.BoundingSphereCenter,
                    BoundingSphereRadius = instanced.BoundingSphereRadius,
                    CollisionInfo = instanced.CollisionInfo,

                });
            }

            return newSbsp;
        }
    }
}
