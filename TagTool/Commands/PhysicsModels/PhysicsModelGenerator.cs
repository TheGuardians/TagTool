using TagTool.Geometry;
using System;
using System.Linq;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.IO;
using TagTool.Geometry.Jms;

namespace TagTool.Commands.PhysicsModels
{
    public class PhysicsModelGenerator
    {
        private readonly GameCache Cache;

        public PhysicsModelGenerator(GameCache cache)
        {
            Cache = cache;
        }

        public object GeneratePhysicsModel(Stream stream, string path, string tagname, bool moppflag, Stream cacheStream)
        {
            bool createNew = true;
            CachedTag tag;

            // Process tagname
            if (!tagname.EndsWith(".physics_model") && tagname.Contains("."))
                tagname = tagname.Substring(0, tagname.IndexOf('.')) + ".physics_model";
            else if (!tagname.EndsWith(".physics_model"))
                tagname += ".physics_model";

            // Check for existing tag
            if (Cache.TagCache.TryGetCachedTag(tagname, out tag))
                createNew = false;

            // Model building logic
            var modelbuilder = new PhysicsModelBuilder();
            if (!modelbuilder.ParseFromFile(path, moppflag, null, (GameCacheHaloOnlineBase)Cache, cacheStream, null))
                return new TagToolError(CommandError.FileIO, "Failed to parse the specified file");

            var phmo = modelbuilder.Build();
            if (phmo == null)
                return new TagToolError(CommandError.OperationFailed, "The built physics model was null!");

            // Serialize or allocate tag
            if (createNew)
            {
                tag = Cache.TagCache.AllocateTag(Cache.TagCache.TagDefinitions.GetTagGroupFromTag(new Tag("phmo")), tagname.Substring(0, tagname.IndexOf('.')));
                if (tag == null)
                {
                    return new TagToolError(CommandError.CustomError, "Tag allocation failed!");
                }
            }

            Cache.Serialize(stream, tag, phmo);
            ApplyGeneralFixesAndDefaults(stream, tag, null);

            return tag;
        }

        public object GeneratePhysicsModelFromJms(Stream stream, JmsFormat jms, string tagname, bool moppflag, CollisionModel weaponCollisionModelInstance, Stream cacheStream)
        {
            // Serialize or allocate tag as before
            bool createNew = true;
            CachedTag tag;

            // Process tagname
            if (!tagname.EndsWith(".physics_model") && tagname.Contains("."))
                tagname = tagname.Substring(0, tagname.IndexOf('.')) + ".physics_model";
            else if (!tagname.EndsWith(".physics_model"))
                tagname += ".physics_model";

            // Check for existing tag
            if (Cache.TagCache.TryGetCachedTag(tagname, out tag))
                createNew = false;

            // Model building logic
            var modelbuilder = new PhysicsModelBuilder();
            if (!modelbuilder.ConvertJmsToJson(jms, (GameCacheHaloOnlineBase)Cache, cacheStream))
                return new TagToolError(CommandError.FileIO, "Failed to parse the specified jms");

            var phmo = modelbuilder.Build();
            if (phmo == null)
                return new TagToolError(CommandError.OperationFailed, "The built physics model was null!");

            // Serialize or allocate tag
            if (createNew)
            {
                tag = Cache.TagCache.AllocateTag(Cache.TagCache.TagDefinitions.GetTagGroupFromTag(new Tag("phmo")), tagname.Substring(0, tagname.IndexOf('.')));
                if (tag == null)
                {
                    return new TagToolError(CommandError.CustomError, "Tag allocation failed!");
                }
            }

            Cache.Serialize(stream, tag, phmo);
            ApplyGeneralFixesAndDefaults(stream, tag, weaponCollisionModelInstance);

            return tag;
        }

        private void ApplyGeneralFixesAndDefaults(Stream stream, CachedTag tag, CollisionModel collDef)
        {
            var phmoDef = Cache.Deserialize<PhysicsModel>(stream, tag);
            if (collDef != null)
            {
                phmoDef.Nodes.Clear();
                foreach (var nodes in collDef.Nodes)
                {
                    string nodeName = Cache.StringTable.GetString(nodes.Name);
                    PhysicsModel.Node phmoNodes = new PhysicsModel.Node
                    {
                        Name = Cache.StringTable.GetStringId(nodeName),
                        Parent = nodes.ParentNode,
                        Sibling = nodes.NextSiblingNode,
                        Child = nodes.FirstChildNode
                    };
                    phmoDef.Nodes.Add(phmoNodes);
                }
            }

            Cache.Serialize(stream, tag, phmoDef);
        }
    }
}