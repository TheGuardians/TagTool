using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using System.Linq;

namespace TagTool.Commands.Forge
{
    class AddForgeItemCommand : Command
    {
        private GameCache Cache { get; }
        private ForgeGlobalsDefinition ForgeGlobals;
        private CachedTag Item;
        private string ItemName;
        private short PaletteCategoryIndex = -1;
        private string PaletteCategoryName;
        private string ItemDescription;

        public AddForgeItemCommand(GameCache cache) :
            base(true,

                "AddForgeItem",
                "Add an item to an existing forge palette category.",

                "AddForgeItem <name> <category> [setters] [description] <tag name>",

                "Adds an item to an existing Forge palette category."
                + "\nIf they contain spaces, put name and/or category in quotes."
                + "\nCategory can be either a block index, exact name, or * for last created)."
                + "\nWhen using \"setters\" argument, each setter must be on a new line"
                + "\nwith the following format: <target> <type> <value>")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            using (var cacheStream = Cache.OpenCacheReadWrite())
            {
                if (args.Count > 4 || args.Count < 2)
                    return new TagToolError(CommandError.ArgCount);

                if (!Cache.TagCache.TryGetCachedTag(args.Last(), out Item))
                    return new TagToolError(CommandError.TagInvalid);
                else if (!Item.IsInGroup("obje"))
                    return new TagToolError(CommandError.CustomError, "Tag is not an object.");

                ItemName = args[0].ToUpper();

                // get object type from group

                ForgeGlobalsDefinition.PaletteItemType itemType;
                switch (Item.Group.ToString().ToLower())
                {
                    case "effect_scenery":
                    case "sound_scenery":
                        itemType = ForgeGlobalsDefinition.PaletteItemType.Effects;
                        break;
                    case "equipment":
                        itemType = ForgeGlobalsDefinition.PaletteItemType.Equipment;
                        break;
                    case "weapon":
                        itemType = ForgeGlobalsDefinition.PaletteItemType.Weapon;
                        break;
                    case "vehicle":
                        itemType = ForgeGlobalsDefinition.PaletteItemType.Vehicle;
                        break;
                    default:
                        itemType = ForgeGlobalsDefinition.PaletteItemType.Prop;
                        break;
                }

                ForgeGlobals = Cache.Deserialize<ForgeGlobalsDefinition>(cacheStream, Cache.TagCache.GetTag($"multiplayer\\forge_globals.forg"));

                var nameIndices = new List<short>();

                if (!short.TryParse(args[1], out PaletteCategoryIndex))
                {
                    if (args[1].ToLower() == "*" || args[1].ToLower() == "last")
                        PaletteCategoryIndex = (short)(ForgeGlobals.PaletteCategories.Count() - 1);
                    else
                    {
                        foreach (var category in ForgeGlobals.PaletteCategories)
                        {
                            if (category.Name.ToLower() == args[1].ToLower())
                                nameIndices.Add((short)ForgeGlobals.PaletteCategories.IndexOf(category));
                        }

                        switch (nameIndices.Count)
                        {
                            case 0:
                                return new TagToolError(CommandError.CustomError, "Category could not be found.");
                            case 1:
                                break;
                            default:
                                return new TagToolWarning("Multiple categories which this name were found. Category will be the last encountered.");
                        }

                        PaletteCategoryIndex = nameIndices.Last();
                    }
                }
                else if (PaletteCategoryIndex >= ForgeGlobals.PaletteCategories.Count || PaletteCategoryIndex < -1)
                    return new TagToolError(CommandError.CustomError, $"Category index must be less than the current category count of {ForgeGlobals.PaletteCategories.Count}.");

                // get setters and description

                var setterList = new List<ForgeGlobalsDefinition.PaletteItem.Setter>();
                string line;

                if (args.Count > 3)
                {
                    if (args[2].ToLower() == "setters")
                    {
                        while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
                        {
                            var currentSetter = new ForgeGlobalsDefinition.PaletteItem.Setter();
                            var segments = line.Split(' ');

                            if (segments.Count() != 3)
                                return new TagToolError(CommandError.CustomError, "Setters must be in this format: <Target> <Real/Integer> <Value>");

                            if (!Enum.TryParse(segments[0], out currentSetter.Target))
                                return new TagToolError(CommandError.CustomError, "Setter target could not be parsed.");

                            if (!Enum.TryParse(segments[1], out currentSetter.Type))
                                return new TagToolError(CommandError.CustomError, "Setter type could not be parsed.");

                            if (!float.TryParse(segments[2], out float value))
                                return new TagToolError(CommandError.CustomError, "Setter value could not be parsed.");

                            switch (currentSetter.Type)
                            {
                                case ForgeGlobalsDefinition.PaletteItem.SetterType.Integer:
                                    currentSetter.IntegerValue = (int)value;
                                    currentSetter.RealValue = 0f;
                                    break;
                                case ForgeGlobalsDefinition.PaletteItem.SetterType.Real:
                                    currentSetter.IntegerValue = 0;
                                    currentSetter.RealValue = value;
                                    break;
                            }

                            setterList.Add(currentSetter);
                        }
                    }
                    else
                        ItemDescription = args[2];

                    if (args.Count == 5)
                        ItemDescription = args[3];
                }

                if (!string.IsNullOrEmpty(ItemDescription))
                {
                    ForgeGlobals.Descriptions.Add(new ForgeGlobalsDefinition.Description()
                    {
                        Text = ItemDescription
                    });
                }

                ForgeGlobals.Palette.Add(new ForgeGlobalsDefinition.PaletteItem()
                {
                    Name = ItemName,
                    Type = itemType,
                    CategoryIndex = PaletteCategoryIndex,
                    DescriptionIndex = (short)(!string.IsNullOrEmpty(ItemDescription) ? (ForgeGlobals.Descriptions.Count - 1) : -1),
                    MaxAllowed = 0,
                    Object = Item,
                    Setters = setterList
                });

                PaletteCategoryName = ForgeGlobals.PaletteCategories[PaletteCategoryIndex].Name;
                Cache.Serialize(cacheStream, Cache.TagCache.GetTag($"multiplayer\\forge_globals.forg"), ForgeGlobals);

                // check for multiplayerobject block

                var itemDefinition = Cache.Deserialize(cacheStream, Item);

                if (((GameObject)itemDefinition).MultiplayerObject.Count == 0)
                {
                    ((GameObject)itemDefinition).MultiplayerObject.Add(new GameObject.MultiplayerObjectBlock()
                    {
                        DefaultSpawnTime = 30,
                        DefaultAbandonTime = 30
                    });

                    Cache.Serialize(cacheStream, Item, itemDefinition);
                }
            }

            Console.WriteLine($"\nItem added to category {PaletteCategoryIndex}: \"{PaletteCategoryName}\".");
            return true;
        }
    }
}