using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sentinel.Render.VertexDefinitions
{
    public sealed class RippleVertexDefinition : VertexDefinition
    {
        public override VertexDeclaration GetDeclaration(Device device)
        {
            return new VertexDeclaration(device, new[]
            {
                new VertexElement(1, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(1, 16, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
                new VertexElement(1, 32, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 1),
                new VertexElement(1, 48, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 2),
                new VertexElement(1, 64, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 3),
                new VertexElement(1, 80, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 4),
                new VertexElement(1, 96, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                new VertexElement(1, 112, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Color, 1),

                new VertexElement(2, 0, DeclarationType.Short2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 5),

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
            public Vector4 Texcoord;
            public Vector4 Texcoord2;
            public Vector4 Texcoord3;
            public Vector4 Texcoord4;
            public Vector4 Texcoord5;
            public Vector4 Color;
            public Vector4 Color2;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Stream2Data
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] Texcoord; // Short2
        }
    }
}