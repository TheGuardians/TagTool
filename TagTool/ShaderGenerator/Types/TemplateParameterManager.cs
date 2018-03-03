using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.ShaderGenerator.Types
{
    class TemplateParameterManager
    {
        public List<TemplateParameter> Parameters { get; internal set; }

        public TemplateParameterManager(List<TemplateParameter> parameters)
        {
            Parameters = parameters;
        }
    }
}
