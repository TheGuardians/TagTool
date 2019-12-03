using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sentinel.Render.VertexDefinitions
{
    public sealed class StaticPrtQuadraticDefinition : VertexDefinition
    {
        public override VertexDeclaration GetDeclaration(Device device)
        {
            return new VertexDeclaration(device, new[]
            {
                new VertexElement(2, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.BlendWeight, 1),
                new VertexElement(2, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.BlendWeight, 2),
                new VertexElement(2, 24, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.BlendWeight, 3),

                VertexElement.VertexDeclarationEnd
            });
        }

        public override Dictionary<int, Type> GetStreamTypes() => new Dictionary<int, Type>
        {
            [2] = typeof(Stream2Data)
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct Stream2Data
        {
            public Vector3 BlendWeight;
            public Vector3 BlendWeight2;
            public Vector3 BlendWeight3;
        }
    }
}