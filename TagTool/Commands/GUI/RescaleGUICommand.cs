using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.GUI;
using TagTool.Tags.Definitions;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Cache.HaloOnline;

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
            if (args.Count > 0)
            {
                scalefactor = float.Parse(args[0]);
            }
            List<string> TargetTagGroups = new List<string>{ "bmp3", "skn3", "txt3", "lst3", "grup", "bkey", "mdl3", "scn3" };
            foreach (var tag in Cache.TagCache.TagTable)
            {
                if (tag == null)
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
                                    textwidget.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(textwidget.GuiRenderBlock, scalefactor);
                                }
                                foreach (var bitmapwidget in skn3tag.BitmapWidgets)
                                {
                                    bitmapwidget.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(bitmapwidget.GuiRenderBlock, scalefactor);
                                }
                                foreach (var modelwidget in skn3tag.ModelWidgets)
                                {
                                    modelwidget.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(modelwidget.GuiRenderBlock, scalefactor);
                                }
                                Cache.Serialize(cacheStream, tag, skn3tag);
                                break;
                            case "scn3":
                                GuiScreenWidgetDefinition scn3tag = (GuiScreenWidgetDefinition)Definition;
                                scn3tag.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(scn3tag.GuiRenderBlock, scalefactor);
                                foreach (var groupwidget in scn3tag.GroupWidgets)
                                {
                                    groupwidget.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(groupwidget.GuiRenderBlock, scalefactor);
                                    foreach (var textwidget in groupwidget.TextWidgets)
                                    {
                                        textwidget.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(textwidget.GuiRenderBlock, scalefactor);
                                    }
                                    foreach (var bitmapwidget in groupwidget.BitmapWidgets)
                                    {
                                        bitmapwidget.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(bitmapwidget.GuiRenderBlock, scalefactor);
                                    }
                                    foreach (var listwidget in groupwidget.ListWidgets)
                                    {
                                        listwidget.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(listwidget.GuiRenderBlock, scalefactor);
                                        foreach (var listwidgetitem in listwidget.ListWidgetItems)
                                        {
                                            listwidgetitem.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(listwidgetitem.GuiRenderBlock, scalefactor);
                                        }
                                    }
                                    foreach (var modelwidget in groupwidget.ModelWidgets)
                                    {
                                        modelwidget.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(modelwidget.GuiRenderBlock, scalefactor);
                                    }
                                }
                                Cache.Serialize(cacheStream, tag, scn3tag);
                                break;
                            case "bkey":
                                GuiButtonKeyDefinition bkeytag = (GuiButtonKeyDefinition)Definition;
                                bkeytag.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(bkeytag.GuiRenderBlock, scalefactor);
                                foreach (var textwidget in bkeytag.TextWidgets)
                                {
                                    textwidget.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(textwidget.GuiRenderBlock, scalefactor);
                                }
                                foreach (var bitmapwidget in bkeytag.BitmapWidgets)
                                {
                                    bitmapwidget.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(bitmapwidget.GuiRenderBlock, scalefactor);
                                }
                                Cache.Serialize(cacheStream, tag, bkeytag);
                                break;
                            case "lst3":
                                GuiListWidgetDefinition lst3tag = (GuiListWidgetDefinition)Definition;
                                lst3tag.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(lst3tag.GuiRenderBlock, scalefactor);
                                foreach (var listwidgetitem in lst3tag.ListWidgetItems)
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
                                    textwidget.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(textwidget.GuiRenderBlock, scalefactor);
                                }
                                foreach (var bitmapwidget in gruptag.BitmapWidgets)
                                {
                                    bitmapwidget.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(bitmapwidget.GuiRenderBlock, scalefactor);
                                }
                                foreach (var listwidget in gruptag.ListWidgets)
                                {
                                    listwidget.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(listwidget.GuiRenderBlock, scalefactor);
                                    foreach (var listwidgetitem in listwidget.ListWidgetItems)
                                    {
                                        listwidgetitem.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(listwidgetitem.GuiRenderBlock, scalefactor);
                                    }
                                }
                                foreach (var modelwidget in gruptag.ModelWidgets)
                                {
                                    modelwidget.GuiRenderBlock = (GuiDefinition)RescaleGUIDef(modelwidget.GuiRenderBlock, scalefactor);
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
            GUIdef.StandardXMax = (short)(GUIdef.StandardXMax * scalefactor);
            GUIdef.StandardXMin = (short)(GUIdef.StandardXMin * scalefactor);
            GUIdef.StandardYMax = (short)(GUIdef.StandardYMax * scalefactor);
            GUIdef.StandardYMin = (short)(GUIdef.StandardYMin * scalefactor);
            GUIdef.WidescreenXMax = (short)(GUIdef.WidescreenXMax * scalefactor);
            GUIdef.WidescreenXMin = (short)(GUIdef.WidescreenXMin * scalefactor);
            GUIdef.WidescreenYMax = (short)(GUIdef.WidescreenYMax * scalefactor);
            GUIdef.WidescreenYMin = (short)(GUIdef.WidescreenYMin * scalefactor);
            return GUIdef;
        }
    }
}