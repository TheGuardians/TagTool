using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sentinel.Render.VertexDefinitions
{
    public sealed class StaticPerVertexDefinition : VertexDefinition
    {
        public override VertexDeclaration GetDeclaration(Device device)
        {
            return new VertexDeclaration(device, new[]
            {
                new VertexElement(4, 0, DeclarationType.Ubyte4N, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 3),
                new VertexElement(4, 4, DeclarationType.Ubyte4N, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 4),
                new VertexElement(4, 8, DeclarationType.Ubyte4N, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 5),
                new VertexElement(4, 12, DeclarationType.Ubyte4N, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 6),
                new VertexElement(4, 16, DeclarationType.Ubyte4N, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 7),

                VertexElement.VertexDeclarationEnd
            });
        }

        public override Dictionary<int, Type> GetStreamTypes() => new Dictionary<int, Type>
        {
            [4] = typeof(Stream4Data)
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct Stream4Data
        {
            public uint Texcoord1; // Ubyte4N
            public uint Texcoord2; // Ubyte4N
            public uint Texcoord3; // Ubyte4N
            public uint Texcoord4; // Ubyte4N
            public uint Texcoord5; // Ubyte4N
        }
    }
}