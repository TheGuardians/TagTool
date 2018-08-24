using System;
using System.Reflection;

namespace TagTool.Serialization
{
	/// <summary>
	/// Class for pairing of <see cref="System.Reflection.FieldInfo"/> and <see cref="Serialization.TagFieldAttribute"/>.
	/// </summary>
	public class TagFieldInfo
	{
		/// <summary>
		/// Constructs a <see cref="TagFieldInfo"/> from a <see cref="System.Reflection.FieldInfo"/> and <see cref="Serialization.TagFieldAttribute"/>.
		/// </summary>
		/// <param name="field">The <see cref="FieldInfo"/> for the field.</param>
		/// <param name="attribute">The <see cref="TagFieldAttribute"/> for the field.</param>
		/// <param name="offset">The offset (in bytes) of the field in it's structure.</param>
		/// <param name="size">The size of the field (in bytes).</param>
		public TagFieldInfo(FieldInfo field, TagFieldAttribute attribute, uint offset, uint size)
		{
			this.Field = field;
			this.Type = this.Field.FieldType;
			this.Attribute = attribute;
			this.Offset = offset;
		}

		/// <summary>
		/// Gets the field offset (in bytes) that was used in construction.
		/// </summary>
		public uint Offset { get; }

		/// <summary>
		/// Gets the field size (in bytes) that was used in construction.
		/// </summary>
		public uint Size { get; }

		/// <summary>
		/// Gets the type of the field.
		/// </summary>
		public Type Type { get; }

		/// <summary>
		/// Gets the <see cref="System.Reflection.FieldInfo"/> that was used in construction.
		/// </summary>
		public FieldInfo Field { get; }

		/// <summary>
		/// Gets the <see cref="Serialization.TagFieldAttribute"/> that was used in construction.
		/// </summary>
		public TagFieldAttribute Attribute { get; }
	}
}