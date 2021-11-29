using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags;
using TagTool.Cache.HaloOnline;
using TagTool.Cache.Gen3;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Tags
{
    class SwitchObjectTypeCommand : Command
    {
        public GameCacheHaloOnlineBase Cache { get; }

        public SwitchObjectTypeCommand(GameCacheHaloOnlineBase cache)
            : base(true,

                  "SwitchObjectType",
                  "Creates a new tag from a specified obje tag with a different type.",

                  "SwitchObjectType <object tag>",

                  "Creates a new object tag of a different type from the one specified, but with otherwise the same data.\n"+
                  "Currently supports scen -> bloc, bloc -> scen.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            if (!Cache.TagCache.TryGetTag(args[0], out CachedTag inputTag))
                return new TagToolError(CommandError.TagInvalid);

            CachedTag instance = null;

            using (var stream = Cache.OpenCacheReadWrite())
            {
                object inputDefinition = Cache.Deserialize(stream, inputTag);
                object instanceDefinition = null;

                if (inputTag.IsInGroup("scen"))
                    instance = Cache.TagCache.AllocateTag(Cache.TagCache.TagDefinitions.GetTagGroupFromTag("bloc"), inputTag.Name);
                else if (inputTag.IsInGroup("bloc"))
                    instance = Cache.TagCache.AllocateTag(Cache.TagCache.TagDefinitions.GetTagGroupFromTag("scen"), inputTag.Name);
                else
                    return new TagToolError(CommandError.TagInvalid);

                Cache.Serialize(stream, instance, Activator.CreateInstance(Cache.TagCache.TagDefinitions.GetTagDefinitionType(instance.Group)));

                instanceDefinition = Cache.Deserialize(stream, instance);

                SetCommonFields((GameObject)inputDefinition, (GameObject)instanceDefinition);

                switch (instance.Group.ToString())
                {
                    case "scenery":
                        ((GameObject)instanceDefinition).ObjectType.Halo3ODST = GameObjectTypeHalo3ODST.Scenery;
                        break;
                    case "crate":
                        ((GameObject)instanceDefinition).ObjectType.Halo3ODST = GameObjectTypeHalo3ODST.Crate;
                        break;
                    case "device_machine":
                        ((GameObject)instanceDefinition).ObjectType.Halo3ODST = GameObjectTypeHalo3ODST.Machine;
                        break;
                    case "device_control":
                        ((GameObject)instanceDefinition).ObjectType.Halo3ODST = GameObjectTypeHalo3ODST.Control;
                        break;
                    case "vehicle":
                        ((GameObject)instanceDefinition).ObjectType.Halo3ODST = GameObjectTypeHalo3ODST.Vehicle;
                        break;
                    case "equipment":
                        ((GameObject)instanceDefinition).ObjectType.Halo3ODST = GameObjectTypeHalo3ODST.Equipment;
                        break;
                    case "terminal":
                        ((GameObject)instanceDefinition).ObjectType.Halo3ODST = GameObjectTypeHalo3ODST.Terminal;
                        break;
                    case "giant":
                        ((GameObject)instanceDefinition).ObjectType.Halo3ODST = GameObjectTypeHalo3ODST.Giant;
                        break;
                }

                Cache.Serialize(stream, instance, instanceDefinition);
                Cache.SaveTagNames(null);
            }

            var tagName = instance.Name ?? $"0x{instance.Index:X4}";

            Console.WriteLine($"[Index: 0x{instance.Index:X4}] {tagName}.{instance.Group}");
            return true;
        }

        private void SetCommonFields(GameObject input, GameObject instance)
        {
            //instance.ObjectType = 
            instance.BoundingRadius = input.BoundingRadius;
            instance.BoundingOffset = input.BoundingOffset;
            instance.AccelerationScale = input.AccelerationScale;
            instance.WaterDensity = input.WaterDensity;
            instance.RuntimeFlags = input.RuntimeFlags;
            instance.DynamicLightSphereRadius = input.DynamicLightSphereRadius;
            instance.DynamicLightSphereOffset = input.DynamicLightSphereOffset;
            instance.DefaultModelVariant = input.DefaultModelVariant;
            instance.Model = input.Model;
            instance.CrateObject = input.CrateObject;
            instance.CollisionDamage = input.CollisionDamage;
            instance.EarlyMoverProperties = input.EarlyMoverProperties;
            instance.CreationEffect = input.CreationEffect;
            instance.MaterialEffects = input.MaterialEffects;
            instance.ArmorSounds = input.ArmorSounds;
            instance.MeleeImpactSound = input.MeleeImpactSound;
            instance.AiProperties = input.AiProperties;
            instance.Functions = input.Functions;
            instance.HudTextMessageIndex = input.HudTextMessageIndex;
            instance.SecondaryFlags = input.SecondaryFlags;
            instance.Attachments = input.Attachments;
            instance.Widgets = input.Widgets;
            instance.ChangeColors = input.ChangeColors;
            instance.NodeMaps = input.NodeMaps;
            instance.MultiplayerObject = input.MultiplayerObject;
            instance.RevivingEquipment = input.RevivingEquipment;
            instance.PathfindingSpheres = input.PathfindingSpheres;
        }
    }
}
