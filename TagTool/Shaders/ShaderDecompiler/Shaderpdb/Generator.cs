using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.ShaderDecompiler.UPDB
{
	public class Generator
	{
		public Shaderpdb GetShaderpdb(byte[] debugData, byte[] constantData, byte[] shaderData)
		{
			var pdb = new Shaderpdb();
			var fullShader = new byte[debugData.Length + constantData.Length + shaderData.Length];
			Array.Copy(debugData, 0, fullShader, 0, debugData.Length);
			Array.Copy(constantData, 0, fullShader, debugData.Length, constantData.Length);
			Array.Copy(shaderData, 0, fullShader, debugData.Length + constantData.Length, shaderData.Length);

			var shaderFile = Path.GetTempFileName();
		    File.WriteAllBytes(shaderFile, fullShader);

			// Disassemble using xsd.exe
			// Reassemble using psa.exe with the /XZi argument to generate a UPDB file
			// Deserialize the UPDB file (it's in XML format)

			return pdb;
		}
	}
}
