using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Commands.Common;
using TagTool.Tags;

namespace TagTool.Effects
{
    public class EditableProperty
    {
        public static bool StateIsSet(uint stateBits, ParticlePropertyScalar.ParticleStates state)
        {
            return ((1 << (int)state) & stateBits) != 0;
        }

        public static ParticlePropertyScalar.ParticleStatesFlags ParticleEditablePropertyEvaluate(ParticlePropertyScalar property, string propertyName, uint validStates, ParticlePropertyScalar.ParticleStates defaultState)
        {
            uint usedStates = 0;

            if ((((int)property.RuntimeMFlags) & 32) == 0)
            {
                if (property.Function.Data[0] != 1) // != constant
                    usedStates = (uint)(1 << (int)property.InputVariable);
                if ((property.Function.Data[1] & 1) > 0) // has range
                    usedStates |= (uint)(1 << (int)property.RangeVariable);
                if (property.OutputModifier != ParticlePropertyScalar.OutputModifierValue.None)
                    usedStates |= (uint)(1 << (int)property.OutputModifierInput);
            }
            uint forbiddenStates = usedStates & ~validStates;

            if (forbiddenStates != 0)
            {
                List<ParticlePropertyScalar.ParticleStates> forbiddenStatesList = new List<ParticlePropertyScalar.ParticleStates>();
                if (property.Function.Data[0] != 1 && StateIsSet(forbiddenStates, property.InputVariable))
                {
                    forbiddenStatesList.Add(property.InputVariable);
                    property.InputVariable = defaultState;
                }
                if ((property.Function.Data[1] & 1) > 0 && StateIsSet(forbiddenStates, property.RangeVariable))
                {
                    forbiddenStatesList.Add(property.InputVariable);
                    property.RangeVariable = defaultState;
                }
                if (property.OutputModifier != ParticlePropertyScalar.OutputModifierValue.None)
                {
                    forbiddenStatesList.Add(property.InputVariable);
                    property.OutputModifier = (ParticlePropertyScalar.OutputModifierValue)defaultState;
                }

                new TagToolWarning($"Property {propertyName} uses forbidden state(s): {string.Join(", ", forbiddenStatesList.Distinct())}...!");
            }

            return (ParticlePropertyScalar.ParticleStatesFlags)usedStates;
        }
    }
}
