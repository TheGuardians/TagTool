using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Forge
{
    class ParseItemsXmlCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CachedTagInstance Instance { get; }
        private ForgeGlobalsDefinition Definition { get; }

        public ParseItemsXmlCommand(HaloOnlineCacheContext cacheContext, CachedTagInstance instance, ForgeGlobalsDefinition definition) :
            base(true,
                
                "ParseItemsXml",
                "",
                
                "ParseItemsXml <File>",
                
                "")
        {
            CacheContext = cacheContext;
            Instance = instance;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var xml = new XmlDocument();
            xml.Load(args[0]);

            foreach (XmlNode node in xml["root"].ChildNodes)
                ParseXmlNode(node);

            return true;
        }

        private Dictionary<string, short> DescriptionIndices { get; } = new Dictionary<string, short>();

        private ForgeGlobalsDefinition.PaletteCategoryType? CategoryType { get; set; } = null;
        private Stack<(string, int)> CategoryStack { get; } = new Stack<(string, int)>();

        private ForgeGlobalsDefinition.PaletteItem Item { get; set; } = null;

        private void ParseXmlNode(XmlNode node)
        {
            if (node.NodeType != XmlNodeType.Element)
                return;

            switch (node.Name)
            {
                case "category":
                    if (Item != null)
                        throw new FormatException();

                    var categoryName = node.Attributes["name"].InnerText;

                    if (node.Attributes["id"] != null)
                        CategoryType = ParseEnum<ForgeGlobalsDefinition.PaletteCategoryType>(node.Attributes["id"].InnerText);

                    if (!CategoryType.HasValue)
                        throw new FormatException();

                    var categoryHelp = "";

                    if (node.Attributes["help"] != null)
                        categoryHelp = node.Attributes["help"].InnerText;

                    var descriptionIndex = -1;

                    if (categoryHelp != "")
                    {
                        if (DescriptionIndices.ContainsKey(categoryHelp))
                            descriptionIndex = DescriptionIndices[categoryHelp];
                        else
                        {
                            descriptionIndex = Definition.Descriptions.Count;
                            Definition.Descriptions.Add(new ForgeGlobalsDefinition.Description
                            {
                                Text = categoryHelp
                            });
                        }
                    }

                    var categoryIndex = (short)Definition.PaletteCategories.Count;
                    Definition.PaletteCategories.Add(new ForgeGlobalsDefinition.PaletteCategory
                    {
                        Name = categoryName,
                        DescriptionIndex = (short)descriptionIndex,
                        ParentCategoryIndex = (short)(CategoryStack.Count > 0 ? CategoryStack.Peek().Item2 : -1),
                        Type = CategoryType.Value
                    });

                    CategoryStack.Push((categoryName, categoryIndex));

                    foreach (XmlNode categoryNode in node.ChildNodes)
                        ParseXmlNode(categoryNode);

                    CategoryStack.Pop();
                    break;

                case "item":
                    if (CategoryStack.Count == 0)
                        throw new FormatException();

                    Item = new ForgeGlobalsDefinition.PaletteItem
                    {
                        Name = node.Attributes["name"].InnerText,
                        CategoryIndex = (short)CategoryStack.Peek().Item2,
                        Object = CacheContext.TryGetTag(node.Attributes["tagindex"].InnerText, out var obj) ? obj : null,
                        Setters = new List<ForgeGlobalsDefinition.PaletteItem.Setter>()
                    };

                    if (node.HasChildNodes)
                        foreach (XmlNode itemNode in node.ChildNodes)
                            ParseXmlNode(itemNode);

                    Definition.Palette.Add(Item);
                    Item = null;
                    break;

                case "setter":
                    if (Item == null || CategoryStack.Count == 0)
                        throw new FormatException();

                    var setterTarget = ParseSetterTarget(node.Attributes["target"].InnerText);

                    var setterType = ParseEnum<ForgeGlobalsDefinition.PaletteItem.SetterType>(node.Attributes["type"].InnerText);
                    var setterHidden = node.Attributes["hidden"] != null && node.Attributes["hidden"].InnerText.ToLower() == "true";

                    if (node.Attributes["value"] == null)
                        throw new FormatException();

                    Item.Setters.Add(new ForgeGlobalsDefinition.PaletteItem.Setter
                    {
                        Target = setterTarget,
                        Type = setterType.Value,
                        Flags = setterHidden ?
                            ForgeGlobalsDefinition.PaletteItem.SetterFlags.Hidden :
                            ForgeGlobalsDefinition.PaletteItem.SetterFlags.None,
                        IntegerValue = setterType.Value == ForgeGlobalsDefinition.PaletteItem.SetterType.Integer ?
                            int.Parse(node.Attributes["value"].InnerText) : 0,
                        RealValue = setterType.Value == ForgeGlobalsDefinition.PaletteItem.SetterType.Real ?
                            float.Parse(node.Attributes["value"].InnerText) : 0.0f
                    });
                    break;
            }
        }

        private ForgeGlobalsDefinition.PaletteItem.SetterTarget ParseSetterTarget(string query)
        {
            switch(query)
            {
                case "on_map_at_start":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.General_OnMapAtStart;
                case "symmetry":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.General_Symmetry;
                case "respawn_rate":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.General_RespawnRate;
                case "spare_clips":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.General_SpareClips;
                case "spawn_order":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.General_SpawnOrder;
                case "team_affiliation":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.General_Team;
                case "engine_flags":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.General_EngineFlags;
                case "physics":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.General_Physics;
                case "reforge_material":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Reforge_Material;
                case "reforge_material_color_r":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Reforge_Material_ColorR;
                case "reforge_material_color_g":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Reforge_Material_ColorG;
                case "reforge_material_color_b":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Reforge_Material_ColorB;
                case "reforge_material_tex_scale":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Reforge_Material_TextureScale;
                case "reforge_material_tex_offset_x":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Reforge_Material_TextureOffsetX;
                case "reforge_material_tex_offset_y":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Reforge_Material_TextureOffsetY;
                case "reforge_material_tex_override":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Reforge_Material_TextureOverride;
                case "reforge_material_allows_projectiles":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Reforge_MaterialAllowsProjectiles;
                case "reforge_material_type":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Reforge_MaterialType;
                case "teleporter_channel":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.General_TeleporterChannel;
                case "shape_type":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.General_ShapeType;
                case "shape_radius":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.General_ShapeRadius;
                case "shape_width":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.General_ShapeWidth;
                case "shape_top":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.General_ShapeTop;
                case "shape_bottom":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.General_ShapeBottom;
                case "shape_depth":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.General_ShapeDepth;
                case "light_type":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Light_Type;
                case "light_field_of_view":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Light_FieldOfView;
                case "light_near_width":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Light_NearWidth;
                case "light_color_r":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Light_ColorR;
                case "light_color_g":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Light_ColorG;
                case "light_color_b":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Light_ColorB;
                case "light_intensity":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Light_Intensity;
                case "light_radius":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Light_Radius;
                case "light_illumination_function_type":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Light_IlluminationType;
                case "light_illumination_function_base":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Light_IlluminationBase;
                case "light_illumination_function_freq":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Light_IlluminationFreq;
                case "fx_range":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Fx_Range;
                case "fx_hue":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Fx_Hue;
                case "fx_light_intensity":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Fx_LightIntensity;
                case "fx_saturation":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Fx_Saturation;
                case "fx_desaturation":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Fx_Desaturation;
                case "fx_color_filter_r":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Fx_ColorFilterR;
                case "fx_color_filter_g":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Fx_ColorFilterG;
                case "fx_color_filter_b":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Fx_ColorFilterB;
                case "fx_color_floor_r":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Fx_ColorFloorR;
                case "fx_color_floor_g":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Fx_ColorFloorG;
                case "fx_color_floor_b":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Fx_ColorFloorB;
                case "fx_gamma_inc":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Fx_GammaIncrease;
                case "fx_gamma_dec":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Fx_GammaDecrease;
                case "fx_tracing":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Fx_Tracing;
                case "garbage_volume_collect_dead_biped":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.GarbageVolume_CollectDeadBiped;
                case "garbage_volume_collect_weapons":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.GarbageVolume_CollectWeapons;
                case "garbage_volume_collect_objectives":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.GarbageVolume_CollectObjectives;
                case "garbage_volume_collect_grenades":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.GarbageVolume_CollectGrenades;
                case "garbage_volume_collect_equipment":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.GarbageVolume_CollectEquipment;
                case "garbage_volume_collect_vehicles":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.GarbageVolume_CollectVehicles;
                case "garbage_volume_interval":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.GarbageVolume_Interval;
                case "kill_volume_always_visible":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.KillVolume_AlwaysVisible;
                case "kill_volume_destroy_vehicles":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.KillVolume_DestroyVehicles;
                case "kill_volume_damage_cause":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.KillVolume_DamageCause;
                case "map_disable_push_barrier":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Map_DisablePushBarrier;
                case "map_disable_death_barrier":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Map_DisableDeathBarrier;
                case "map_physics_gravity":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Map_PhysicsGravity;
                case "camera_fx_exposure":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.CameraFx_Exposure;
                case "camera_fx_light_intensity":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.CameraFx_LightIntensity;
                case "camera_fx_bloom":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.CameraFx_Bloom;
                case "camera_fx_light_bloom":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.CameraFx_LightBloom;
                case "atmosphere_properties_enabled":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.AtmosphereProperties_Enabled;
                case "atmosphere_properties_weather":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.AtmosphereProperties_Weather;
                case "atmosphere_properties_brightness":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.AtmosphereProperties_Brightness;
                case "atmosphere_properties_fog_density":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.AtmosphereProperties_FogDensity;
                case "atmosphere_properties_fog_visibility":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.AtmosphereProperties_FogVisibility;
                case "atmosphere_properties_fog_color_r":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.AtmosphereProperties_FogColorR;
                case "atmosphere_properties_fog_color_g":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.AtmosphereProperties_FogColorG;
                case "atmosphere_properties_fog_color_b":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.AtmosphereProperties_FogColorB;
                case "atmosphere_properties_skybox":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.AtmosphereProperties_Skybox;
                case "atmosphere_properties_skybox_offset_z":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.AtmosphereProperties_SkyboxOffsetZ;
                case "atmosphere_properties_skybox_override_transform":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.AtmosphereProperties_SkyboxOverrideTransform;
                case "summary_runtime_minimum":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Budget_Minimum;
                case "summary_runtime_maximum":
                    return ForgeGlobalsDefinition.PaletteItem.SetterTarget.Budget_Maximum;
                default:
                    throw new FormatException(query);
            }
        }

        private T? ParseEnum<T>(string query) where T: struct
        {
            object found;

            switch (query.ToLower())
            {
                case "koth" when typeof(T) == typeof(ForgeGlobalsDefinition.PaletteCategoryType):
                    query = "KingOfTheHill";
                    break;
                case "props" when typeof(T) == typeof(ForgeGlobalsDefinition.PaletteCategoryType):
                    query = "Prop";
                    break;
                case "gameplay" when typeof(T) == typeof(ForgeGlobalsDefinition.PaletteCategoryType):
                    query = "Game";
                    break;
                case "prefabs" when typeof(T) == typeof(ForgeGlobalsDefinition.PaletteCategoryType):
                    query = "Prefab";
                    break;

                case "int" when typeof(T) == typeof(ForgeGlobalsDefinition.PaletteItem.SetterType):
                    query = "Integer";
                    break;
                case "float" when typeof(T) == typeof(ForgeGlobalsDefinition.PaletteItem.SetterType):
                    query = "Real";
                    break;
            }

            try
            {
                found = Enum.Parse(typeof(T), query);
            }
            catch
            {
                found = null;
            }

            var names = Enum.GetNames(typeof(T)).ToList();

            if (found == null)
            {
                var nameLow = query.ToLower();
                var namesLow = names.Select(i => i.ToLower()).ToList();

                found = namesLow.Find(n => n == nameLow);

                if (found == null)
                {
                    var nameSnake = query.ToSnakeCase();
                    var namesSnake = names.Select(i => i.ToSnakeCase()).ToList();
                    found = namesSnake.Find(n => n == nameSnake);

                    if (found != null)
                        found = Enum.Parse(typeof(T), names[namesSnake.IndexOf((string)found)]);
                }
                else
                {
                    found = Enum.Parse(typeof(T), names[namesLow.IndexOf((string)found)]);
                }
            }

            if (found == null)
                return null;

            return (T)found;
        }
    }
}