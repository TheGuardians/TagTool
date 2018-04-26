using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TagTool.Tools;

namespace TagTool.ShaderDecompiler.UPDB
{
	public static class Generator
	{
		public static Shaderpdb GetShaderpdb(byte[] debugData, byte[] constantData, byte[] shaderData)
		{
			var fullShader = new byte[debugData.Length + constantData.Length + shaderData.Length];
			Array.Copy(debugData, 0, fullShader, 0, debugData.Length);
			Array.Copy(constantData, 0, fullShader, debugData.Length, constantData.Length);
			Array.Copy(shaderData, 0, fullShader, debugData.Length + constantData.Length, shaderData.Length);

			var shaderTemp = Path.GetTempFileName();
		    File.WriteAllBytes(shaderTemp, fullShader);

			// Disassemble using xsd.exe with the /o argument to name the output file
			var asmTemp = Path.GetTempFileName();
			new Tool($"/o {asmTemp} {shaderTemp}", "xsd.exe");
			// Reassemble using psa.exe with the /XZi argument to generate a UPDB file, and /Xfd to name it.
			var updbTemp = Path.GetTempFileName();
			new Tool($"/XZi /XFd {updbTemp} {asmTemp}", "psa.exe");

			// This line is only printed to help work out all the XML fields
			Console.WriteLine(File.ReadAllText(asmTemp));

			// Deserialize the UPDB file (it's in XML format)
			var shaderPdb = new Shaderpdb();
			using (var updbReader = new StreamReader(updbTemp))
			{
				try
				{
					var xmlSerializer = new XmlSerializer(typeof(Shaderpdb));
					shaderPdb = (Shaderpdb)xmlSerializer.Deserialize(updbReader);
				}
				catch
				{
					Console.WriteLine(File.ReadAllText(updbTemp));
					Console.ReadLine();
				}
			}

			File.Delete(shaderTemp);
			File.Delete(asmTemp);
			File.Delete(updbTemp);

			return shaderPdb;
		}
	}
}
