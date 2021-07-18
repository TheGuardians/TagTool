using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TagTool.IO;
using TagTool.Common;

namespace TagTool.Shaders
{
    public class CompiledShaderInfo
    {
        public bool PixelShader;
        public byte[] ShaderData;   //xbox
        public byte[] DebugData;    //xbox
        public byte[] ConstantData; //xbox
        public List<ShaderParameter> Parameters;

        public CompiledShaderInfo(bool pixl, byte[] shaderData, byte[] debugData, byte[] constantData, List<ShaderParameter> parameters)
        {
            PixelShader = pixl;
            ShaderData = shaderData;
            DebugData = debugData;
            ConstantData = constantData;
            Parameters = parameters;
        }

        public CompiledShaderInfo(bool pixl, PixelShaderReference shaderRef, List<ShaderParameter> parameters)
        {
            PixelShader = pixl;
            ShaderData = shaderRef.ShaderData;
            DebugData = shaderRef.DebugData;
            ConstantData = shaderRef.ConstantData;
            Parameters = parameters;
        }

        public CompiledShaderInfo(bool pixl, VertexShaderReference shaderRef, List<ShaderParameter> parameters)
        {
            PixelShader = pixl;
            ShaderData = shaderRef.ShaderData;
            DebugData = shaderRef.DebugData;
            ConstantData = shaderRef.ConstantData;
            Parameters = parameters;
        }

        private void ReorderParameter(List<ShaderParameter> newParameters, ShaderParameter.RType rType, int rMax)
        {
            Dictionary<int, int> tempMapping = new Dictionary<int, int>();

            for (int i = 0; i < Parameters.Count; i++)
                if (Parameters[i].RegisterType == rType)
                    tempMapping.Add(Parameters[i].RegisterIndex, i);

            for (int i = 0; i <= rMax; i++)
                if (tempMapping.ContainsKey(i))
                    newParameters.Add(Parameters[tempMapping[i]]);
        }

        public List<ShaderParameter> ReorderParameters()
        {
            List<ShaderParameter> newParameters = new List<ShaderParameter>();

            ReorderParameter(newParameters, ShaderParameter.RType.Boolean, 128);
            ReorderParameter(newParameters, ShaderParameter.RType.Integer, 16);
            ReorderParameter(newParameters, ShaderParameter.RType.Vector, 256);
            ReorderParameter(newParameters, ShaderParameter.RType.Sampler, 16);

            return newParameters;
        }

        public List<RealQuaternion> GetXboxShaderConstants()
        {
            var constants = new List<RealQuaternion>();

            using (var stream = new MemoryStream(ConstantData))
            using (var reader = new EndianReader(stream, EndianFormat.BigEndian))
            {
                for (var i = 0; i < ConstantData.Length / 16; i++)
                {
                    constants.Add(new RealQuaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
                }
                constants.Reverse(); //they are stored in reverse order
            }

            return constants;
        }
    }
}
