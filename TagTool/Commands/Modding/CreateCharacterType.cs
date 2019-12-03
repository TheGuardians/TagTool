using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Modding
{
    class CreateCharacterType : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private ModGlobalsDefinition ModGlobals;
        private Globals Globals;

        public CreateCharacterType(HaloOnlineCacheContext cacheContext) :
            base(true,

                "CreateCharacterType",
                "Builds a character type from the current biped tag.",

                "CreateCharacterType",

                "Builds a character type from the current biped tag")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            OpenGlobalTags();

            //
            // Wizard to set up a new playable character
            //

            SectionBreak();

            Console.Write("Create new campaign representation? [y/n] ");

            var answer = Console.ReadLine().ToLower();
            if (answer.Length == 0 || !(answer.StartsWith("y") || answer.StartsWith("n")))
                return false;
            if (answer.StartsWith("y"))
            {
                CreatePlayerRepresentation();
                SectionBreak();
            }

            Console.Write("Create new multiplayer representation? [y/n] ");

            answer = Console.ReadLine().ToLower();
            if (answer.Length == 0 || !(answer.StartsWith("y") || answer.StartsWith("n")))
                return false;
            if (answer.StartsWith("y"))
            {
                CreatePlayerRepresentation();
                SectionBreak();
            }

            Console.Write("Create new character type? [y/n] ");

            answer = Console.ReadLine().ToLower();
            if (answer.Length == 0 || !(answer.StartsWith("y") || answer.StartsWith("n")))
                return false;
            if (answer.StartsWith("y"))
            {
                CreateNewCharacterType();
                SectionBreak();
            }

            int setIndex = GetIntFromUser("Enter the character set index (-1 for new block): ");

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
            using (var cacheStream = CacheContext.TagCacheFile.OpenRead())
            {
                Globals = CacheContext.Deserialize<Globals>(cacheStream, CacheContext.GetTag($"globals\\globals.matg"));
                ModGlobals = CacheContext.Deserialize<ModGlobalsDefinition>(cacheStream, CacheContext.GetTag($"multiplayer\\mod_globals.modg"));
            }
        }

        private void SaveTags()
        {
            using (var stream = CacheContext.OpenTagCacheReadWrite())
            {
                CacheContext.Serialize(stream, CacheContext.GetTag($"multiplayer\\mod_globals.modg"), ModGlobals);
                CacheContext.Serialize(stream, CacheContext.GetTag($"globals\\globals.matg"), Globals);
            }
        }

        private void CreatePlayerRepresentation()
        {
            Globals.PlayerRepresentationBlock rep = new Globals.PlayerRepresentationBlock();

            rep.Name = GetStringId("Enter the representation name: ");
            rep.FirstPersonHands = GetTag("Enter the first person hands render model tag (press enter for null): ");
            rep.FirstPersonBody = GetTag("Enter the first person body render model tag (press enter for null): ");
            rep.ThirdPersonUnit = GetTag("Enter the third person unit biped tag (press enter for null): ");
            rep.ThirdPersonVariant = GetStringId("Enter the third person variant name (press enter for none): ");
            rep.BinocularsZoomInSound = GetTag("Enter the binocular zoom in sound tag (press enter for null): ");
            rep.BinocularsZoomOutSound = GetTag("Enter the binocular zoom out sound tag (press enter for null): ");
            rep.CombatDialogue = GetTag("Enter the first person combat dialogue (udlg) tag (press enter for null): ");

            Globals.PlayerRepresentation.Add(rep);
        }

        private void CreateNewCharacterType()
        {
            Globals.PlayerCharacterType type = new Globals.PlayerCharacterType();

            type.Name = GetStringId("Enter the representation name: ");
            type.PlayerInformation = (sbyte)GetIntFromUser("Enter the player information block index: ");
            type.PlayerControl = (sbyte)GetIntFromUser("Enter the player control block index: ");
            type.CampaignRepresentation = (sbyte)GetIntFromUser("Enter the campaign player representation block index: ");
            type.MultiplayerRepresentation = (sbyte)GetIntFromUser("Enter the multiplayer player representation block index: ");
            type.MultiplayerArmorCustomization = (sbyte)GetIntFromUser("Enter the multiplayer player customization block index: ");
            type.ChudGlobals = (sbyte)GetIntFromUser("Enter the chud globals block index: ");
            type.FirstPersonInterface = (sbyte)GetIntFromUser("Enter the first person interface block index: ");

            Globals.PlayerCharacterTypes.Add(type);
        }

        private void CreateNewCharacterSet(ModGlobalsDefinition.PlayerCharacterSet set)
        {
            Console.WriteLine("Enter the character set display name: ");
            string value = Console.ReadLine().Trim();
            set.DisplayName = value.Substring(0, Math.Max(0, value.Length));

            set.Name = GetStringId("Enter the character set name (stringid): ");
            set.RandomChance = GetFloatFromUser("Enter the random chance (float): ");
            set.Characters = new List<ModGlobalsDefinition.PlayerCharacterSet.PlayerCharacter>();
        }

        private void CreateNewPlayerCharacter(ModGlobalsDefinition.PlayerCharacterSet set)
        {
            ModGlobalsDefinition.PlayerCharacterSet.PlayerCharacter character = new ModGlobalsDefinition.PlayerCharacterSet.PlayerCharacter();
            Console.WriteLine("Enter the character display name: ");
            string value = Console.ReadLine().Trim();
            character.DisplayName = value.Substring(0, Math.Max(0, value.Length));

            character.Name = GetStringId("Enter the character name (stringid): ");
            character.RandomChance = GetFloatFromUser("Enter the random chance (float): ");
            set.Characters.Add(character);
        }

        private CachedTagInstance GetTag(string message)
        {
            Console.WriteLine(message);
            string tagName = Console.ReadLine().Trim();

            if (tagName == "\n")
                return null;

            if (CacheContext.TryGetTag(tagName, out CachedTagInstance tag))
                return tag;
            else
                return null;
        }

        private StringId GetStringId(string message)
        {
            StringId stringId;
            Console.WriteLine(message);
            string value = Console.ReadLine().Trim();

            if (value == "\n")
                return StringId.Invalid;

            stringId = CacheContext.GetStringId(value);
            if(stringId == StringId.Invalid && value != CacheContext.GetString(StringId.Invalid))
            {
                stringId = CacheContext.StringIdCache.AddString(value);
                using (var stringIdCacheStream = CacheContext.OpenStringIdCacheReadWrite())
                    CacheContext.StringIdCache.Save(stringIdCacheStream);
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
