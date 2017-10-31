namespace TagTool.Layouts
{
    /// <summary>
    /// A tag block field in a tag layout.
    /// </summary>
    public class TagBlockTagLayoutField : TagLayoutField
    {
        public TagBlockTagLayoutField(string name, TagLayout elementLayout)
            : base(name)
        {
            ElementLayout = elementLayout;
        }

        /// <summary>
        /// The layout of each element in the tag block.
        /// </summary>
        public TagLayout ElementLayout { get; set; }

        public override void Accept(ITagLayoutFieldVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
