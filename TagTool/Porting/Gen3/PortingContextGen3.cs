using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TagTool.Audio;
using TagTool.BlamFile;
using TagTool.Cache;
using TagTool.Cache.Gen3;
using TagTool.Cache.Resources;
using TagTool.Commands.Common;
using TagTool.Commands.Porting;
using TagTool.Common;
using TagTool.Common.Logging;
using TagTool.Damage;
using TagTool.Geometry;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Havok;
using TagTool.IO;
using TagTool.Shaders;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Definitions.Common;

namespace TagTool.Porting.Gen3
{
    public partial class PortingContextGen3 : PortingContext
    {
        private readonly RenderGeometryConverter GeometryConverter;

        public PortingContextGen3(GameCacheHaloOnlineBase cacheContext, GameCache blamCache) : base(cacheContext, blamCache)
        {
            GeometryConverter = new RenderGeometryConverter(cacheContext, blamCache);
        }

        protected override void FinishInternal()
        {
            Matcher.DeInit();
            PendingTemplates.Clear();
        }

        protected override object ConvertDefinition(Stream cacheStream, Stream blamCacheStream, CachedTag edTag, CachedTag blamTag, object blamDefinition)
        {
            // Perform pre-conversion fixups
            blamDefinition = PreConvertDefinition(cacheStream, blamCacheStream, blamTag, blamDefinition);
            // Perform automatic conversion
            blamDefinition = ConvertData(cacheStream, blamCacheStream, blamDefinition, blamDefinition, blamTag.Name);
            // Perform post-conversion fixups
            blamDefinition = PostConvertDefinition(cacheStream, blamCacheStream, edTag, blamTag, blamDefinition);

            return blamDefinition;
        }

        /// <summary>
        /// Perform pre-conversion fixups to Blam data
        /// </summary>
        private object PreConvertDefinition(Stream cacheStream, Stream blamCacheStream, CachedTag blamTag, object blamDefinition)
        {
            ((TagStructure)blamDefinition).PreConvert(BlamCache.Version, CacheContext.Version);

            if (BlamCache.Version >= CacheVersion.HaloReach)
            {
                PreConvertReachDefinition(cacheStream, blamCacheStream, blamDefinition);
            }

            switch (blamDefinition)
            {
                case Scenario scenario:
                    if (!FlagIsSet(PortingFlags.Squads))
                    {
                        scenario.Squads.Clear();
                    }
                    if (!FlagIsSet(PortingFlags.ForgePalette))
                    {
                        scenario.SandboxEquipment.Clear();
                        scenario.SandboxGoalObjects.Clear();
                        scenario.SandboxScenery.Clear();
                        scenario.SandboxSpawning.Clear();
                        scenario.SandboxTeleporters.Clear();
                        scenario.SandboxVehicles.Clear();
                        scenario.SandboxWeapons.Clear();
                    }
                    break;

                case ShieldImpact shit when BlamCache.Version < CacheVersion.HaloOnlineED:
                    shit = PreConvertShieldImpact(shit, BlamCache.Version, CacheContext);
                    // These won't convert automatically due to versioning
                    shit.Plasma.PlasmaNoiseBitmap1 = ConvertTag(cacheStream, blamCacheStream, shit.Plasma.PlasmaNoiseBitmap1);
                    shit.Plasma.PlasmaNoiseBitmap2 = ConvertTag(cacheStream, blamCacheStream, shit.Plasma.PlasmaNoiseBitmap2);
                    shit.ExtrusionOscillation.OscillationBitmap1 = shit.Plasma.PlasmaNoiseBitmap1;
                    shit.ExtrusionOscillation.OscillationBitmap2 = shit.Plasma.PlasmaNoiseBitmap2;
                    blamDefinition = shit;
                    break;
            }

            return blamDefinition;
        }

        /// <summary>
        /// Perform post-conversion fixups to Blam data
        /// </summary>
        private object PostConvertDefinition(Stream cacheStream, Stream blamCacheStream, CachedTag blamTag, CachedTag edTag, object blamDefinition)
        {
            ((TagStructure)blamDefinition).PostConvert(BlamCache.Version, CacheContext.Version);

            switch (blamDefinition)
            {
                case AreaScreenEffect sefc:
                    blamDefinition = ConvertAreaScreenEffect(sefc);
                    break;

                case Bitmap bitm:
                    blamDefinition = ConvertBitmap(cacheStream, edTag, blamTag, bitm);
                    break;

                case CameraFxSettings cfxs:
                    blamDefinition = ConvertCameraFxSettings(cfxs, blamTag.Name);
                    break;

                case Character character:
                    blamDefinition = ConvertCharacter(cacheStream, character);
                    break;

                case ChudDefinition chdt:
                    blamDefinition = ConvertChudDefinition(chdt);
                    break;

                case ChudGlobalsDefinition chudGlobals:
                    blamDefinition = ConvertChudGlobalsDefinition(cacheStream, blamCacheStream, chudGlobals);
                    break;

                case ChudAnimationDefinition chudAnimation:
                    blamDefinition = ConvertChudAnimationDefinition(chudAnimation);
                    break;

                case CinematicScene cisc:
                    blamDefinition = ConvertCinematicScene(cisc);
                    break;

                case CortanaEffectDefinition crte:
                    blamDefinition = ConvertCortanaEffectDefinition(crte);
                    break;

                case DamageEffect damageEffect:
                    blamDefinition = ConvertDamageEffect(damageEffect);
                    break;
                case DamageResponseDefinition damageResponse:
                    blamDefinition = ConvertDamageResponseDefinition(blamCacheStream, damageResponse);
                    break;

                case DecoratorSet decoratorSet when BlamCache.Version >= CacheVersion.HaloReach:
                    blamDefinition = ConvertDecoratorSetReach(decoratorSet);
                    break;

                case Dialogue udlg:
                    blamDefinition = ConvertDialogue(cacheStream, udlg);
                    break;

                case Effect effe:
                    blamDefinition = ConvertEffect(cacheStream, effe, blamTag.Name);
                    break;

                case Equipment eqip:
                    Enum.TryParse(eqip.EquipmentFlagsH3.ToString(), out eqip.EquipmentFlags);
                    break;

                case GameObject gameobject:
                    blamDefinition = ConvertGameObject(cacheStream, blamCacheStream, gameobject);
                    break;

                case Globals matg:
                    blamDefinition = ConvertGlobals(matg, cacheStream);
                    break;

                case LensFlare lens:
                    blamDefinition = ConvertLensFlare(lens, cacheStream, blamCacheStream);
                    break;

                case Light ligh:
                    blamDefinition = ConvertLight(ligh);
                    break;

                case Model hlmt:
                    blamDefinition = ConvertModel(hlmt);
                    break;

                case ModelAnimationGraph jmad:
                    blamDefinition = ConvertModelAnimationGraph(cacheStream, blamCacheStream, jmad);
                    break;

                case MultilingualUnicodeStringList unic:
                    blamDefinition = ConvertMultilingualUnicodeStringList(cacheStream, blamCacheStream, unic);
                    break;

                case Particle particle:
                    blamDefinition = ConvertParticle(particle);
                    break;

                case ParticleModel particleModel:
                    blamDefinition = ConvertParticleModel(edTag, blamTag, particleModel);
                    break;

                case PhysicsModel phmo:
                    blamDefinition = ConvertPhysicsModel(edTag, phmo);
                    break;

                case RasterizerGlobals rasg:
                    blamDefinition = ConvertRasterizerGlobals(rasg);
                    break;

                case RenderMethodOption rmop:
                    blamDefinition = ConvertRenderMethodOption(rmop);
                    break;

                case RenderModel mode:
                    blamDefinition = ConvertGen3RenderModel(edTag, blamTag, mode);
                    break;

                case Scenario scnr:
                    blamDefinition = ConvertScenario(cacheStream, blamCacheStream, scnr, blamTag.Name);
                    break;

                case ScenarioLightmap sLdT:
                    blamDefinition = ConvertScenarioLightmap(cacheStream, blamCacheStream, blamTag.Name, sLdT);
                    break;

                case ScenarioLightmapBspData Lbsp:
                    blamDefinition = ConvertScenarioLightmapBspData(Lbsp);
                    break;

                case ScenarioStructureBsp sbsp:
                    blamDefinition = ConvertScenarioStructureBsp(sbsp, edTag);
                    break;

                case Sound sound:
                    blamDefinition = ConvertSound(cacheStream, blamCacheStream, sound, edTag, blamTag.Name);
                    break;

                case SoundClasses sncl:
                    blamDefinition = ConvertSoundClasses(sncl, BlamCache.Version);
                    break;

                case SoundLooping lsnd:
                    blamDefinition = ConvertSoundLooping(lsnd);
                    break;

                case SoundMix snmx:
                    blamDefinition = ConvertSoundMix(snmx);
                    break;

                case Style style:
                    blamDefinition = ConvertStyle(style);
                    break;

                case TextValuePairDefinition sily:
                    Enum.TryParse(sily.ParameterH3.ToString(), out sily.Parameter);
                    break;

                case UserInterfaceSharedGlobalsDefinition wigl:
                    blamDefinition = ConvertUserInterfaceSharedGlobals(wigl);
                    break;
            }

            return blamDefinition;
        }

        public override object ConvertData(Stream cacheStream, Stream blamCacheStream, object data, object definition, string blamTagName)
        {
            switch (data)
            {
                case TagResourceReference:
                    return data;

                case TagFunction tagFunction:
                    return ConvertTagFunction(tagFunction);

                case TagHkpMoppCode hkpMoppCode when definition is not StructureDesign:
                    //Structure design mopp codes are NOT converted
                    hkpMoppCode.Data.Elements = HavokConverter.ConvertMoppCodes(BlamCache.Version, BlamCache.Platform, CacheContext.Version, hkpMoppCode.Data.Elements);
                    return hkpMoppCode;

                case PhysicsModel.PhantomTypeFlags phantomTypeFlags:
                    return ConvertPhantomTypeFlags(blamTagName, phantomTypeFlags);

                case PhysicsModel.Shape shape:
                    shape = ConvertStructure(cacheStream, blamCacheStream, shape, definition, blamTagName);
                    // might be from 3, had no reference
                    shape.ProxyCollisionGroup = shape.ProxyCollisionGroup > 2 ? (sbyte)(shape.ProxyCollisionGroup + 1) : shape.ProxyCollisionGroup;
                    return shape;

                case DamageReportingType damageReportingType:
                    return ConvertDamageReportingType(damageReportingType);

                case GameObjectType8 gameObjectType:
                    gameObjectType.SetValue(CacheContext.Version, gameObjectType.GetValue(BlamCache.Version));
                    return gameObjectType;
                case GameObjectType16 gameObjectType:
                    gameObjectType.SetValue(CacheContext.Version, gameObjectType.GetValue(BlamCache.Version));
                    return gameObjectType;
                case GameObjectType32 gameObjectType:
                    gameObjectType.SetValue(CacheContext.Version, gameObjectType.GetValue(BlamCache.Version));
                    return gameObjectType;

                case ObjectTypeFlags objectTypeFlags:
                    return ConvertObjectTypeFlags(objectTypeFlags);

                case VersionedFlags versionedFlags:
                    versionedFlags.ConvertFlags(BlamCache.Version, BlamCache.Platform, CacheContext.Version, CacheContext.Platform);
                    return versionedFlags;

                case GameObject.MultiplayerObjectBlock multiplayer:
                    multiplayer = ConvertStructure(cacheStream, blamCacheStream, multiplayer, definition, blamTagName);
                    return ConvertMultiplayerObject(cacheStream, blamCacheStream, definition, blamTagName, multiplayer);

                case BipedPhysicsFlags bipedPhysicsFlags:
                    return ConvertBipedPhysicsFlags(bipedPhysicsFlags);

                case WeaponFlags weaponFlags:
                    return ConvertWeaponFlags(weaponFlags);

                case Weapon.Trigger trigger:
                    trigger = ConvertStructure(cacheStream, blamCacheStream, trigger, definition, blamTagName);
                    return ConvertWeaponTrigger(trigger);

                case BarrelFlags barrelflags:
                    return ConvertBarrelFlags(barrelflags);

                case Model.TargetLockOnData lockOnData:
                    return ConvertTargetLockOnData(lockOnData);

                case RenderMethod renderMethod:
                    return ConvertRenderMethod(cacheStream, blamCacheStream, definition, blamTagName, renderMethod);

                case Scenario.MultiplayerObjectProperties scnrObj:
                    scnrObj = ConvertStructure(cacheStream, blamCacheStream, scnrObj, definition, blamTagName);
                    return ConvertScenarioObjectMultiplayer(cacheStream, blamCacheStream, definition, blamTagName, scnrObj);

                case SoundClass soundClass:
                    return ConvertSoundClass(soundClass);

                case GuiTextWidgetDefinition guiTextWidget:
                    guiTextWidget = ConvertStructure(cacheStream, blamCacheStream, guiTextWidget, definition, blamTagName);
                    return ConvertGuiTextWidget(cacheStream, blamCacheStream, definition, blamTagName, guiTextWidget);

                case GuiDefinition guidefinition:
                    guidefinition = ConvertStructure(cacheStream, blamCacheStream, guidefinition, definition, blamTagName);
                    return ConvertGuiDefinition(cacheStream, blamCacheStream, definition, blamTagName, guidefinition);

                case RuntimeGpuData runMGpu:
                    return ConvertRuntimeGpuData(runMGpu);

                case CollisionGeometry collisionGeometry:
                    return ConvertCollisionBsp(collisionGeometry);

                case HkpMoppBvTreeShape hkpMoppBvTreeShape when BlamCache.Version >= CacheVersion.HaloReach:
                    hkpMoppBvTreeShape.Child = hkpMoppBvTreeShape.ChildReach;
                    return ConvertStructure(cacheStream, blamCacheStream, hkpMoppBvTreeShape, definition, blamTagName);

                case RenderGeometry renderGeometry when BlamCache.Version >= CacheVersion.Halo3Retail:
                    renderGeometry = ConvertStructure(cacheStream, blamCacheStream, renderGeometry, definition, blamTagName);
                    return renderGeometry;

                case Part part when BlamCache.Version < CacheVersion.Halo3Retail:
                    part = ConvertStructure(cacheStream, blamCacheStream, part, definition, blamTagName);
                    if (!Enum.TryParse(part.TypeOld.ToString(), out part.TypeNew))
                        throw new NotSupportedException(part.TypeOld.ToString());
                    return part;

                case RenderMaterial.Property property when BlamCache.Version < CacheVersion.Halo3Retail:
                    property = ConvertStructure(cacheStream, blamCacheStream, property, definition, blamTagName);
                    property.IntValue = property.ShortValue;
                    return property;

                case PixelShaderReference _:
                case VertexShaderReference _:
                    return null;

                default:
                    return base.ConvertData(cacheStream, blamCacheStream, data, definition, blamTagName);
            }
        }

        private TagFunction ConvertTagFunction(TagFunction function)
        {
            return TagFunction.ConvertTagFunction(CacheVersionDetection.IsLittleEndian(BlamCache.Version, BlamCache.Platform) ? EndianFormat.LittleEndian : EndianFormat.BigEndian, function);
        }

        protected override object ConvertFieldvalue(Stream cacheStream, Stream blamCacheStream, TagFieldInfo tagFieldInfo, object value, object definition, string blamTagName)
        {
            value = base.ConvertFieldvalue(cacheStream, blamCacheStream, tagFieldInfo, value, definition, blamTagName);

            if (tagFieldInfo.Attribute.Flags.HasFlag(TagFieldFlags.GlobalMaterial))
            {
                value = ConvertGlobalMaterialTypeField(cacheStream, blamCacheStream, tagFieldInfo, value);
            }

            return value;
        }

        protected override void MergeTags(Stream cacheStream, Stream blamCacheStream, CachedTag blamTag, CachedTag edTag, object blamDefinition)
        {
            switch (blamDefinition)
            {
                case Character character:
                    MergeCharacter(cacheStream, blamCacheStream, edTag, blamTag, character);
                    break;

                case MultiplayerGlobals mulg:
                    MergeMultiplayerGlobals(cacheStream, blamCacheStream, edTag, blamTag, mulg);
                    break;

                case MultilingualUnicodeStringList unic:
                    MergeMultilingualUnicodeStringList(cacheStream, blamCacheStream, edTag, blamTag, unic);
                    break;
            }

            if (blamTag.IsInGroup("rm  ") || PortingConstants.EffectGroups.Contains(blamTag.Group.Tag))
            {
                new RenderMethodMerger(this).MergeRenderMethods(cacheStream, blamCacheStream, edTag, blamTag, blamDefinition);
            }
        }

        protected override bool TagIsValid(CachedTag blamTag, Stream cacheStream, Stream blamCacheStream, object blamDefinition, out CachedTag resultTag)
        {
            resultTag = null;

            switch (blamTag.Group.Tag.ToString())
            {
                case "snd!" when !FlagIsSet(PortingFlags.Audio):
                case "udlg" when !FlagIsSet(PortingFlags.Dialogue):
                    resultTag = GetFallbackTag(blamTag);
                    return false;

                case "bipd":
                    if (!FlagIsSet(PortingFlags.Elites) && (blamTag.Name.Contains("elite") || blamTag.Name.Contains("dervish")))
                        return false;
                    break;

                case "char" when BlamCache.Version >= CacheVersion.HaloReach:
                    return false;

                case "sncl" when BlamCache.Version >= CacheVersion.HaloReach:
                    resultTag = GetFallbackTag(blamTag);
                    return false;

                case "cmps":
                case "cmpu":
                case "fwtg":
                case "rsod":
                case "sdzg":
                case "spda":
                case "wave":
                    return false;

                // these tags will be generated in the template generation code
                case "rmt2":
                case "rmdf":
                case "glvs":
                case "glps":
                    return false;
                default:
                    break;
            }

            if (blamTag.Group.Tag == "snd!")
            {
                var sound = (Sound)blamDefinition;
                BlamSoundGestalt ??= PortingContextFactory.LoadSoundGestalt(BlamCache, blamCacheStream);

                if (BlamCache.Platform != CachePlatform.MCC)
                {
                    if (sound.SoundReference != null)
                    {
                        var xmaFileSize = BlamSoundGestalt.GetFileSize(sound.SoundReference.PitchRangeIndex, sound.SoundReference.PitchRangeCount, BlamCache.Platform);
                        if (xmaFileSize < 0)
                            return false;
                    }

                    if (!BlamCache.ResourceCache.IsResourceValid(sound.GetResource(BlamCache.Version, BlamCache.Platform)))
                    {
                        Log.Warning($"Invalid resource for sound {blamTag.Name}");
                        return false;
                    }
                }
                else
                {
                    if (sound.Resource.Gen3ResourceID == DatumHandle.None || !CheckSoundBank(sound))
                    {
                        Log.Warning($"Invalid resource for sound {blamTag.Name}");
                        resultTag = GetFallbackTag(blamTag);
                        return false;
                    }
                }
            }
            else if (blamTag.Group.Tag == "bitm")
            {
                var bitmap = (Bitmap)blamDefinition;

                for (int i = 0; i < bitmap.Images.Count; i++)
                {
                    var image = bitmap.Images[i];

                    TagResourceReference bitmapResource;

                    if (image.XboxFlags.HasFlag(TagTool.Bitmaps.BitmapFlagsXbox.Xbox360UseInterleavedTextures))
                        bitmapResource = bitmap.InterleavedHardwareTextures[image.InterleavedInterop];
                    else
                        bitmapResource = bitmap.HardwareTextures[i];

                    if (!BlamCache.ResourceCache.IsResourceValid(bitmapResource))
                    {
                        Log.Warning($"Invalid resource for bitm {blamTag.Name}");
                        return false;
                    }
                }
            }
            else if (blamTag.Group.Tag == "Lbsp")
            {
                var Lbsp = (ScenarioLightmapBspData)blamDefinition;

                if (!BlamCache.ResourceCache.IsResourceValid(Lbsp.Geometry.Resource))
                {
                    Log.Warning($"Invalid resource for Lbsp {blamTag.Name}");
                    return false;
                }
            }
            else if (PortingConstants.RenderMethodGroups.Contains(blamTag.Group.Tag))
            {
                // "noshaders" / "!MatchShaders": don't port render methods at all, use the destination's default shader
                if (!FlagIsSet(PortingFlags.MatchShaders))
                {
                    resultTag = GetDefaultShader(blamTag);
                    return false;
                }

                if (BlamCache.Version >= CacheVersion.HaloReach)
                {
                    switch (blamTag.Group.Tag.ToString())
                    {
                        case "rmcs":
                        case "rmgl":
                            resultTag = GetDefaultShader(blamTag);
                            return false;
                    }
                }
            }

            return true;
        }
    }
}
