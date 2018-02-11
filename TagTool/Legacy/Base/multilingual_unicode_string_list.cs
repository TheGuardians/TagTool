using System.Collections.Generic;

namespace TagTool.Legacy
{
    public abstract class multilingual_unicode_string_list
    {
        //using lists rather than a bunch of ints makes it easier
        //to access a specific language (just use language index)
        public List<int> Indices;
        public List<int> Lengths;

        public multilingual_unicode_string_list()
        {
            Indices = new List<int>();
            Lengths = new List<int>();
        }
    }
}
