namespace TagTool.Layouts
{
    /// <summary>
    /// An array of fields in a tag layout.
    /// </summary>
    public class ArrayTagLayoutField : TagLayoutField
    {
        public ArrayTagLayoutField(TagLayoutField underlyingField, int count)
            : base(underlyingField.Name)
        {
            UnderlyingField = underlyingField;
            Count = count;
        }

        /// <summary>
        /// Gets the repeated field.
        /// </summary>
        public TagLayoutField UnderlyingField { get; private set; }

        /// <summary>
        /// The number of elements in the array.
        /// </summary>
        public int Count { get; set; }

        public override void Accept(ITagLayoutFieldVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}