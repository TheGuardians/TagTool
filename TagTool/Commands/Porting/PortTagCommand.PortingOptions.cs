using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
	public partial class PortTagCommand
	{
		/// <summary>
		/// Current set of <see cref="PortingFlags"/> which will affect the behavior of <see cref="PortTagCommand"/>.
		/// </summary>
		private PortingFlags Flags { get; set; }

		/// <summary>
		/// Flags which can be used to affect the behavior of <see cref="PortTagCommand"/>.
		/// </summary>
		[Flags]
		public enum PortingFlags
		{
			/// <summary>
			/// Replace tags of the same name when porting.
			/// </summary>
			[PortingFlagDescription("Replace tags of the same name when porting.")]
			Replace = 1 << 0,

			/// <summary>
			/// Recursively port all tag references available.
			/// </summary>
			[PortingFlagDescription("Recursively port all tag references available.")]
			Recursive = 1 << 1,

			/// <summary>
			/// Create a new tag after the last index.
			/// </summary>
			[PortingFlagDescription("Create a new tag after the last index.")]
			New = 1 << 2,

			/// <summary>
			/// Port a tag using nulled tag indices where available.
			/// </summary>
			[PortingFlagDescription("Port a tag using nulled tag indices where available.")]
			UseNull = 1 << 3,

			/// <summary>
			/// Include Sound 'snd!' tags.
			/// </summary>
			[PortingFlagDescription("Include 'Sound' (snd!) tags.")]
			Audio = 1 << 4,

			/// <summary>
			/// Include elite <see cref="Biped"/> tags.
			/// </summary>
			[PortingFlagDescription("Include elite 'Biped' (bipd) tags.")]
			Elites = 1 << 5,
			/// <summary>
			/// Include <see cref="Scenario.SandboxObject"/> tag-blocks.
			/// </summary>
			[PortingFlagDescription("Include 'Scenario.SandboxObject' tag-blocks.")]
			ForgePalette = 1 << 6,

			/// <summary>
			/// Include <see cref="Scenario.Squad"/> tag-blocks.
			/// </summary>
			[PortingFlagDescription("Include 'Scenario.Squad' tag-blocks.")]
			Squads = 1 << 7,

			/// <summary>
			/// Include <see cref="Scripting.HsScript"/> tag-blocks.
			/// </summary>
			[PortingFlagDescription("Include 'Scripting.Script' tag-blocks.")]
			Scripts = 1 << 8,

			/// <summary>
			/// Include udlg tags and their referenced audio.
			/// </summary>
			[PortingFlagDescription("Port udlg tags and associated audio for bipeds.")]
			Dialogue = 1 << 9,

			/// <summary>
			/// Attempt to match shaders to existing tags.
			/// </summary>
			[PortingFlagDescription("Attempt to match shaders to existing tags.")]
			MatchShaders = 1 << 10,

			/// <summary>
			/// Only use templates that have the exact same render method options
			/// </summary>
			[PortingFlagDescription("Only use templates that have the exact same render method options")]
			PefectShaderMatchOnly = 1 << 11,

			/// <summary>
			/// Keep cache in memory during porting.
			/// </summary>
			[PortingFlagDescription("Keep cache in memory during porting.")]
			Memory = 1 << 12,

			/// <summary>
			/// Allow using existing tags from Ms30.
			/// </summary>
			[PortingFlagDescription("Allow using existing tags from Ms30.")]
			Ms30 = 1 << 13,

			/// <summary>
			/// Allow writing output to the console.
			/// </summary>
			[PortingFlagDescription("Allow writing output to the console.")]
			Print = 1 << 14,

            /// <summary>
            /// Merges new data if tags exist.
            /// </summary>
            [PortingFlagDescription("Merges new data if tags exist.")]
            Merge = 1 << 15,

            /// <summary>
            /// Use a regular expression to determine target tags.
            /// </summary>
            [PortingFlagDescription("Use a regular expression to determine target tags")]
            Regex = 1 << 16,

			/// <summary>
			/// Attempt to generate shaders.
			/// </summary>
			[PortingFlagDescription("Attempt to generate shaders.")]
			GenerateShaders = 1 << 17,

			/// <summary>
			/// Add a multiplayerobject block for spawnable tag types.
			/// </summary>
			[PortingFlagDescription("Add a multiplayerobject block for spawnable tag types.")]
			MPobject = 1 << 18,

			[PortingFlagDescription("Multipurpose Reach flag used for specific tweaks.")]
			ReachMisc = 1 << 19,

			// No [PortingFlagDescription] here means we'll flag names as the description.
			Default = Print | Recursive | Merge | Scripts | Squads | ForgePalette | Elites | Audio | Dialogue | MatchShaders | GenerateShaders
		}

		/// <summary>
		/// True if ALL of the supplied <see cref="PortingFlags"/> are set, false if any aren't:
		/// (<see cref="PortTagCommand.Flags"/> &amp; flags) == flags
		/// </summary>
		/// <param name="flags">The <see cref="PortingFlags"/> to check.</param>
		public bool FlagsAllSet(PortingFlags flags) => (Flags & flags) == flags;
		
		/// <summary>
		/// True if the flag is set (this is 100% the same as <see cref="FlagsAnySet(PortingFlags)"/>,
		/// other than the name.
		/// </summary>
		/// <param name="flag"></param>
		/// <returns></returns>
		public bool FlagIsSet(PortingFlags flag) => (Flags & flag) != 0;

		/// <summary>
		/// True if ANY of the supplied <see cref="PortingFlags"/> are set, false if none are:
		/// (<see cref="PortTagCommand.Flags"/> &amp; flags) != 0
		/// </summary>
		/// <param name="flags">The <see cref="PortingFlags"/> to check.</param>
		public bool FlagsAnySet(PortingFlags flags) => (Flags & flags) != 0;

		/// <summary>
		/// Sets flags explicitly (<see cref="PortTagCommand.Flags"/> |= <see cref="PortingFlags"/>).
		/// </summary>
		/// <param name="flags">The <see cref="PortingFlags"/> to set.</param>
		public PortingFlags SetFlags(PortingFlags flags) => Flags |= flags;

		/// <summary>
		/// Removes flags explicitly (<see cref="PortTagCommand.Flags"/> &amp;= ~<see cref="PortingFlags"/>).
		/// </summary>
		/// <param name="flags">The <see cref="PortingFlags"/> to remove.</param>
		public PortingFlags RemoveFlags(PortingFlags flags) => Flags &= ~flags;

		/// <summary>
		/// Toggles flags on or off (<see cref="PortTagCommand.Flags"/> ^= <see cref="PortingFlags"/>).
		/// </summary>
		/// <param name="flags">The <see cref="PortingFlags"/> to toggle.</param>
		public PortingFlags ToggleFlags(PortingFlags flags) => Flags ^= flags;

		/// <summary>
		/// Parses porting flag options from a <see cref="List{T}"/> of <see cref="string"/>.
		/// </summary>
		/// <param name="args"></param>
		private string[] ParsePortingOptions(List<string> args)
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
							SetFlags(flagValues[i]);
						else
							RemoveFlags(flagValues[i]);

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

		/// <summary>
		/// Generic description attribute
		/// </summary>
		private class PortingFlagDescriptionAttribute : Attribute
		{
			public string Description;

			public PortingFlagDescriptionAttribute(string description)
			{
				Description = description;
			}
		}
	}
}
