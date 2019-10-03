using System;
using System.Collections.Generic;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.HUD
{
    class UpdateChudTextCommand : Command
    {
        private ChudDefinition Definition { get; }


        public UpdateChudTextCommand(ChudDefinition chud_definition)
            : base(true,

                  "UpdateChudText",
                  "Attempts to rescale text widget fonts to size 32.",

                  "UpdateChudText",
                  "Attempts to rescale text widget fonts to size 32.")
        {
            Definition = chud_definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            if (Definition.HudWidgets.Count == 0)
            {
                Console.WriteLine("No HUD widgets found!");
                return true;
            }

            int widgetsUpdated = 0;

            foreach (var hudwidget in Definition.HudWidgets)
            {
                if (hudwidget.PlacementData.Count == 0)
                    Console.WriteLine("WARNING: HUD widget contains no placement data.");

                foreach (var textwidget in hudwidget.TextWidgets)
                {
                    float widgetMultiplier = 1.0f;

                    switch (textwidget.Font)
                    {
                        case ChudDefinition.HudWidget.TextWidget.FontValue.Conduit14:
                            widgetMultiplier = 0.4375f;
                            break;

                        case ChudDefinition.HudWidget.TextWidget.FontValue.Agency16:
                        case ChudDefinition.HudWidget.TextWidget.FontValue.Conduit16:
                            widgetMultiplier = 0.5f;
                            break;

                        case ChudDefinition.HudWidget.TextWidget.FontValue.Agency18:
                        case ChudDefinition.HudWidget.TextWidget.FontValue.Conduit18:
                        case ChudDefinition.HudWidget.TextWidget.FontValue.Conduit18_2:
                            widgetMultiplier = 0.5625f;
                            break;

                        case ChudDefinition.HudWidget.TextWidget.FontValue.Agency23:
                        case ChudDefinition.HudWidget.TextWidget.FontValue.Conduit23:
                            widgetMultiplier = 0.71875f;
                            break;
                    }

                    if (textwidget.Font.ToString().Contains("Conduit"))
                    {
                        textwidget.Font = ChudDefinition.HudWidget.TextWidget.FontValue.Conduit32;
                        widgetsUpdated++;
                    }

                    else if (textwidget.Font.ToString().Contains("Agency"))
                    {
                        textwidget.Font = ChudDefinition.HudWidget.TextWidget.FontValue.Agency32;
                        widgetsUpdated++;
                    }

                    textwidget.PlacementData[0].Scale.X *= widgetMultiplier;
                    textwidget.PlacementData[0].Scale.Y *= widgetMultiplier;
                }
            }

            Console.WriteLine("{0} widgets updated.", widgetsUpdated);
            return true;
        }
    }
}
