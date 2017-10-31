using System;
using System.Collections.Generic;

namespace TagTool.Layouts
{
    /// <summary>
    /// An enum field in a tag layout.
    /// </summary>
    public class EnumTagLayoutField : TagLayoutField
    {
        public EnumTagLayoutField(string name, EnumLayout layout)
            : base(name)
        {
            Layout = layout;
        }

        /// <summary>
        /// The layout of the enum field.
        /// </summary>
        public EnumLayout Layout { get; set; }

        public override void Accept(ITagLayoutFieldVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    /// <summary>
    /// A layout for an enum in a tag layout.
    /// </summary>
    public class EnumLayout
    {
        private readonly List<EnumValue> _values = new List<EnumValue>();

        /// <summary>
        /// Creates a named enum layout.
        /// </summary>
        /// <param name="name">The name of the layout.</param>
        /// <param name="underlyingType">The underlying type of the enum. Must be an integer type.</param>
        public EnumLayout(string name, BasicFieldType underlyingType)
        {
            switch (underlyingType)
            {
                case BasicFieldType.UInt8:
                case BasicFieldType.Int8:
                case BasicFieldType.UInt16:
                case BasicFieldType.Int16:
                case BasicFieldType.UInt32:
                case BasicFieldType.Int32:
                case BasicFieldType.StringID:
                    break;
                default:
                    throw new ArgumentException("The underlying type of an enum must be an integer type.");
            }
            Name = name;
            UnderlyingType = underlyingType;
            Values = _values.AsReadOnly();
        }

        /// <summary>
        /// Gets the name of the enum.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the underlying type of the enum.
        /// Must be an integer type.
        /// </summary>
        public BasicFieldType UnderlyingType { get; private set; }

        /// <summary>
        /// Gets a list of the values in the enum.
        /// </summary>
        public IReadOnlyList<EnumValue> Values { get; private set; }

        /// <summary>
        /// Adds a value to the enum which follows the last value added.
        /// If no values are in the enum, its value will be 0.
        /// </summary>
        /// <param name="name">The name of the value.</param>
        /// <returns>The created <see cref="EnumValue"/>.</returns>
        public EnumValue Add(string name)
        {
            var val = (_values.Count > 0) ? _values[_values.Count - 1].Value + 1 : 0;
            return Add(name, val);
        }

        /// <summary>
        /// Adds a value to the enum.
        /// </summary>
        /// <param name="name">The name of the value.</param>
        /// <param name="val">The value.</param>
        /// <returns>The created <see cref="EnumValue"/>.</returns>
        public EnumValue Add(string name, int val)
        {
            var result = new EnumValue(name, val);
            Add(result);
            return result;
        }

        /// <summary>
        /// Adds a value to the enum.
        /// </summary>
        /// <param name="val">The value.</param>
        public void Add(EnumValue val)
        {
            _values.Add(val);
        }

        /// <summary>
        /// Adds a range of values to the enum.
        /// </summary>
        /// <param name="values">The values to add.</param>
        public void AddRange(IEnumerable<EnumValue> values)
        {
            foreach (var val in values)
                _values.Add(val);
        }
    }

    /// <summary>
    /// Maps an enum value name to an integer value.
    /// </summary>
    public class EnumValue
    {
        public EnumValue(string name, int value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets the name of the value.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public int Value { get; private set; }
    }
}
