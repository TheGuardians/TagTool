using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using System.Linq;

namespace TagTool.Commands.Forge
{
    class AddForgeCategoryCommand : Command
    {
        private GameCache Cache { get; }
        private ForgeGlobalsDefinition ForgeGlobals;
        private string CategoryName;
        private short ParentIndex = -1;
        private string CategoryDescription;

        public AddForgeCategoryCommand(GameCache cache) :
            base(true,

                "AddForgeCategory",
                "Add a custom category to host Forge objects.",

                "AddForgeCategory <name> <parent category> [description]",

                "Adds a custom category to host Forge objects."
                + "\nCategory names containing spaces must be in quotes."
                + "\nParent category must be either the block index or exact name of the parent category. (-1 for none)")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            using (var cacheStream = Cache.OpenCacheRead())
            {
                if (args.Count > 3 || args.Count < 2)
                    return new TagToolError(CommandError.ArgCount);

                ForgeGlobals = Cache.Deserialize<ForgeGlobalsDefinition>(cacheStream, Cache.TagCache.GetTag($"multiplayer\\forge_globals.forg"));

                CategoryName = args[0].ToUpper();

                var nameIndices = new List<short>();

                if (!short.TryParse(args[1], out ParentIndex))
                {
                    foreach (var cat in ForgeGlobals.PaletteCategories)
                    {
                        if (cat.Name.ToLower() == args[1].ToLower())
                        {
                            nameIndices.Add((short)ForgeGlobals.PaletteCategories.IndexOf(cat));
                        }
                    }

                    switch (nameIndices.Count)
                    {
                        case 0:
                            return new TagToolError(CommandError.CustomError, "Parent category could not be found.");
                        case 1:
                            break;
                        default:
                            return new TagToolWarning("Multiple categories which this name were found. Parent category will be the last encountered.");
                    }

                    ParentIndex = nameIndices.Last();
                }
                else if (ParentIndex >= ForgeGlobals.PaletteCategories.Count || ParentIndex < -1)
                    return new TagToolError(CommandError.CustomError, $"Parent category index must be less than the current category count of {ForgeGlobals.PaletteCategories.Count}.");

                if (args.Count == 3)
                {
                    CategoryDescription = args[2];

                    if (string.IsNullOrEmpty(CategoryDescription))
                    {
                        ForgeGlobals.Descriptions.Add(new ForgeGlobalsDefinition.Description()
                        {
                            Text = CategoryDescription
                        });
                    }
                }

                ForgeGlobals.PaletteCategories.Add(new ForgeGlobalsDefinition.PaletteCategory()
                {
                    Name = CategoryName,
                    DescriptionIndex = (short)(string.IsNullOrEmpty(CategoryDescription) ? (ForgeGlobals.Descriptions.Count - 1) : -1),
                    ParentCategoryIndex = ParentIndex
                });

                Console.WriteLine($"\nPalette category \"{CategoryName}\" added at index {ForgeGlobals.PaletteCategories.Count - 1}.");
                Cache.Serialize(cacheStream, Cache.TagCache.GetTag($"multiplayer\\forge_globals.forg"), ForgeGlobals);
            }

            return true;
        }
    }
}