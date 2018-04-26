/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */

 using System.Xml.Serialization;
using System.Collections.Generic;


// TODO: this needs a LOT of cleanup

namespace TagTool.ShaderDecompiler.UPDB
{
	[XmlRoot(ElementName = "type")]
	public class Type
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "class")]
		public string Class { get; set; }
		[XmlAttribute(AttributeName = "type")]
		public string _type { get; set; }
		[XmlAttribute(AttributeName = "rows")]
		public string Rows { get; set; }
		[XmlAttribute(AttributeName = "columns")]
		public string Columns { get; set; }
	}

	[XmlRoot(ElementName = "constant")]
	public class Constant
	{
		[XmlElement(ElementName = "type")]
		public Type Type { get; set; }
		[XmlAttribute(AttributeName = "register")]
		public string Register { get; set; }
		[XmlAttribute(AttributeName = "count")]
		public string Count { get; set; }
	}

	[XmlRoot(ElementName = "constanttable")]
	public class Constanttable
	{
		[XmlElement(ElementName = "constant")]
		public Constant Constant { get; set; }
	}

	[XmlRoot(ElementName = "statement")]
	public class Statement
	{
		[XmlAttribute(AttributeName = "pc")]
		public string Pc { get; set; }
		[XmlAttribute(AttributeName = "file")]
		public string File { get; set; }
		[XmlAttribute(AttributeName = "line")]
		public string Line { get; set; }
		[XmlAttribute(AttributeName = "scope")]
		public string Scope { get; set; }
		[XmlAttribute(AttributeName = "lastinstruction")]
		public string Lastinstruction { get; set; }
	}

	[XmlRoot(ElementName = "sourcemap")]
	public class Sourcemap
	{
		[XmlElement(ElementName = "statement")]
		public List<Statement> Statement { get; set; }
	}

	[XmlRoot(ElementName = "shader")]
	public class Shader
	{
		[XmlElement(ElementName = "constanttable")]
		public List<Constant> Constanttable { get; set; }
		[XmlElement(ElementName = "sourcemap")]
		public List<Statement> Sourcemap { get; set; }
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
		public string VfetchInfo { get; set; }
		[XmlElement(ElementName = "InterpolatorInfo")]
		public string InterpolatorInfo { get; set; }
		[XmlElement(ElementName = "LiteralFloats")]
		public string LiteralFloats { get; set; }
		[XmlElement(ElementName = "LiteralInts")]
		public string LiteralInts { get; set; }
		[XmlElement(ElementName = "LiteralBools")]
		public string LiteralBools { get; set; }
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
