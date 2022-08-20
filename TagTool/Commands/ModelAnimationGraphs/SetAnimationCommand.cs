using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using TagTool.Commands.Common;
using System;

namespace TagTool.Commands.ModelAnimationGraphs
{
    public class SetAnimationCommand : Command
    {
        private GameCache CacheContext { get; }
        private ModelAnimationGraph Animation { get; set; }
        private CachedTag Jmad { get; set; }

        public SetAnimationCommand(GameCache cachecontext, ModelAnimationGraph animation, CachedTag jmad)
            : base(false,

                  "SetAnimation",
                  "Assign an animation as an action or overlay.",

                  "SetAnimation <mode:class:type:label> <index/last/-1>",

                  "Assign an animation as an action or overlay."
                  + "Animation name must contain 4 parts: a mode, weapon class, weapon type, and action/overlay name, separated by colons (:)."
                  + "Index must be between 0 and the animation count, or -1 to disable the animation. Use \"last\" to a")
        {
            CacheContext = cachecontext;
            Animation = animation;
            Jmad = jmad;
        }

        public override object Execute(List<string> args)
        {
            //Arguments needed: <filepath>
            if (args.Count != 2)
                return new TagToolError(CommandError.ArgCount);

            if (!short.TryParse(args[1], out short animationIndex))
            {
                switch (args[1])
                {
                    case "last":
                        animationIndex = (short)(Animation.Animations.Count - 1);
                        break;
                    default:
                        return new TagToolError(CommandError.CustomError, "Animation index could not be parsed.");
                }
            }

            if (animationIndex < -1 || animationIndex >= Animation.Animations.Count)
                return new TagToolError(CommandError.CustomError, $"Animation index must be within the bounds 0 and the animation count ({Animation.Animations.Count}).");

            var nameSplit = args[0].ToString().Split(':');

            if (nameSplit.Length != 4)
                return new TagToolError(CommandError.CustomError, "Animation name must contain 4 parts: a mode, weapon class, weapon type, and action/overlay name, separated by colons (:).");


            ModelAnimationGraph.Mode mode = Animation.Modes.Where(x => x.Name == CacheContext.StringTable.GetStringId(nameSplit[0])).FirstOrDefault();

            if (mode == null)
                return new TagToolError(CommandError.CustomError, $"Mode \"{nameSplit[0]}\" could not be found in this jmad.");

            ModelAnimationGraph.Mode.WeaponClassBlock weapClass = mode.WeaponClass
                .Where(y => y.Label == CacheContext.StringTable.GetStringId(nameSplit[1])).FirstOrDefault();

            if (weapClass == null)
                return new TagToolError(CommandError.CustomError, $"Weapon class \"{nameSplit[1]}\" could not be found in mode \"{nameSplit[0]}\".");

            ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock weapType = weapClass.WeaponType
                .Where(z => z.Label == CacheContext.StringTable.GetStringId(nameSplit[2])).FirstOrDefault();

            if (weapType == null)
                return new TagToolError(CommandError.CustomError, $"Weapon type \"{nameSplit[2]}\" could not be found in weapon class \"{nameSplit[1]}\".");

            ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry entry = weapType.Set.Actions
                .Where(a => a.Label == CacheContext.StringTable.GetStringId(nameSplit[3])).FirstOrDefault();

            string type = "Action";

            if (entry == null)
            {
                entry = weapType.Set.Overlays
                .Where(b => b.Label == CacheContext.StringTable.GetStringId(nameSplit[3])).FirstOrDefault();

                if (entry == null)
                    return new TagToolError(CommandError.CustomError, $"Action or overlay \"{nameSplit[3]}\" could not be found in weapon type \"{nameSplit[2]}\".");
                else
                    type = "Overlay";
            }

            entry.Animation = animationIndex;
            entry.GraphIndex = -1;

            Console.WriteLine($"{type} found, animation set to {animationIndex}.");
            return true;
        }
    }
}
