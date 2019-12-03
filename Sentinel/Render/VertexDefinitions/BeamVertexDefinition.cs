using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sentinel.Render.VertexDefinitions
{
    public sealed class BeamVertexDefinition : VertexDefinition
    {
        public override VertexDeclaration GetDeclaration(Device device)
        {
            return new VertexDeclaration(device, new[]
            {
                new VertexElement(1, 48, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 1),
                new VertexElement(1, 64, DeclarationType.Short4N, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 3),
                new VertexElement(1, 72, DeclarationType.Float16Four, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 4),
                new VertexElement(1, 80, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 1),
                new VertexElement(1, 84, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 14),

                new VertexElement(2, 0, DeclarationType.Short2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 1),

                VertexElement.VertexDeclarationEnd
            });
        }

        public override Dictionary<int, Type> GetStreamTypes() => new Dictionary<int, Type>
        {
            [1] = typeof(Stream1Data),
            [2] = typeof(Stream2Data)
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct Stream1Data
        {
            public Vector4 Position;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public short[] Texcoord1; // Short4N

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public short[] Texcoord2; // Float16Four

            public int Color;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] Position2; // Short2
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Stream2Data
        {
            public short[] Texcoord;
        }
    }
}