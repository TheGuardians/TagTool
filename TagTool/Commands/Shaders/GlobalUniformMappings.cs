using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Shaders;
using static TagTool.Shaders.ShaderParameter;
using static TagTool.Tags.Definitions.RenderMethodTemplate;
using static TagTool.Tags.Definitions.RenderMethodTemplate.DrawModeRegisterOffsetBlock;

namespace TagTool.Commands.Shaders
{
    class GlobalUniformMappings
    {
        public class Mapping
        {
            public readonly ShaderParameter.RType ExpectedType;
            public string Name;
            public int RegisterIndex, ArgumentIndex;
            public DrawModeRegisterOffsetType OffsetType;
            public ShaderMode shaderMode;

            public Mapping(string name, ShaderParameter.RType type, int register_index, int argument_index, DrawModeRegisterOffsetType offset_type, ShaderMode shadermode)
            {
                Name = name;
                ExpectedType = type;
                RegisterIndex = register_index;
                ArgumentIndex = argument_index;
                OffsetType = offset_type;
                shaderMode = shadermode;
            }
        }

        static Mapping[] CombineMappings()
        {
            List<Mapping> mappings = new List<Mapping>();

            mappings.AddRange(UnknownBlock);

            return mappings.ToArray();
        }

        static Mapping[] MappingsSource = CombineMappings();

        public static Mapping GetMapping(string name, RType register_type, ShaderMode mode)
        {
            string _name = name.ToLower();

            var query = MappingsSource.Where(
                mapping => mapping.Name == _name &&
                mapping.ExpectedType == register_type &&
                mapping.shaderMode == mode);

            if (query.Count() > 1) throw new Exception("Invalid data/query");
            return query.Count() > 0 ? query.ElementAt(0) : null;
        }

        static Mapping[] UnknownBlock = new Mapping[]
        {


        };

    }
}
