using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sentinel.Render.VertexDefinitions
{
    public sealed class FlatWorldVertexDefinition : VertexDefinition
    {
        public override VertexDeclaration GetDeclaration(Device device)
        {
            return new VertexDeclaration(device, new[]
            {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 12, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
                new VertexElement(0, 20, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
                new VertexElement(0, 32, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Tangent, 0),
                new VertexElement(0, 44, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.BiNormal, 0),

                VertexElement.VertexDeclarationEnd
            });
        }

        public override Dictionary<int, Type> GetStreamTypes() => new Dictionary<int, Type>
        {
            [0] = typeof(Stream0Data)
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct Stream0Data
        {
            public Vector3 Position;
            public Vector2 Texcoord;
            public Vector3 Normal;
            public Vector3 Tangent;
            public Vector3 Binormal;
        }
    }
}