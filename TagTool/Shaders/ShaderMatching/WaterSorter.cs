using System;
using System.Collections.Generic;

namespace TagTool.Shaders.ShaderMatching
{
    public class WaterSorter : SortingInterface
    {
        private static List<WaterOptionTypes> TypeOrder = new List<WaterOptionTypes> {
           WaterOptionTypes.appearance,
           WaterOptionTypes.waveshape,
           WaterOptionTypes.reflection,
           WaterOptionTypes.refraction,
           WaterOptionTypes.watercolor,
           WaterOptionTypes.global_shape,
           WaterOptionTypes.foam,
           WaterOptionTypes.bankalpha,
        };

        private static List<WaveshapeOptions> WaveshapeOrder = new List<WaveshapeOptions> {
           WaveshapeOptions.default_,
           WaveshapeOptions.none,
           WaveshapeOptions.bump
        };

        private static List<WatercolorOptions> WatercolorOrder = new List<WatercolorOptions> {
           WatercolorOptions.pure,
           WatercolorOptions.texture
        };

        private static List<ReflectionOptions> ReflectionOrder = new List<ReflectionOptions> {
           ReflectionOptions.none,
           ReflectionOptions.static_,
           ReflectionOptions.dynamic
        };

        private static List<RefractionOptions> RefractionOrder = new List<RefractionOptions> {
           RefractionOptions.none,
           RefractionOptions.dynamic
        };

        private static List<BankalphaOptions> BankalphaOrder = new List<BankalphaOptions> {
           BankalphaOptions.none,
           BankalphaOptions.depth,
           BankalphaOptions.paint
        };

        private static List<Appearance> AppearanceOrder = new List<Appearance> {
           Appearance.default_
        };

        private static List<GlobalShapeOptions> GlobalShapeOrder = new List<GlobalShapeOptions> {
           GlobalShapeOptions.none,
           GlobalShapeOptions.paint,
           GlobalShapeOptions.depth
        };

        private static List<FoamOptions> FoamOrder = new List<FoamOptions> {
           FoamOptions.none,
           FoamOptions.auto,
           FoamOptions.paint,
           FoamOptions.both
        };

        public int GetTypeCount() => 8;

        public int GetOptionCount(int typeIndex)
        {
            // HO counts
            switch ((WaterOptionTypes)(typeIndex))
            {
                case WaterOptionTypes.waveshape:        return 3;
                case WaterOptionTypes.watercolor:       return 2;
                case WaterOptionTypes.reflection:       return 3;
                case WaterOptionTypes.refraction:       return 2;
                case WaterOptionTypes.bankalpha:        return 3;
                case WaterOptionTypes.appearance:       return 1;
                case WaterOptionTypes.global_shape:     return 3;
                case WaterOptionTypes.foam:             return 4;
                default:                                return 0;
            }
        }

        public int GetTypeIndex(int typeIndex)
        {
            return TypeOrder.IndexOf((WaterOptionTypes)typeIndex);
        }

        public int GetOptionIndex(int typeIndex, int optionIndex)
        {
            switch ((WaterOptionTypes)(typeIndex))
            {
                case WaterOptionTypes.waveshape:    return WaveshapeOrder.IndexOf((WaveshapeOptions)optionIndex);
                case WaterOptionTypes.watercolor:   return WatercolorOrder.IndexOf((WatercolorOptions)optionIndex);
                case WaterOptionTypes.reflection:   return ReflectionOrder.IndexOf((ReflectionOptions)optionIndex);
                case WaterOptionTypes.refraction:   return RefractionOrder.IndexOf((RefractionOptions)optionIndex);
                case WaterOptionTypes.bankalpha:    return BankalphaOrder.IndexOf((BankalphaOptions)optionIndex);
                case WaterOptionTypes.appearance:   return AppearanceOrder.IndexOf((Appearance)optionIndex);
                case WaterOptionTypes.global_shape: return GlobalShapeOrder.IndexOf((GlobalShapeOptions)optionIndex);
                case WaterOptionTypes.foam:         return FoamOrder.IndexOf((FoamOptions)optionIndex);
                default: return 0;
            }
        }

        public string ToString(List<int> options)
        {
            if (options.Count < GetTypeCount())
                return "Invalid option count";

            string result = "";
            result += $"Wave Shape:     {(WaveshapeOptions)options[0]} \n";
            result += $"Water Color:    {(WatercolorOptions)options[1]} \n";
            result += $"Reflection:     {(ReflectionOptions)options[2]} \n";
            result += $"Refraction:     {(RefractionOptions)options[3]} \n";
            result += $"Bank Alpha:     {(BankalphaOptions)options[4]} \n";
            result += $"Appearance:     {(Appearance)options[5]} \n";
            result += $"Global Shape:   {(GlobalShapeOptions)options[6]} \n";
            result += $"Foam:           {(FoamOptions)options[7]} \n";

            return result;
        }

        public void PrintOptions(List<int> options)
        {
            Console.WriteLine(ToString(options));
        }

        private enum WaterOptionTypes
        {
            waveshape,
            watercolor,
            reflection,
            refraction,
            bankalpha,
            appearance,
            global_shape,
            foam
        }

        private enum WaveshapeOptions
        {
            default_,
            none,
            bump
        }

        private enum WatercolorOptions
        {
            pure,
            texture
        }

        private enum ReflectionOptions
        {
            none,
            static_,
            dynamic
        }

        private enum RefractionOptions
        {
            none,
            dynamic
        }

        private enum BankalphaOptions
        {
            none,
            depth,
            paint
        }

        private enum Appearance
        {
            default_
        }

        private enum GlobalShapeOptions
        {
            none,
            paint,
            depth
        }

        private enum FoamOptions
        {
            none,
            auto,
            paint,
            both
        }
    }
}
