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
			/// Include <see cref="Scripting.Script"/> tag-blocks.
			/// </summary>
			[PortingFlagDescription("Include 'Scripting.Script' tag-blocks.")]
			Scripts = 1 << 8,

			/// <summary>
			/// TBD.
			/// </summary>
			[PortingFlagDescription("TBD.")]
			ShaderTest = 1 << 9,

			/// <summary>
			/// Attempt to match shaders to existing tags.
			/// </summary>
			[PortingFlagDescription("Attempt to match shaders to existing tags.")]
			MatchShaders = 1 << 10,

			/// <summary>
			/// Keep cache in memory during porting.
			/// </summary>
			[PortingFlagDescription("Keep cache in memory during porting.")]
			Memory = 1 << 11,

			/// <summary>
			/// Include <see cref="ShaderHalogram"/> (rmhg) tags.
			/// </summary>
			[PortingFlagDescription("Include 'ShaderHalogram' (rmhg) tags.")]
			Rmhg = 1 << 12,

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

			// No [PortingFlagDescription] here means we'll flag names as the description.
			Default = Recursive | Audio | Elites | ForgePalette | Squads | Scripts | MatchShaders | Rmhg | Ms30 | Print
		}

		/// <summary>
		/// True if ALL of the supplied <see cref="PortingFlags"/> are set, false if any aren't:
		/// (<see cref="PortTagCommand.Flags"/> &amp; flags) == flags
		/// </summary>
		/// <param name="flags">The <see cref="PortingFlags"/> to check.</param>
		private bool FlagsAllSet(PortingFlags flags) => (this.Flags & flags) == flags;
		
		/// <summary>
		/// True if the flag is set (this is 100% the same as <see cref="FlagsAnySet(PortingFlags)"/>,
		/// other than the name.
		/// </summary>
		/// <param name="flag"></param>
		/// <returns></returns>
		private bool FlagIsSet(PortingFlags flag) => (this.Flags & flag) != 0;

		/// <summary>
		/// True if ANY of the supplied <see cref="PortingFlags"/> are set, false if none are:
		/// (<see cref="PortTagCommand.Flags"/> &amp; flags) != 0
		/// </summary>
		/// <param name="flags">The <see cref="PortingFlags"/> to check.</param>
		private bool FlagsAnySet(PortingFlags flags) => (this.Flags & flags) != 0;

		/// <summary>
		/// Sets flags explicitly (<see cref="PortTagCommand.Flags"/> |= <see cref="PortingFlags"/>).
		/// </summary>
		/// <param name="flags">The <see cref="PortingFlags"/> to set.</param>
		private PortingFlags SetFlags(PortingFlags flags) => this.Flags |= flags;

		/// <summary>
		/// Removes flags explicitly (<see cref="PortTagCommand.Flags"/> &amp;= ~<see cref="PortingFlags"/>).
		/// </summary>
		/// <param name="flags">The <see cref="PortingFlags"/> to remove.</param>
		private PortingFlags RemoveFlags(PortingFlags flags) => this.Flags &= ~flags;

		/// <summary>
		/// Toggles flags on or off (<see cref="PortTagCommand.Flags"/> ^= <see cref="PortingFlags"/>).
		/// </summary>
		/// <param name="flags">The <see cref="PortingFlags"/> to toggle.</param>
		private PortingFlags ToggleFlags(PortingFlags flags) => this.Flags ^= flags;

		/// <summary>
		/// Parses porting flag options from a <see cref="List{T}"/> of <see cref="string"/>.
		/// </summary>
		/// <param name="args"></param>
		private void ParsePortingOptions(List<string> args)
		{
			this.Flags = PortingFlags.Default;

			var flagNames = Enum.GetNames(typeof(PortingFlags)).Select(name => name.ToLower());
			var flagValues = Enum.GetValues(typeof(PortingFlags)) as PortingFlags[];

			for (var a = 0; a < args.Count(); a++)
			{
				var arg = args[a].ToLower();

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

				// Throw exceptiosn at clumsy typers.
				if (!flagNames.Contains(arg))
					throw new FormatException($"Invalid {typeof(PortingFlags).FullName}: {args[0]}");

				// Add/remove flags based on if they appeared as arguments, 
				// and whether they were negated with '!' or 'No'
				for (var i = 0; i < flagNames.Count(); i++)
					if (arg == flagNames.ElementAt(i))
						if (toggleOn)
							this.SetFlags(flagValues[i]);
						else
							this.RemoveFlags(flagValues[i]);
			}
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
				this.Description = description;
			}
		}
	}
}
