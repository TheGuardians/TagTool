using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Commands.Porting
{
    public class PortingTestCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CacheFile BlamCache;

        public PortingTestCommand(HaloOnlineCacheContext cacheContext, CacheFile blamCache) :
            base(true,

                "PortingTest",
                "A test command for porting-related actions.",

                "PortingTest [...]",

                "A test command. Used for various testing and temporary functionality.\n" +
                "Example: 'PortingTest UpdateMapFiles'")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
        }
        
        public override object Execute(List<string> args)
        {
            if (args.Count == 0)
                return false;

            var name = args[0].ToLower();
            args.RemoveAt(0);

            var methods = GetType().GetMethods(BindingFlags.Public).Where(method =>
            {
                var parameters = method.GetParameters();

                return (method.ReturnType == typeof(bool))
                    && (parameters.Length == 1)
                    && (parameters[0].ParameterType != typeof(List<string>));
            });

            var foundMethods = methods.Where(i => i.Name.ToLower() == name);

            if (foundMethods.Count() == 0)
            {
                Console.WriteLine($"Invalid command: {name}");
                Console.WriteLine($"Available commands: {methods.Count()}");

                foreach (var method in methods)
                    Console.WriteLine($"\t{method.Name}");

                Console.WriteLine();
                
                return false;
            }

            return foundMethods.First().Invoke(this, new[] { args });
        }
        
        public bool RestoreWeaponAnimation(List<string> args)
        {
            if (args.Count != 1)
            {
                Console.WriteLine("Required:[1] weapon name (port only FP animation)");

                Console.WriteLine(@"Example: test RestoreWeaponAnimation D:\Halo\Cache\Halo3\sandbox.map objects\weapons\rifle\assault_rifle\assault_rifle");

                return false;
            }

            var blamWeapName = args[0];
            var blamWeapTag = BlamCache.IndexItems.Find(x => x.Name == blamWeapName);

            if (blamWeapTag == null)
            {
                Console.WriteLine($"{blamWeapName} does not exist.");
                return false;
            }

            if (!CacheContext.TryGetTag<Weapon>(blamWeapName, out var edWeapTag))
            {
                Console.WriteLine($"{blamWeapName} does not exist or is named incorrectly or tags are not named.");
                return false;
            }

            string blamFPmodeName = null;
            string edFPmodeName = null;

            // Blam:
            // Deserialize weap tag
            var blamContext = new CacheSerializationContext(ref BlamCache, blamWeapTag);
            Weapon blamWeap = BlamCache.Deserializer.Deserialize<Weapon>(blamContext);

            // Get blam FP mode name
            blamFPmodeName = BlamCache.IndexItems.Find(x => x.ID == blamWeap.FirstPerson[0].FirstPersonAnimations.Index).Name;

            // Deserialize weap tag
            Weapon edWeap;
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, edWeapTag);
                edWeap = CacheContext.Deserializer.Deserialize<Weapon>(edContext);
            }

            // This will break on turrets, good because they need additional fixes. Weapons that do not require animations port:
            // shotgun, smg, sniper, hammer, plasma pistol, spartan laser (maybe), fuel rod
            if (blamWeap.FirstPerson.Count == 0)
                throw new Exception($"Unsupported weapon due to missing first person animations: {blamWeapName}");

            if (blamWeap.FirstPerson[0].FirstPersonAnimations == null || (uint)blamWeap.FirstPerson[0].FirstPersonAnimations.Index == 0xFFFFFFFF)
                throw new Exception($"Unsupported weapon due to missing first person animations: {blamWeapName}");

            // Rename FP mode tag
            edFPmodeName = CacheContext.TagNames.ContainsKey(edWeap.FirstPerson[0].FirstPersonAnimations.Index) ? CacheContext.TagNames[edWeap.FirstPerson[0].FirstPersonAnimations.Index] : null;
            if (edFPmodeName != null)
                CacheContext.TagNames[edWeap.FirstPerson[0].FirstPersonAnimations.Index] = $"{CacheContext.TagNames[edWeap.FirstPerson[0].FirstPersonAnimations.Index]}_HO";

            // Port FP mode
            var portTagCommand = new PortTagCommand(CacheContext, BlamCache);
            portTagCommand.Execute(new List<string> { "replace", "jmad", blamFPmodeName });

            // Set new model animations
            if (!CacheContext.TryGetTag<ModelAnimationGraph>(blamFPmodeName, out edWeap.FirstPerson[0].FirstPersonAnimations))
            {
                Console.WriteLine($"Failed to find the ported jmad tag: {blamFPmodeName}");
                return false;
            }

            // Serialize ED weap tag
            using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var context = new TagSerializationContext(stream, CacheContext, edWeapTag);
                CacheContext.Serializer.Serialize(context, edWeap);
            }

            // DEBUG: Test
            var edFPmodeTag = edWeap.FirstPerson[0].FirstPersonAnimations;

            edFPmodeName = CacheContext.TagNames.ContainsKey(edFPmodeTag.Index) ? CacheContext.TagNames[edFPmodeTag.Index] : null;

            Console.WriteLine($"DEBUG: Test: jmad: [{edFPmodeTag.Group}] 0x{edFPmodeTag.Index:X4} {edFPmodeName}");

            return true;
        }

        public bool RestoreWeaponFiringSound(List<string> args)
        {
            if (args.Count != 1)
            {
                Console.WriteLine("Required: [0] weapon name");

                Console.WriteLine(@"Example: test RestoreWeaponFiringSound D:\Halo\Cache\Halo3\sandbox.map objects\weapons\rifle\assault_rifle\assault_rifle");

                return false;
            }

            var blamWeapName = args[1];
            var blamWeapTag = BlamCache.IndexItems.Find(x => x.Name == blamWeapName);

            var portTagCommand = new PortTagCommand(CacheContext, BlamCache);
            
            if (blamWeapTag == null)
            {
                Console.WriteLine($"{blamWeapName} does not exist.");
                return false;
            }

            // Check if ED has named tags, or has that weapon.
            if (!CacheContext.TagNames.ContainsValue(blamWeapName))
            {
                Console.WriteLine($"{blamWeapName} does not exist or is named incorrectly or tags are not named.");
                return false;
            }

            if (!CacheContext.TryGetTag<Weapon>(blamWeapName, out var edWeapTag))
            {
                Console.WriteLine($"{blamWeapName} does not exist or is named incorrectly or tags are not named.");
                return false;
            }

            string blamSndName = null;
            string edSndName = null;

            // Blam:
            // Deserialize weap tag
            var blamContext = new CacheSerializationContext(ref BlamCache, blamWeapTag);
            var blamWeap = BlamCache.Deserializer.Deserialize<Weapon>(blamContext);

            var blamContext2 = new CacheSerializationContext(ref BlamCache, BlamCache.IndexItems.Find(x => x.ID == blamWeap.Barrels[0].FiringEffects[0].FiringEffect2.Index));
            var blamEffe = BlamCache.Deserializer.Deserialize<Effect>(blamContext2);
            
            // Deserialize weap tag
            Weapon edWeap;
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, edWeapTag);
                edWeap = CacheContext.Deserializer.Deserialize<Weapon>(edContext);
            }

            // Different for the smg compared to the rest of the weapons
            if (blamWeapTag.Name.Contains("smg"))
            {
                for (int j = 1; j < 7; j++)
                {
                    blamSndName = BlamCache.IndexItems.Find(x => x.ID == blamWeap.Attachments[j].Attachment2.Index).Name;

                    // Rename firing sound tag
                    edSndName = CacheContext.TagNames.ContainsKey(edWeap.Attachments[j].Attachment2.Index) ? CacheContext.TagNames[edWeap.Attachments[j].Attachment2.Index] : null;
                    if (edSndName != null)
                        CacheContext.TagNames[edWeap.Attachments[j].Attachment2.Index] = $"{CacheContext.TagNames[edWeap.Attachments[j].Attachment2.Index]}_HO";

                    portTagCommand.Execute(new List<string> { "lsnd", blamSndName });

                    if (!CacheContext.TryGetTag<SoundLooping>(blamSndName, out edWeap.Attachments[j].Attachment2))
                        throw new KeyNotFoundException($"{blamSndName}.sound_looping");
                }

                // Serialize ED weap tag
                using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
                {
                    var context = new TagSerializationContext(stream, CacheContext, edWeapTag);
                    CacheContext.Serializer.Serialize(context, edWeap);
                }

                // DEBUG: verify if tags ported and are assigned
                foreach (var a in edWeap.Attachments)
                {
                    edSndName = CacheContext.TagNames.ContainsKey(a.Attachment2.Index) ? CacheContext.TagNames[a.Attachment2.Index] : null;

                    if (edSndName != null)
                        Console.WriteLine($"DEBUG: ported: [{a.Attachment2.Group}] 0x{a.Attachment2.Index:X4} {edSndName}");
                }

                return true;
            }

            // Cheap checks, only convert weapons that have a first person firing effect
            // This will break on turrets: sword, hammer, bomb, flag, ball, spartan laser
            if (blamWeap.Barrels.Count == 0)
                throw new Exception($"Unsupported weapon due to missing 'Barrels': {blamWeapName}");

            if (blamWeap.Barrels[0].FiringEffects.Count == 0)
                throw new Exception($"Unsupported weapon due to missing 'Barrels[0].FiringEffects': {blamWeapName}");

            if (blamWeap.Barrels[0].FiringEffects[0].FiringEffect2 == null || (uint)blamWeap.Barrels[0].FiringEffects[0].FiringEffect2.Index == 0xFFFFFFFF)
                throw new Exception($"Unsupported weapon due to missing 'Barrels[0].FiringEffects[0].FiringEffect2' tag ref: {blamWeapName}");

            Effect edEffe;
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, edWeap.Barrels[0].FiringEffects[0].FiringEffect2);
                edEffe = CacheContext.Deserializer.Deserialize<Effect>(edContext);
            }

            // Rename firing sound tag
            foreach (var a in edEffe.Events[0].Parts)
            {
                edSndName = CacheContext.TagNames.ContainsKey(a.Type.Index) ? CacheContext.TagNames[a.Type.Index] : null;
                if (edSndName != null)
                    CacheContext.TagNames[a.Type.Index] = $"{CacheContext.TagNames[a.Type.Index]}_HO";
            }

            // Port firing sound
            foreach (var a in blamEffe.Events[0].Parts)
            {
                // Get blam sound name
                blamSndName = BlamCache.IndexItems.Find(x => x.ID == a.Type.Index).Name;

                portTagCommand.Execute(new List<string> { "snd!", blamSndName });
            }

            // Set new sounds
            int i = -1;
            int index = 0;
            if (blamWeapTag.Name.Contains("needler"))
                index = 2;

            edEffe.Events[0].Parts = new List<Effect.Event.Part>();
            foreach (var a in blamEffe.Events[index].Parts)
            {
                a.RuntimeBaseGroupTag = a.Type.Group.Tag;

                edEffe.Events[index].Parts.Add(a);

                i++;

                // Get blam sound name
                blamSndName = BlamCache.IndexItems.Find(x => x.ID == a.Type.Index).Name;

                if (!CacheContext.TryGetTag<Sound>(blamSndName, out edEffe.Events[index].Parts[i].Type) || edEffe.Events[index].Parts[i].Type.Index > 0x7FFF)
                    throw new Exception($"edEffe.Events[0].Parts[{i}].Type is null, sound tag has not converted.");
            }

            // Serialize ED effe tag
            using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var context = new TagSerializationContext(stream, CacheContext, edWeap.Barrels[0].FiringEffects[0].FiringEffect2);
                CacheContext.Serializer.Serialize(context, edEffe);
            }

            // DEBUG: verify if tags ported and are assigned
            foreach (var a in edEffe.Events[index].Parts)
            {
                edSndName = CacheContext.TagNames.ContainsKey(a.Type.Index) ? CacheContext.TagNames[a.Type.Index] : null;
                if (edSndName != null)
                    Console.WriteLine($"DEBUG: ported: [{a.Type.Group}] 0x{a.Type.Index:X4} {edSndName}");
            }

            return true;
        }
        
        public bool FixCinematicScene(List<string> args)
        {
            if (args.Count != 1 || !CacheContext.TryGetTag(args[0], out var edTag))
                return false;

            CinematicScene cisc;
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, edTag);
                cisc = CacheContext.Deserializer.Deserialize<CinematicScene>(edContext);
            }

            foreach (var shot in cisc.Shots)
            {
                var newFrames = new List<CinematicScene.ShotBlock.FrameBlock>();
                
                for (int i = 0; i < shot.LoadedFrameCount; i++)
                {
                    newFrames.Add(shot.Frames[i]);

                    if (i + 1 == shot.LoadedFrameCount)
                        break;

                    newFrames.Add(new CinematicScene.ShotBlock.FrameBlock
                    {
                        Position = new RealPoint3d
                        (
                            (shot.Frames[i].Position.X + shot.Frames[i + 1].Position.X) / 2f,
                            (shot.Frames[i].Position.Y + shot.Frames[i + 1].Position.Y) / 2f,
                            (shot.Frames[i].Position.Z + shot.Frames[i + 1].Position.Z) / 2f
                        ),
                        Unknown1 = (shot.Frames[i].Unknown1 + shot.Frames[i + 1].Unknown1) / 2,
                        Unknown2 = (shot.Frames[i].Unknown2 + shot.Frames[i + 1].Unknown2) / 2,
                        Unknown3 = (shot.Frames[i].Unknown3 + shot.Frames[i + 1].Unknown3) / 2,
                        Unknown4 = (shot.Frames[i].Unknown4 + shot.Frames[i + 1].Unknown4) / 2,
                        Unknown5 = (shot.Frames[i].Unknown5 + shot.Frames[i + 1].Unknown5) / 2,
                        Unknown6 = (shot.Frames[i].Unknown6 + shot.Frames[i + 1].Unknown6) / 2,
                        Unknown7 = (shot.Frames[i].Unknown7 + shot.Frames[i + 1].Unknown7) / 2,
                        Unknown8 = (shot.Frames[i].Unknown8 + shot.Frames[i + 1].Unknown8) / 2,
                        FOV = (shot.Frames[i].FOV + shot.Frames[i + 1].FOV) / 2,
                        NearPlane = (shot.Frames[i].NearPlane + shot.Frames[i + 1].NearPlane) / 2,
                        FarPlane = (shot.Frames[i].FarPlane + shot.Frames[i + 1].FarPlane) / 2,
                        FocalDepth = (shot.Frames[i].FocalDepth + shot.Frames[i + 1].FocalDepth) / 2,
                        BlurAmount = (shot.Frames[i].BlurAmount + shot.Frames[i + 1].BlurAmount) / 2,

                    });
                }

                shot.Frames = newFrames;
                newFrames = new List<CinematicScene.ShotBlock.FrameBlock>();
                shot.LoadedFrameCount *= 2;

                break;

            }

            using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var context = new TagSerializationContext(stream, CacheContext, edTag);
                CacheContext.Serializer.Serialize(context, cisc);
            }

            Console.WriteLine("Done.");

            return true;
        }

        public bool DumpBspGeometry(List<string> args)
        {
            if (args.Count != 2)
                return false;

            //
            // Verify the Blam scenario_structure_bsp tag
            //

            var blamTagName = args[0];

            CacheFile.IndexItem blamTag = null;

            Console.WriteLine("Verifying Blam scenario_structure_bsp tag...");

            foreach (var tag in BlamCache.IndexItems)
            {
                if (tag.GroupTag == "sbsp" && tag.Name == blamTagName)
                {
                    blamTag = tag;
                    break;
                }
            }

            if (blamTag == null)
            {
                Console.WriteLine("Blam tag does not exist: " + blamTagName);
                return false;
            }

            //
            // Load the Blam scenario_structure_bsp tag
            //

            var blamContext = new CacheSerializationContext(ref BlamCache, blamTag);
            var blamSbsp = BlamCache.Deserializer.Deserialize<ScenarioStructureBsp>(blamContext);

            //
            // Load blam ScenarioLightmapBspData to get geometry for geometry2
            //

            if (BlamCache.Version > CacheVersion.Halo3Retail)
            {
                CacheFile.IndexItem blamLbspTag = null;

                foreach (var tag in BlamCache.IndexItems)
                {
                    if (tag.GroupTag == "sbsp" && tag.Name == blamTagName)
                    {
                        blamLbspTag = tag;
                        break;
                    }
                }

                var blamLbsp = BlamCache.Deserializer.Deserialize<ScenarioLightmapBspData>(new CacheSerializationContext(ref BlamCache, blamLbspTag));

                blamSbsp.Geometry2.ZoneAssetHandle = blamLbsp.Geometry.ZoneAssetHandle;
            }
            else
            {
                // H3:
                // Deserialize scnr to get each sbsp's index
                // Order of sbsp's in the scnr is the same as in the sLdT

                CacheFile.IndexItem blamScenarioTag = null;

                foreach (var tag in BlamCache.IndexItems)
                {
                    if (tag.GroupTag == "scnr")
                    {
                        blamScenarioTag = tag;
                        break;
                    }
                }

                var blamScenario = BlamCache.Deserializer.Deserialize<Scenario>(new CacheSerializationContext(ref BlamCache, blamScenarioTag));

                int sbspIndex = 0;
                for (int i = 0; i < blamScenario.StructureBsps.Count; i++)
                {
                    if (blamScenario.StructureBsps[i].StructureBsp.Index == blamTag.ID)
                    {
                        sbspIndex = i;
                        break;
                    }
                }

                //
                // Get sLdT
                //

                CacheFile.IndexItem blamsLdTTag = null;

                foreach (var tag in BlamCache.IndexItems)
                {
                    if (tag.GroupTag == "sLdT")
                    {
                        blamsLdTTag = tag;
                        break;
                    }
                }

                var blamsLdT = BlamCache.Deserializer.Deserialize<ScenarioLightmap>(new CacheSerializationContext(ref BlamCache, blamsLdTTag));

                blamSbsp.Geometry2.ZoneAssetHandle = blamsLdT.Lightmaps[sbspIndex].Geometry.ZoneAssetHandle;
            }

            //
            // Load Blam resource data
            //

            var geometry = blamSbsp.Geometry;
            var resourceData = BlamCache.GetRawFromID(geometry.ZoneAssetHandle);

            if (resourceData == null)
            {
                Console.WriteLine("Blam render_geometry resource contains no data. Created empty resource.");
                return false;
            }

            //
            // Load Blam resource definition
            //

            Console.Write("Loading Blam render_geometry resource definition...");

            var definitionEntry = BlamCache.ResourceGestalt.TagResources[geometry.ZoneAssetHandle & ushort.MaxValue];

            var resourceDefinition = new RenderGeometryApiResourceDefinition
            {
                VertexBuffers = new List<D3DPointer<VertexBufferDefinition>>(),
                IndexBuffers = new List<D3DPointer<IndexBufferDefinition>>()
            };

            using (var definitionStream = new MemoryStream(BlamCache.ResourceGestalt.FixupInformation))
            using (var definitionReader = new EndianReader(definitionStream, EndianFormat.BigEndian))
            {
                var dataContext = new DataSerializationContext(definitionReader, null, CacheAddressType.Definition);

                definitionReader.SeekTo(definitionEntry.FixupInformationOffset + (definitionEntry.FixupInformationLength - 24));

                var vertexBufferCount = definitionReader.ReadInt32();
                definitionReader.Skip(8);
                var indexBufferCount = definitionReader.ReadInt32();

                definitionReader.SeekTo(definitionEntry.FixupInformationOffset);

                for (var i = 0; i < vertexBufferCount; i++)
                {
                    resourceDefinition.VertexBuffers.Add(new D3DPointer<VertexBufferDefinition>
                    {
                        Definition = new VertexBufferDefinition
                        {
                            Count = definitionReader.ReadInt32(),
                            Format = (VertexBufferFormat)definitionReader.ReadInt16(),
                            VertexSize = definitionReader.ReadInt16(),
                            Data = new TagData
                            {
                                Size = definitionReader.ReadInt32(),
                                Unused4 = definitionReader.ReadInt32(),
                                Unused8 = definitionReader.ReadInt32(),
                                Address = new CacheAddress(CacheAddressType.Memory, definitionReader.ReadInt32()),
                                Unused10 = definitionReader.ReadInt32()
                            }
                        }
                    });
                }

                definitionReader.Skip(vertexBufferCount * 12);

                for (var i = 0; i < indexBufferCount; i++)
                {
                    resourceDefinition.IndexBuffers.Add(new D3DPointer<IndexBufferDefinition>
                    {
                        Definition = new IndexBufferDefinition
                        {
                            Format = (IndexBufferFormat)definitionReader.ReadInt32(),
                            Data = new TagData
                            {
                                Size = definitionReader.ReadInt32(),
                                Unused4 = definitionReader.ReadInt32(),
                                Unused8 = definitionReader.ReadInt32(),
                                Address = new CacheAddress(CacheAddressType.Memory, definitionReader.ReadInt32()),
                                Unused10 = definitionReader.ReadInt32()
                            }
                        }
                    });
                }
            }

            Console.WriteLine("done.");

            //
            // Convert Blam data to ElDorado data
            //

            using (var fileStream = File.Create(args[1]))
            using (var fileWriter = new StreamWriter(fileStream))
            {
                using (var blamResourceStream = new MemoryStream(resourceData))
                using (var blamResourceReader = new EndianReader(blamResourceStream, EndianFormat.LittleEndian))
                {
                    //
                    // Convert Blam vertex buffers
                    //

                    Console.Write("Converting vertex buffers...");
                    
                    for (var i = 0; i < resourceDefinition.VertexBuffers.Count; i++)
                    {
                        blamResourceStream.Position = definitionEntry.ResourceFixups[i].Offset;

                        var vertexBuffer = resourceDefinition.VertexBuffers[i].Definition;

                        fileWriter.WriteLine($"Offset = {vertexBuffer.Data.Address.Offset.ToString("X8")} Count = {vertexBuffer.Count} Size = {vertexBuffer.VertexSize}, Format = {vertexBuffer.Format.ToString()}");

                        switch (vertexBuffer.Format)
                        {
                            case VertexBufferFormat.TinyPosition:
                                for (var j = 0; j < vertexBuffer.Count; j++)
                                {
                                    fileWriter.WriteLine($"(I,J,K,W) = ({blamResourceReader.ReadUInt32().ToString("X2")},{blamResourceReader.ReadUInt32().ToString("X2")},{blamResourceReader.ReadUInt32().ToString("X2")},{blamResourceReader.ReadUInt32().ToString("X2")})");
                                }
                                break;
                        }
                    }

                }
            }
            return true;
        }

        private string ConvertTexcoord(uint input)
        {
            RealQuaternion vector = new RealQuaternion(BitConverter.GetBytes(input).Select(e => ConvertByte(e)).ToArray());

            return vector.ToString();
        }

        private float ConvertByte(byte e)
        {
            var element = e;
            float result;
            if (element < 0)
                result = (float)element / (float)sbyte.MinValue;
            else if (element > 0)
                result = (float)element / (float)sbyte.MaxValue;
            else
                result = 0.0f;

            result = (result + 1.0f) / 2.0f;
            return result;
        }

        public bool DumpScriptInfo(List<string> args)
        {
            if (args.Count != 0)
                return false;

            //
            // Verify the Blam scenario tag
            //

            CacheFile.IndexItem blamTag = null;

            Console.WriteLine("Verifying Blam scenario tag...");

            foreach (var tag in BlamCache.IndexItems)
            {
                if (tag.GroupTag == "scnr")
                {
                    blamTag = tag;
                    break;
                }
            }

            //
            // Load the Blam scenario tag
            //

            var blamContext = new CacheSerializationContext(ref BlamCache, blamTag);
            var blamScenario = BlamCache.Deserializer.Deserialize<Scenario>(blamContext);

            foreach (var script in blamScenario.Scripts)
            {
                var index = (int)blamScenario.ScriptExpressions[(int)(script.RootExpressionHandle & ushort.MaxValue) + 1].NextExpressionHandle & ushort.MaxValue;
                Console.WriteLine($"{script.ScriptName}: 0x{blamScenario.ScriptExpressions[index].Opcode:X3}");
            }

            return true;
        }

        public bool ListShaderBitmaps(List<string> args)
        {
            if (args.Count != 1)
                return false;

            CacheFile.IndexItem item = null;

            Console.WriteLine("Verifying blam shader tag...");

            var shaderName = args[0];

            foreach (var tag in BlamCache.IndexItems)
            {
                if ((tag.ParentGroupTag == "rm") && tag.Name == shaderName)
                {
                    item = tag;
                    break;
                }
            }

            if (item == null)
            {
                Console.WriteLine("Blam shader tag does not exist: " + shaderName);
                return false;
            }

            var blamContext = new CacheSerializationContext(ref BlamCache, item);
            var blamShader = BlamCache.Deserializer.Deserialize<RenderMethod>(blamContext);

            var templateItem = BlamCache.IndexItems.Find(i =>
                i.ID == blamShader.ShaderProperties[0].Template.Index);

            blamContext = new CacheSerializationContext(ref BlamCache, templateItem);
            var template = BlamCache.Deserializer.Deserialize<RenderMethodTemplate>(blamContext);

            for (var i = 0; i < template.ShaderMaps.Count; i++)
            {
                var entry = template.ShaderMaps[i].Name;

                var bitmItem = BlamCache.IndexItems.Find(j =>
                j.ID == blamShader.ShaderProperties[0].ShaderMaps[i].Bitmap.Index);
                Console.WriteLine(string.Format("{0:D2} {2}\t {1}", i, bitmItem, BlamCache.Strings.GetString(entry)));
            }

            return true;
        }

        public bool ListBspMoppCodes(List<string> args)
        {
            if (args.Count != 1)
                return false;

            CacheFile.IndexItem blamTag = null;

            var blamTagName = args[0];

            foreach (var tag in BlamCache.IndexItems)
            {
                if (tag.GroupTag == "sbsp" && tag.Name == blamTagName)
                {
                    blamTag = tag;
                    break;
                }
            }

            if (blamTag == null)
            {
                Console.WriteLine("Blam scenario_structure_bsp tag does not exist: " + blamTagName);
                return false;
            }

            var blamContext = new CacheSerializationContext(ref BlamCache, blamTag);
            var blamSbsp = BlamCache.Deserializer.Deserialize<ScenarioStructureBsp>(blamContext);

            var blamMoppCodes = new List<byte>();

            foreach (var mopp in blamSbsp.CollisionMoppCodes)
                GetMoppCodes(mopp.Data.Select(i => i.Value).ToList(), ref blamMoppCodes);

            var resourceData = BlamCache.GetRawFromID(blamSbsp.ZoneAssetIndex3);

            if (resourceData != null)
            {
                var resourceEntry = BlamCache.ResourceGestalt.TagResources[blamSbsp.ZoneAssetIndex3 & ushort.MaxValue];

                var definitionAddress = new CacheAddress(CacheAddressType.Definition, resourceEntry.DefinitionAddress);
                var definitionData = BlamCache.ResourceGestalt.FixupInformation.Skip(resourceEntry.FixupInformationOffset).Take(resourceEntry.FixupInformationLength).ToArray();

                StructureBspTagResources resourceDefinition = null;

                using (var definitionStream = new MemoryStream(definitionData, true))
                using (var definitionReader = new EndianReader(definitionStream, EndianFormat.BigEndian))
                {
                    definitionStream.Position = definitionAddress.Offset;
                    resourceDefinition = BlamCache.Deserializer.Deserialize<StructureBspTagResources>(
                        new DataSerializationContext(definitionReader, CacheAddressType.Definition));
                }

                using (var blamResourceStream = new MemoryStream(resourceData))
                using (var resourceReader = new EndianReader(blamResourceStream, EndianFormat.BigEndian))
                {
                    var dataContext = new DataSerializationContext(resourceReader);

                    foreach (var instance in resourceDefinition.InstancedGeometry)
                    {
                        foreach (var moppCode in instance.CollisionMoppCodes)
                        {
                            blamResourceStream.Position = moppCode.Data.Address.Offset;
                            var moppData = resourceReader.ReadBytes(moppCode.Data.Count).ToList();
                            GetMoppCodes(moppData, ref blamMoppCodes);
                        }
                    }
                }
            }

            blamMoppCodes.Sort();

            foreach (var blamMoppCode in blamMoppCodes)
                Console.WriteLine($"0x{blamMoppCode:X2}");

            return true;
        }

        private void GetMoppCodes(List<byte> moppData, ref List<byte> moppCodes)
        {
            for (var i = 0; i < moppData.Count; i++)
            {
                var moppCode = moppData[i];

                switch (moppCode)
                {
                    case 0x00: // HK_MOPP_RETURN
                        break;

                    case 0x01: // HK_MOPP_SCALE1
                    case 0x02: // HK_MOPP_SCALE2
                    case 0x03: // HK_MOPP_SCALE3
                    case 0x04: // HK_MOPP_SCALE4
                        i += 3;
                        break;

                    case 0x05: // HK_MOPP_JUMP8
                        i += 1;
                        break;

                    case 0x06: // HK_MOPP_JUMP16
                        i += 2;
                        break;

                    case 0x07: // HK_MOPP_JUMP24
                        i += 3;
                        break;

                    /*case 0x08: // HK_MOPP_JUMP32 (NOT IMPLEMENTED)
                        Array.Reverse(moppData, i + 1, 4);
                        i += 4;
                        break;*/

                    case 0x09: // HK_MOPP_TERM_REOFFSET8
                        i += 1;
                        break;

                    case 0x0A: // HK_MOPP_TERM_REOFFSET16
                        i += 2;
                        break;

                    case 0x0B: // HK_MOPP_TERM_REOFFSET32
                        i += 4;
                        break;

                    case 0x0C: // HK_MOPP_JUMP_CHUNK
                        i += 2;
                        break;

                    case 0x0D: // HK_MOPP_DATA_OFFSET
                        i += 5;
                        break;

                    /*case 0x0E: // UNUSED
                    case 0x0F: // UNUSED
                        break;*/

                    case 0x10: // HK_MOPP_SPLIT_X
                    case 0x11: // HK_MOPP_SPLIT_Y
                    case 0x12: // HK_MOPP_SPLIT_Z
                    case 0x13: // HK_MOPP_SPLIT_YZ
                    case 0x14: // HK_MOPP_SPLIT_YMZ
                    case 0x15: // HK_MOPP_SPLIT_XZ
                    case 0x16: // HK_MOPP_SPLIT_XMZ
                    case 0x17: // HK_MOPP_SPLIT_XY
                    case 0x18: // HK_MOPP_SPLIT_XMY
                    case 0x19: // HK_MOPP_SPLIT_XYZ
                    case 0x1A: // HK_MOPP_SPLIT_XYMZ
                    case 0x1B: // HK_MOPP_SPLIT_XMYZ
                    case 0x1C: // HK_MOPP_SPLIT_XMYMZ
                        i += 3;
                        break;

                    /*case 0x1D: // UNUSED
                    case 0x1E: // UNUSED
                    case 0x1F: // UNUSED
                        break;*/

                    case 0x20: // HK_MOPP_SINGLE_SPLIT_X
                    case 0x21: // HK_MOPP_SINGLE_SPLIT_Y
                    case 0x22: // HK_MOPP_SINGLE_SPLIT_Z
                        i += 2;
                        break;

                    case 0x23: // HK_MOPP_SPLIT_JUMP_X
                    case 0x24: // HK_MOPP_SPLIT_JUMP_Y
                    case 0x25: // HK_MOPP_SPLIT_JUMP_Z
                        i += 6;
                        break;


                    case 0x26: // HK_MOPP_DOUBLE_CUT_X
                    case 0x27: // HK_MOPP_DOUBLE_CUT_Y
                    case 0x28: // HK_MOPP_DOUBLE_CUT_Z
                        i += 2;
                        break;

                    case 0x29: // HK_MOPP_DOUBLE_CUT24_X
                    case 0x2A: // HK_MOPP_DOUBLE_CUT24_Y
                    case 0x2B: // HK_MOPP_DOUBLE_CUT24_Z
                        i += 6;
                        break;

                    /*case 0x2C: // UNUSED
                    case 0x2D: // UNUSED
                    case 0x2E: // UNUSED
                    case 0x2F: // UNUSED
                        break;*/

                    case 0x30: // HK_MOPP_TERM4_0
                    case 0x31: // HK_MOPP_TERM4_1
                    case 0x32: // HK_MOPP_TERM4_2
                    case 0x33: // HK_MOPP_TERM4_3
                    case 0x34: // HK_MOPP_TERM4_4
                    case 0x35: // HK_MOPP_TERM4_5
                    case 0x36: // HK_MOPP_TERM4_6
                    case 0x37: // HK_MOPP_TERM4_7
                    case 0x38: // HK_MOPP_TERM4_8
                    case 0x39: // HK_MOPP_TERM4_9
                    case 0x3A: // HK_MOPP_TERM4_A
                    case 0x3B: // HK_MOPP_TERM4_B
                    case 0x3C: // HK_MOPP_TERM4_C
                    case 0x3D: // HK_MOPP_TERM4_D
                    case 0x3E: // HK_MOPP_TERM4_E
                    case 0x3F: // HK_MOPP_TERM4_F
                    case 0x40: // HK_MOPP_TERM4_10
                    case 0x41: // HK_MOPP_TERM4_11
                    case 0x42: // HK_MOPP_TERM4_12
                    case 0x43: // HK_MOPP_TERM4_13
                    case 0x44: // HK_MOPP_TERM4_14
                    case 0x45: // HK_MOPP_TERM4_15
                    case 0x46: // HK_MOPP_TERM4_16
                    case 0x47: // HK_MOPP_TERM4_17
                    case 0x48: // HK_MOPP_TERM4_18
                    case 0x49: // HK_MOPP_TERM4_19
                    case 0x4A: // HK_MOPP_TERM4_1A
                    case 0x4B: // HK_MOPP_TERM4_1B
                    case 0x4C: // HK_MOPP_TERM4_1C
                    case 0x4D: // HK_MOPP_TERM4_1D
                    case 0x4E: // HK_MOPP_TERM4_1E
                    case 0x4F: // HK_MOPP_TERM4_1F
                               // TODO: Does this function take any operands?
                        break;

                    case 0x50: // HK_MOPP_TERM8
                        i += 1;
                        break;

                    case 0x51: // HK_MOPP_TERM16
                        i += 2;
                        break;

                    case 0x52: // HK_MOPP_TERM24
                        i += 3;
                        break;

                    case 0x53: // HK_MOPP_TERM32
                        i += 4;
                        break;

                    case 0x54: // HK_MOPP_NTERM_8
                        i += 1;
                        break;

                    case 0x55: // HK_MOPP_NTERM_16
                        i += 2;
                        break;

                    case 0x56: // HK_MOPP_NTERM_24
                        i += 3;
                        break;

                    case 0x57: // HK_MOPP_NTERM_32
                        i += 4;
                        break;

                    /*case 0x58: // UNUSED
                    case 0x59: // UNUSED
                    case 0x5A: // UNUSED
                    case 0x5B: // UNUSED
                    case 0x5C: // UNUSED
                    case 0x5D: // UNUSED
                    case 0x5E: // UNUSED
                    case 0x5F: // UNUSED
                        break;*/

                    case 0x60: // HK_MOPP_PROPERTY8_0
                    case 0x61: // HK_MOPP_PROPERTY8_1
                    case 0x62: // HK_MOPP_PROPERTY8_2
                    case 0x63: // HK_MOPP_PROPERTY8_3
                        i += 1;
                        break;

                    case 0x64: // HK_MOPP_PROPERTY16_0
                    case 0x65: // HK_MOPP_PROPERTY16_1
                    case 0x66: // HK_MOPP_PROPERTY16_2
                    case 0x67: // HK_MOPP_PROPERTY16_3
                        i += 2;
                        break;

                    case 0x68: // HK_MOPP_PROPERTY32_0
                    case 0x69: // HK_MOPP_PROPERTY32_1
                    case 0x6A: // HK_MOPP_PROPERTY32_2
                    case 0x6B: // HK_MOPP_PROPERTY32_3
                        i += 4;
                        break;

                    default:
                        throw new NotSupportedException($"Opcode 0x{moppCode:X2}");
                }

                if (!moppCodes.Contains(moppCode))
                    moppCodes.Add(moppCode);
            }
        }
    }
}