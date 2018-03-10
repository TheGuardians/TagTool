using TagTool.Cache;
using TagTool.Common;
using TagTool.Havok;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using System;
using System.IO;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private PhysicsModel ConvertPhysicsModel(PhysicsModel phmo)
        {
            //
            // Fix mopp code array headers for both H3 and ODST
            //

            byte[] result = new byte[phmo.MoppCodes.Length];

            using (var inputReader = new EndianReader(new MemoryStream(phmo.MoppCodes), EndianFormat.BigEndian))
            using (var outputWriter = new EndianWriter(new MemoryStream(result), EndianFormat.LittleEndian))
            {
                var dataContext = new DataSerializationContext(inputReader, outputWriter);

                for (int i = 0; i < phmo.Mopps.Count; i++)
                {
                    var header = BlamDeserializer.Deserialize<MoppCode>(dataContext);
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

        private Projectile ConvertProjectile(Projectile proj)
        {
            //Seems to fix projectile speed

            float temp = proj.AiGuidedAngularVelocityScale;
            proj.AiGuidedAngularVelocityScale = proj.AiVelocityScale;
            proj.AiVelocityScale = temp;

            return proj;
        }
    }
}