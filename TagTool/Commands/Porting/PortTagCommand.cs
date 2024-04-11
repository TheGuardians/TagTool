using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;
using System.Reflection;
using TagTool.Porting;
using static TagTool.Porting.PortingContext;

namespace TagTool.Commands.Porting
{
    public partial class PortTagCommand : Command
	{
		private GameCacheHaloOnlineBase CacheContext { get; }
		private GameCache BlamCache;

		public PortTagCommand(GameCacheHaloOnlineBase cacheContext, GameCache blamCache) :
			base(true,

				"PortTag",
				PortTagCommand.GetPortingFlagsDescription(),
				"PortTag [Options] <Tag>",
				"")
		{
			CacheContext = cacheContext;
			BlamCache = blamCache;
		}

		public override object Execute(List<string> args)
		{
			if (args.Count < 1)
                return new TagToolError(CommandError.ArgCount);

            var portingOptions = args.Take(args.Count - 1).ToList();

			string[] argParameters = ParsePortingOptions(portingOptions, out PortingFlags flags);

            var porting = new PortingContext(CacheContext, BlamCache);
            var tagName = args.Last();

            foreach (var blamTag in porting.ParseLegacyTag(tagName))
            {
                if (blamTag == null)
                    return new TagToolError(CommandError.TagInvalid, tagName);

                porting.PortTag(blamTag, portingFlags: flags, optMpObjectParams: argParameters);
            }

            return true;
		}

        /// <summary>
		/// Parses porting flag options from a <see cref="List{T}"/> of <see cref="string"/>.
		/// </summary>
		/// <param name="args"></param>
		public string[] ParsePortingOptions(List<string> args, out PortingFlags Flags)
        {
            Flags = PortingFlags.Default;

            var flagNames = Enum.GetNames(typeof(PortingFlags)).Select(name => name.ToLower());
            var flagValues = Enum.GetValues(typeof(PortingFlags)) as PortingFlags[];

            string[] argParameters = new string[0];

            for (var a = 0; a < args.Count(); a++)
            {
                string[] argSegments = args[a].Split('[');

                var arg = argSegments[0].ToLower();

                // Support legacy arguments
                arg = arg.Replace("single", $"!{nameof(PortingFlags.Recursive)}");
                arg = arg.Replace("noshaders", $"!{nameof(PortingFlags.MatchShaders)}");
                arg = arg.Replace("silent", $"!{nameof(PortingFlags.Print)}");
                arg = arg.ToLower(); // do this again incase the argument was replaced

                // Use '!' or 'No' to negate an argument.
                var toggleOn = !(arg.StartsWith("!") || arg.StartsWith("no"));
                if (!toggleOn && arg.StartsWith("!"))
                    arg = arg.Remove(0, 1);
                else if (!toggleOn && arg.StartsWith("no"))
                    arg = arg.Remove(0, 2);

                // Throw exceptions at clumsy typers.
                if (!flagNames.Contains(arg))
                    throw new FormatException($"Invalid {typeof(PortingFlags).FullName}: {args[0]}");

                // Add/remove flags based on if they appeared as arguments, 
                // and whether they were negated with '!' or 'No'
                for (var i = 0; i < flagNames.Count(); i++)
                    if (arg == flagNames.ElementAt(i))
                        if (toggleOn)
                            Flags |= flagValues[i];
                        else
                            Flags &= ~flagValues[i];

                // Get forge palette info if provided
                if (arg == "mpobject" && argSegments.Count() > 1)
                {
                    argParameters = argSegments[1].Split(']')[0].Split(',');
                }
            }
            return argParameters;
        }

        /// <summary>
		/// Generates a <see cref="Command.Description"/> based on the <see cref="PortingFlags"/> <see cref="Enum"/>.
		/// </summary>
		/// <returns>A <see cref="Command.Description"/> based on <see cref="PortingFlags"/>.</returns>
		private static string GetPortingFlagsDescription()
        {
            var info =
                "Ports a tag from the current cache file." + Environment.NewLine +
                "Available Options:" + Environment.NewLine +
                Environment.NewLine;

            var padCount = Enum.GetNames(typeof(PortingFlags)).Max(flagName => flagName.Length);

            foreach (var portingFlagInfo in typeof(PortingFlags).GetMembers(BindingFlags.Public | BindingFlags.Static).OrderBy(m => m.MetadataToken))
            {
                var attr = portingFlagInfo.GetCustomAttribute<PortingFlagDescriptionAttribute>(false);

                // Use the attribute description for the flags help-description.
                if (attr != null)
                    info += $"{portingFlagInfo.Name.PadRight(padCount)} - " +
                        $"{attr.Description}" + Environment.NewLine;

                // Use the flags sub-set of the flag for the flags help-description
                else
                {
                    var portingFlags = (PortingFlags)Enum.Parse(typeof(PortingFlags), portingFlagInfo.Name);
                    info += $"{portingFlagInfo.Name.PadRight(padCount)} - " +
                        $"{string.Join(", ", portingFlags.GetIndividualFlags())}" + Environment.NewLine;
                }
            }

            return info + Environment.NewLine +
                "*Any option can be negated by prefixing it with `!`." + Environment.NewLine;
        }
    }
}
