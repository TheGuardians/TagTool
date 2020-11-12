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

        public void Import(string fileName)
        {
            using (TextReader textReader = (TextReader)File.OpenText(fileName))
            {
                textReader.ReadLine(); //version
                frameCount = int.Parse(textReader.ReadLine());
                textReader.ReadLine(); //framerate
                textReader.ReadLine();
                textReader.ReadLine(); //actor name
                int nodecount = int.Parse(textReader.ReadLine());
                nodeChecksum = int.Parse(textReader.ReadLine());
                AnimationNodes = new List<AnimationNode>();
                for (int i = 0; i < nodecount; i++)
                {
                    AnimationNodes.Add(new AnimationNode
                    {
                        Name = textReader.ReadLine(),
                        FirstChildNode = short.Parse(textReader.ReadLine()),
                        NextSiblingNode = short.Parse(textReader.ReadLine()),
                        ParentNode = -1
                    });
                }

                //set parent nodes
                for (int i = 0; i < nodecount; i++)
                {
                    if (AnimationNodes[i].FirstChildNode != -1)
                    {
                        AnimationNodes[AnimationNodes[i].FirstChildNode].ParentNode = (short)i;
                    }
                    if (AnimationNodes[i].NextSiblingNode != -1 && AnimationNodes[i].ParentNode != -1)
                    {
                        AnimationNodes[AnimationNodes[i].NextSiblingNode].ParentNode = (short)AnimationNodes[i].ParentNode;
                    }
                }

                for (int frame_index = 0; frame_index < frameCount; frame_index++)
                {
                    for (int node_index = 0; node_index < nodecount; node_index++)
                    {
                        var rotation = textReader.ReadLine().Split(' ');
                        var translation = textReader.ReadLine().Split(' ');
                        var scale = textReader.ReadLine();
                        AnimationNodes[node_index].Frames.Add(new AnimationFrame
                        {
                            Rotation = new RealQuaternion(float.Parse(rotation[0]), float.Parse(rotation[1]), float.Parse(rotation[2]), float.Parse(rotation[3])),
                            Translation = new RealPoint3d(float.Parse(translation[0]) * 0.01f, float.Parse(translation[1]) * 0.01f, float.Parse(translation[2]) * 0.01f),
                            Scale = float.Parse(scale)
                        });
                        //check to see if node frame is different from last one, to see if node is used
                        if(AnimationNodes[node_index].Frames.Count > 1)
                        {
                            var currentnode = AnimationNodes[node_index];
                            if (currentnode.Frames[frame_index].Rotation != currentnode.Frames[frame_index - 1].Rotation)
                                currentnode.isRotated = true;
                                rotatedNodeCount++;
                            if (currentnode.Frames[frame_index].Translation != currentnode.Frames[frame_index - 1].Translation)
                                currentnode.isTranslated = true;
                                translatedNodeCount++;
                            if (currentnode.Frames[frame_index].Scale != currentnode.Frames[frame_index - 1].Scale)
                                currentnode.isScaled = true;
                                scaledNodeCount++;
                        }
                    }
                }
            }
        }

        public ModelAnimationTagResource.GroupMember SerializeAnimationData(GameCacheHaloOnlineBase CacheContext)
        {
            var groupmember = new ModelAnimationTagResource.GroupMember
            {
                Checksum = nodeChecksum,
                MovementDataType = ModelAnimationTagResource.GroupMemberMovementDataType.None,
                FrameCount = (short)frameCount,
                NodeCount = (byte)AnimationNodes.Count
            };

            using (MemoryStream stream = new MemoryStream())
            using(EndianWriter writer = new EndianWriter(stream))
            {
                var dataContext = new DataSerializationContext(writer);

                var codecheader = new AnimationCodecHeader
                {
                    Type = AnimationCodecType._8ByteQuantizedRotationOnly,
                    RotationCount = (sbyte)rotatedNodeCount,
                    TranslationCount = (sbyte)translatedNodeCount,
                    ScaleCount = (sbyte)scaledNodeCount
                };
                CacheContext.Serializer.Serialize(dataContext, codecheader);

                var staticcodecheaderoffset = stream.Position.DeepClone();
                //write an empty static codec header for now, will return to this later 
                CacheContext.Serializer.Serialize(dataContext, new StaticCodecHeader());

                var datastartoffset = stream.Position.DeepClone();
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
                var staticcodecheader = new StaticCodecHeader
                {
                    TranslationDataOffset = (int)translationdataoffset,
                    ScaleDataOffset = (int)scaledataoffset,
                    RotationFrameSize = 0x8 * frameCount, //four shorts times number of frames in animation
                    TranslationFrameSize = 0xC * frameCount, //three floats times number of frames in animation
                    ScaleFrameSize = 0x4 * frameCount //one float times number of frames in animation
                };
                CacheContext.Serializer.Serialize(dataContext, staticcodecheader);

                //return to the end of the stream and record size
                stream.Position = stream.Length;
                groupmember.PackedDataSizes.CompressedDataSize = (int)stream.Length;

                List<int> AnimationFlags = BuildFlags();
                groupmember.PackedDataSizes.AnimatedNodeFlags = (byte)(0x4 * AnimationFlags.Count);
                foreach(var flagset in AnimationFlags)
                    CacheContext.Serializer.Serialize(dataContext, flagset);

                groupmember.AnimationData = new TagData(stream.ToArray());
            }

            return groupmember;
        }

        public void WriteScaleFrameData(GameCacheHaloOnlineBase CacheContext, DataSerializationContext dataContext)
        {
            for (int frame_index = 0; frame_index < frameCount; frame_index++)
            {
                for (int node_index = 0; node_index < AnimationNodes.Count; node_index++)
                {
                    if (AnimationNodes[node_index].isScaled)
                    {
                        var scale = AnimationNodes[node_index].Frames[frame_index].Scale;
                        CacheContext.Serializer.Serialize(dataContext, scale);
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
                    if (AnimationNodes[node_index].isRotated)
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
            for (int frame_index = 0; frame_index < frameCount; frame_index++)
            {
                for (int node_index = 0; node_index < AnimationNodes.Count; node_index++)
                {
                    if (AnimationNodes[node_index].isTranslated)
                    {
                        var translation = new RealPoint3d
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

        public List<int> BuildFlags()
        {
            int flagcount = (int)Math.Ceiling(AnimationNodes.Count / 32.0f);

            List<int> rotationflags = new List<int>(new int[flagcount]);
            List<int> translationflags = new List<int>(new int[flagcount]);
            List<int> scaleflags = new List<int>(new int[flagcount]);

            for(int nodeindex = 0; nodeindex < AnimationNodes.Count; nodeindex++)
            {
                int flagset = (int)Math.Floor(nodeindex / 32.0f);
                if (AnimationNodes[nodeindex].isRotated)
                    rotationflags[flagset] |= 1 << (nodeindex - (flagset * 32));
                if (AnimationNodes[nodeindex].isTranslated)
                    translationflags[flagset] |= 1 << (nodeindex - (flagset * 32));
                if (AnimationNodes[nodeindex].isScaled)
                    scaleflags[flagset] |= 1 << (nodeindex - (flagset * 32));
            }

            List<int> Flags = new List<int>();
            for (int flagset = 0; flagset < flagcount; flagset++)
            {
                Flags.Add(rotationflags[flagset]);
                Flags.Add(translationflags[flagset]);
                Flags.Add(scaleflags[flagset]);
            }
            
            return Flags;
        }

        public class AnimationNode
        {
            public string Name;
            public short ParentNode;
            public short FirstChildNode;
            public short NextSiblingNode;
            public List<AnimationFrame> Frames;
            public bool isTranslated;
            public bool isRotated;
            public bool isScaled;
        }

        public class AnimationFrame
        {
            public RealQuaternion Rotation;
            public RealPoint3d Translation;
            public float Scale;
        }
    }
}
