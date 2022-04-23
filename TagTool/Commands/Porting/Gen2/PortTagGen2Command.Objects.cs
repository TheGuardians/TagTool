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
using VehicleGen2 = TagTool.Tags.Definitions.Gen2.Vehicle;
using WeaponGen2 = TagTool.Tags.Definitions.Gen2.Weapon;
using CrateGen2 = TagTool.Tags.Definitions.Gen2.Crate;
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
                case CrateGen2 crate:
                    Crate newcrate = new Crate();
                    ConvertStructure(crate, newcrate, typeof(CrateGen2), typeof(Crate));
                    return newcrate;
                case WeaponGen2 weapon:
                    Weapon newweapon = new Weapon();
                    ConvertStructure(weapon, newweapon, typeof(WeaponGen2), typeof(Weapon));
                    return newweapon;
                case VehicleGen2 vehicle:
                    Vehicle newvehicle = new Vehicle();
                    ConvertStructure(vehicle, newvehicle, typeof(VehicleGen2), typeof(Vehicle));
                    return newvehicle;
                default:
                    return null;
            }
        }

        public Weapon ConvertWeapon(WeaponGen2 gen2Tag)
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
