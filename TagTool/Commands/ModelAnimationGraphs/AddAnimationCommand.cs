using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Common;
using System.IO;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Tags;
using TagTool.Commands.Common;
using TagTool.Animations;
using TagTool.Tags.Resources;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Bson;
using System.Reflection.Emit;

namespace TagTool.Commands.ModelAnimationGraphs
{
    public class AddAnimationCommand : Command
    {
        private GameCache CacheContext { get; }
        private ModelAnimationGraph Animation { get; set; }
        private ModelAnimationGraph.FrameType AnimationType = ModelAnimationGraph.FrameType.Base;
        private ModelAnimationTagResource.GroupMemberMovementDataType FrameInfoType = ModelAnimationTagResource.GroupMemberMovementDataType.None;
        private bool isWorldRelative { get; set; }
        private CachedTag Jmad { get; set; }
        private bool BaseFix = false;
        private bool ScaleFix = false;

        public AddAnimationCommand(GameCache cachecontext, ModelAnimationGraph animation, CachedTag jmad)
            : base(false,

                  "AddAnimation",
                  "Add an animation to a ModelAnimationGraph tag",

                  "AddAnimation [basefix] [scalefix] <filepath>",

                  "Add an animation to a ModelAnimationGraph tag from an animation in JMA/JMM/JMO/JMR/JMW/JMZ/JMT format")
        {
            CacheContext = cachecontext;
            Animation = animation;
            Jmad = jmad;
        }

        public override object Execute(List<string> args)
        {
            //Arguments needed: <filepath>
            if (args.Count < 1 || args.Count > 3)
                return new TagToolError(CommandError.ArgCount);

            var argStack = new Stack<string>(args.AsEnumerable().Reverse());

            BaseFix = false;
            ScaleFix = false;
            while (argStack.Count > 1)
            {
                var arg = argStack.Peek();
                switch (arg.ToLower())
                {
                    case "basefix":
                        BaseFix = true;
                        argStack.Pop();
                        break;
                    case "scalefix":
                        ScaleFix = true;
                        argStack.Pop();
                        break;
                    default:
                        return new TagToolError(CommandError.ArgInvalid);
                }
            }

            List<FileInfo> fileList = new List<FileInfo>();

            string directoryarg = argStack.Pop();

            if (Directory.Exists(directoryarg))
            {
                foreach (var file in Directory.GetFiles(directoryarg))
                {
                    fileList.Add(new FileInfo(file));
                }
            }
            else if (File.Exists(directoryarg))
            {
                fileList.Add(new FileInfo(directoryarg));
            }
            else
                return new TagToolError(CommandError.FileNotFound);

            Console.WriteLine($"###Adding {fileList.Count} animation(s)...");

            foreach (var filepath in fileList)
            {
                string file_extension = filepath.Extension;

                AnimationType = ModelAnimationGraph.FrameType.Base;
                isWorldRelative = false;
                FrameInfoType = ModelAnimationTagResource.GroupMemberMovementDataType.None;

                switch (file_extension.ToUpper())
                {
                    case ".JMM":
                        break;
                    case ".JMW":
                        isWorldRelative = true;
                        break;
                    case ".JMO":
                        AnimationType = ModelAnimationGraph.FrameType.Overlay;
                        break;
                    case ".JMR":
                        AnimationType = ModelAnimationGraph.FrameType.Replacement;
                        break;
                    case ".JMA":
                        FrameInfoType = ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy;
                        break;
                    case ".JMT":
                        FrameInfoType = ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dyaw;
                        break;
                    case ".JMZ":
                        FrameInfoType = ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dz_dyaw;
                        break;
                    default:
                        new TagToolError(CommandError.CustomError, $"Filetype {file_extension.ToUpper()} not recognized!");
                        return false;
                }

                //get or create stringid for animation block name
                bool replacing = false;
                string file_name = Path.GetFileNameWithoutExtension(filepath.FullName).Replace(' ', ':');
                StringId animation_name = CacheContext.StringTable.GetStringId(file_name);

                int existingIndex = -1;
                if (animation_name == StringId.Invalid)
                    animation_name = CacheContext.StringTable.AddString(file_name);
                else
                {
                    existingIndex = Animation.Animations.FindIndex(n => n.Name == animation_name);
                    if(existingIndex != -1)
                        replacing = true;
                }                                

                //create new importer class and import the source file
                var importer = new AnimationImporter();
                importer.ScaleFix = ScaleFix;
                if (!importer.Import(filepath.FullName))
                    continue;

                if(importer.Version >= 16394)
                {
                    string errormessage = "Only Halo:CE animation files are currently supported because newer versions offer no benefits but add node-space complications. " + 
                        "Please export your animations to Halo:CE format (JMA Version < 16394) and try importing again.";
                    return new TagToolError(CommandError.CustomError, errormessage);
                }

                //fixup Base node position/rotation/scale
                if (BaseFix)
                    FixupBaseNode(importer);

                //Adjust imported nodes to ensure that they align with the jmad
                AdjustImportedNodes(importer);

                //process node data in advance of serialization
                importer.ProcessNodeFrames((GameCacheHaloOnlineBase)CacheContext, AnimationType, FrameInfoType);

                //Check the nodes to verify that this animation can be imported to this jmad
                //if (!importer.CompareNodes(Animation.SkeletonNodes, (GameCacheHaloOnlineBase)CacheContext))
                //    return false;

                //serialize animation block values
                var AnimationBlock = new ModelAnimationGraph.Animation
                {
                    Name = animation_name,
                    AnimationData = new ModelAnimationGraph.Animation.SharedAnimationData
                    {
                        AnimationType = AnimationType,
                        FrameInfoType = (FrameInfoType)FrameInfoType,
                        BlendScreen = -1,
                        DesiredCompression = ModelAnimationGraph.Animation.CompressionValue.BestAccuracy,
                        CurrentCompression = ModelAnimationGraph.Animation.CompressionValue.BestAccuracy,
                        FrameCount = (short)importer.frameCount,
                        NodeCount = (sbyte)importer.AnimationNodes.Count,
                        NodeListChecksum = 0,
                        ImporterVersion = 5,
                        CompressorVersion = 6,
                        Heading = new RealVector3d(1, 0, 0),
                        ParentAnimation = -1,
                        NextAnimation = -1,
                        ResourceGroupIndex = (short)(Animation.ResourceGroups.Count - 1),
                        ResourceGroupMemberIndex = 0,
                    }
                };

                if (isWorldRelative)
                    AnimationBlock.AnimationData.InternalFlags |= ModelAnimationGraph.Animation.InternalFlagsValue.WorldRelative;

                //build a new resource 
                ModelAnimationTagResource newResource = new ModelAnimationTagResource
                {
                    GroupMembers = new TagTool.Tags.TagBlock<ModelAnimationTagResource.GroupMember>()
                };
                newResource.GroupMembers.Add(importer.SerializeAnimationData((GameCacheHaloOnlineBase)CacheContext));
                newResource.GroupMembers.AddressType = CacheAddressType.Definition;
                //serialize the new resource into the cache
                TagResourceReference resourceref = CacheContext.ResourceCache.CreateModelAnimationGraphResource(newResource);

                //add resource reference to the animation tag
                Animation.ResourceGroups.Add(new ModelAnimationGraph.ResourceGroup
                {
                    ResourceReference = resourceref,
                    MemberCount = 1
                });

                AnimationBlock.AnimationData.ResourceGroupIndex = (short)(Animation.ResourceGroups.Count - 1);
                AnimationBlock.AnimationData.ResourceGroupMemberIndex = 0;

                if (replacing)
                {
                    Animation.Animations[existingIndex] = AnimationBlock;
                }
                else
                {
                    Animation.Animations.Add(AnimationBlock);
                    existingIndex = Animation.Animations.Count - 1;
                }

                AddModeEntries(file_name, existingIndex);
                
                if(replacing)
                    Console.WriteLine($"Replaced {file_name} successfully!");
                else
                    Console.WriteLine($"Added {file_name} successfully!");
            }
            //save changes to the current tag
            CacheContext.SaveStrings();
            using (Stream cachestream = CacheContext.OpenCacheReadWrite())
            {
                CacheContext.Serialize(cachestream, Jmad, Animation);
            }

            Console.WriteLine("Done!");
            return true;
        }

        public void AddModeEntries(string filename, int animationIndex)
        {
            List<string> tokens = filename.Split(':').ToList();
            List<string> unsupportedTokens = 
                new List<string> { "s_kill", "s_ping", "h_kill", "h_ping", "sync_actions", "suspension", "tread", "object", "2", "device"};
            if(tokens.Any(t => unsupportedTokens.Contains(t)))
            {
                new TagToolWarning($"Unsupported string token found in filename {filename}. Manual mode addition is required.");
                return;
            }

            //variants are all handled by the same mode entry
            if(tokens.Last().StartsWith("var"))
                tokens.RemoveAt(tokens.Count - 1);

            //fixups for specific mode tokens
            List<string> edgeCases = new List<string> { "vehicle", "first_person", "weapon"};
            if (edgeCases.Contains(tokens[0]))
                tokens[0] = "any";

            string modeString = "";
            string classString = "";
            string typeString = "";
            string actionString = "";

            switch (tokens.Count)
            {
                case 1:
                    modeString = "any";
                    classString = "any";
                    typeString = "any";
                    actionString = tokens[0];
                    break;
                case 2:
                    modeString = tokens[0];
                    classString = "any";
                    typeString = "any";
                    actionString = tokens[1];
                    break;
                case 3:
                    modeString = tokens[0];
                    classString = tokens[1];
                    typeString = "any";
                    actionString = tokens[2];
                    break;
                case 4:
                    modeString = tokens[0];
                    classString = tokens[1];
                    typeString = tokens[2];
                    actionString = tokens[3];
                    break;
            }

            int modeIndex = AddMode(modeString);
            int classIndex = AddClass(modeIndex, classString);
            int typeIndex = AddType(modeIndex, classIndex, typeString);
            AddAction(modeIndex, classIndex, typeIndex, actionString, animationIndex);

            Console.WriteLine($"Successfully added mode entries for '{modeString}:{classString}:{typeString}:{actionString}'!");
        }

        public int AddMode(string modeString)
        {
            StringId modeStringID = CacheContext.StringTable.GetStringId(modeString);
            int modesIndex = Animation.Modes.FindIndex(m => m.Name == modeStringID);
            if (modesIndex != -1)
                return modesIndex;
            else
            {
                Animation.Modes.Add(new ModelAnimationGraph.Mode
                {
                    Name = modeStringID,
                    WeaponClass = new List<ModelAnimationGraph.Mode.WeaponClassBlock>()
                });
                return Animation.Modes.Count - 1;
            }
        }

        public int AddClass(int modeIndex, string classString)
        {
            StringId classStringID = CacheContext.StringTable.GetStringId(classString);
            int classIndex = Animation.Modes[modeIndex].WeaponClass.FindIndex(m => m.Label == classStringID);
            if (classIndex != -1)
                return classIndex;
            else
            {
                Animation.Modes[modeIndex].WeaponClass.Add(new ModelAnimationGraph.Mode.WeaponClassBlock
                {
                    Label = classStringID,
                    WeaponType = new List<ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock>()
                });
                return Animation.Modes[modeIndex].WeaponClass.Count - 1;
            }
        }

        public int AddType(int modeIndex, int classIndex, string typeString)
        {
            StringId typeStringID = CacheContext.StringTable.GetStringId(typeString);
            int typeIndex = Animation.Modes[modeIndex].WeaponClass[classIndex].WeaponType.
                FindIndex(m => m.Label == typeStringID);
            if (typeIndex != -1)
                return typeIndex;
            else
            {
                Animation.Modes[modeIndex].WeaponClass[classIndex].WeaponType.Add(new ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock
                {
                    Label = typeStringID,
                    Set = new ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.AnimationSet
                    {
                        Actions = new List<ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry>(),
                        Overlays = new List<ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry>()
                    }
                });
                return Animation.Modes[modeIndex].WeaponClass[classIndex].WeaponType.Count - 1;
            }
        }

        public void AddAction(int modeIndex, int classIndex, int typeIndex, string actionString, int animationIndex)
        {
            StringId actionStringID = CacheContext.StringTable.GetStringId(actionString);
            var set = Animation.Modes[modeIndex].WeaponClass[classIndex].WeaponType[typeIndex].Set;
            var newAction = new ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry
            {
                Label = actionStringID,
                GraphIndex = -1,
                Animation = (short)animationIndex
            };
            if (AnimationType == ModelAnimationGraph.FrameType.Base) //for base actions
            {
                int actionIndex = set.Actions.FindIndex(m => m.Label == actionStringID);
                if (actionIndex != -1)
                    set.Actions[actionIndex] = newAction;
                else
                    set.Actions.Add(newAction);
            }
            else //handles overlays and replacements
            {
                int actionIndex = set.Overlays.FindIndex(m => m.Label == actionStringID);
                if (actionIndex != -1)
                    set.Overlays[actionIndex] = newAction;
                else
                    set.Overlays.Add(newAction);
            }           
        }
        public void AdjustImportedNodes(AnimationImporter importer)
        {
            //now order imported nodes according to jmad nodes
            List<AnimationImporter.AnimationNode> newAnimationNodes = new List<AnimationImporter.AnimationNode>();
            foreach (var skellynode in Animation.SkeletonNodes)
            {
                string nodeName = CacheContext.StringTable.GetString(skellynode.Name);
                int matching_index = importer.AnimationNodes.FindIndex(x => x.Name.Equals(nodeName));
                if (matching_index == -1)
                {
                    new TagToolWarning($"No node matching '{nodeName}' found in imported file! Will proceed with blank data for missing node");
                    newAnimationNodes.Add(new AnimationImporter.AnimationNode() { Name = nodeName, FirstChildNode = skellynode.FirstChildNodeIndex, NextSiblingNode = skellynode.NextSiblingNodeIndex, ParentNode = skellynode.ParentNodeIndex });
                }
                else
                {
                    AnimationImporter.AnimationNode matching_node = importer.AnimationNodes[matching_index];
                    matching_node.FirstChildNode = skellynode.FirstChildNodeIndex;
                    matching_node.NextSiblingNode = skellynode.NextSiblingNodeIndex;
                    matching_node.ParentNode = skellynode.ParentNodeIndex;
                    newAnimationNodes.Add(matching_node);
                }
            }

            //set importer animation nodes to newly sorted list
            importer.AnimationNodes = newAnimationNodes;
        }

        public void FixupBaseNode(AnimationImporter importer)
        {
            var imported_nodes = importer.AnimationNodes;

            int basenode_index = imported_nodes.FindIndex(x => x.Name.Equals("base"));
            if (basenode_index != -1)
            {
                //fixup base node
                foreach (var Frame in imported_nodes[basenode_index].Frames)
                {
                    Frame.Rotation = new RealQuaternion(0, 0, 0, 1);
                    Frame.Translation = new RealPoint3d(0, 0, 0);
                    Frame.Scale = 1.0f;
                }
            }
        }
    }
}
