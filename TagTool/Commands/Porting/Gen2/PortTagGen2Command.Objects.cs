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
                    newcrate.ObjectType = new GameObjectType { Halo3ODST = GameObjectTypeHalo3ODST.Crate };
                    return newcrate;
                case TagTool.Tags.Definitions.Gen2.Scenery scenery:
                    Scenery newscenery = new Scenery();
                    TranslateTagStructure(scenery, newscenery, typeof(TagTool.Tags.Definitions.Gen2.Scenery), typeof(Scenery));
                    newscenery.ObjectType = new GameObjectType { Halo3ODST = GameObjectTypeHalo3ODST.Scenery };
                    return newscenery;
                case TagTool.Tags.Definitions.Gen2.Weapon weapon:
                    Weapon newweapon = new Weapon();
                    TranslateTagStructure(weapon, newweapon, typeof(TagTool.Tags.Definitions.Gen2.Weapon), typeof(Weapon));
                    newweapon.ObjectType = new GameObjectType { Halo3ODST = GameObjectTypeHalo3ODST.Weapon };
                    return FixupWeapon(weapon, newweapon);
                case TagTool.Tags.Definitions.Gen2.Vehicle vehicle:
                    Vehicle newvehicle = new Vehicle();
                    TranslateTagStructure(vehicle, newvehicle, typeof(TagTool.Tags.Definitions.Gen2.Vehicle), typeof(Vehicle));
                    newvehicle.ObjectType = new GameObjectType { Halo3ODST = GameObjectTypeHalo3ODST.Vehicle };
                    return newvehicle;
                case TagTool.Tags.Definitions.Gen2.Projectile projectile:
                    Projectile newprojectile = new Projectile();
                    TranslateTagStructure(projectile, newprojectile, typeof(TagTool.Tags.Definitions.Gen2.Projectile), typeof(Projectile));
                    newprojectile.ObjectType = new GameObjectType { Halo3ODST = GameObjectTypeHalo3ODST.Projectile };
                    return newprojectile;
                default:
                    return null;
            }
        }

        public Weapon FixupWeapon(TagTool.Tags.Definitions.Gen2.Weapon gen2Tag, Weapon newweapon)
        {
            newweapon.FirstPerson = new List<Weapon.FirstPersonBlock>();
            foreach(var firstperson in gen2Tag.PlayerInterface.FirstPerson)
            {
                newweapon.FirstPerson.Add(new Weapon.FirstPersonBlock
                {
                    FirstPersonModel = firstperson.FirstPersonModel,
                    FirstPersonAnimations = firstperson.FirstPersonAnimations
                });
            }

            return newweapon;
        }

        
    }
}
