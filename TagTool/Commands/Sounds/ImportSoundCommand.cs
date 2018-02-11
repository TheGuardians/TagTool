using TagTool.Cache;
using TagTool.Commands;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.TagDefinitions;
using TagTool.TagResources;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Commands.Sounds
{
    class ImportSoundCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private Sound Definition { get; }

        public ImportSoundCommand(GameCacheContext cacheContext, CachedTagInstance tag, Sound definition) :
            base(CommandFlags.Inherit,
                
                "ImportSound",
                "Import a MP3 file into the current snd! tag. See documentation for formatting and options.",
                
                "ImportSound <Sound File>",
                "")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var resourceFile = new FileInfo(args[0]);
            var fileSize = 0;

            if (!resourceFile.Exists)
            {
                Console.WriteLine($"ERROR: File not found: \"{resourceFile.FullName}\"");
                return true;
            }

            //
            // Create new resource
            //

            Console.Write("Creating new sound resource...");
            
            Definition.Unknown12 = 0;
            
            using (var dataStream = resourceFile.OpenRead())
            {

                fileSize = (int)dataStream.Length;
                var resourceContext = new ResourceSerializationContext(Definition.Resource);
                CacheContext.Serializer.Serialize(resourceContext,
                    new SoundResourceDefinition
                    {
                        Data = new TagData(fileSize, new CacheAddress(CacheAddressType.Resource, 0))
                    });

                Definition.Resource = new PageableResource
                {
                    Page = new RawPage(),
                    Resource = new TagResource
                    {
                        Type = TagResourceType.Sound,
                        DefinitionAddress = new CacheAddress(CacheAddressType.Definition, 536870912),
                        ResourceFixups = new List<TagResource.ResourceFixup>(),
                        ResourceDefinitionFixups = new List<TagResource.ResourceDefinitionFixup>(),
                        Unknown2 = 1
                    }
                };
                var definitionFixup = new TagResource.ResourceFixup()
                {
                    BlockOffset = 12,
                    Address = new CacheAddress(CacheAddressType.Resource, 1073741824)
                };
                Definition.Resource.Resource.ResourceFixups.Add(definitionFixup);

                CacheContext.AddResource(Definition.Resource, ResourceLocation.ResourcesB, dataStream);

                Definition.Resource.Resource.DefinitionData = new byte[20];

                for (int i = 0; i < 4; i++)
                {
                    Definition.Resource.Resource.DefinitionData[i] = (byte)(Definition.Resource.Page.UncompressedBlockSize >> (i * 8));
                }

                Console.WriteLine("done.");
            }

            //
            // Adjust tag definition to use correctly the sound file.
            //

            var chunkSize = (ushort)fileSize;
            
            var permutationChunk = new SoundCacheFileGestalt.PermutationChunk
            {
                Offset = 0,
                Size = chunkSize,
                Unknown2 = (byte)((fileSize - chunkSize) / 65536),
                Unknown3 = 4,
                RuntimeIndex = -1,
                UnknownA = 0,
                UnknownSize = 0
            };

            var permutation = Definition.PitchRanges[0].Permutations[0];

            permutation.PermutationChunks = new List<SoundCacheFileGestalt.PermutationChunk>
            {
                permutationChunk
            };

            permutation.PermutationNumber = 0;
            permutation.SampleSize = 0;
            permutation.IsNotFirstPermutation = 0;

            Definition.PitchRanges[0].Permutations = new List<SoundCacheFileGestalt.Permutation>
            {
                permutation
            };

            Definition.PlatformCodec.Compression = 8; // MP3
            
            return true;
        }
    }
}