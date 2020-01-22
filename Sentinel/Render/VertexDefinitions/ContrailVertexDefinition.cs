using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sentinel.Render.VertexDefinitions
{
    public sealed class ContrailVertexDefinition : VertexDefinition
    {
        public override VertexDeclaration GetDeclaration(Device device)
        {
            return new VertexDeclaration(device, new[]
            {
                new VertexElement(1, 64, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 3),
                new VertexElement(1, 80, DeclarationType.Float16Four, DeclarationMethod.Default, DeclarationUsage.Position, 4),
                new VertexElement(1, 88, DeclarationType.Short4N, DeclarationMethod.Default, DeclarationUsage.Position, 5),
                new VertexElement(1, 96, DeclarationType.Float16Four, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 4),
                new VertexElement(1, 104, DeclarationType.Short4N, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 5),
                new VertexElement(1, 112, DeclarationType.Float16Two, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 6),
                new VertexElement(1, 116, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 2),
                new VertexElement(1, 120, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 3),
                new VertexElement(1, 128, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 6),

                VertexElement.VertexDeclarationEnd
            });
        }

        public override Dictionary<int, Type> GetStreamTypes() => new Dictionary<int, Type>
        {
            [1] = typeof(Stream1Data)
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct Stream1Data
        {
            public Vector4 Position1;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public short[] Position2; // Float16Four

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public short[] Position3; // Short4N

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public short[] Texcoord1; // Float16Four

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public short[] Texcoord2; // Short4N

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public short[] Texcoord3; // Float16Two

            public int Color1;
            public int Color2;
            public Vector4 Position4;
        }
    }
}