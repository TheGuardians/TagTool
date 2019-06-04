using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sentinel.Render.VertexDefinitions
{
    public sealed class DecoratorVertexDefinition : VertexDefinition
    {
        public override VertexDeclaration GetDeclaration(Device device)
        {
            return new VertexDeclaration(device, new[]
            {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 12, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
                new VertexElement(0, 20, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),

                new VertexElement(1, 0, DeclarationType.Short4, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 1),
                new VertexElement(1, 8, DeclarationType.Ubyte4N, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 2),
                new VertexElement(1, 12, DeclarationType.Ubyte4N, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 3),

                VertexElement.VertexDeclarationEnd
            });
        }

        public override Dictionary<int, Type> GetStreamTypes() => new Dictionary<int, Type>
        {
            [0] = typeof(Stream0Data),
            [1] = typeof(Stream1Data)
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct Stream0Data
        {
            public Vector3 Position;
            public Vector2 Texcoord;
            public Vector3 Normal;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Stream1Data
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public short[] Texcoord1; // Short4
            public uint Texcoord2; // Ubyte
            public uint Texcoord3; // Ubyte4N
        }
    }
}