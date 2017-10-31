using System.IO;

namespace TagTool.Layouts
{
    /// <summary>
    /// Base for a class which can write tag layouts to files.
    /// </summary>
    public abstract class TagLayoutWriter
    {
        /// <summary>
        /// Gets the suggested filename for a layout.
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <returns>A suggested filename to write the layout to.</returns>
        public abstract string GetSuggestedFileName(TagLayout layout);

        /// <summary>
        /// Writes a layout to a stream.
        /// </summary>
        /// <param name="layout">The layout to write.</param>
        /// <param name="writer">The writer to write to.</param>
        public abstract void WriteLayout(TagLayout layout, TextWriter writer);

        /// <summary>
        /// Writes a layout to a file.
        /// </summary>
        /// <param name="layout">The layout to write.</param>
        /// <param name="path">The path to write to.</param>
        public void WriteLayout(TagLayout layout, string path)
        {
            using (var writer = new StreamWriter(File.Open(path, FileMode.Create, FileAccess.Write)))
                WriteLayout(layout, writer);
        }
    }
}
