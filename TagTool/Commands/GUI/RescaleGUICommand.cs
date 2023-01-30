using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Tags.Definitions.Common;

namespace TagTool.Commands.GUI
{
    class RescaleGUICommand : Command
    {
        private GameCache Cache { get; }

        public RescaleGUICommand(GameCache cache)
            : base(true,

                  "RescaleGUI",
                  "Rescales H3UI elements to fit a different internal resolution",

                  "RescaleGUI <scalefactor>",

                  "Rescales H3UI elements to fit a different internal resolution")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            float scalefactor = 1.3125f;
            if (args.Count == 1 && (args[0] != "ignore" && args[0] != "whitelist") && !float.TryParse(args[0], out scalefactor))
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\"");
            else
            if (args.Count == 2 && (args[0] == "ignore" || args[0] == "whitelist") && !float.TryParse(args[1], out scalefactor))
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[1]}\"");

            var ignoredTagIndicies = new HashSet<int>();
            var whitelistedTagIndicies = new HashSet<int>();

            if (args.Count > 0 && args[0] == "ignore")
            {
                Console.WriteLine("Please specify the tags to be ignored:");

                string ignoreLine;

                while ((ignoreLine = Console.ReadLine()) != "")
                {
                    if (!Cache.TagCache.TryGetTag(ignoreLine, out var instance))
                        continue;

                    ignoredTagIndicies.Add(instance.Index);
                }
            }
            else
            if (args.Count > 0 && args[0] == "whitelist")
            {
                Console.WriteLine("Please specify the tags to be rescaled:");

                string whitelistLine;

                while ((whitelistLine = Console.ReadLine()) != "")
                {
                    if (!Cache.TagCache.TryGetTag(whitelistLine, out var instance))
                        continue;

                    whitelistedTagIndicies.Add(instance.Index);
                }
            }

            List<string> TargetTagGroups = new List<string>{ "bmp3", "skn3", "txt3", "lst3", "grup", "bkey", "mdl3", "scn3" };
            foreach (var tag in Cache.TagCache.TagTable)
            {
                if (tag == null)
                    continue;

                if (!whitelistedTagIndicies.Contains(tag.Index))
                        continue;

                if (ignoredTagIndicies.Contains(tag.Index))
                    continue;

                if (TargetTagGroups.Contains(tag.Group.Tag.ToString()))
                {
                    using (var cacheStream = Cache.OpenCacheReadWrite())
                    using (var reader = new EndianReader(cacheStream))
                    {
                        var Definition = Cache.Deserialize(cacheStream, tag);
                        switch (tag.Group.Tag.ToString())
                        {
                            case "bmp3":
                                GuiBitmapWidgetDefinition bmp3tag = (GuiBitmapWidgetDefinition)Definition;
                                bmp3tag.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(bmp3tag.GuiRenderBlock, scalefactor);
                                Cache.Serialize(cacheStream, tag, bmp3tag);
                                break;
                            case "txt3":
                                GuiTextWidgetDefinition txt3tag = (GuiTextWidgetDefinition)Definition;
                                txt3tag.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(txt3tag.GuiRenderBlock, scalefactor);
                                Cache.Serialize(cacheStream, tag, txt3tag);
                                break;
                            case "mdl3":
                                GuiModelWidgetDefinition mdl3tag = (GuiModelWidgetDefinition)Definition;
                                mdl3tag.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(mdl3tag.GuiRenderBlock, scalefactor);
                                Cache.Serialize(cacheStream, tag, mdl3tag);
                                break;
                            case "skn3":
                                GuiSkinDefinition skn3tag = (GuiSkinDefinition)Definition;
                                foreach(var textwidget in skn3tag.TextWidgets)
                                {
                                    textwidget.Definition.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(textwidget.Definition.GuiRenderBlock, scalefactor);
                                }
                                foreach (var bitmapwidget in skn3tag.BitmapWidgets)
                                {
                                    bitmapwidget.Definition.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(bitmapwidget.Definition.GuiRenderBlock, scalefactor);
                                }
                                foreach (var modelwidget in skn3tag.ModelWidgets)
                                {
                                    modelwidget.Definition.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(modelwidget.Definition.GuiRenderBlock, scalefactor);
                                }
                                Cache.Serialize(cacheStream, tag, skn3tag);
                                break;
                            case "scn3":
                                GuiScreenWidgetDefinition scn3tag = (GuiScreenWidgetDefinition)Definition;
                                scn3tag.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(scn3tag.GuiRenderBlock, scalefactor);
                                foreach (var groupwidget in scn3tag.GroupWidgets)
                                {
                                    groupwidget.Definition.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(groupwidget.Definition.GuiRenderBlock, scalefactor);
                                    foreach (var textwidget in groupwidget.Definition.TextWidgets)
                                    {
                                        textwidget.Definition.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(textwidget.Definition.GuiRenderBlock, scalefactor);
                                    }
                                    foreach (var bitmapwidget in groupwidget.Definition.BitmapWidgets)
                                    {
                                        bitmapwidget.Definition.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(bitmapwidget.Definition.GuiRenderBlock, scalefactor);
                                    }
                                    foreach (var listwidget in groupwidget.Definition.ListWidgets)
                                    {
                                        listwidget.Definition.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(listwidget.Definition.GuiRenderBlock, scalefactor);
                                        foreach (var listwidgetitem in listwidget.Definition.Items)
                                        {
                                            listwidgetitem.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(listwidgetitem.GuiRenderBlock, scalefactor);
                                        }
                                    }
                                    foreach (var modelwidget in groupwidget.Definition.ModelWidgets)
                                    {
                                        modelwidget.Definition.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(modelwidget.Definition.GuiRenderBlock, scalefactor);
                                    }
                                }
                                Cache.Serialize(cacheStream, tag, scn3tag);
                                break;
                            case "bkey":
                                GuiButtonKeyDefinition bkeytag = (GuiButtonKeyDefinition)Definition;
                                bkeytag.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(bkeytag.GuiRenderBlock, scalefactor);
                                foreach (var textwidget in bkeytag.TextWidgets)
                                {
                                    textwidget.Definition.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(textwidget.Definition.GuiRenderBlock, scalefactor);
                                }
                                foreach (var bitmapwidget in bkeytag.BitmapWidgets)
                                {
                                    bitmapwidget.Definition.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(bitmapwidget.Definition.GuiRenderBlock, scalefactor);
                                }
                                Cache.Serialize(cacheStream, tag, bkeytag);
                                break;
                            case "lst3":
                                GuiListWidgetDefinition lst3tag = (GuiListWidgetDefinition)Definition;
                                lst3tag.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(lst3tag.GuiRenderBlock, scalefactor);
                                foreach (var listwidgetitem in lst3tag.Items)
                                {
                                    listwidgetitem.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(listwidgetitem.GuiRenderBlock, scalefactor);
                                }
                                Cache.Serialize(cacheStream, tag, lst3tag);
                                break;
                            case "grup":
                                GuiGroupWidgetDefinition gruptag = (GuiGroupWidgetDefinition)Definition;
                                gruptag.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(gruptag.GuiRenderBlock, scalefactor);
                                foreach (var textwidget in gruptag.TextWidgets)
                                {
                                    textwidget.Definition.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(textwidget.Definition.GuiRenderBlock, scalefactor);
                                }
                                foreach (var bitmapwidget in gruptag.BitmapWidgets)
                                {
                                    bitmapwidget.Definition.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(bitmapwidget.Definition.GuiRenderBlock, scalefactor);
                                }
                                foreach (var listwidget in gruptag.ListWidgets)
                                {
                                    listwidget.Definition.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(listwidget.Definition.GuiRenderBlock, scalefactor);
                                    foreach (var listwidgetitem in listwidget.Definition.Items)
                                    {
                                        listwidgetitem.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(listwidgetitem.GuiRenderBlock, scalefactor);
                                    }
                                }
                                foreach (var modelwidget in gruptag.ModelWidgets)
                                {
                                    modelwidget.Definition.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(modelwidget.Definition.GuiRenderBlock, scalefactor);
                                }                              
                                Cache.Serialize(cacheStream, tag, gruptag);
                                break;
                        }
                        
                    }
                }
            }
            Console.WriteLine($"Done!");
            return true;
        }

        public object RescaleGUIDef(GuiDefinition GUIdef, float scalefactor)
        {
            GUIdef.Bounds480i.Top = (short)(GUIdef.Bounds480i.Top * scalefactor);
            GUIdef.Bounds480i.Left = (short)(GUIdef.Bounds480i.Left * scalefactor);
            GUIdef.Bounds480i.Bottom = (short)(GUIdef.Bounds480i.Bottom * scalefactor);
            GUIdef.Bounds480i.Right = (short)(GUIdef.Bounds480i.Right * scalefactor);
            GUIdef.Bounds720p.Top = (short)(GUIdef.Bounds720p.Top * scalefactor);
            GUIdef.Bounds720p.Left = (short)(GUIdef.Bounds720p.Left * scalefactor);
            GUIdef.Bounds720p.Bottom = (short)(GUIdef.Bounds720p.Bottom * scalefactor);
            GUIdef.Bounds720p.Right = (short)(GUIdef.Bounds720p.Right * scalefactor);
            return GUIdef;
        }
    }
}