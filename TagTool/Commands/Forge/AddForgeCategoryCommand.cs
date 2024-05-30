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

                "AddForgeCategory <name> <parent category> [desc]",

                "Adds a custom category to host Forge objects."
                + "\nCategory names containing spaces must be in quotes."
                + "\nParent category can be an index (-1 = root, * = last), or an exact name.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            using (var cacheStream = Cache.OpenCacheReadWrite())
            {
                if (args.Count > 3 || args.Count < 2)
                    return new TagToolError(CommandError.ArgCount);

                ForgeGlobals = Cache.Deserialize<ForgeGlobalsDefinition>(cacheStream, Cache.TagCache.GetTag($"multiplayer\\forge_globals.forg"));

                CategoryName = args[0].ToUpper();

                var nameIndices = new List<short>();

                if (!short.TryParse(args[1], out ParentIndex))
                {
                    if (args[1] == "*" || args[1].ToLower() == "last")
                        ParentIndex = (short)(ForgeGlobals.PaletteCategories.Count - 1);

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

                short descriptionIndex = -1;
                if (args.Count == 3 && args[2].ToLower().StartsWith("desc"))
                {
                    Console.WriteLine("Enter your category description:");
                    CategoryDescription = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(CategoryDescription))
                    {
                        ForgeGlobals.Descriptions.Add(new ForgeGlobalsDefinition.Description()
                        {
                            Text = CategoryDescription
                        });

                        descriptionIndex = (short)(ForgeGlobals.Descriptions.Count - 1);
                    }
                }

                ForgeGlobals.PaletteCategories.Add(new ForgeGlobalsDefinition.PaletteCategory()
                {
                    Name = CategoryName,
                    DescriptionIndex = descriptionIndex,
                    ParentCategoryIndex = ParentIndex
                });

                Console.WriteLine($"\nPalette category \"{CategoryName}\" added at index {ForgeGlobals.PaletteCategories.Count - 1}.");
                Cache.Serialize(cacheStream, Cache.TagCache.GetTag($"multiplayer\\forge_globals.forg"), ForgeGlobals);
            }

            return true;
        }
    }
}