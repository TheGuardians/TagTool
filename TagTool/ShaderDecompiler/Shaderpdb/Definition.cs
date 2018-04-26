using System.Xml.Serialization;
using System.Collections.Generic;

namespace TagTool.ShaderDecompiler.UPDB
{
	[XmlRoot(ElementName = "vfetch")]
	public class Vfetch
	{
		[XmlElement(ElementName = "Address")]
		public string Address = "";
		[XmlAttribute(AttributeName = "Register")]
		public string Register = "";
		[XmlAttribute(AttributeName = "DestSwizzle")]
		public string DestSwizzle = "";
		[XmlAttribute(AttributeName = "string")]
		public string String = "";
		[XmlAttribute(AttributeName = "End")]
		public string End = "";
	}
	[XmlRoot(ElementName = "VfetchInfo")]
	public class VfetchInfo
	{
		[XmlElement(ElementName = "Vfetch")]
		public List<Vfetch> Vfetch { get; set; }
	}

	[XmlRoot(ElementName = "Interpolator")]
	public class Interpolator
	{
		[XmlAttribute(AttributeName = "Register")]
		public string Register { get; set; }
		[XmlAttribute(AttributeName = "Semantic")]
		public string Semantic { get; set; }
		[XmlAttribute(AttributeName = "Mask")]
		public string Mask { get; set; }
	}
	[XmlRoot(ElementName = "InterpolatorInfo")]
	public class InterpolatorInfo
	{
		[XmlElement(ElementName = "Interpolator")]
		public List<Interpolator> Interpolator { get; set; }
	}

	[XmlRoot(ElementName = "Float")]
	public class Float
	{
		[XmlAttribute(AttributeName = "Register")]
		public string Register { get; set; }
		[XmlAttribute(AttributeName = "value0")]
		public string Value0 { get; set; }
		[XmlAttribute(AttributeName = "value1")]
		public string Value1 { get; set; }
		[XmlAttribute(AttributeName = "value2")]
		public string Value2 { get; set; }
		[XmlAttribute(AttributeName = "value3")]
		public string Value3 { get; set; }
	}
	[XmlRoot(ElementName = "LiteralFloats")]
	public class LiteralFloats
	{
		[XmlElement(ElementName = "Float")]
		public List<Float> Float { get; set; }
	}

	[XmlRoot(ElementName = "Int")]
	public class Int
	{
		[XmlElement(ElementName = "Register")]
		public string Register = "";
		[XmlAttribute(AttributeName = "Count")]
		public string Count = "";
		[XmlAttribute(AttributeName = "Start")]
		public string Start = "";
		[XmlAttribute(AttributeName = "Inc")]
		public string Increment = "";
	}
	[XmlRoot(ElementName = "LiteralInts")]
	public class LiteralInts
	{
		[XmlElement(ElementName = "Float")]
		public List<Int> Int { get; set; }
	}

	[XmlRoot(ElementName = "Bool")]
	public class Bool
	{
		[XmlElement(ElementName = "Register")]
		public string Register = "";
		[XmlAttribute(AttributeName = "Value")]
		public string Value = "";
	}
	[XmlRoot(ElementName = "LiteralBools")]
	public class LiteralBools
	{
		[XmlElement(ElementName = "Float")]
		public List<Bool> Bool { get; set; }
	}

	[XmlRoot(ElementName = "shader")]
	public class Shader
	{
		[XmlElement(ElementName = "VfetchInfo")]
		public VfetchInfo VfetchInfo { get; set; }
		[XmlElement(ElementName = "InterpolatorInfo")]
		public InterpolatorInfo InterpolatorInfo { get; set; }
		[XmlElement(ElementName = "LiteralFloats")]
		public LiteralFloats LiteralFloats { get; set; }
		[XmlElement(ElementName = "LiteralInts")]
		public LiteralInts LiteralInts { get; set; }
		[XmlElement(ElementName = "LiteralBools")]
		public LiteralBools LiteralBools { get; set; }
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
		public Shaders Shaders { get; set; }
	}
}
