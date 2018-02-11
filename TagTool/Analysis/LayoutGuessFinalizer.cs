using TagTool.Common;
using TagTool.Layouts;

namespace TagTool.Analysis
{
    /// <summary>
    /// Converts tag layout guesses into final layouts.
    /// </summary>
    public class LayoutGuessFinalizer : ITagElementGuessVisitor
    {
        /// <summary>
        /// Makes a layout from a layout guess.
        /// </summary>
        /// <param name="layout">The layout guess.</param>
        /// <param name="name">The name of the final layout.</param>
        /// <param name="groupTag">The group tag of the final layout. Can be <c>null</c>.</param>
        /// <returns></returns>
        public static TagLayout MakeLayout(TagLayoutGuess layout, string name, Tag groupTag)
        {
            var result = new TagLayout(name, layout.Size, groupTag);
            var finalizer = new LayoutGuessFinalizer(result, 0);
            finalizer.ProcessLayout(layout);
            return result;
        }

        private readonly TagLayout _result;
        private int _nextTagBlock;

        private LayoutGuessFinalizer(TagLayout result, int nextTagBlock)
        {
            _result = result;
            _nextTagBlock = nextTagBlock;
        }

        private void ProcessLayout(TagLayoutGuess layout)
        {
            for (uint offset = 0; offset < layout.Size; offset += 4)
            {
                var guess = layout.TryGet(offset);
                if (guess != null)
                {
                    guess.Accept(offset, this);
                    offset += guess.Size - 4;
                }
                else
                {
                    var remaining = layout.Size - offset;
                    switch (remaining)
                    {
                        case 3:
                            _result.Add(MakeField(offset, BasicFieldType.Int16));
                            _result.Add(MakeField(offset, BasicFieldType.Int8));
                            break;
                        case 2:
                            _result.Add(MakeField(offset, BasicFieldType.Int16));
                            break;
                        case 1:
                            _result.Add(MakeField(offset, BasicFieldType.Int8));
                            break;
                        default: // >= 4
                            _result.Add(MakeField(offset, BasicFieldType.Int32));
                            break;
                    }
                }
            }
        }

        public void Visit(uint offset, TagBlockGuess guess)
        {
            var name = string.Format("Tag Block {0}", _nextTagBlock);
            _nextTagBlock++;
            var elementLayout = new TagLayout(name, guess.ElementLayout.Size, new Tag(0));
            var finalizer = new LayoutGuessFinalizer(elementLayout, _nextTagBlock);
            finalizer.ProcessLayout(guess.ElementLayout);
            var align = guess.Align;
            if ((guess.ElementLayout.Size & (guess.Align - 1)) != 0)
                align = 0;
            _nextTagBlock = finalizer._nextTagBlock;
            _result.Add(new TagBlockTagLayoutField(MakeName(offset), elementLayout) { DataAlign = align });
        }

        public void Visit(uint offset, DataReferenceGuess guess)
        {
            var field = MakeField(offset, BasicFieldType.DataReference);
            _result.Add(field);
            field.DataAlign = guess.Align;
        }

        public void Visit(uint offset, TagReferenceGuess guess)
        {
            _result.Add(MakeField(offset, BasicFieldType.TagReference));
        }

        public void Visit(uint offset, ResourceReferenceGuess guess)
        {
            _result.Add(MakeField(offset, BasicFieldType.ResourceReference));
        }

        private static BasicTagLayoutField MakeField(uint offset, BasicFieldType type)
        {
            return new BasicTagLayoutField(MakeName(offset), type);
        }

        private static string MakeName(uint offset)
        {
            return string.Format("Unknown {0:X}", offset);
        }
    }
}
