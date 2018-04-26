/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */

 using System.Xml.Serialization;
using System.Collections.Generic;
using TagTool.ShaderDecompiler.ConstantData;


// TODO: this needs a LOT of cleanup

namespace TagTool.ShaderDecompiler.UPDB
{
	[XmlRoot(ElementName = "Interpolator")]
	public class Interpolator
	{
		[XmlElement(ElementName = "Register")]
		public int Register { get; set; }
		[XmlAttribute(AttributeName = "Semantic")]
		public Semantic Semantic { get; set; }
		[XmlAttribute(AttributeName = "Mask")]
		public string Mask { get; set; }
	}

	[XmlRoot(ElementName = "Float")]
	public class Float
	{
		[XmlElement(ElementName = "Register")]
		public int Register { get; set; }
		[XmlAttribute(AttributeName = "value0")]
		public float Value0 { get; set; }
		[XmlAttribute(AttributeName = "value1")]
		public float Value1 { get; set; }
		[XmlAttribute(AttributeName = "value2")]
		public float Value2 { get; set; }
		[XmlAttribute(AttributeName = "value3")]
		public float Value3 { get; set; }
	}

	[XmlRoot(ElementName = "Int")]
	public class Int
	{
		[XmlElement(ElementName = "Register")]
		public int Register { get; set; }
		[XmlAttribute(AttributeName = "Count")]
		public int Count { get; set; }
		[XmlAttribute(AttributeName = "Start")]
		public int Start { get; set; }
		[XmlAttribute(AttributeName = "Inc")]
		public int Increment { get; set; }
	}

	[XmlRoot(ElementName = "Bool")]
	public class Bool
	{
		[XmlElement(ElementName = "Register")]
		public int Register { get; set; }
		[XmlAttribute(AttributeName = "Value")]
		public bool Value { get; set; }
	}

	[XmlRoot(ElementName = "vfetch")]
	public class Vfetch
	{
		[XmlElement(ElementName = "Address")]
		public int Address { get; set; }
		[XmlAttribute(AttributeName = "Register")]
		public bool Register { get; set; }
		[XmlAttribute(AttributeName = "DestSwizzle")]
		public string DestSwizzle { get; set; }
		[XmlAttribute(AttributeName = "Semantic")]
		public Semantic Semantic { get; set; }
		[XmlAttribute(AttributeName = "End")]
		public bool End { get; set; }
	}

	[XmlRoot(ElementName = "shader")]
	public class Shader
	{
		[XmlElement(ElementName = "variables")]
		public string Variables { get; set; }
		[XmlElement(ElementName = "variableDebugInfo")]
		public string VariableDebugInfo { get; set; }
		[XmlElement(ElementName = "scopes")]
		public string Scopes { get; set; }
		[XmlElement(ElementName = "funcInfo")]
		public string FuncInfo { get; set; }
		[XmlElement(ElementName = "funcEntryExitInfo")]
		public string FuncEntryExitInfo { get; set; }
		[XmlElement(ElementName = "VfetchInfo")]
		public List<Vfetch> VfetchInfo { get; set; }
		[XmlElement(ElementName = "InterpolatorInfo")]
		public List<Interpolator> InterpolatorInfo { get; set; }
		[XmlElement(ElementName = "LiteralFloats")]
		public List<Float> LiteralFloats { get; set; }
		[XmlElement(ElementName = "LiteralInts")]
		public List<Int> LiteralInts { get; set; }
		[XmlElement(ElementName = "LiteralBools")]
		public List<Bool> LiteralBools { get; set; }
		[XmlAttribute(AttributeName = "ZPass")]
		public string ZPass { get; set; }
	}

	[XmlRoot(ElementName = "shaders")]
	public class Shaders
	{
		[XmlElement(ElementName = "shader")]
		public Shader Shader { get; set; }
	}

	[XmlRoot(ElementName = "shader-pdb")]
	public class Shaderpdb
	{
		[XmlElement(ElementName = "shaders")]
		public List<Shader> Shaders { get; set; }
	}
}
