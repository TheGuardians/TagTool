using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Cache.Gen2;
using System.IO;
using TagTool.Commands.Common;

namespace TagTool.Commands.Porting.Gen2
{
	partial class PortTagGen2Command : Command
	{
        public object ConvertObject(object gen2Tag)
        {
            switch (gen2Tag)
            {
                case TagTool.Tags.Definitions.Gen2.Crate crate:
                    Crate newcrate = new Crate();
                    TranslateTagStructure(crate, newcrate, typeof(TagTool.Tags.Definitions.Gen2.Crate), typeof(Crate));
                    return newcrate;
                case TagTool.Tags.Definitions.Gen2.Scenery scenery:
                    Scenery newscenery = new Scenery();
                    TranslateTagStructure(scenery, newscenery, typeof(TagTool.Tags.Definitions.Gen2.Scenery), typeof(Scenery));
                    return newscenery;
                case TagTool.Tags.Definitions.Gen2.Weapon weapon:
                    Weapon newweapon = new Weapon();
                    TranslateTagStructure(weapon, newweapon, typeof(TagTool.Tags.Definitions.Gen2.Weapon), typeof(Weapon));
                    return newweapon;
                case TagTool.Tags.Definitions.Gen2.Vehicle vehicle:
                    Vehicle newvehicle = new Vehicle();
                    TranslateTagStructure(vehicle, newvehicle, typeof(TagTool.Tags.Definitions.Gen2.Vehicle), typeof(Vehicle));
                    return newvehicle;
                default:
                    return null;
            }
        }

        public Weapon ConvertWeapon(TagTool.Tags.Definitions.Gen2.Weapon gen2Tag)
        {
            Weapon gameObject = new Weapon
            {
                ObjectFlags = (ObjectDefinitionFlags)gen2Tag.Flags,
                BoundingOffset = gen2Tag.BoundingOffset,
                BoundingRadius = gen2Tag.BoundingRadius,
                AccelerationScale = gen2Tag.AccelerationScale,
                LightmapShadowMode = (GameObject.LightmapShadowModeValue)gen2Tag.LightmapShadowMode,
                SweetenerSize = (GameObject.SweetenerSizeValue)gen2Tag.SweetenerSize, //TODO: default enum value added need to account for this
                DynamicLightSphereRadius = gen2Tag.DynamicLightSphereRadius,
                DynamicLightSphereOffset = gen2Tag.DynamicLightSphereOffset,

            };

            return gameObject;
        }

        
    }
}
