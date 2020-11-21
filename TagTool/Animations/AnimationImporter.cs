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

namespace TagTool.Animations
{
    public class AnimationImporter
    {
        public List<AnimationNode> AnimationNodes;
        public int frameCount;
        public int nodeChecksum;
        public int rotatedNodeCount;
        public int translatedNodeCount;
        public int scaledNodeCount;
        public bool buildstaticdata = false;

        public void Import(string fileName, GameCacheHaloOnlineBase CacheContext)
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
                    nodeChecksum = int.Parse(textReader.ReadLine()); //version part 2
                frameCount = int.Parse(textReader.ReadLine());
                textReader.ReadLine(); //framerate
                textReader.ReadLine(); //actor count
                textReader.ReadLine(); //actor name
                int nodecount = int.Parse(textReader.ReadLine());
                if (Version < 16394)
                    nodeChecksum = int.Parse(textReader.ReadLine());
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

                for (int frame_index = 0; frame_index < frameCount; frame_index++)
                {
                    for (int node_index = 0; node_index < nodecount; node_index++)
                    {
                        var translation = textReader.ReadLine().Split('\t');
                        var rotation = textReader.ReadLine().Split('\t');
                        var scale = textReader.ReadLine();
                        AnimationNodes[node_index].Frames.Add(new AnimationFrame
                        {
                            Rotation = new RealQuaternion(float.Parse(rotation[0]), float.Parse(rotation[1]), float.Parse(rotation[2]), float.Parse(rotation[3])),
                            Translation = new RealPoint3d(float.Parse(translation[0]) * 0.01f, float.Parse(translation[1]) * 0.01f, float.Parse(translation[2]) * 0.01f),
                            Scale = float.Parse(scale)
                        });
                        //check to see if node frame is different from last one, to see if node is used
                        if (AnimationNodes[node_index].Frames.Count > 1)
                        {
                            var currentnode = AnimationNodes[node_index];
                            if (!CompareRotations(currentnode.Frames[frame_index], currentnode.Frames[0]) &&
                                currentnode.hasStaticRotation)
                            {
                                currentnode.hasStaticRotation = false;
                                rotatedNodeCount++;
                            }

                            if (!CompareTranslations(currentnode.Frames[frame_index], currentnode.Frames[0]) &&
                                currentnode.hasStaticTranslation)
                            {
                                currentnode.hasStaticTranslation = false;
                                translatedNodeCount++;
                            }

                            if (currentnode.Frames[frame_index].Scale != 1.0f &&
                                currentnode.hasStaticScale)
                            {
                                currentnode.hasStaticScale = false;
                                scaledNodeCount++;
                            }
                        }
                    }
                }
            }
        }

        public bool CompareTranslations(AnimationFrame Frame1, AnimationFrame Frame2)
        {
            if(Math.Abs(Frame2.Translation.X - Frame1.Translation.X) > 0.0001 || 
               Math.Abs(Frame2.Translation.Y - Frame1.Translation.Y) > 0.0001 ||
               Math.Abs(Frame2.Translation.Z - Frame1.Translation.Z) > 0.0001)
                return false;
            return true;
        }

        public bool CompareRotations(AnimationFrame Frame1, AnimationFrame Frame2)
        {
            if (Math.Abs(Frame2.Rotation.I - Frame1.Rotation.I) > 0.0001 ||
                Math.Abs(Frame2.Rotation.J - Frame1.Rotation.J) > 0.0001 ||
                Math.Abs(Frame2.Rotation.K - Frame1.Rotation.K) > 0.0001 ||
                Math.Abs(Frame2.Rotation.W - Frame1.Rotation.W) > 0.0001)
                return false;
            return true;
        }

        public ModelAnimationTagResource.GroupMember SerializeAnimationData(GameCacheHaloOnlineBase CacheContext)
        {
            var groupmember = new ModelAnimationTagResource.GroupMember
            {
                Checksum = nodeChecksum,
                MovementDataType = ModelAnimationTagResource.GroupMemberMovementDataType.None,
                FrameCount = (short)frameCount,
                NodeCount = (byte)AnimationNodes.Count,
                PackedDataSizes = new ModelAnimationTagResource.GroupMember.PackedDataSizesStructBlock()
            };

            //check if there is any static data that we need to build
            foreach (var currentnode in AnimationNodes)
            {
                if (currentnode.hasStaticRotation || currentnode.hasStaticScale || currentnode.hasStaticTranslation)
                    buildstaticdata = true;
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
                    PlaybackRate = 1.0f
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
                    TranslationDataOffset = (int)translationdataoffset,
                    ScaleDataOffset = (int)scaledataoffset,
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
                RotationCount = (sbyte)(AnimationNodes.Count - rotatedNodeCount),
                TranslationCount = (sbyte)(AnimationNodes.Count - translatedNodeCount),
                ScaleCount = (sbyte)(AnimationNodes.Count - scaledNodeCount),
                PlaybackRate = 1.0f
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
                        X = (short)(Math.Round(currentnode.Frames[0].Rotation.I * short.MaxValue)),
                        Y = (short)(Math.Round(currentnode.Frames[0].Rotation.J * short.MaxValue)),
                        Z = (short)(Math.Round(currentnode.Frames[0].Rotation.K * short.MaxValue)),
                        W = (short)(Math.Round(currentnode.Frames[0].Rotation.W * short.MaxValue))
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

        public int GetNodeListChecksum(List<RenderModel.Node> Nodes)
        {
            int checksum = 0;
            foreach (RenderModel.Node Node in Nodes)
            {
                checksum ^= (int)Node.Name.Value;
            }
            return checksum;
        }

        public int GetNodeListChecksum(List<ModelAnimationGraph.SkeletonNode> Nodes)
        {
            int checksum = 0;
            foreach (ModelAnimationGraph.SkeletonNode Node in Nodes)
            {
                checksum ^= (int)Node.Name.Value;
            }
            return checksum;
        }

        public int CalculateNodeListChecksum(List<AnimationNode> Nodes, GameCacheHaloOnlineBase CacheContext)
        {
            int checksum = 0;
            foreach (AnimationNode Node in Nodes)
            {
                int value = (int)CacheContext.StringTable.GetStringId(Node.Name).Value;
                checksum ^= value;
            }
            return checksum;
        }

        public void RemoveOverlayBase()
        {
            //remove base frame from frame count
            frameCount--;
            foreach (var node in AnimationNodes)
            {
                //copy and then remove unnecessary base frame
                AnimationFrame BaseFrame = node.Frames[0].DeepClone();
                node.Frames.RemoveAt(0);
                //remove basis of overlay to just leave the actual overlay data
                foreach(var frame in node.Frames)
                {
                    if (!node.hasStaticRotation)
                    {
                        //using system.numerics.quaternion here because it has a division operator
                        var temprotation = new Quaternion(frame.Rotation.I, frame.Rotation.J, frame.Rotation.K, frame.Rotation.W);
                        var tempbase = new Quaternion(BaseFrame.Rotation.I, BaseFrame.Rotation.J, BaseFrame.Rotation.K, BaseFrame.Rotation.W);
                        var dividend = temprotation / tempbase;
                        frame.Rotation = new RealQuaternion(dividend.X, dividend.Y, dividend.Z, dividend.W);
                    }
                    if (!node.hasStaticTranslation)
                    {
                        frame.Translation -= BaseFrame.Translation;
                    }
                    if (!node.hasStaticScale)
                    {
                        frame.Scale -= BaseFrame.Scale;
                    }
                }
            }
        }

        public void WriteScaleFrameData(GameCacheHaloOnlineBase CacheContext, DataSerializationContext dataContext)
        {
            for (int frame_index = 0; frame_index < frameCount; frame_index++)
            {
                for (int node_index = 0; node_index < AnimationNodes.Count; node_index++)
                {
                    if (!AnimationNodes[node_index].hasStaticScale)
                    {
                        var scale = AnimationNodes[node_index].Frames[frame_index].Scale;
                        dataContext.Writer.Write(scale);
                    }
                }
            }
        }

        public void WriteRotationFrameData(GameCacheHaloOnlineBase CacheContext, DataSerializationContext dataContext)
        {
            for (int frame_index = 0; frame_index < frameCount; frame_index++)
            {
                for (int node_index = 0; node_index < AnimationNodes.Count; node_index++)
                {
                    if (!AnimationNodes[node_index].hasStaticRotation)
                    {
                        var rotation = new ModelAnimationTagResource.GroupMember.RotationFrame
                        {
                            X = (short)(Math.Round(AnimationNodes[node_index].Frames[frame_index].Rotation.I * short.MaxValue)),
                            Y = (short)(Math.Round(AnimationNodes[node_index].Frames[frame_index].Rotation.J * short.MaxValue)),
                            Z = (short)(Math.Round(AnimationNodes[node_index].Frames[frame_index].Rotation.K * short.MaxValue)),
                            W = (short)(Math.Round(AnimationNodes[node_index].Frames[frame_index].Rotation.W * short.MaxValue))
                        };
                        CacheContext.Serializer.Serialize(dataContext, rotation);
                    }
                }
            }
        }

        public void WriteTranslationFrameData(GameCacheHaloOnlineBase CacheContext, DataSerializationContext dataContext)
        {
            for (int frame_index = 0; frame_index < frameCount; frame_index++)
            {
                for (int node_index = 0; node_index < AnimationNodes.Count; node_index++)
                {
                    if (!AnimationNodes[node_index].hasStaticTranslation)
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

        public List<int> BuildFlags(bool isStaticFlags)
        {
            int flagcount = (int)Math.Ceiling(AnimationNodes.Count / 32.0f);

            List<int> rotationflags = new List<int>(new int[flagcount]);
            List<int> translationflags = new List<int>(new int[flagcount]);
            List<int> scaleflags = new List<int>(new int[flagcount]);

            for (int nodeindex = 0; nodeindex < AnimationNodes.Count; nodeindex++)
            {
                int flagset = (int)Math.Floor(nodeindex / 32.0f);
                if ((isStaticFlags && AnimationNodes[nodeindex].hasStaticTranslation) || (!isStaticFlags && !AnimationNodes[nodeindex].hasStaticTranslation))
                    rotationflags[flagset] |= 1 << (nodeindex - (flagset * 32));
                if ((isStaticFlags && AnimationNodes[nodeindex].hasStaticRotation) || (!isStaticFlags && !AnimationNodes[nodeindex].hasStaticRotation))
                    translationflags[flagset] |= 1 << (nodeindex - (flagset * 32));
                if ((isStaticFlags && AnimationNodes[nodeindex].hasStaticScale) || (!isStaticFlags && !AnimationNodes[nodeindex].hasStaticScale))
                    scaleflags[flagset] |= 1 << (nodeindex - (flagset * 32));
            }

            //all translation flags are first serialized, then all rotation flags etc
            List<int> Flags = new List<int>();
            Flags.AddRange(translationflags);
            Flags.AddRange(rotationflags);
            Flags.AddRange(scaleflags);
            
            return Flags;
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
            public bool hasStaticTranslation = true;
            public bool hasStaticRotation = true;
            public bool hasStaticScale = true;
            public List<AnimationFrame> Frames = new List<AnimationFrame>();
        }

        public class AnimationFrame
        {
            public RealQuaternion Rotation;
            public RealPoint3d Translation;
            public float Scale;
        }
    }
}
