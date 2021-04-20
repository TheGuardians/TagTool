using SimpleJSON;
using System;
using System.IO;
using TagTool.Commands.Common;

namespace TagTool.Geometry
{
    /// <summary>
    /// This class loads, reads, tokenises, and parses a simple file format
    /// designed to store data exported from the Blender modeling program. 
    /// </summary>
    class BlenderPhmoReader
    {
        public string filename;

        public BlenderPhmoReader(string fname)
        {
            filename = fname;
        }

        public JSONNode ReadFile()
        {
            string contents;
            try
            {
                // open the file as a text-stream
                StreamReader sr = new StreamReader(filename);
                contents = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException || ex is DirectoryNotFoundException)
                    new TagToolError(CommandError.FileNotFound);
                return null;
            };

            //parse the file as json
            var json = JSON.Parse(contents);

            return json;
        }

    }
}
