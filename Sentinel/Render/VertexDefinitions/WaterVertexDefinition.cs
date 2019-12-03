using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sentinel.Render.VertexDefinitions
{
    public sealed class WaterVertexDefinition : VertexDefinition
    {
        public override VertexDeclaration GetDeclaration(Device device)
        {
            return new VertexDeclaration(device, new[]
            {
                new VertexElement(1, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(1, 16, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 1),
                new VertexElement(1, 32, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 2),
                new VertexElement(1, 48, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 3),
                new VertexElement(1, 64, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 4),
                new VertexElement(1, 80, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 5),
                new VertexElement(1, 96, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 6),
                new VertexElement(1, 112, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 7),
                new VertexElement(1, 128, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
                new VertexElement(1, 144, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 1),

                new VertexElement(3, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
                new VertexElement(3, 16, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Normal, 1),
                new VertexElement(3, 32, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Normal, 2),
                new VertexElement(3, 48, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Normal, 3),
                new VertexElement(3, 64, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.Normal, 4),

                new VertexElement(2, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 2),

                VertexElement.VertexDeclarationEnd
            });
        }

        public override Dictionary<int, Type> GetStreamTypes() => new Dictionary<int, Type>
        {
            [1] = typeof(Stream1Data),
            [2] = typeof(Stream2Data),
            [3] = typeof(Stream3Data)
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct Stream1Data
        {
            public Vector4 Position;
            public Vector4 Position2;
            public Vector4 Position3;
            public Vector4 Position4;
            public Vector4 Position5;
            public Vector4 Position6;
            public Vector4 Position7;
            public Vector4 Position8;
            public Vector4 Texcoord;
            public Vector3 Texcoord2;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Stream2Data
        {
            public Vector3 Texcoord;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Stream3Data
        {
            public Vector4 Normal;
            public Vector4 Normal2;
            public Vector4 Normal3;
            public Vector4 Normal4;
            public Vector2 Normal5;
        }
    }
}