using TagTool.Cache;
using TagTool.Commands;
using TagTool.TagDefinitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Commands.CollisionModels
{
    class ExtractModelCommand : Command
    {
        private CollisionModel Definition { get; }

        public ExtractModelCommand(CollisionModel definition) :
            base(CommandFlags.Inherit,
                
                "ExtractModel",
                "",
                
                "ExtractModel <File>",
                
                "")
        {
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var file = new FileInfo(args[0]);

            using (var _writer = new StreamWriter(file.Create()))
            {
                _writer.WriteLine("# Extracted on {0}", DateTime.Now);

                foreach (var region in Definition.Regions)
                {
                    foreach (var permutation in region.Permutations)
                    {
                        foreach (var bsp in permutation.Bsps)
                        {
                            foreach (var vertex in bsp.Geometry.Vertices)
                            {
                                _writer.WriteLine("v {0} {1} {2}", vertex.Point.X, vertex.Point.Y, vertex.Point.Z);
                            }

                            foreach (var entry in bsp.Geometry.GenerateIndices())
                            {
                                _writer.Write("f");
                                foreach (var index in entry.Value)
                                    _writer.Write($" {index}");
                                _writer.WriteLine();
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
