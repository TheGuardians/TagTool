namespace TagTool.Layouts
{
    /// <summary>
    /// A string field in a tag layout.
    /// </summary>
    public class StringTagLayoutField : TagLayoutField
    {
        public StringTagLayoutField(string name, int size)
            : base(name)
        {
            Size = size;
        }

        /// <summary>
        /// The size of the string buffer in bytes.
        /// </summary>
        public int Size { get; set; }

        public override void Accept(ITagLayoutFieldVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
