using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Cache.Gen2;
using System.IO;
using TagTool.Commands.Common;
using TagTool.Commands.Porting;

namespace TagTool.Commands.Porting.Gen2
{
	partial class PortTagGen2Command : Command
	{
        public TagStructure ConvertEffect(object gen2Tag)
        {
            switch (gen2Tag)
            {
                case TagTool.Tags.Definitions.Gen2.DamageEffect damage:
                    DamageEffect newdamage = new DamageEffect();
                    AutoConverter.TranslateTagStructure(damage, newdamage);
                    return newdamage;
                case TagTool.Tags.Definitions.Gen2.Effect effect:
                    return ConvertEffect(effect);
                default:
                    return null;
            }
        }

        private TagStructure ConvertEffect(TagTool.Tags.Definitions.Gen2.Effect effect)
        {
            Effect newEffect = new Effect();

            newEffect.Flags = (EffectFlags)effect.Flags;
            newEffect.LoopStartEvent = effect.LoopStartEvent;

            newEffect.LoopingSound = effect.LoopingSound;
            newEffect.LocationIndex = (sbyte)effect.Location;
            newEffect.AlwaysPlayDistance = effect.AlwaysPlayDistance;
            newEffect.NeverPlayDistance = effect.NeverPlayDistance;

            foreach (var location in effect.Locations)
            {
                newEffect.Locations.Add(new Effect.Location { MarkerName = location.MarkerName });
            }

            for (int i = 0; i < effect.Events.Count; i++)
            {
                var eventBlock = effect.Events[i];
                Effect.Event newEvent = new Effect.Event();

                newEvent.Name = Cache.StringTable.GetOrAddString($"event_{i}");
                newEvent.Flags = (Effect.Event.EventFlags)eventBlock.Flags;
                newEvent.SkipFraction = eventBlock.SkipFraction;
                newEvent.DelayBounds.Lower = eventBlock.DelayBounds.Lower;
                newEvent.DelayBounds.Upper = eventBlock.DelayBounds.Upper;
                newEvent.DurationBounds.Lower = eventBlock.DurationBounds.Lower;
                newEvent.DurationBounds.Upper = eventBlock.DurationBounds.Upper;

                foreach (var part in eventBlock.Parts)
                {
                    Effect.Event.Part newPart = new Effect.Event.Part
                    {
                        CreateInEnvironment = (EffectEnvironment)part.CreateIn,
                        CreateInDisposition = (EffectViolenceMode)part.CreateIn1,
                        PrimaryLocation = part.Location,
                        Flags = (EffectEventPartFlags)part.Flags,
                        RuntimeBaseGroupTag = part.Type.Group.Tag,
                        Type = part.Type,
                        VelocityBounds = part.VelocityBounds,
                        VelocityConeAngle = part.VelocityConeAngle,
                        AngularVelocityBounds = part.AngularVelocityBounds,
                        RadiusModifierBounds = part.RadiusModifierBounds,
                        AScalesValues = (EffectEventPartScales)part.AScalesValues,
                        BScalesValues = (EffectEventPartScales)part.BScalesValues
                    };

                    newEvent.Parts.Add(newPart);
                }

                //
                // BEAMS -- we will need to create a BeamSystem and reference that in a part.
                // low priority as it seems not much actually uses them in H2
                //

                foreach (var acceleration in eventBlock.Accelerations)
                {
                    Effect.Event.Acceleration newAcceleration = new Effect.Event.Acceleration();
                    AutoConverter.TranslateTagStructure(acceleration, newAcceleration);
                    newEvent.Accelerations.Add(newAcceleration);
                }

                // TODO: particle systems

                newEffect.Events.Add(newEvent);
            }

            return newEffect;
        }
    }
}
