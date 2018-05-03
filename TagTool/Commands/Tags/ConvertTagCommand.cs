using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Common;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Geometry;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using TagTool.Scripting;
using TagTool.Commands.Common;
using TagTool.Tags;

namespace TagTool.Commands.Tags
{
    class ConvertTagCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private bool IsDecalShader { get; set; } = false;

        public ConvertTagCommand(GameCacheContext info)
            : base(CommandFlags.None,

                  "ConvertTag",
                  "Convert a tag and its dependencies to another engine version",

                  "ConvertTag <tag> <input csv> <output csv> <target directory>",

                  "The tag map CSV should be generated using the \"MatchTags\" command.\n" +
                  "If a tag is listed in the CSV file, it will not be converted.\n" +
                  "The output CSV file is used for converting multiple maps.\n" +
                  "Subsequent convert commands should use the new CSV.\n" +
                  "The target directory should be the maps folder for the target engine.")
        {
            CacheContext = info;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 4)
                return false;

            var srcTag = ArgumentParser.ParseTagSpecifier(CacheContext, args[0]);

            if (srcTag == null)
                return false;

            var csvPath = args[1];
            var csvOutPath = args[2];
            var targetDir = args[3];

            // Load the CSV
            Console.WriteLine("Reading {0}...", csvPath);
            TagVersionMap tagMap;

            using (var reader = new StreamReader(File.Exists(csvPath) ? File.OpenRead(csvPath) : File.Create(csvPath)))
                tagMap = TagVersionMap.ParseTagVersionMap(reader);

            // Load destination cache files
            var destCacheContext = new GameCacheContext(new DirectoryInfo(targetDir));
            using (var stream = destCacheContext.OpenTagCacheRead())
                destCacheContext.TagCache = new TagCache(stream);
            
            Console.WriteLine();
            Console.WriteLine("CONVERTING FROM VERSION {0} TO {1}", CacheVersionDetection.GetBuildName(CacheContext.Version), CacheVersionDetection.GetBuildName(destCacheContext.Version));
            Console.WriteLine();
            
            CachedTagInstance resultTag;
            using (Stream srcStream = CacheContext.OpenTagCacheRead(), destStream = destCacheContext.OpenTagCacheReadWrite())
                resultTag = ConvertTag(srcTag, CacheContext, srcStream, destCacheContext, destStream, tagMap);

            Console.WriteLine();
            Console.WriteLine("Repairing decal systems...");

            if (CacheContext.Version != destCacheContext.Version)
                FixDecalSystems(destCacheContext, resultTag.Index);

            Console.WriteLine();
            Console.WriteLine("Saving stringIDs...");
            using (var stream = destCacheContext.OpenStringIdCacheReadWrite())
                destCacheContext.StringIdCache.Save(stream);

            Console.WriteLine("Writing {0}...", csvOutPath);
            using (var stream = new StreamWriter(File.Open(csvOutPath, FileMode.Create, FileAccess.ReadWrite)))
                tagMap.WriteCsv(stream);

            // Uncomment this to add the new tag as a dependency to cfgt to make testing easier
            /*using (var stream = destCacheContext.OpenCacheReadWrite())
            {
                destCacheContext.Cache.Tags[0].Dependencies.Add(resultTag.Index);
                destCacheContext.Cache.UpdateTag(stream, destCacheContext.Cache.Tags[0]);
            }*/

            Console.WriteLine();
            Console.WriteLine("All done! The converted tag is:");
            TagPrinter.PrintTagShort(resultTag);

            destCacheContext.SaveTagNames();

            return true;
        }

        private CachedTagInstance ConvertTag(CachedTagInstance srcTag, GameCacheContext srcCacheContext, Stream srcStream, GameCacheContext destCacheContext, Stream destStream, TagVersionMap tagMap)
        {
            TagPrinter.PrintTagShort(srcTag);

            // Uncomment this to use 0x101F for all shaders
            /*if (srcTag.IsClass("rm  "))
                return destCacheContext.Cache.Tags[0x101F];*/

            // Check if the tag is in the map, and just return the translated tag if so
            var destIndex = tagMap.Translate(srcCacheContext.Version, srcTag.Index, destCacheContext.Version);
            if (destIndex >= 0)
            {
                Console.WriteLine("- Using already-known index {0:X4}", destIndex);
                return destCacheContext.TagCache.Index[destIndex];
            }

            // Deserialize the tag from the source cache
            var structureType = TagDefinition.Find(srcTag.Group.Tag);
            var srcContext = new TagSerializationContext(srcStream, srcCacheContext, srcTag);
            var tagData = srcCacheContext.Deserializer.Deserialize(srcContext, structureType);
            
            // Uncomment this to use 0x101F in place of shaders that need conversion
            /*if (tagData is RenderMethod)
            {
                var rm = (RenderMethod)tagData;
                foreach (var prop in rm.ShaderProperties)
                {
                    if (tagMap.Translate(srcCacheContext.Version, prop.Template.Index, destCacheContext.Version) < 0)
                        return destCacheContext.Cache.Tags[0x101F];
                }
            }*/

            // Allocate a new tag and create a mapping for it

            CachedTagInstance instance = null;

            if (srcCacheContext.Version != destCacheContext.Version)
            {
                for (var i = 0; i < destCacheContext.TagCache.Index.Count; i++)
                {
                    if (destCacheContext.TagCache.Index[i] == null)
                    {
                        destCacheContext.TagCache.Index[i] = instance = new CachedTagInstance(i, TagGroup.Instances[srcTag.Group.Tag]);
                        break;
                    }
                }
            }
            else
            {
                if (destCacheContext.TagCache.Index[srcTag.Index] != null)
                {
                    if (destCacheContext.TagCache.Index[srcTag.Index].IsInGroup(srcTag.Group))
                        return destCacheContext.TagCache.Index[srcTag.Index];
                }
                else
                {
                    if (srcCacheContext.TagNames.ContainsKey(srcTag.Index))
                        destCacheContext.TagNames[srcTag.Index] = srcCacheContext.TagNames[srcTag.Index];
                    
                    destCacheContext.TagCache.Index[srcTag.Index] = instance = new CachedTagInstance(srcTag.Index, TagGroup.Instances[srcTag.Group.Tag]);
                }
            }

            if (instance == null)
                instance = destCacheContext.TagCache.AllocateTag(srcTag.Group);

            tagMap.Add(srcCacheContext.Version, srcTag.Index, destCacheContext.Version, instance.Index);

            if (srcTag.IsInGroup("decs") || srcTag.IsInGroup("rmd "))
                IsDecalShader = true;

            // Convert it
            tagData = Convert(tagData, srcCacheContext, srcStream, destCacheContext, destStream, tagMap);

            if (srcTag.IsInGroup("decs") || srcTag.IsInGroup("rmd "))
                IsDecalShader = false;

            // Re-serialize into the destination cache
            var destContext = new TagSerializationContext(destStream, destCacheContext, instance);
            destCacheContext.Serializer.Serialize(destContext, tagData);
            return instance;
        }

        private object Convert(object data, GameCacheContext srcCacheContext, Stream srcStream, GameCacheContext destCacheContext, Stream destStream, TagVersionMap tagMap)
        {
            if (data == null)
                return null;
            var type = data.GetType();
            if (type.IsPrimitive)
                return data;
            if (type == typeof(StringId))
                return ConvertStringID((StringId)data, srcCacheContext, destCacheContext);
            if (type == typeof(CachedTagInstance))
                return ConvertTag((CachedTagInstance)data, srcCacheContext, srcStream, destCacheContext, destStream, tagMap);
            if (type == typeof(PageableResource))
                return ConvertResource((PageableResource)data, srcCacheContext, destCacheContext);
            if (type == typeof(RenderGeometry))
                return ConvertGeometry((RenderGeometry)data, srcCacheContext, destCacheContext);
            if (type == typeof(GameObjectType))
                return ConvertGameObjectType((GameObjectType)data, srcCacheContext, destCacheContext);
            if (type == typeof(ObjectTypeFlags))
                return ConvertObjectTypeFlags((ObjectTypeFlags)data, srcCacheContext, destCacheContext);
            if (type == typeof(ScenarioObjectType))
                return ConvertScenarioObjectType((ScenarioObjectType)data, srcCacheContext, destCacheContext);
            if (type.IsArray)
                return ConvertArray((Array)data, srcCacheContext, srcStream, destCacheContext, destStream, tagMap);
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return ConvertList(data, type, srcCacheContext, srcStream, destCacheContext, destStream, tagMap);
            if (type.GetCustomAttributes(typeof(TagStructureAttribute), false).Length > 0)
                return ConvertStructure(data, type, srcCacheContext, srcStream, destCacheContext, destStream, tagMap);
            return data;
        }

        private ObjectTypeFlags ConvertObjectTypeFlags(ObjectTypeFlags data, GameCacheContext srcCacheContext, GameCacheContext destCacheContext)
        {
            if (destCacheContext.Version < CacheVersion.HaloOnline449175)
                if (!Enum.TryParse(data.HaloOnline.ToString(), out data.Halo3ODST))
                    throw new FormatException(destCacheContext.Version.ToString());

            return data;
        }

        private Array ConvertArray(Array array, GameCacheContext srcCacheContext, Stream srcStream, GameCacheContext destCacheContext, Stream destStream, TagVersionMap tagMap)
        {
            if (array.GetType().GetElementType().IsPrimitive)
                return array;
            for (var i = 0; i < array.Length; i++)
            {
                var oldValue = array.GetValue(i);
                var newValue = Convert(oldValue, srcCacheContext, srcStream, destCacheContext, destStream, tagMap);
                array.SetValue(newValue, i);
            }
            return array;
        }

        private object ConvertList(object list, Type type, GameCacheContext srcCacheContext, Stream srcStream, GameCacheContext destCacheContext, Stream destStream, TagVersionMap tagMap)
        {
            if (type.GenericTypeArguments[0].IsPrimitive)
                return list;
            var count = (int)type.GetProperty("Count").GetValue(list);
            var getItem = type.GetMethod("get_Item");
            var setItem = type.GetMethod("set_Item");
            for (var i = 0; i < count; i++)
            {
                var oldValue = getItem.Invoke(list, new object[] { i });
                var newValue = Convert(oldValue, srcCacheContext, srcStream, destCacheContext, destStream, tagMap);
                setItem.Invoke(list, new object[] { i, newValue });
            }
            return list;
        }

        private object ConvertStructure(object data, Type type, GameCacheContext srcCacheContext, Stream srcStream, GameCacheContext destCacheContext, Stream destStream, TagVersionMap tagMap)
        {
            // Convert each field
            var enumerator = new TagFieldEnumerator(new TagStructureInfo(type, destCacheContext.Version));
            while (enumerator.Next())
            {
                var oldValue = enumerator.Field.GetValue(data);
                var newValue = Convert(oldValue, srcCacheContext, srcStream, destCacheContext, destStream, tagMap);
                enumerator.Field.SetValue(data, newValue);
            }

            // Perform fixups
            FixShaders(data);
            if (data is Scenario scenario)
                FixScenario(scenario);

            return data;
        }

        private StringId ConvertStringID(StringId stringId, GameCacheContext srcCacheContext, GameCacheContext destCacheContext)
        {
            if (stringId == StringId.Invalid)
                return stringId;
            var srcString = srcCacheContext.GetString(stringId);
            if (srcString == null)
                return StringId.Invalid;
            var destStringID = destCacheContext.GetStringId(srcString);
            if (destStringID == StringId.Invalid)
                destStringID = destCacheContext.StringIdCache.AddString(srcString);
            return destStringID;
        }

        private PageableResource ConvertResource(PageableResource resource, GameCacheContext srcCacheContext, GameCacheContext destCacheContext)
        {
            if (resource == null || resource.Page.Index < 0 || !resource.GetLocation(out var location))
                return null;

            Console.WriteLine("- Copying resource {0} in {1}...", resource.Page.Index, location);

            var data = srcCacheContext.ExtractRawResource(resource);
            var newLocation = FixResourceLocation(location, srcCacheContext.Version, destCacheContext.Version);
            destCacheContext.AddRawResource(resource, newLocation, data);

            return resource;
        }

        private ResourceLocation FixResourceLocation(ResourceLocation location, CacheVersion srcVersion, CacheVersion destVersion)
        {
            return ResourceLocation.ResourcesB;
            /*if (CacheVersionDetection.Compare(destVersion, CacheVersion.HaloOnline235640) >= 0)
                return location;
            switch (location)
            {
                case ResourceLocation.RenderModels:
                    return ResourceLocation.Resources;
                case ResourceLocation.Lightmaps:
                    return ResourceLocation.Textures;
            }
            return location;*/
        }

        private RenderGeometry ConvertGeometry(RenderGeometry geometry, GameCacheContext srcCacheContext, GameCacheContext destCacheContext)
        {
            if (geometry == null || geometry.Resource == null || geometry.Resource.Page.Index < 0 || !geometry.Resource.GetLocation(out var location))
                return geometry;

            // The format changed starting with version 1.235640, so if both versions are on the same side then they can be converted normally
            var srcCompare = CacheVersionDetection.Compare(srcCacheContext.Version, CacheVersion.HaloOnline235640);
            var destCompare = CacheVersionDetection.Compare(destCacheContext.Version, CacheVersion.HaloOnline235640);
            if ((srcCompare < 0 && destCompare < 0) || (srcCompare >= 0 && destCompare >= 0))
            {
                geometry.Resource = ConvertResource(geometry.Resource, srcCacheContext, destCacheContext);
                return geometry;
            }

            Console.WriteLine("- Rebuilding geometry resource {0} in {1}...", geometry.Resource.Page.Index, location);
            using (MemoryStream inStream = new MemoryStream(), outStream = new MemoryStream())
            {
                // First extract the model data
                srcCacheContext.ExtractResource(geometry.Resource, inStream);

                // Now open source and destination vertex streams
                inStream.Position = 0;
                var inVertexStream = VertexStreamFactory.Create(srcCacheContext.Version, inStream);
                var outVertexStream = VertexStreamFactory.Create(destCacheContext.Version, outStream);

                // Deserialize the definition data
                var resourceContext = new ResourceSerializationContext(geometry.Resource);
                var definition = srcCacheContext.Deserializer.Deserialize<RenderGeometryApiResourceDefinition>(resourceContext);

                // Convert each vertex buffer
                foreach (var buffer in definition.VertexBuffers)
                    ConvertVertexBuffer(srcCacheContext, destCacheContext, buffer.Definition, inStream, inVertexStream, outStream, outVertexStream);

                // Copy each index buffer over
                foreach (var buffer in definition.IndexBuffers)
                {
                    if (buffer.Definition.Data.Size == 0)
                        continue;
                    inStream.Position = buffer.Definition.Data.Address.Offset;
                    buffer.Definition.Data.Address = new CacheAddress(CacheAddressType.Resource, (int)outStream.Position);
                    var bufferData = new byte[buffer.Definition.Data.Size];
                    inStream.Read(bufferData, 0, bufferData.Length);
                    outStream.Write(bufferData, 0, bufferData.Length);
                    StreamUtil.Align(outStream, 4);
                }

                // Update the definition data
                destCacheContext.Serializer.Serialize(resourceContext, definition);

                // Now inject the new resource data
                var newLocation = FixResourceLocation(location, srcCacheContext.Version, destCacheContext.Version);

                outStream.Position = 0;
                destCacheContext.AddResource(geometry.Resource, newLocation, outStream);
            }

            return geometry;
        }

        private void ConvertVertexBuffer(GameCacheContext srcCacheContext, GameCacheContext destCacheContext, VertexBufferDefinition buffer, MemoryStream inStream, IVertexStream inVertexStream, MemoryStream outStream, IVertexStream outVertexStream)
        {
            if (buffer.Data.Size == 0)
                return;
            var count = buffer.Count;
            var startPos = (int)outStream.Position;
            inStream.Position = buffer.Data.Address.Offset;
            buffer.Data.Address = new CacheAddress(CacheAddressType.Resource, startPos);
            switch (buffer.Format)
            {
                case VertexBufferFormat.World:
                    ConvertVertices(count, inVertexStream.ReadWorldVertex, v =>
                    {
                        if (srcCacheContext.Version > CacheVersion.HaloOnline235640)
                            v.Binormal = new RealVector3d(v.Position.W, v.Tangent.W, 0); // Converted shaders use this

                        outVertexStream.WriteWorldVertex(v);
                    });
                    break;
                case VertexBufferFormat.Rigid:
                    ConvertVertices(count, inVertexStream.ReadRigidVertex, v =>
                    {
                        if (srcCacheContext.Version > CacheVersion.HaloOnline235640)
                            v.Binormal = new RealVector3d(v.Position.W, v.Tangent.W, 0); // Converted shaders use this

                        outVertexStream.WriteRigidVertex(v);
                    });
                    break;
                case VertexBufferFormat.Skinned:
                    ConvertVertices(count, inVertexStream.ReadSkinnedVertex, v =>
                    {
                        if (srcCacheContext.Version > CacheVersion.HaloOnline235640)
                            v.Binormal = new RealVector3d(v.Position.W, v.Tangent.W, 0); // Converted shaders use this

                        outVertexStream.WriteSkinnedVertex(v);
                    });
                    break;
                case VertexBufferFormat.StaticPerPixel:
                    ConvertVertices(count, inVertexStream.ReadStaticPerPixelData, outVertexStream.WriteStaticPerPixelData);
                    break;
                case VertexBufferFormat.StaticPerVertex:
                    ConvertVertices(count, inVertexStream.ReadStaticPerVertexData, outVertexStream.WriteStaticPerVertexData);
                    break;
                case VertexBufferFormat.AmbientPrt:
                    ConvertVertices(count, inVertexStream.ReadAmbientPrtData, outVertexStream.WriteAmbientPrtData);
                    break;
                case VertexBufferFormat.LinearPrt:
                    ConvertVertices(count, inVertexStream.ReadLinearPrtData, outVertexStream.WriteLinearPrtData);
                    break;
                case VertexBufferFormat.QuadraticPrt:
                    ConvertVertices(count, inVertexStream.ReadQuadraticPrtData, outVertexStream.WriteQuadraticPrtData);
                    break;
                case VertexBufferFormat.StaticPerVertexColor:
                    ConvertVertices(count, inVertexStream.ReadStaticPerVertexColorData, outVertexStream.WriteStaticPerVertexColorData);
                    break;
                case VertexBufferFormat.Decorator:
                    ConvertVertices(count, inVertexStream.ReadDecoratorVertex, outVertexStream.WriteDecoratorVertex);
                    break;
                case VertexBufferFormat.World2:
                    buffer.Format = VertexBufferFormat.World;
                    goto default;
                default:
                    // Just copy the raw buffer over and pray that it works...
                    var bufferData = new byte[buffer.Data.Size];
                    inStream.Read(bufferData, 0, bufferData.Length);
                    outStream.Write(bufferData, 0, bufferData.Length);
                    break;
            }
            buffer.Data.Size = (int)outStream.Position - startPos;
            buffer.VertexSize = (short)(buffer.Data.Size / buffer.Count);
        }

        private void ConvertVertices<T>(int count, Func<T> readFunc, Action<T> writeFunc)
        {
            for (var i = 0; i < count; i++)
                writeFunc(readFunc());
        }

        private GameObjectType ConvertGameObjectType(GameObjectType objectType, GameCacheContext srcCacheContext, GameCacheContext destCacheContext)
        {
            if (srcCacheContext.Version >= CacheVersion.HaloOnline498295)
                if (Enum.TryParse<GameObjectTypeHalo3ODST>(objectType.HaloOnline.ToString(), out var result))
                    objectType.Halo3ODST = result;

            return objectType;
        }

        private ScenarioObjectType ConvertScenarioObjectType(ScenarioObjectType objectType, GameCacheContext srcCacheContext, GameCacheContext destCacheContext)
        {
            if (srcCacheContext.Version >= CacheVersion.HaloOnline498295)
                if (Enum.TryParse<GameObjectTypeHalo3ODST>(objectType.HaloOnline.ToString(), out var result))
                    objectType.Halo3ODST = result;

            return objectType;
        }

        private void FixScenario(Scenario data)
        {
            FixSandboxMenu(data.SandboxVehicles);
            FixSandboxMenu(data.SandboxWeapons);
            FixSandboxMenu(data.SandboxEquipment);
            FixSandboxMenu(data.SandboxScenery);
            FixSandboxMenu(data.SandboxTeleporters);
            FixSandboxMenu(data.SandboxGoalObjects);
            FixSandboxMenu(data.SandboxSpawning);
            FixScripts(data);
        }

        private void FixSandboxMenu(List<Scenario.SandboxObject> menu)
        {
            for (var i = 0; i < menu.Count; i++)
            {
                if (menu[i].Object == null || !menu[i].Object.IsInGroup("obje"))
                    menu.RemoveAt(i--);
            }
        }

        private void FixScripts(Scenario data)
        {
            foreach (var expr in data.ScriptExpressions)
            {
                if (expr.ExpressionType == ScriptExpressionType.Group || (expr.ExpressionType == ScriptExpressionType.Expression && expr.ValueType.HaloOnline == ScriptValueType.HaloOnlineValue.FunctionName))
                {
                    // Either a function call or a function_name
                    expr.Opcode = FixOpcode(expr.Opcode);
                }
            }
        }

        private ushort FixOpcode(ushort opcode)
        {
            // ZBT -> 1.106708
            // thanks zedd <3
            if (opcode >= 0x685)
                opcode -= 1;
            if (opcode >= 0x5D2)
                opcode += 2;
            if (opcode >= 0x5C6)
                opcode += 3;
            if (opcode >= 0x5AE)
                opcode += 3;
            if (opcode >= 0x527)
                opcode += 4;
            if (opcode >= 0x516)
                opcode += 1;
            if (opcode >= 0x48C)
                opcode += 2;
            if (opcode >= 0x345)
                opcode -= 1;
            if (opcode >= 0x2F2)
                opcode -= 3;
            if (opcode >= 0x15)
                opcode -= 1;
            return opcode;
        }

        private void FixShaders(object data)
        {
            if (CacheContext.Version <= CacheVersion.HaloOnline235640)
                return;

            if (data is RenderMethodTemplate template)
                FixRenderMethodTemplate(template);
            if (data is RenderMethodDefinition rmdf)
                FixRenderMethodDefinition(rmdf);
            if (data is GlobalPixelShader glps)
                FixGlobalPixelShader(glps);
            if (data is GlobalVertexShader glvs)
                FixGlobalVertexShader(glvs);
            if (data is PixelShader ps)
                FixPixelShader(ps);
            if (data is VertexShader vs)
                FixVertexShader(vs);
            if (data is RenderMethod.ShaderProperty property)
                FixDrawModeList(property.DrawModes);
        }

        private void FixRenderMethodTemplate(RenderMethodTemplate template)
        {
            FixDrawModeList(template.DrawModes);
            if (template.DrawModes.Count > 18)
                template.DrawModes[18].PixelShaderMode = RenderMethodTemplate.ShaderMode.Default; // Use default z-only

            // Rebuild the bitmask of valid draw modes
            template.DrawModeBitmask = 0;
            for (var i = 0; i < template.DrawModes.Count; i++)
            {
                if (template.DrawModes[i].PixelShaderMode != RenderMethodTemplate.ShaderMode.Default)
                    template.DrawModeBitmask |= (RenderMethodTemplate.ShaderModeBitmask)(1 << i);
            }
        }

        private void FixRenderMethodDefinition(RenderMethodDefinition definition)
        {
            for (var i = definition.DrawModes.Count - 1; i >= 0; i--)
            {
                var mode = definition.DrawModes[i];
                if (mode.Mode == 2 || mode.Mode == 10 || mode.Mode == 11 || mode.Mode == 12)
                    definition.DrawModes.RemoveAt(i);
                else if (mode.Mode > 12)
                    mode.Mode -= 4;
                else if (mode.Mode > 2)
                    mode.Mode -= 1;
            }
        }

        private void FixGlobalPixelShader(GlobalPixelShader glps)
        {
            FixDrawModeList(glps.DrawModes);
            // glps tags don't appear to need recompilation?
        }

        private void FixGlobalVertexShader(GlobalVertexShader glvs)
        {
            var usedShaders = new bool[glvs.Shaders.Count];
            for (var i = 0; i < glvs.VertexTypes.Count; i++)
            {
                FixDrawModeList(glvs.VertexTypes[i].DrawModes);
                if (glvs.VertexTypes[i].DrawModes.Count > 18)
                    glvs.VertexTypes[i].DrawModes[18].ShaderIndex = -1; // Disable z_only
                var type = (VertexType)i;
                for (var j = 0; j < glvs.VertexTypes[i].DrawModes.Count; j++)
                {
                    var mode = glvs.VertexTypes[i].DrawModes[j];
                    if (mode.ShaderIndex < 0)
                        continue;
                    Console.WriteLine("- Recompiling vertex shader {0}...", mode.ShaderIndex);
                    var shader = glvs.Shaders[mode.ShaderIndex];
                    var newBytecode = ShaderConverter.ConvertNewVertexShaderToOld(shader.PCShaderBytecode, j, type);
                    if (newBytecode != null)
                        shader.PCShaderBytecode = newBytecode;
                    usedShaders[mode.ShaderIndex] = true;
                }
            }

            // Null unused shaders
            for (var i = 0; i < glvs.Shaders.Count; i++)
            {
                if (!usedShaders[i])
                    glvs.Shaders[i].PCShaderBytecode = null;
            }
        }

        private void FixPixelShader(PixelShader ps)
        {
            FixDrawModeList(ps.DrawModes);

            // Disable z_only
            if (ps.DrawModes.Count > 18)
            {
                ps.DrawModes[18].Index = 0;
                ps.DrawModes[18].Count = 0;
            }

            var usedShaders = new bool[ps.Shaders.Count];
            for (var i = 0; i < ps.DrawModes.Count; i++)
            {
                var mode = ps.DrawModes[i];
                for (var j = 0; j < mode.Count; j++)
                {
                    if (i != 0 || IsDecalShader)
                    {
                        Console.WriteLine("- Recompiling pixel shader {0}...", mode.Index + j);
                        var shader = ps.Shaders[mode.Index + j];
                        var newBytecode = ShaderConverter.ConvertNewPixelShaderToOld(shader.PCShaderBytecode, i);
                        if (newBytecode != null)
                            shader.PCShaderBytecode = newBytecode;
                    }
                    usedShaders[mode.Index + j] = true;
                }
            }

            // Null unused shaders
            for (var i = 0; i < ps.Shaders.Count; i++)
            {
                if (!usedShaders[i])
                    ps.Shaders[i].PCShaderBytecode = null;
            }
        }

        private void FixVertexShader(VertexShader vs)
        {
            foreach (var list in vs.DrawModeLists)
                FixDrawModeList(list.DrawModes);
            // We don't need to recompile these because vtsh tags will never actually be used in a ported map
        }

        private void FixDrawModeList<T>(IList<T> modes)
        {
            if (modes.Count > 12)
                modes.RemoveAt(12);
            if (modes.Count > 11)
                modes.RemoveAt(11);
            if (modes.Count > 10)
                modes.RemoveAt(10);
            if (modes.Count > 2)
                modes.RemoveAt(2);
        }
        
        private void FixDecalSystems(GameCacheContext destCacheContext, int firstNewIndex)
        {
            // decs tags need to be updated to use the old rmdf for decals,
            // because the decal planes seem to be generated by the engine and
            // therefore need to use the old vertex format.
            //
            // This could probably be done as a post-processing step in
            // ConvertStructure to avoid the extra deserialize-reserialize
            // pass, but we'd have to store the rmdf somewhere and frankly I'm
            // too lazy to do that...

            var firstDecalSystemTag = destCacheContext.TagCache.Index.FindFirstInGroup("decs");
            if (firstDecalSystemTag == null)
                return;
            using (var stream = destCacheContext.OpenTagCacheReadWrite())
            {
                var firstDecalSystemContext = new TagSerializationContext(stream, destCacheContext, firstDecalSystemTag);
                var firstDecalSystem = destCacheContext.Deserializer.Deserialize<DecalSystem>(firstDecalSystemContext);
                foreach (var decalSystemTag in destCacheContext.TagCache.Index.FindAllInGroup("decs").Where(t => t.Index >= firstNewIndex))
                {
                    TagPrinter.PrintTagShort(decalSystemTag);
                    var context = new TagSerializationContext(stream, destCacheContext, decalSystemTag);
                    var decalSystem = destCacheContext.Deserializer.Deserialize<DecalSystem>(context);
                    foreach (var system in decalSystem.DecalSystem2)
                        system.RenderMethod.BaseRenderMethod = firstDecalSystem.DecalSystem2[0].RenderMethod.BaseRenderMethod;
                    destCacheContext.Serializer.Serialize(context, decalSystem);
                }
            }
        }
    }
}