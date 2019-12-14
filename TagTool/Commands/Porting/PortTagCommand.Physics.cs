using TagTool.Havok;
using TagTool.IO;
using TagTool.Tags.Definitions;
using System;
using System.IO;
using TagTool.Serialization;
using TagTool.Cache;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private PhysicsModel ConvertPhysicsModel(CachedTagInstance instance, PhysicsModel phmo)
        {
            /*
            // Allow syncing of specific tags in MP (hax)
            //

            switch (instance.Name)
            {
                case @"objects\levels\solo\060_floodship\flood_danglers\large_dangler\large_dangler":
                case @"objects\levels\solo\060_floodship\flood_danglers\small_dangler\small_dangler":
                    phmo.Flags &= ~PhysicsModel.PhysicsModelFlags.MakePhysicalChildrenKeyframed;
                    break;
            }*/

            //
            // Fix mopp code array headers for both H3 and ODST
            //

            byte[] result = new byte[phmo.MoppCodes.Length];

            using (var inputReader = new EndianReader(new MemoryStream(phmo.MoppCodes), BlamCache.Reader.Format))
            using (var outputWriter = new EndianWriter(new MemoryStream(result), EndianFormat.LittleEndian))
            {
                var dataContext = new DataSerializationContext(inputReader, outputWriter);

                for (int i = 0; i < phmo.Mopps.Count; i++)
                {
                    var header = BlamCache.Deserializer.Deserialize<MoppCode>(dataContext);
                    CacheContext.Serializer.Serialize(dataContext, header);
                    
                    var adjustedDataSize = header.DataSize % 16 == 0 ? header.DataSize : (header.DataSize / 16 + 1) * 16;       //Align on 16 bytes.

                    Array.Copy(phmo.MoppCodes, inputReader.Position, result, inputReader.Position, adjustedDataSize);
                    inputReader.SeekTo(inputReader.Position + adjustedDataSize);
                    outputWriter.Seek((int)inputReader.Position, 0);
                }

                phmo.MoppCodes = result;
            }

            return phmo;
        }
    }
}