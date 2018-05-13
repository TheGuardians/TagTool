using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Serialization;
using TagTool.Commands;
using TagTool.Tags;

namespace TagTool.Commands.Tags
{
    class GenerateTagNamesCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private Dictionary<int, object> LoadedDefinitions { get; } = new Dictionary<int, object>();

        public GenerateTagNamesCommand(HaloOnlineCacheContext cacheContext)
            : base(CommandFlags.Inherit,
                  
                  "GenerateTagNames",
                  "Generates tag names into a csv file (overwriting existing entries).",

                  "GenerateTagNames [csv file]",

                  "Generates tag names into a csv file (overwriting existing entries).")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1)
                return false;

            var csvFile = (args.Count == 1) ?
                new FileInfo(args[0]) :
                new FileInfo(Path.Combine(CacheContext.Directory.FullName, "tag_list.csv"));

            var tagNames = new Dictionary<int, string>();

            if (csvFile.Exists)
                tagNames = LoadTagNames(csvFile);

            using (var cacheStream = CacheContext.OpenTagCacheRead())
            {
                var scenarioTags = CacheContext.TagCache.Index.FindAllInGroup(new Tag("scnr"));
                foreach (var scenarioTag in scenarioTags)
                    SetScenarioName(cacheStream, scenarioTag, ref tagNames);

                var objectTags = CacheContext.TagCache.Index.FindAllInGroup(new Tag("obje"));
                foreach (var objectTag in objectTags)
                    SetGameObjectName(cacheStream, objectTag, ref tagNames);

                var renderModelTags = CacheContext.TagCache.Index.FindAllInGroup(new Tag("mode"));
                foreach (var renderModelTag in renderModelTags)
                    SetRenderModelName(cacheStream, renderModelTag, ref tagNames);

                var modelTags = CacheContext.TagCache.Index.FindAllInGroup(new Tag("hlmt"));
                foreach (var modelTag in modelTags)
                    SetModelName(cacheStream, modelTag, ref tagNames);

                SetRenderMethodTemplates(cacheStream, ref tagNames);
            }
            
            var sortedNames = tagNames.ToList();
            sortedNames.Sort((a, b) => a.Key.CompareTo(b.Key));

            if (csvFile.Exists)
                csvFile.Delete();

            using (var csvStream = csvFile.Create())
            {
                var writer = new StreamWriter(csvStream);

                foreach (var entry in sortedNames)
                {
                    var value = entry.Value;

                    writer.WriteLine($"0x{entry.Key:X8},{value}");
                }

                writer.Close();
            }

            CacheContext.TagNames = tagNames;

            return true;
        }

        private object GetTagDefinition(Stream stream, CachedTagInstance tag)
        {
            object definition = null;

            if (!LoadedDefinitions.ContainsKey(tag.Index))
            {
                var context = new TagSerializationContext(stream, CacheContext, tag);
                definition = LoadedDefinitions[tag.Index] = CacheContext.Deserializer.Deserialize(context, TagDefinition.Find(tag.Group.Tag));
            }
            else
            {
                definition = LoadedDefinitions[tag.Index];
            }

            return definition;
        }

        private T GetTagDefinition<T>(Stream stream, CachedTagInstance tag) => (T)GetTagDefinition(stream, tag);

        private void SetRenderModelName(Stream stream, CachedTagInstance tag, ref Dictionary<int, string> tagNames)
        {
            if (tagNames.ContainsKey(tag.Index))
                return;

            tagNames[tag.Index] = $"{CacheContext.GetString(GetTagDefinition<RenderModel>(stream, tag).Name)}";
        }

        private void SetModelName(Stream stream, CachedTagInstance tag, ref Dictionary<int, string> tagNames)
        {
            if (tag == null || tagNames.ContainsKey(tag.Index))
                return;
            
            var definition = GetTagDefinition<Model>(stream, tag);

            if (definition.RenderModel == null)
                return;

            SetRenderModelName(stream, definition.RenderModel, ref tagNames);

            var tagName = tagNames[definition.RenderModel.Index];

            if (tagName.StartsWith("0x"))
                tagName = $"0x{tag.Index:X4}";

            tagNames[tag.Index] = tagName;

            if (definition.CollisionModel != null && !tagName.StartsWith("0x"))
                tagNames[definition.CollisionModel.Index] = $"{tagName}";

            if (definition.Animation != null && !tagName.StartsWith("0x"))
                tagNames[definition.Animation.Index] = $"{tagName}";
        }

        private void SetGameObjectName(Stream stream, CachedTagInstance tag, ref Dictionary<int, string> tagNames)
        {
            var context = new TagSerializationContext(stream, CacheContext, tag);

            var definition = GetTagDefinition<GameObject>(stream, tag);

            if (definition.Model == null)
                return;
            
            var modelDefinition = GetTagDefinition<Model>(stream, definition.Model);

            if (modelDefinition.RenderModel == null)
                return;
            
            var renderModelDefinition = GetTagDefinition<RenderModel>(stream, modelDefinition.RenderModel);

            var objectName = CacheContext.GetString(renderModelDefinition.Name);
            
            if (tag.Group.Tag == new Tag("bipd"))
            {
                var biped = (Biped)definition;

                var isMultiplayer = objectName.StartsWith("mp_");
                var isMonitor = objectName.StartsWith("monitor");

                var objectRootName = isMultiplayer ?
                    objectName.Substring(3) :
                    objectName;

                var objectGenericName = $"objects\\characters\\{objectRootName}\\{objectRootName}";

                if (objectRootName != objectName)
                    objectName = $"objects\\characters\\{objectRootName}\\{objectName}\\{objectName}";
                else if (isMonitor)
                    objectName = $"{objectGenericName}_editor";
                else
                    objectName = objectGenericName;

                tagNames[definition.Model.Index] = objectName;

                if (modelDefinition.RenderModel != null)
                    tagNames[modelDefinition.RenderModel.Index] = objectName;

                if (modelDefinition.CollisionModel != null)
                    tagNames[modelDefinition.CollisionModel.Index] = objectGenericName;

                if (modelDefinition.PhysicsModel != null)
                    tagNames[modelDefinition.PhysicsModel.Index] = objectGenericName;

                if (modelDefinition.Animation != null)
                    tagNames[modelDefinition.Animation.Index] = objectGenericName;

                if (biped.CollisionDamage != null && !tagNames.ContainsKey(biped.CollisionDamage.Index))
                    tagNames[biped.CollisionDamage.Index] = isMonitor ?
                        "globals\\collision_damage\\invulnerable_harmless" :
                        "globals\\collision_damage\\biped_player";

                if (biped.MaterialEffects != null && !tagNames.ContainsKey(biped.MaterialEffects.Index))
                    tagNames[biped.MaterialEffects.Index] =
                        $"fx\\material_effects\\objects\\characters\\{objectRootName}";

                if (biped.MeleeImpact != null && !tagNames.ContainsKey(biped.MeleeImpact.Index))
                    tagNames[biped.MeleeImpact.Index] =
                        "sounds\\materials\\soft\\organic_flesh\\melee_impact";

                if (biped.CameraTracks.Count != 0 && biped.CameraTracks[0].CameraTrack != null && !tagNames.ContainsKey(biped.CameraTracks[0].CameraTrack.Index))
                    tagNames[biped.CameraTracks[0].CameraTrack.Index] = isMonitor ?
                        "camera\\biped_follow_camera" :
                        "camera\\biped_support_camera";

                if (biped.MeleeDamage != null && !tagNames.ContainsKey(biped.MeleeDamage.Index))
                    tagNames[biped.MeleeDamage.Index] =
                        $"objects\\characters\\{objectRootName}\\damage_effects\\{objectRootName}_melee";

                if (biped.BoardingMeleeDamage != null && !tagNames.ContainsKey(biped.BoardingMeleeDamage.Index))
                    tagNames[biped.BoardingMeleeDamage.Index] =
                        $"objects\\characters\\{objectRootName}\\damage_effects\\{objectRootName}_boarding_melee";

                if (biped.BoardingMeleeResponse != null && !tagNames.ContainsKey(biped.BoardingMeleeResponse.Index))
                    tagNames[biped.BoardingMeleeResponse.Index] =
                        $"objects\\characters\\{objectRootName}\\damage_effects\\{objectRootName}_boarding_melee_response";

                if (biped.EjectionMeleeDamage != null && !tagNames.ContainsKey(biped.EjectionMeleeDamage.Index))
                    tagNames[biped.EjectionMeleeDamage.Index] =
                        $"objects\\characters\\{objectRootName}\\damage_effects\\{objectRootName}_ejection_melee";

                if (biped.EjectionMeleeResponse != null && !tagNames.ContainsKey(biped.EjectionMeleeResponse.Index))
                    tagNames[biped.EjectionMeleeResponse.Index] =
                        $"objects\\characters\\{objectRootName}\\damage_effects\\{objectRootName}_ejection_melee_response";

                if (biped.LandingMeleeDamage != null && !tagNames.ContainsKey(biped.LandingMeleeDamage.Index))
                    tagNames[biped.LandingMeleeDamage.Index] =
                        $"objects\\characters\\{objectRootName}\\damage_effects\\{objectRootName}_landing_melee";

                if (biped.FlurryMeleeDamage != null && !tagNames.ContainsKey(biped.FlurryMeleeDamage.Index))
                    tagNames[biped.FlurryMeleeDamage.Index] =
                        $"objects\\characters\\{objectRootName}\\damage_effects\\{objectRootName}_flurry_melee";

                if (biped.ObstacleSmashMeleeDamage != null && !tagNames.ContainsKey(biped.ObstacleSmashMeleeDamage.Index))
                    tagNames[biped.ObstacleSmashMeleeDamage.Index] =
                        $"objects\\characters\\{objectRootName}\\damage_effects\\{objectRootName}_obstacle_smash";

                if (biped.AreaDamageEffect != null && !tagNames.ContainsKey(biped.AreaDamageEffect.Index))
                    tagNames[biped.AreaDamageEffect.Index] =
                        $"fx\\material_effects\\objects\\characters\\contact\\collision\\blood_aoe_{objectRootName}";
            }
            else if (tag.Group.Tag == new Tag("weap"))
            {
                var weapon = (Weapon)definition;

                if (weapon.HudInterface != null && !tagNames.ContainsKey(weapon.HudInterface.Index))
                    tagNames[weapon.HudInterface.Index] = $"ui\\chud\\{objectName}";

                if (weapon.FirstPerson.Count > 0)
                {
                    var spartanJmadTag = weapon.FirstPerson[0].FirstPersonAnimations;
                    if (spartanJmadTag != null)
                        tagNames[spartanJmadTag.Index] = $"objects\\characters\\mp_masterchief\\fp\\weapons\\fp_{objectName}\\fp_{objectName}";
                }

                if (weapon.FirstPerson.Count > 1)
                {
                    var eliteJmadTag = weapon.FirstPerson[1].FirstPersonAnimations;
                    if (eliteJmadTag != null)
                        tagNames[eliteJmadTag.Index] = $"objects\\characters\\mp_elite\\fp\\weapons\\fp_{objectName}\\fp_{objectName}";
                }

                var weaponClassName =
                    // HUNTER WEAPONS
                    objectName.StartsWith("flak_cannon") ?
                        "hunter\\hunter_flak_cannon" :
                    // MELEE WEAPONS
                    objectName.StartsWith("energy_blade") ?
                        "melee\\energy_blade" :
                    objectName.StartsWith("gravity_hammer") ?
                        "melee\\gravity_hammer" :
                    // MULTIPLAYER WEAPONS
                    objectName.StartsWith("assault_bomb") ?
                        "multiplayer\\assault_bomb" :
                    objectName.StartsWith("ball") ?
                        "multiplayer\\ball" :
                    objectName.StartsWith("flag") ?
                        "multiplayer\\flag" :
                    // PISTOL WEAPONS
                    objectName.StartsWith("excavator") ?
                        "pistol\\excavator" :
                    objectName.StartsWith("magnum") ?
                        "pistol\\magnum" :
                    objectName.StartsWith("needler") ?
                        "pistol\\needler" :
                    objectName.StartsWith("plasma_pistol") ?
                        "pistol\\plasma_pistol" :
                    // RIFLE WEAPONS
                    (objectName.StartsWith("assault_rifle") || objectName.StartsWith("ar_variant")) ?
                        "rifle\\assault_rifle" :
                    (objectName.StartsWith("battle_rifle") || objectName.StartsWith("br_variant")) ?
                        "rifle\\battle_rifle" :
                    objectName.StartsWith("beam_rifle") ?
                        "rifle\\beam_rifle" :
                    objectName.StartsWith("covenant_carbine") ?
                        "rifle\\covenant_carbine" :
                    objectName.StartsWith("dmr") ?
                        "rifle\\dmr" :
                    objectName.StartsWith("needle_rifle") ?
                        "rifle\\needle_rifle" :
                    objectName.StartsWith("plasma_rifle") ?
                        "rifle\\plasma_rifle" :
                    objectName.StartsWith("shotgun") ?
                        "rifle\\shotgun" :
                    objectName.StartsWith("smg") ?
                        "rifle\\smg" :
                    objectName.StartsWith("sniper_rifle") ?
                        "rifle\\sniper_rifle" :
                    objectName.StartsWith("spike_rifle") ?
                        "rifle\\spike_rifle" :
                    // SUPPORT WEAPONS
                    objectName.StartsWith("rocket_launcher") ?
                        "support_high\\rocket_launcher" :
                    objectName.StartsWith("spartan_laser") ?
                        "support_high\\spartan_laser" :
                    objectName.StartsWith("brute_shot") ?
                        "support_low\\brute_shot" :
                    objectName.StartsWith("sentinel_gun") ?
                        "support_low\\sentinel_gun" :
                    // OTHER WEAPONS
                    objectName;

                objectName = $"objects\\weapons\\{weaponClassName}\\{objectName}";

                if (objectName.EndsWith("energy_blade") && definition.WaterDensity == GameObject.WaterDensityValue.Default)
                    objectName += "_useless";
            }
            else if (tag.Group.Tag == new Tag("eqip"))
            {
                var equipment = (Equipment)definition;

                var equipmentClassName =
                    (objectName.StartsWith("health_pack") || objectName.EndsWith("ammo")) ?
                        $"powerups\\{objectName}" :
                    objectName.StartsWith("powerup") ?
                        $"multi\\powerups\\{objectName}" :
                    objectName.EndsWith("grenade") ?
                        $"weapons\\grenade\\{objectName}" :
                    $"equipment\\{objectName}";

                objectName = $"objects\\{equipmentClassName}\\{objectName}";
            }
            else if (tag.Group.Tag == new Tag("vehi"))
            {
                objectName = $"objects\\vehicles\\{objectName}\\{objectName}";

                tagNames[definition.Model.Index] = objectName;
            }
            else if (tag.Group.Tag == new Tag("armr"))
            {
                // TODO: figure out spartan/elite armor name differences

                objectName = $"objects\\characters\\masterchief\\mp_masterchief\\armor\\{objectName}";
            }

            if (!tagNames.ContainsKey(tag.Index))
                tagNames[tag.Index] = objectName;

            if (!tagNames.ContainsKey(definition.Model.Index))
                tagNames[definition.Model.Index] = objectName;

            if (modelDefinition.RenderModel != null)
                tagNames[modelDefinition.RenderModel.Index] = objectName;

            if (modelDefinition.CollisionModel != null)
                tagNames[modelDefinition.CollisionModel.Index] = objectName;

            if (modelDefinition.PhysicsModel != null)
                tagNames[modelDefinition.PhysicsModel.Index] = objectName;

            if (modelDefinition.Animation != null)
                tagNames[modelDefinition.Animation.Index] = objectName;
        }

        private void SetScenarioName(Stream stream, CachedTagInstance tag, ref Dictionary<int, string> tagNames)
        {
            var definition = GetTagDefinition<Scenario>(stream, tag);

            var tagName = CacheContext.GetString(definition.ZoneSets[0].Name);
            var slashIndex = tagName.LastIndexOf('\\');
            var scenarioName = tagName.Substring(slashIndex + 1);

            tagNames[tag.Index] = tagName;

            var bsp = definition.StructureBsps[0].StructureBsp;
            if (bsp != null)
                tagNames[bsp.Index] = tagName;

            var design = definition.StructureBsps[0].Design;
            if (design != null)
                tagNames[design.Index] = $"{tagName}_design";

            var cubemap = definition.StructureBsps[0].Cubemap;
            if (cubemap != null)
                tagNames[cubemap.Index] = $"{tagName}_{scenarioName}_cubemaps";

            var skyObject = definition.SkyReferences[0].SkyObject;
            if (skyObject != null)
                tagNames[skyObject.Index] = $"{tagName.Substring(0, slashIndex)}\\sky\\sky";
        }

        private Dictionary<int, string> LoadTagNames(FileInfo csvFile)
        {
            var result = new Dictionary<int, string>();

            using (var streamReader = new StreamReader(csvFile.OpenRead()))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine().Split(',');

                    if (line[0] == line[1] || line[1].Length == 0)
                        continue;


                    if (!int.TryParse(line[0].Replace("0x", ""), NumberStyles.HexNumber, null, out int tagIndex))
                        continue;

                    var name = line[1];
                    if (!name.StartsWith("0x"))
                        result[tagIndex] = name.Replace(' ', '_');
                }
            }

            return result;
        }

        public bool SetRenderMethodTemplates(Stream stream, ref Dictionary<int, string> tagNames) // restore mode or sbsp shaders from H3 or ODST
        {
            // rm
            foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup("rm  "))
            {
                ShaderDecal shader;
                var edContext = new TagSerializationContext(stream, CacheContext, instance);
                shader = CacheContext.Deserializer.Deserialize<ShaderDecal>(edContext);
                
                string baseRenderMethodName = null;

                if (!tagNames.ContainsKey(shader.BaseRenderMethod.Index))
                {
                    string rmdfName = CacheContext.GetString(instance.Group.Name);
                    if (rmdfName != "shader")
                    {
                        tagNames.Add(shader.BaseRenderMethod.Index, "shaders\\" + rmdfName.Substring(7));
                        baseRenderMethodName = "shaders\\" + rmdfName.Substring(7);
                    }
                    else
                    {
                        tagNames.Add(shader.BaseRenderMethod.Index, "shaders\\" + rmdfName);
                        baseRenderMethodName = "shaders\\" + rmdfName;
                    }
                }

                string rmt2Name = tagNames[shader.BaseRenderMethod.Index] + "_templates\\";

                foreach (var val in shader.Unknown)
                {
                    rmt2Name = rmt2Name + "_" + val.Unknown;
                }

                if (!tagNames.ContainsKey(shader.ShaderProperties[0].Template.Index))
                    tagNames.Add(shader.ShaderProperties[0].Template.Index, rmt2Name);
            }

            // beam
            foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup("beam"))
            {
                BeamSystem shader;
                var edContext = new TagSerializationContext(stream, CacheContext, instance);
                shader = CacheContext.Deserializer.Deserialize<BeamSystem>(edContext);

                if (!tagNames.ContainsKey(shader.Beam[0].RenderMethod.BaseRenderMethod.Index))
                {
                    tagNames.Add(shader.Beam[0].RenderMethod.BaseRenderMethod.Index, "shaders\\" + "beam");
                }

                string baseRenderMethodName = "shaders\\" + "beam";

                string name = baseRenderMethodName + "_templates\\";

                foreach (var val in shader.Beam[0].RenderMethod.Unknown)
                {
                    name = name + "_" + val.Unknown;
                }

                if (!tagNames.ContainsKey(shader.Beam[0].RenderMethod.ShaderProperties[0].Template.Index))
                    tagNames.Add(shader.Beam[0].RenderMethod.ShaderProperties[0].Template.Index, name);
            }

            // contrail
            foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup("cntl"))
            {
                ContrailSystem shader;
                var edContext = new TagSerializationContext(stream, CacheContext, instance);
                shader = CacheContext.Deserializer.Deserialize<ContrailSystem>(edContext);

                if (!tagNames.ContainsKey(shader.Contrail[0].RenderMethod.BaseRenderMethod.Index))
                {
                    tagNames.Add(shader.Contrail[0].RenderMethod.BaseRenderMethod.Index, "shaders\\" + "contrail");
                }

                string baseRenderMethodName = "shaders\\" + "contrail";

                string name = baseRenderMethodName + "_templates\\";

                foreach (var val in shader.Contrail[0].RenderMethod.Unknown)
                {
                    name = name + "_" + val.Unknown;
                }

                if (!tagNames.ContainsKey(shader.Contrail[0].RenderMethod.ShaderProperties[0].Template.Index))
                    tagNames.Add(shader.Contrail[0].RenderMethod.ShaderProperties[0].Template.Index, name);
            }

            // LightVolumeSystem
            foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup("ltvl"))
            {
                LightVolumeSystem shader;
                var edContext = new TagSerializationContext(stream, CacheContext, instance);
                shader = CacheContext.Deserializer.Deserialize<LightVolumeSystem>(edContext);

                if (!tagNames.ContainsKey(shader.LightVolume[0].RenderMethod.BaseRenderMethod.Index))
                {
                    tagNames.Add(shader.LightVolume[0].RenderMethod.BaseRenderMethod.Index, "shaders\\" + "light_volume");
                }

                string baseRenderMethodName = "shaders\\" + "light_volume";

                string name = baseRenderMethodName + "_templates\\";

                foreach (var val in shader.LightVolume[0].RenderMethod.Unknown)
                {
                    name = name + "_" + val.Unknown;
                }

                if (!tagNames.ContainsKey(shader.LightVolume[0].RenderMethod.ShaderProperties[0].Template.Index))
                    tagNames.Add(shader.LightVolume[0].RenderMethod.ShaderProperties[0].Template.Index, name);
            }

            // Particle
            foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup("prt3"))
            {
                Particle shader;
                var edContext = new TagSerializationContext(stream, CacheContext, instance);
                shader = CacheContext.Deserializer.Deserialize<Particle>(edContext);

                if (!tagNames.ContainsKey(shader.RenderMethod.BaseRenderMethod.Index))
                {
                    tagNames.Add(shader.RenderMethod.BaseRenderMethod.Index, "shaders\\" + "particle");
                }

                string baseRenderMethodName = "shaders\\" + "particle";

                string name = baseRenderMethodName + "_templates\\";

                foreach (var val in shader.RenderMethod.Unknown)
                {
                    name = name + "_" + val.Unknown;
                }

                if (!tagNames.ContainsKey(shader.RenderMethod.ShaderProperties[0].Template.Index))
                    tagNames.Add(shader.RenderMethod.ShaderProperties[0].Template.Index, name);
            }

            // DecalSystem
            foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup("decs"))
            {
                DecalSystem shader;
                var edContext = new TagSerializationContext(stream, CacheContext, instance);
                shader = CacheContext.Deserializer.Deserialize<DecalSystem>(edContext);

                foreach (var decalSystem in shader.DecalSystem2)
                {
                    if (!tagNames.ContainsKey(decalSystem.RenderMethod.BaseRenderMethod.Index))
                    {
                        tagNames.Add(decalSystem.RenderMethod.BaseRenderMethod.Index, "shaders\\" + "decal");
                    }

                    string baseRenderMethodName = "shaders\\" + "decal";

                    string name = baseRenderMethodName + "_templates\\";

                    foreach (var val in decalSystem.RenderMethod.Unknown)
                    {
                        name = name + "_" + val.Unknown;
                    }

                    if (!tagNames.ContainsKey(decalSystem.RenderMethod.ShaderProperties[0].Template.Index))
                        tagNames.Add(decalSystem.RenderMethod.ShaderProperties[0].Template.Index, name);
                }
            }
            
            return true;
        }

        [TagStructure(Name = "render_method", Tag = "rm  ", Size = 0x14)]
        public class RenderMethod_BaseRenderMethod
        {
            public CachedTagInstance BaseRenderMethod = null;
        }
    }
}