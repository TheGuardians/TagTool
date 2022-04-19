using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using System.Linq;

namespace TagTool.Commands.Modding
{
    class CreateCharacterType : Command
    {
        private GameCache Cache { get; }
        private ModGlobalsDefinition ModGlobals;
        private Globals Globals;
        private CachedTag UnitTag;
        private bool Simple = true;
        private bool Binocs = false;
        private int MpRepIndex = 0;
        private int CampRepIndex = 0;
        private string CharacterName;
        private StringId CharacterStringID;

        public CreateCharacterType(GameCache cache) :
            base(true,

                "CreateCharacterType",
                "Builds a character type from a biped tag.",

                "CreateCharacterType [campaigndistinct] [binocs] <bipd tagname>",

                "Builds a character type from the provided biped tag."
                + "\nThe \"campaigndistinct\" argument enables creation of separate Campaign and Multiplayer representations."
                + "By default, they are the same.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            bool campaignDistinct = false;
            
            if (args.Count > 0)
            {
                if (!Cache.TagCache.TryGetCachedTag(args.Last(), out UnitTag))
                    return new TagToolError(CommandError.TagInvalid);

                if (args.Contains("campaigndistinct"))
                {
                    campaignDistinct = true;
                    Simple = false;
                }
                if (args.Contains("binocs"))
                    Binocs = true;
            }
            else
                return new TagToolError(CommandError.ArgCount);

            OpenGlobalTags();

            // Wizard to set up a new playable character

            SectionBreak();
            Console.WriteLine("Enter the character name for display: ");
            CharacterName = Console.ReadLine().Trim();

            SectionBreak();

            if (Simple)
            {
                MpRepIndex = CreatePlayerRepresentation(null);
                CampRepIndex = MpRepIndex;
                SectionBreak();
            }
            else
            {
                if (campaignDistinct)
                {
                    CampRepIndex = CreatePlayerRepresentation("campaign");
                    SectionBreak();
                    MpRepIndex = CreatePlayerRepresentation("multiplayer");
                    SectionBreak();
                }
            }

            CreateNewCharacterType();
            SectionBreak();

            int setIndex = Simple ? 0 : GetIntFromUser("Enter the character set index (-1 to create new): ");

            ModGlobalsDefinition.PlayerCharacterSet current_set;

            if ( setIndex == -1)
            {
                current_set = new ModGlobalsDefinition.PlayerCharacterSet();
                CreateNewCharacterSet(current_set);
                ModGlobals.PlayerCharacterSets.Add(current_set);
            }
            else
            {
                current_set = ModGlobals.PlayerCharacterSets[setIndex];
            }

            CreateNewPlayerCharacter(current_set);

            SectionBreak();

            Console.WriteLine("Done!");
            SaveTags();
            return true;
        }

        private void OpenGlobalTags()
        {
            using (var cacheStream = Cache.OpenCacheRead())
            {
                Globals = Cache.Deserialize<Globals>(cacheStream, Cache.TagCache.GetTag($"globals\\globals.matg"));
                ModGlobals = Cache.Deserialize<ModGlobalsDefinition>(cacheStream, Cache.TagCache.GetTag($"multiplayer\\mod_globals.modg"));
            }
        }

        private void SaveTags()
        {
            using (var stream = Cache.OpenCacheReadWrite())
            {
                Cache.Serialize(stream, Cache.TagCache.GetTag($"multiplayer\\mod_globals.modg"), ModGlobals);
                Cache.Serialize(stream, Cache.TagCache.GetTag($"globals\\globals.matg"), Globals);
            }
        }

        private int CreatePlayerRepresentation(string type)
        {
            Globals.PlayerRepresentationBlock rep = new Globals.PlayerRepresentationBlock();

            if (!string.IsNullOrEmpty(type))
                type += " ";

            if (Simple)
            {
                rep.Name = GetStringId(CharacterName.ToLower().Replace(' ','_'));
                CharacterStringID = rep.Name;
            }
            else
                rep.Name = GetStringIdFromUser($"Enter the {type}representation name: ");

            rep.FirstPersonHands = GetTag("Enter the first person hands render model tag (use \"skip\" or hit enter to skip): ");
            rep.FirstPersonBody = GetTag("Enter the first person body render model tag (use \"skip\" or hit enter to skip): ");
            rep.ThirdPersonUnit = (type == "campaign ") ? GetTag("Enter the third person unit biped tag (use \"skip\" or hit enter to skip): ") : UnitTag;
            rep.ThirdPersonVariant = GetStringIdFromUser("Enter the third person variant name (use \"skip\" or hit enter to skip): ");
            
            if (Binocs)
            {
                rep.BinocularsZoomInSound = GetTag("Enter the binocular zoom in sound tag (press enter for null): ");
                rep.BinocularsZoomOutSound = GetTag("Enter the binocular zoom out sound tag (press enter for null): ");
            }
            
            rep.CombatDialogue = GetTag("Enter the first person combat dialogue (udlg) tag (press enter for null): ");

            Globals.PlayerRepresentation.Add(rep);

            return Globals.PlayerRepresentation.Count - 1;
        }

        private void CreateNewCharacterType()
        {
            Globals.PlayerCharacterType type = new Globals.PlayerCharacterType
            {
                Name = Simple ? CharacterStringID : GetStringIdFromUser("Enter the character type name: "),
                PlayerInformation = (sbyte)(Simple ? 0 : GetIntFromUser("Enter the player information block index: ")),
                PlayerControl = (sbyte)(Simple ? 0 : GetIntFromUser("Enter the player control block index: ")),
                CampaignRepresentation = (sbyte)CampRepIndex,
                MultiplayerRepresentation = (sbyte)MpRepIndex,
                MultiplayerArmorCustomization = 0,
                ChudGlobals = 0,
                FirstPersonInterface = 0
            };

            Globals.PlayerCharacterTypes.Add(type);
        }

        private void CreateNewCharacterSet(ModGlobalsDefinition.PlayerCharacterSet set)
        {
            Console.WriteLine("Enter the character set display name: ");
            string value = Console.ReadLine().Trim();
            set.DisplayName = value.Substring(0, Math.Max(0, value.Length));

            set.Name = GetStringIdFromUser("Enter the character set name (stringid): ");
            set.RandomChance = GetFloatFromUser("Enter the random chance (float): ");
            set.Characters = new List<ModGlobalsDefinition.PlayerCharacterSet.PlayerCharacter>();
        }

        private void CreateNewPlayerCharacter(ModGlobalsDefinition.PlayerCharacterSet set)
        {
            ModGlobalsDefinition.PlayerCharacterSet.PlayerCharacter character = new ModGlobalsDefinition.PlayerCharacterSet.PlayerCharacter();

            if (Simple)
            {
                character.DisplayName = CharacterName;
                character.Name = CharacterStringID;
            }
            else
            {
                Console.WriteLine("Enter the character display name: ");
                string value = Console.ReadLine().Trim();
                character.DisplayName = value.Substring(0, Math.Max(0, value.Length));
                character.Name = GetStringIdFromUser("Enter the character name (stringid): ");
            }

            character.RandomChance = Simple ? 0.1f : GetFloatFromUser("Enter the random chance (float): ");
            set.Characters.Add(character);
        }

        private CachedTag GetTag(string message)
        {
            Console.WriteLine(message);
            string tagName = Console.ReadLine().Trim();

            switch(tagName)
            {
                case "\n":
                case "null":
                case "skip":
                    return null;
                default:
                    {
                        if (Cache.TagCache.TryGetTag(tagName, out CachedTag tag))
                            return tag;
                        else
                            return null;
                    }
            }
        }

        private StringId GetStringIdFromUser(string message)
        {
            Console.WriteLine(message);
            string value = Console.ReadLine().Trim();

            if (value == "\n" || value == "skip")
                return StringId.Invalid;

            return GetStringId(value);
        }

        private StringId GetStringId(string value)
        {
            StringId stringId = Cache.StringTable.GetStringId(value);
            if (stringId == StringId.Invalid && value != Cache.StringTable.GetString(StringId.Invalid))
            {
                stringId = Cache.StringTable.AddString(value);
                Cache.SaveStrings();
            }

            return stringId;
        }

        private static int GetIntFromUser(string message)
        {
            string userInput;
            int result;
            try
            {
                Console.Write(message);
                userInput = Console.ReadLine();
                result = Convert.ToInt32(userInput);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invalid input: {e.Message}");
            }
            return -1;
        }

        private static float GetFloatFromUser(string message)
        {
            string userInput;
            double result;
            try
            {
                Console.Write(message);
                userInput = Console.ReadLine();
                result = Convert.ToDouble(userInput);
                return (float)result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invalid input: {e.Message}");
            }
            return 1.0f;
        }

        private void SectionBreak()
        {
            Console.WriteLine("-----------------------------------------");
        }
    }
}
