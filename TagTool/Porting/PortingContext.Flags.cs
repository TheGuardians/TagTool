using System;
using System.ComponentModel;
using TagTool.Tags.Definitions;

namespace TagTool.Porting
{
    /// <summary>
    /// Flags which can be used to affect the behavior of <see cref="PortingContext"/>.
    /// </summary>
    [Flags]
    public enum PortingFlags
    {
        /// <summary>
        /// Replace tags of the same name when porting.
        /// </summary>
        [Description("Replace tags of the same name when porting.")]
        Replace = 1 << 0,

        /// <summary>
        /// Recursively port all tag references available.
        /// </summary>
        [Description("Recursively port all tag references available.")]
        Recursive = 1 << 1,

        /// <summary>
        /// Create a new tag after the last index.
        /// </summary>
        [Description("Create a new tag after the last index.")]
        New = 1 << 2,

        /// <summary>
        /// Port a tag using nulled tag indices where available.
        /// </summary>
        [Description("Port a tag using nulled tag indices where available.")]
        UseNull = 1 << 3,

        /// <summary>
        /// Include Sound 'snd!' tags.
        /// </summary>
        [Description("Include 'Sound' (snd!) tags.")]
        Audio = 1 << 4,

        /// <summary>
        /// Include elite <see cref="Biped"/> tags.
        /// </summary>
        [Description("Include elite 'Biped' (bipd) tags.")]
        Elites = 1 << 5,
        /// <summary>
        /// Include <see cref="Scenario.SandboxObject"/> tag-blocks.
        /// </summary>
        [Description("Include 'Scenario.SandboxObject' tag-blocks.")]
        ForgePalette = 1 << 6,

        /// <summary>
        /// Include <see cref="Scenario.Squad"/> tag-blocks.
        /// </summary>
        [Description("Include 'Scenario.Squad' tag-blocks.")]
        Squads = 1 << 7,

        /// <summary>
        /// Include <see cref="Scripting.HsScript"/> tag-blocks.
        /// </summary>
        [Description("Include 'Scripting.Script' tag-blocks.")]
        Scripts = 1 << 8,

        /// <summary>
        /// Include udlg tags and their referenced audio.
        /// </summary>
        [Description("Port udlg tags and associated audio for bipeds.")]
        Dialogue = 1 << 9,

        /// <summary>
        /// Attempt to match shaders to existing tags.
        /// </summary>
        [Description("Port render methods (shaders). Negate (\"noshaders\") to use the destination's default shader instead.")]
        MatchShaders = 1 << 10,

        /// <summary>
        /// Only use templates that have the exact same render method options
        /// </summary>
        [Description("Only use templates that have the exact same render method options")]
        PefectShaderMatchOnly = 1 << 11,

        /// <summary>
        /// Allow using existing tags from Ms30.
        /// </summary>
        [Description("Allow using existing tags from Ms30.")]
        Ms30 = 1 << 13,

        /// <summary>
        /// Allow writing output to the console.
        /// </summary>
        [Description("Allow writing output to the console.")]
        Print = 1 << 14,

        /// <summary>
        /// Merges new data if tags exist.
        /// </summary>
        [Description("Merges new data if tags exist.")]
        Merge = 1 << 15,

        /// <summary>
        /// Use a regular expression to determine target tags.
        /// </summary>
        [Description("Use a regular expression to determine target tags")]
        Regex = 1 << 16,

        /// <summary>
        /// Attempt to generate shaders.
        /// </summary>
        [Description("Attempt to generate shaders.")]
        GenerateShaders = 1 << 17,

        /// <summary>
        /// Add a multiplayerobject block for spawnable tag types.
        /// </summary>
        [Description("Add a multiplayerobject block for spawnable tag types")]
        MPobject = 1 << 18,

        [Description("Multipurpose Reach flag used for specific tweaks.")]
        ReachMisc = 1 << 19,

        [Description("Auto rescale gui during porting")]
        AutoRescaleGui = 1 << 20,

        [Description("Auto Update map files")]
        UpdateMapFiles = 1 << 21,

        // No [PortingFlagDescription] here means we'll flag names as the description.
        Default = Print | Recursive | Merge | Scripts | Squads | ForgePalette | Elites | Audio | Dialogue | MatchShaders | GenerateShaders | UpdateMapFiles
    }

    public partial class PortingContext
    {
        /// <summary>
        /// True if ALL of the supplied <see cref="PortingFlags"/> are set, false if any aren't:
        /// (<see cref="Flags"/> &amp; flags) == flags
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
        /// (<see cref="Flags"/> &amp; flags) != 0
        /// </summary>
        /// <param name="flags">The <see cref="PortingFlags"/> to check.</param>
        public bool FlagsAnySet(PortingFlags flags) => (Flags & flags) != 0;

        /// <summary>
        /// Sets flags explicitly (<see cref="Flags"/> |= <see cref="PortingFlags"/>).
        /// </summary>
        /// <param name="flags">The <see cref="PortingFlags"/> to set.</param>
        public PortingFlags SetFlags(PortingFlags flags) => Flags |= flags;

        /// <summary>
        /// Removes flags explicitly (<see cref="Flags"/> &amp;= ~<see cref="PortingFlags"/>).
        /// </summary>
        /// <param name="flags">The <see cref="PortingFlags"/> to remove.</param>
        public PortingFlags RemoveFlags(PortingFlags flags) => Flags &= ~flags;

        /// <summary>
        /// Toggles flags on or off (<see cref="Flags"/> ^= <see cref="PortingFlags"/>).
        /// </summary>
        /// <param name="flags">The <see cref="PortingFlags"/> to toggle.</param>
        public PortingFlags ToggleFlags(PortingFlags flags) => Flags ^= flags;
    }
}
