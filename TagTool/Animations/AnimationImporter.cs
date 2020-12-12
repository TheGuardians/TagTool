using System;
using System.IO;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Cache;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.Tags.Resources;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using System.Numerics;
using Assimp;

namespace TagTool.Animations
{
    public class AnimationImporter
    {
        public List<AnimationNode> AnimationNodes;
        public int frameCount;
        public uint nodeChecksum;
        public double framerate = 30;
        public int rotatedNodeCount;
        public int translatedNodeCount;
        public int scaledNodeCount;
        public int staticRotatedNodeCount;
        public int staticTranslatedNodeCount;
        public int staticScaledNodeCount;
        public bool buildstaticdata = false;

        public bool AssimpImport(string fileName)
        {
            //import the mesh and get the vertices and indices
            Scene model;
            using (var importer = new AssimpContext())
            {
                using (var logStream = new LogStream((msg, userData) => Console.Write(msg)))
                {
                    logStream.Attach();
                    model = importer.ImportFile(fileName);
                    logStream.Detach();
                }
            }

            if (model.HasAnimations && model.Animations[0].HasNodeAnimations)
            {
                foreach(var modelnode in model.Animations[0].NodeAnimationChannels)
                {
                    List<AnimationFrame> NodeFrames = new List<AnimationFrame>();
                }
            }
            else
            {
                Console.WriteLine("###ERROR: Model has no animations!");
                return false;
            }
            return true;
        }

        public void Import(string fileName)
        {
            using (FileStream textStream = (FileStream)File.OpenRead(fileName))
            {
                StreamReader textReader = new StreamReader(textStream);
                int Version = 0;

                //this garbage is because some H2V JMA exporters use UCS-2 LE text encoding, which crashes the streamreader
                try
                {
                    Version = int.Parse(textReader.ReadLine());
                }
                catch
                {
                    textStream.Position = 0;
                    textReader = new StreamReader(textStream, Encoding.Unicode, true);
                    Version = int.Parse(textReader.ReadLine()); //version
                }

                if (Version >= 16394)
                    nodeChecksum = uint.Parse(textReader.ReadLine()); //version part 2
                frameCount = int.Parse(textReader.ReadLine());
                framerate = double.Parse(textReader.ReadLine()); //framerate
                textReader.ReadLine(); //actor count
                textReader.ReadLine(); //actor name
                int nodecount = int.Parse(textReader.ReadLine());
                if (Version < 16394)
                    nodeChecksum = uint.Parse(textReader.ReadLine());
                AnimationNodes = new List<AnimationNode>();
                for (int i = 0; i < nodecount; i++)
                {
                    var newnode = new AnimationNode();
                    newnode.Name = textReader.ReadLine();
                    if (Version == 16392 || Version == 16393)
                    {
                        newnode.FirstChildNode = short.Parse(textReader.ReadLine());
                        newnode.NextSiblingNode = short.Parse(textReader.ReadLine());
                        newnode.ParentNode = -1;
                    }
                    else if (Version >= 16394)
                    {
                        newnode.ParentNode = short.Parse(textReader.ReadLine());
                        newnode.FirstChildNode = -1;
                        newnode.NextSiblingNode = -1;
                    }
                    AnimationNodes.Add(newnode);
                }

                //add first child and next sibling nodes for newer animation formats, or parent nodes for old formats
                FixupNodeTree(Version);

                //populate node frames values
                for (int frame_index = 0; frame_index < frameCount; frame_index++)
                {
                    for (int node_index = 0; node_index < nodecount; node_index++)
                    {
                        var translation = textReader.ReadLine().Split('\t');
                        var rotation = textReader.ReadLine().Split('\t');
                        var scale = textReader.ReadLine();

                        //Conjugate the quaternion during import, this is what tool.exe does
                        var newRotation = new System.Numerics.Quaternion((float)double.Parse(rotation[0]), (float)double.Parse(rotation[1]), (float)double.Parse(rotation[2]), (float)double.Parse(rotation[3]));

                        //rotations are conjugated for early versions of the JMA format
                        if(Version < 16394)
                            newRotation = System.Numerics.Quaternion.Conjugate(newRotation);

                        AnimationNodes[node_index].Frames.Add(new AnimationFrame
                        {
                            Rotation = new RealQuaternion(newRotation.X, newRotation.Y, newRotation.Z, newRotation.W),
                            Translation = new RealPoint3d((float)double.Parse(translation[0]) * 0.01f, (float)double.Parse(translation[1]) * 0.01f, (float)double.Parse(translation[2]) * 0.01f),
                            Scale = (float)double.Parse(scale)
                        });                        
                    }
                }
            }
        }

        public bool CompareTranslations(AnimationFrame Frame1, AnimationFrame Frame2)
        {
            if(Math.Abs(Frame2.Translation.X - Frame1.Translation.X) >= 0.00009999999747378752 || 
               Math.Abs(Frame2.Translation.Y - Frame1.Translation.Y) >= 0.00009999999747378752 ||
               Math.Abs(Frame2.Translation.Z - Frame1.Translation.Z) >= 0.00009999999747378752)
                return false;
            return true;
        }

        public bool CompareRotations(AnimationFrame Frame1, AnimationFrame Frame2)
        {
            if (Math.Abs(Frame2.Rotation.I - Frame1.Rotation.I) >= 0.00009999999747378752 ||
                Math.Abs(Frame2.Rotation.J - Frame1.Rotation.J) >= 0.00009999999747378752 ||
                Math.Abs(Frame2.Rotation.K - Frame1.Rotation.K) >= 0.00009999999747378752 ||
                Math.Abs(Frame2.Rotation.W - Frame1.Rotation.W) >= 0.00009999999747378752)
                return false;
            return true;
        }

        public void ProcessNodeFrames(GameCacheHaloOnlineBase CacheContext, List<string> ModelList, ModelAnimationGraph.FrameType AnimationType)
        {
            //if the animation is of the overlay type, remove the base frame and subtract it from all other frames
            if (AnimationType == ModelAnimationGraph.FrameType.Overlay)
                RemoveOverlayBase();

            //check to see if each node frame is different from last one, to see if node is used dynamically
            for (int node_index = 0; node_index < AnimationNodes.Count; node_index++)
            {
                if (AnimationNodes[node_index].Frames.Count < 1)
                    continue;

                for (int frame_index = 0; frame_index < frameCount; frame_index++)
                {
                    var currentnode = AnimationNodes[node_index];

                    if (!CompareRotations(currentnode.Frames[frame_index], currentnode.Frames[0]) && !currentnode.hasAnimatedRotation)
                    {
                        currentnode.hasAnimatedRotation = true;
                    }
                    if (!CompareTranslations(currentnode.Frames[frame_index], currentnode.Frames[0]) && !currentnode.hasAnimatedTranslation)
                    {
                        currentnode.hasAnimatedTranslation = true;
                    }
                    if (Math.Abs(currentnode.Frames[frame_index].Scale - currentnode.Frames[0].Scale) >= 0.00009999999747378752 && !currentnode.hasAnimatedScale)
                    {
                        currentnode.hasAnimatedScale = true;
                    }
                }
            }

            //Get node default positions from mode tag
            SetDefaultNodePositions(CacheContext, ModelList);

            //setup static nodes
            for (int node_index = 0; node_index < AnimationNodes.Count; node_index++)
            {
                if (AnimationNodes[node_index].Frames.Count < 1)
                    continue;

                var currentnode = AnimationNodes[node_index];

                var DefaultPositionFrame = new AnimationFrame
                {
                    Rotation = currentnode.DefaultRotation,
                    Translation = currentnode.DefaultTranslation,
                    Scale = currentnode.DefaultScale
                };

                if (!CompareRotations(currentnode.Frames[0], DefaultPositionFrame) && !currentnode.hasAnimatedRotation)
                {
                    currentnode.hasStaticRotation = true;
                }
                if (!CompareTranslations(currentnode.Frames[0], DefaultPositionFrame) && !currentnode.hasAnimatedTranslation)
                {
                    currentnode.hasStaticTranslation = true;
                }
                if (Math.Abs(currentnode.Frames[0].Scale - currentnode.DefaultScale) >= 0.00009999999747378752 && !currentnode.hasAnimatedScale)
                {
                    currentnode.hasStaticScale = true;
                }
            }
        }

        public ModelAnimationTagResource.GroupMember SerializeAnimationData(GameCacheHaloOnlineBase CacheContext)
        {
            var groupmember = new ModelAnimationTagResource.GroupMember
            {
                Checksum = (int)nodeChecksum,
                MovementDataType = ModelAnimationTagResource.GroupMemberMovementDataType.None,
                FrameCount = (short)frameCount,
                NodeCount = (byte)AnimationNodes.Count,
                PackedDataSizes = new ModelAnimationTagResource.GroupMember.PackedDataSizesStructBlock()
            };

            //check if there is any static data that we need to build, count nodes of each type
            foreach (var currentnode in AnimationNodes)
            {
                if (currentnode.hasStaticRotation || currentnode.hasStaticScale || currentnode.hasStaticTranslation)
                    buildstaticdata = true;
                if (currentnode.hasStaticRotation)
                    staticRotatedNodeCount++;
                if (currentnode.hasStaticTranslation)
                    staticTranslatedNodeCount++;
                if (currentnode.hasStaticScale)
                    staticScaledNodeCount++;

                if (currentnode.hasAnimatedRotation)
                    rotatedNodeCount++;
                if (currentnode.hasAnimatedTranslation)
                    translatedNodeCount++;
                if (currentnode.hasAnimatedScale)
                    scaledNodeCount++;
            }

            using (MemoryStream stream = new MemoryStream())
            using(EndianWriter writer = new EndianWriter(stream, EndianFormat.LittleEndian))
            {
                var dataContext = new DataSerializationContext(writer);

                //serialize the static data first if needed
                if(buildstaticdata)
                    groupmember.PackedDataSizes.StaticDataSize = (short)SerializeStaticData(CacheContext, dataContext, stream);

                var datastartoffset = stream.Position.DeepClone();

                var codecheader = new AnimationCodecHeader
                {
                    Type = AnimationCodecType._8ByteQuantizedRotationOnly,
                    RotationCount = (sbyte)rotatedNodeCount,
                    TranslationCount = (sbyte)translatedNodeCount,
                    ScaleCount = (sbyte)scaledNodeCount,
                    PlaybackRate = (float)(framerate / 60.0d)
                };
                CacheContext.Serializer.Serialize(dataContext, codecheader);

                var staticcodecheaderoffset = stream.Position.DeepClone();
                //write an empty static codec header for now, will return to this later 
                CacheContext.Serializer.Serialize(dataContext, new StaticCodecHeader());

                //write rotation frame data
                WriteRotationFrameData(CacheContext, dataContext);

                //record translation data offset to write to header later on
                var translationdataoffset = stream.Position.DeepClone();
                //write translation frame data
                WriteTranslationFrameData(CacheContext, dataContext);

                //record scale data offset to write to header later on
                var scaledataoffset = stream.Position.DeepClone();
                //write scale frame data
                WriteScaleFrameData(CacheContext, dataContext);

                //record end of data offset
                var dataendoffset = stream.Position.DeepClone();
                //move back to header to write data sizes
                stream.Position = staticcodecheaderoffset;
                var staticcodecsizes = new StaticCodecHeader
                {
                    TranslationDataOffset = (int)translationdataoffset - (int)datastartoffset,
                    ScaleDataOffset = (int)scaledataoffset - (int)datastartoffset,
                    RotationFrameSize = 0x8 * frameCount, //four shorts times number of frames in animation
                    TranslationFrameSize = 0xC * frameCount, //three floats times number of frames in animation
                    ScaleFrameSize = 0x4 * frameCount //one float times number of frames in animation
                };
                CacheContext.Serializer.Serialize(dataContext, staticcodecsizes);

                //return to the end of the stream and record size
                stream.Position = stream.Length;
                groupmember.PackedDataSizes.CompressedDataSize = (int)stream.Length - (int)datastartoffset;

                if (buildstaticdata)
                {
                    List<int> StaticFlags = BuildFlags(true);
                    groupmember.PackedDataSizes.StaticNodeFlags = (byte)(0x4 * StaticFlags.Count);
                    foreach (var flagset in StaticFlags)
                        dataContext.Writer.Write(flagset);
                }

                List<int> AnimationFlags = BuildFlags(false);
                groupmember.PackedDataSizes.AnimatedNodeFlags = (byte)(0x4 * AnimationFlags.Count);
                foreach (var flagset in AnimationFlags)
                    dataContext.Writer.Write(flagset);

                groupmember.AnimationData = new TagData(stream.ToArray());
            }

            return groupmember;
        }

        public int SerializeStaticData(GameCacheHaloOnlineBase CacheContext, DataSerializationContext dataContext, MemoryStream stream)
        {
            var staticcodecheader = new AnimationCodecHeader
            {
                Type = AnimationCodecType.UncompressedStatic,
                RotationCount = (sbyte)(staticRotatedNodeCount),
                TranslationCount = (sbyte)(staticTranslatedNodeCount),
                ScaleCount = (sbyte)(staticScaledNodeCount),
                PlaybackRate = 0.0f //this is 0 for static data
            };
            CacheContext.Serializer.Serialize(dataContext, staticcodecheader);

            var staticcodecheaderoffset = stream.Position.DeepClone();
            //write an empty static codec header for now, will return to this later 
            CacheContext.Serializer.Serialize(dataContext, new StaticCodecHeader());

            var datastartoffset = stream.Position.DeepClone();
            //write rotation frame data
            foreach(var currentnode in AnimationNodes)
            {
                if (currentnode.hasStaticRotation)
                {
                    var rotation = new ModelAnimationTagResource.GroupMember.RotationFrame
                    {
                        X = (short)(currentnode.Frames[0].Rotation.I * short.MaxValue),
                        Y = (short)(currentnode.Frames[0].Rotation.J * short.MaxValue),
                        Z = (short)(currentnode.Frames[0].Rotation.K * short.MaxValue),
                        W = (short)(currentnode.Frames[0].Rotation.W * short.MaxValue)
                    };
                    CacheContext.Serializer.Serialize(dataContext, rotation);
                }
            }

            //record translation data offset to write to header later on
            var translationdataoffset = stream.Position.DeepClone();
            //write translation frame data
            foreach(var currentnode in AnimationNodes)
            {
                if (currentnode.hasStaticTranslation)
                {
                    var translation = new TranslationFrameFloat
                    {
                        X = currentnode.Frames[0].Translation.X,
                        Y = currentnode.Frames[0].Translation.Y,
                        Z = currentnode.Frames[0].Translation.Z
                    };
                    CacheContext.Serializer.Serialize(dataContext, translation);
                }
            }

            //record scale data offset to write to header later on
            var scaledataoffset = stream.Position.DeepClone();
            //write scale frame data
            foreach (var currentnode in AnimationNodes)
            {
                if (currentnode.hasStaticScale)
                {
                    var scale = currentnode.Frames[0].Scale;
                    dataContext.Writer.Write(scale);
                }
            }

            //record end of data offset
            var dataendoffset = stream.Position.DeepClone();
            //move back to header to write data sizes
            stream.Position = staticcodecheaderoffset;

            var staticcodecsizes = new StaticCodecHeader
            {
                TranslationDataOffset = (int)translationdataoffset,
                ScaleDataOffset = (int)scaledataoffset,
                RotationFrameSize = 0, //four shorts times number of frames in animation
                TranslationFrameSize = 0, //three floats times number of frames in animation
                ScaleFrameSize = 0 //one float times number of frames in animation
            };
            CacheContext.Serializer.Serialize(dataContext, staticcodecsizes);

            //reset stream position to end of stream
            stream.Position = stream.Length;

            return (int)dataendoffset;
        }

        public void RemoveOverlayBase()
        {
            //remove base frame from frame count
            frameCount--;
            foreach (var node in AnimationNodes)
            {
                if(node.Frames.Count > 0)
                {
                    //copy and then remove unnecessary base frame
                    AnimationFrame BaseFrame = node.Frames[0].DeepClone();
                    node.Frames.RemoveAt(0);
                    //remove basis of overlay to just leave the actual overlay data
                    foreach (var frame in node.Frames)
                    {
                        //using system.numerics.quaternion here because it has a division operator
                        var temprotation = new System.Numerics.Quaternion(frame.Rotation.I, frame.Rotation.J, frame.Rotation.K, frame.Rotation.W);
                        var tempbase = new System.Numerics.Quaternion(BaseFrame.Rotation.I, BaseFrame.Rotation.J, BaseFrame.Rotation.K, BaseFrame.Rotation.W);
                        var dividend = temprotation / tempbase;
                        frame.Rotation = new RealQuaternion(dividend.X, dividend.Y, dividend.Z, dividend.W);

                        frame.Translation -= BaseFrame.Translation;

                        frame.Scale /= BaseFrame.Scale;
                    }
                }
            }
        }

        public void WriteScaleFrameData(GameCacheHaloOnlineBase CacheContext, DataSerializationContext dataContext)
        {
            for (int node_index = 0; node_index < AnimationNodes.Count; node_index++)
            {
                for (int frame_index = 0; frame_index < frameCount; frame_index++)
                {
                    if (AnimationNodes[node_index].hasAnimatedScale)
                    {
                        var scale = AnimationNodes[node_index].Frames[frame_index].Scale;
                        dataContext.Writer.Write(scale);
                    }
                }
            }
        }

        public void WriteRotationFrameData(GameCacheHaloOnlineBase CacheContext, DataSerializationContext dataContext)
        {      
            for (int node_index = 0; node_index < AnimationNodes.Count; node_index++)
            {
                for (int frame_index = 0; frame_index < frameCount; frame_index++)
                {
                    if (AnimationNodes[node_index].hasAnimatedRotation)
                    {
                        var rotation = new ModelAnimationTagResource.GroupMember.RotationFrame
                        {
                            X = (short)(AnimationNodes[node_index].Frames[frame_index].Rotation.I * short.MaxValue),
                            Y = (short)(AnimationNodes[node_index].Frames[frame_index].Rotation.J * short.MaxValue),
                            Z = (short)(AnimationNodes[node_index].Frames[frame_index].Rotation.K * short.MaxValue),
                            W = (short)(AnimationNodes[node_index].Frames[frame_index].Rotation.W * short.MaxValue)
                        };
                        CacheContext.Serializer.Serialize(dataContext, rotation);
                    }
                }
            }            
        }

        public void WriteTranslationFrameData(GameCacheHaloOnlineBase CacheContext, DataSerializationContext dataContext)
        {
            for (int node_index = 0; node_index < AnimationNodes.Count; node_index++)
            {
                for (int frame_index = 0; frame_index < frameCount; frame_index++)
                {
                    if (AnimationNodes[node_index].hasAnimatedTranslation)
                    {
                        var translation = new TranslationFrameFloat
                        {
                            X = AnimationNodes[node_index].Frames[frame_index].Translation.X,
                            Y = AnimationNodes[node_index].Frames[frame_index].Translation.Y,
                            Z = AnimationNodes[node_index].Frames[frame_index].Translation.Z
                        };
                        CacheContext.Serializer.Serialize(dataContext, translation);
                    }
                }
            }
        }

        public List<RealPoint3d> HandleMovementData(ModelAnimationGraph.FrameType AnimationType, AnimationMovementDataType FrameInfoType)
        {
            List<RealPoint3d> MovementData = new List<RealPoint3d>();
            switch (FrameInfoType)
            {
                case AnimationMovementDataType.DxDy:
                    if(AnimationType != ModelAnimationGraph.FrameType.Base)
                    {
                        Console.WriteLine("###ERROR: Only base type animations can have movement data!");
                        return MovementData;
                    }
                    //extract data only from the first (root) node
                    //data collection starts at the end of the frames, moving backwards to the beginning
                    for(int frame_index = AnimationNodes[0].Frames.Count - 2; frame_index >= 0; frame_index--)
                    {
                        AnimationFrame CurrentFrame = AnimationNodes[0].Frames[frame_index];
                        AnimationFrame NextFrame = AnimationNodes[0].Frames[frame_index + 1];
                        AnimationFrame FirstFrame = AnimationNodes[0].Frames[0];

                        //don't include z axis for basic movement data
                        RealPoint3d MovementFrame = new RealPoint3d(NextFrame.Translation.X - CurrentFrame.Translation.X,
                            NextFrame.Translation.Y - CurrentFrame.Translation.Y,0);

                        //set 'nextframe' translation to be equivalent to that of the first frame
                        AnimationNodes[0].Frames[frame_index + 1].Translation = FirstFrame.Translation;

                        //since we are moving backwards, insert the movementframe at the beginning of the list
                        MovementData.Insert(0, MovementFrame);
                    }
                    return MovementData;
                default:
                    return MovementData;
            }
        }

        public List<int> BuildFlags(bool isStaticFlags)
        {
            int flagcount = (int)Math.Ceiling(AnimationNodes.Count / 32.0f);

            List<int> rotationflags = new List<int>(new int[flagcount]);
            List<int> translationflags = new List<int>(new int[flagcount]);
            List<int> scaleflags = new List<int>(new int[flagcount]);

            for (int nodeindex = 0; nodeindex < AnimationNodes.Count; nodeindex++)
            {
                int flagset = (int)Math.Floor(nodeindex / 32.0f);
                if ((isStaticFlags && AnimationNodes[nodeindex].hasStaticRotation) || (!isStaticFlags && AnimationNodes[nodeindex].hasAnimatedRotation))
                    rotationflags[flagset] |= 1 << (nodeindex - (flagset * 32));
                if ((isStaticFlags && AnimationNodes[nodeindex].hasStaticTranslation) || (!isStaticFlags && AnimationNodes[nodeindex].hasAnimatedTranslation))
                    translationflags[flagset] |= 1 << (nodeindex - (flagset * 32));
                if ((isStaticFlags && AnimationNodes[nodeindex].hasStaticScale) || (!isStaticFlags && AnimationNodes[nodeindex].hasAnimatedScale))
                    scaleflags[flagset] |= 1 << (nodeindex - (flagset * 32));
            }

            //all translation flags are first serialized, then all rotation flags etc
            List<int> Flags = new List<int>();
            Flags.AddRange(rotationflags);
            Flags.AddRange(translationflags);
            Flags.AddRange(scaleflags);
            
            return Flags;
        }

        public void SetDefaultNodePositions(GameCacheHaloOnlineBase CacheContext, List<string> ModelList)
        {
            if (ModelList.Count == 0)
            {
                return;
            }

            //string tagname = @"objects\characters\masterchief\fp\fp";
            List<string> NoMatchList = new List<string>();
            
            using (var CacheStream = CacheContext.OpenCacheReadWrite())
            {
                for(var node_index = 0; node_index < AnimationNodes.Count; node_index++)
                {
                    var matching_index = -1;
                    foreach (var tagname in ModelList)
                    {
                        var mode_tag_ref = CacheContext.TagCacheGenHO.GetTag<RenderModel>(tagname);
                        var mode_tag = CacheContext.Deserialize<RenderModel>(CacheStream, mode_tag_ref);
                        var mode_nodes = mode_tag.Nodes;
                    
                        for (var mode_node_index = 0; mode_node_index < mode_nodes.Count; mode_node_index++)
                        {
                            if (CacheContext.StringTable.GetString(mode_nodes[mode_node_index].Name) == AnimationNodes[node_index].Name)
                            {
                                matching_index = mode_node_index;
                                AnimationNodes[node_index].DefaultRotation = mode_nodes[mode_node_index].DefaultRotation;
                                AnimationNodes[node_index].DefaultTranslation = mode_nodes[mode_node_index].DefaultTranslation;
                                AnimationNodes[node_index].DefaultScale = mode_nodes[mode_node_index].DefaultScale;
                                break;
                            }
                        }
                        if (matching_index != -1)
                            break;
                    }
                    if (matching_index == -1)
                        NoMatchList.Add(AnimationNodes[node_index].Name);                
                }
            }
            /*
            if(NoMatchList.Count > 0)
            {
                Console.WriteLine("###WARNING: The following node(s) could not be found in the render models provided!");
                Console.Write(">>>>> ");
                foreach (var nomatchnode in NoMatchList)
                {
                    Console.Write(nomatchnode + ' ');
                }
                Console.WriteLine();
            }
            */

        }       

        public void FixupNodeTree(int Version)
        {
            //fixups for newer animation files with only the parent index present
            if(Version >= 16394)
            {
                for(var currentindex = 0; currentindex < AnimationNodes.Count; currentindex++)
                {
                    var node = AnimationNodes[currentindex];
                    if (node.ParentNode != -1)
                    {
                        //if this is the first child of this parent, just set this node as first child
                        if (AnimationNodes[node.ParentNode].FirstChildNode == -1)
                        {
                            AnimationNodes[node.ParentNode].FirstChildNode = (short)currentindex;
                            continue;
                        }
                        //if there is an existing child, loop through the children until you find the last one, 
                        //then set this new one as the next child index
                        if (AnimationNodes[node.ParentNode].FirstChildNode != -1)
                        {
                            int ChildIndex = AnimationNodes[node.ParentNode].FirstChildNode;
                            while(AnimationNodes[ChildIndex].NextSiblingNode != -1)
                            {
                                ChildIndex = AnimationNodes[ChildIndex].NextSiblingNode;
                            }
                            AnimationNodes[ChildIndex].NextSiblingNode = (short)currentindex;
                        }
                    }
                }
            }
            //fixups for older animation files that have both the first child and next sibling indices
            else
            {
                FixupSingleTreeNode(0);
            }
        }

        public void FixupSingleTreeNode(int currentindex)
        {
            if (AnimationNodes[currentindex].FirstChildNode != -1)
            {
                AnimationNodes[AnimationNodes[currentindex].FirstChildNode].ParentNode = (short)currentindex;
                FixupSingleTreeNode(AnimationNodes[currentindex].FirstChildNode);
            }
            if (AnimationNodes[currentindex].NextSiblingNode != -1)
            {
                AnimationNodes[AnimationNodes[currentindex].NextSiblingNode].ParentNode = AnimationNodes[currentindex].ParentNode;
                FixupSingleTreeNode(AnimationNodes[currentindex].NextSiblingNode);
            }
        }

        public bool CompareNodeListChecksums(List<ModelAnimationGraph.SkeletonNode> Nodes, GameCacheHaloOnlineBase CacheContext)
        {
            int checksum = 0;
            foreach (ModelAnimationGraph.SkeletonNode Node in Nodes)
            {
                checksum ^= (int)Node.Name.Value;
            }
            int importedchecksum = 0;
            foreach (AnimationNode newNode in AnimationNodes)
            {
                int value = (int)CacheContext.StringTable.GetStringId(newNode.Name).Value;
                importedchecksum ^= value;
            }

            return checksum == importedchecksum;
        }

        //this function generates a nodelist checksum identical to the official halo 1 blitzkrieg jma exporter
        //later halo games also use this same format
        public uint CalculateNodeListChecksum(int node_index, uint checksum = 0)
        {
            AnimationNode node = AnimationNodes[node_index];
            checksum = ((checksum >> 31 | checksum << 1) & 0xFFFFFFFF);
            checksum += CalculateSingleNodeChecksum(node.Name);
            checksum = ((checksum >> 30 | checksum << 2) & 0xFFFFFFFF);

            int nextnodeindex = node.FirstChildNode;
            while(nextnodeindex != -1)
            {
                checksum = CalculateNodeListChecksum(nextnodeindex, checksum);
                nextnodeindex = AnimationNodes[nextnodeindex].NextSiblingNode;
            }

            return checksum = ((checksum << 30 | checksum >> 2) & 0xFFFFFFFF);
        }

        public uint CalculateSingleNodeChecksum(string nodename)
        {
            uint checksum = 0;
            foreach (var chardata in nodename.ToArray())
            {
                checksum = (checksum >> 31 | checksum << 1) & 0xFFFFFFFF;
                checksum += (byte)chardata;
            }
            return checksum & 0xFFFFFFFF;
        }

        public bool CompareNodes(List<ModelAnimationGraph.SkeletonNode> jmadNodes, GameCacheHaloOnlineBase CacheContext)
        {
            if(CompareNodeListChecksums(jmadNodes, CacheContext))
            {
                //Console.WriteLine("Node List Checksums Match!");
                return true;
            }
            else
            {
                Console.WriteLine("###ERROR: Node List Checksum mismatch! Check that your animation nodes match the jmad!");
                return false;
                //if (!CompareSingleNode(jmadNodes, CacheContext, 0))
                //    return false;
            }
        }

        public bool CompareSingleNode(List<ModelAnimationGraph.SkeletonNode> jmadNodes, GameCacheHaloOnlineBase CacheContext, int index)
        {
            string Node = CacheContext.StringTable.GetString(jmadNodes[index].Name);
            string NextSibling = "null";
            string FirstChild = "null";
            string Parent = "null";
            if(jmadNodes[index].NextSiblingNodeIndex != -1)
                NextSibling = CacheContext.StringTable.GetString(jmadNodes[jmadNodes[index].NextSiblingNodeIndex].Name);
            if (jmadNodes[index].FirstChildNodeIndex != -1)
                FirstChild = CacheContext.StringTable.GetString(jmadNodes[jmadNodes[index].FirstChildNodeIndex].Name);
            if (jmadNodes[index].ParentNodeIndex != -1)
                Parent = CacheContext.StringTable.GetString(jmadNodes[jmadNodes[index].ParentNodeIndex].Name);

            string newSibling = "null";
            string newChild = "null";
            string newParent = "null";
            if (AnimationNodes[index].NextSiblingNode != -1)
                newSibling = AnimationNodes[AnimationNodes[index].NextSiblingNode].Name;
            if (AnimationNodes[index].FirstChildNode != -1)
                newChild = AnimationNodes[AnimationNodes[index].FirstChildNode].Name;
            if (AnimationNodes[index].ParentNode != -1)
                newParent = AnimationNodes[AnimationNodes[index].ParentNode].Name;

            if (AnimationNodes[index].Name != Node)
            {
                Console.WriteLine($"###ERROR: Node '{AnimationNodes[index].Name}' has a different name than the jmad ({Node})!");
                return false;
            }
            if(jmadNodes[index].NextSiblingNodeIndex != AnimationNodes[index].NextSiblingNode)
            {
                Console.WriteLine($"###ERROR: Node '{AnimationNodes[index].Name}' has a different next sibling ({newSibling}) than the jmad ({NextSibling})!");
                return false;
            }
            if (jmadNodes[index].FirstChildNodeIndex != AnimationNodes[index].FirstChildNode)
            {
                Console.WriteLine($"###ERROR: Node '{AnimationNodes[index].Name}' has a different first child ({newChild}) than the jmad ({FirstChild})!");
                return false;
            }
            if (jmadNodes[index].ParentNodeIndex != AnimationNodes[index].ParentNode)
            {
                Console.WriteLine($"###ERROR: Node '{AnimationNodes[index].Name}' has a different parent ({newParent}) than the jmad ({Parent})!");
                return false;
            }

            if (jmadNodes[index].FirstChildNodeIndex != -1)
            {
                if (!CompareSingleNode(jmadNodes, CacheContext, jmadNodes[index].FirstChildNodeIndex))
                    return false;
            }
            if (jmadNodes[index].NextSiblingNodeIndex != -1)
            {
                if (!CompareSingleNode(jmadNodes, CacheContext, jmadNodes[index].NextSiblingNodeIndex))
                    return false;
            }

            return true;
        }

        [TagStructure(Size = 0xC)]
        public class TranslationFrameFloat : TagStructure
        {
            public float X;
            public float Y;
            public float Z;
        }

        public class AnimationNode
        {
            public string Name;
            public short ParentNode;
            public short FirstChildNode;
            public short NextSiblingNode;
            public bool hasStaticTranslation;
            public bool hasStaticRotation;
            public bool hasStaticScale;
            public bool hasAnimatedTranslation;
            public bool hasAnimatedRotation;
            public bool hasAnimatedScale;
            public RealQuaternion DefaultRotation = new RealQuaternion(0, 0, 0, 1);
            public RealPoint3d DefaultTranslation = new RealPoint3d(0, 0, 0);
            public float DefaultScale = 1.0f;
            public List<AnimationFrame> Frames = new List<AnimationFrame>();
        }

        public class AnimationFrame
        {
            public RealQuaternion Rotation = new RealQuaternion(0,0,0,1);
            public RealPoint3d Translation = new RealPoint3d(0,0,0);
            public float Scale = 1.0f;
        }
    }
}
