/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Xml2CSharp
{
	[XmlRoot(ElementName = "file")]
	public class File
	{
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "path")]
		public string Path { get; set; }
		[XmlAttribute(AttributeName = "time")]
		public string Time { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "files")]
	public class Files
	{
		[XmlElement(ElementName = "file")]
		public File File { get; set; }
	}

	[XmlRoot(ElementName = "argument")]
	public class Argument
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "arguments")]
	public class Arguments
	{
		[XmlElement(ElementName = "argument")]
		public List<Argument> Argument { get; set; }
	}

	[XmlRoot(ElementName = "tool")]
	public class Tool
	{
		[XmlElement(ElementName = "arguments")]
		public Arguments Arguments { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
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

	[XmlRoot(ElementName = "shader")]
	public class Shader
	{
		[XmlElement(ElementName = "constanttable")]
		public string Constanttable { get; set; }
		[XmlElement(ElementName = "sourcemap")]
		public Sourcemap Sourcemap { get; set; }
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
		public LiteralFloats LiteralFloats { get; set; }
		[XmlElement(ElementName = "LiteralInts")]
		public string LiteralInts { get; set; }
		[XmlElement(ElementName = "LiteralBools")]
		public string LiteralBools { get; set; }
		[XmlAttribute(AttributeName = "ZPass")]
		public string ZPass { get; set; }
		[XmlAttribute(AttributeName = "pdbHint")]
		public string PdbHint { get; set; }
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
		[XmlElement(ElementName = "files")]
		public Files Files { get; set; }
		[XmlElement(ElementName = "tool")]

		public Tool Tool { get; set; }
		[XmlElement(ElementName = "shaders")]
		public Shaders Shaders { get; set; }
		[XmlAttribute(AttributeName = "pdbHint")]
		public string PdbHint { get; set; }
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }
	}
}
