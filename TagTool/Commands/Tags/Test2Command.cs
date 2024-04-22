using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Commands.Porting;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Commands.Tags
{
    class Test2Command : Command
    {
        private readonly GameCacheHaloOnlineBase Cache;

        public Test2Command(GameCacheHaloOnlineBase cache)
            : base(false, "Test2", "Test2", "Test2", "Test2")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            var ignore = new HashSet<string>(new string[]
            {
                "shaders\\default_bitmaps\\bitmaps\\default_dynamic_cube_map",
                "fx\\particles\\_gradients\\blood_human",
                "ui\\chud\\bitmaps\\red_flash",
                "ui\\chud\\bitmaps\\hud_reticles",
                "ui\\chud\\bitmaps\\ballistic_meters",
                "ui\\chud\\bitmaps\\weapon_scematics",
                "objects\\weapons\\rifle\\smg\\bitmaps\\smg",
                "objects\\powerups\\smg_ammo\\bitmaps\\smg_ammo",
                "objects\\gear\\covenant\\military\\bitmaps\\cov_storage_bump",
                "objects\\weapons\\support_low\\brute_shot\\bitmaps\\brute_shot_bump",
                "fx\\particles\\atmospheric\\_bitmaps\\smoke_fiery_large",
                "objects\\weapons\\melee\\energy_blade\\bitmaps\\energy_blade_handle_bump",
                "objects\\weapons\\pistol\\excavator\\bitmaps\\excavator_bump",
                "objects\\weapons\\support_high\\flak_cannon\\bitmaps\\flak_cannon",
                "objects\\weapons\\support_high\\flak_cannon\\bitmaps\\flak_cannon_bump",
                "objects\\characters\\marine\\bitmaps\\eyes_hazel",
                "objects\\characters\\marine\\bitmaps\\marine_standard_game_diffuse",
                "objects\\characters\\marine\\bitmaps\\marine_standard_zbump",
                "objects\\characters\\marine\\bitmaps\\marine_arm_pack_perm_diffuse",
                "ui\\chud\\bitmaps\\top_middle",
                "ui\\chud\\bitmaps\\bottom_middle",
                "ui\\chud\\bitmaps\\vox",
                "ui\\chud\\bitmaps\\scopes\\human_binoculars",
                "ui\\chud\\bitmaps\\rulers_h",
                "ui\\chud\\bitmaps\\rulers_v",
                "ui\\chud\\bitmaps\\help",
                "levels\\shared\\decorators\\bitmaps\\human_garbage_set",
                "ui\\chud\\bitmaps\\equipment_scematics",
                "ui\\halox\\start_menu\\start_menu_background",
                "ui\\halox\\start_menu\\player_gradient",
                "ui\\halox\\start_menu\\panes\\common\\start_menu_emblems_ui",
                "ui\\halox\\common\\standard_list\\black_bar",
                "ui\\halox\\common\\common_bitmaps\\third_column",
                "ui\\halox\\start_menu\\panes\\game_editor\\controller_ui",
                "ui\\halox\\start_menu\\panes\\settings\\settings_ui",
                "ui\\halox\\start_menu\\panes\\settings_controls_button\\buttons_ui",
                "ui\\halox\\start_menu\\panes\\settings_controls_thumbstick\\thumbsticks_ui",
                "ui\\halox\\start_menu\\panes\\settings_display\\calibration_range_ui",
                "ui\\halox\\alert\\toast_icons_ui",
                "ui\\halox\\matchmaking\\outer_ring_ui",
                "ui\\halox\\matchmaking\\middle_ring_ui",
                "ui\\halox\\matchmaking\\inner_ring_ui",
                "ui\\halox\\alert\\alert_bkd",
                "ui\\halox\\in_progress\\outer_ring_ui",
                "ui\\halox\\in_progress\\middle_ring_ui",
                "ui\\halox\\in_progress\\inner_ring_ui",
                "ui\\halox\\scoreboard\\player_ui",
                "ui\\halox\\common\\roster\\outer_ring_ui",
                "ui\\halox\\common\\roster\\exp_med_ui",
                "ui\\halox\\director\\camera_ui",
                "ui\\halox\\director\\play_ui",
                "ui\\halox\\director\\pause_ui",
                "ui\\halox\\director\\eject_ui",
                "ui\\halox\\pregame_lobby\\difficulty_large_ui",
                "ui\\halox\\main_menu\\bungielogo",
                "ui\\chud\\bitmaps\\chud_microtexture",
                "ui\\chud\\bitmaps\\waypoints",
                "ui\\chud\\bitmaps\\scoreboard_meter",
                "ui\\chud\\bitmaps\\medals\\campaign_medals",
                "ui\\chud\\bitmaps\\savedfilm_baroutline",
                "levels\\shared\\bitmaps\\nature\\water\\water_ripples",
                "levels\\shared\\bitmaps\\nature\\fog\\patchy_fog_generic",
                "objects\\vehicles\\pelican\\bitmaps\\pelican_hull",
                "objects\\vehicles\\phantom\\bitmaps\\phantom_bump",
                "objects\\weapons\\turret\\plasma_cannon\\bitmaps\\plasma_cannon_bump",
                "ui\\chud\\bitmaps\\hud_vehicle_reticles",
                "objects\\weapons\\support_high\\rocket_launcher\\bitmaps\\rocket_launcher_bump",
                "objects\\powerups\\sniper_rifle_ammo\\bitmaps\\sr_ammo",
                "objects\\powerups\\sniper_rifle_ammo\\bitmaps\\sr_ammo_bump",
                "objects\\gear\\human\\military\\bitmaps\\decals_weathered",
                "objects\\powerups\\shotgun_ammo\\bitmaps\\sg_ammo",
                "objects\\powerups\\shotgun_ammo\\bitmaps\\sg_ammo_bump",
                "objects\\gear\\human\\industrial\\bitmaps\\propane_tank_illume",
                "levels\\solo\\010_jungle\\bitmaps\\tech\\tech_dock_valve",
                "levels\\solo\\030_outskirts\\bitmaps\\out_voi_corrug_diff",
                "levels\\solo\\030_outskirts\\bitmaps\\out_voi_corrug_bump",
                "levels\\solo\\040_voi\\bitmaps\\metals\\metal_detail_rust_a",
                "levels\\solo\\040_voi\\bitmaps\\metals\\metal_detail_bump_a",
                "objects\\vehicles\\warthog\\bitmaps\\glass_damage_bump",
                "objects\\vehicles\\warthog\\turrets\\chaingun\\bitmaps\\chaingun_bump",
                "objects\\vehicles\\warthog\\turrets\\gauss\\bitmaps\\gauss_bump",
                "objects\\vehicles\\warthog\\turrets\\troop\\bitmaps\\troop_bump",
                "objects\\vehicles\\civilian\\bitmaps\\truck_cab_large_changecolor",
                "objects\\gear\\human\\residential\\bitmaps\\tables_diff",
                "objects\\gear\\human\\residential\\bitmaps\\tables_bump",
                "objects\\vehicles\\civilian\\bitmaps\\tanker_bump",
                "objects\\gear\\human\\military\\bitmaps\\barricade_large",
                "levels\\solo\\100_citadel\\bitmaps\\detail_panel_tile_int",
                "objects\\characters\\bugger\\bitmaps\\bugger_diffuse",
                "objects\\characters\\bugger\\bitmaps\\bugger_illum",
                "objects\\characters\\hunter\\bitmaps\\hunter_diffuse",
                "levels\\solo\\040_voi\\bitmaps\\metals\\metal_factory_outerpanel_i_bump",
                "objects\\vehicles\\wraith\\bitmaps\\wraith_bump",
                "objects\\vehicles\\wraith\\turrets\\mortar\\bitmaps\\mortar_bump",
                "objects\\vehicles\\wraith\\turrets\\anti_infantry\\bitmaps\\anti_infantry_bump",
                "objects\\gear\\human\\industrial\\bitmaps\\crate_heavy_tech_bump",
                "levels\\shared\\decorators\\bitmaps\\grass_thick_outskirts",
                "levels\\solo\\030_outskirts\\bitmaps\\natural\\valley_dirt",
                "objects\\vehicles\\ghost\\bitmaps\\ghost_bump",
                "levels\\shared\\decals\\nature\\rock\\decal_dirt",
                "objects\\gear\\human\\industrial\\bitmaps\\dumpster",
                "objects\\cinematics\\human\\frigate\\bitmaps\\frigate_3_base",
                "objects\\cinematics\\human\\frigate\\bitmaps\\frigate_shrine_decals",
                "levels\\shared\\decorators\\bitmaps\\bolt_voi",
                "levels\\solo\\040_voi\\bitmaps\\concrete\\asphalt_a_diffuse",
                "levels\\solo\\040_voi\\bitmaps\\metals\\metal_floorplate_b_01",
                "objects\\characters\\odst\\bitmaps\\odst_helmet_diffuse",
                "objects\\characters\\odst\\bitmaps\\odst_helmet_zbump",
                "objects\\characters\\odst\\bitmaps\\odst_armor_detailbump",
                "objects\\characters\\odst\\bitmaps\\sarge_stripe_cc",
                "objects\\characters\\odst\\bitmaps\\badges_diffuse",
                "objects\\characters\\odst\\bitmaps\\odst_body_diffuse",
                "objects\\characters\\odst\\bitmaps\\odst_body_zbump",
                "levels\\solo\\070_waste\\bitmaps\\waste_panel_wall_diagonal",
                "levels\\solo\\070_waste\\bitmaps\\light_plate_illum",
                "levels\\solo\\070_waste\\bitmaps\\waste_door_frame_small",
                "levels\\solo\\070_waste\\bitmaps\\light_plate",
                "objects\\vehicles\\scorpion\\bitmaps\\scorpion_hull_bump",
                "objects\\vehicles\\scorpion\\turrets\\anti_infantry\\bitmaps\\anti_infantry_bump",
                "levels\\solo\\070_waste\\bitmaps\\waste_panel_trim",
                "levels\\solo\\070_waste\\bitmaps\\waste_panel_wall_inset",
                "levels\\solo\\070_waste\\bitmaps\\waste_panel_wall_inset_albedo",
                "levels\\solo\\100_citadel\\bitmaps\\panel_hall_floor",
                "levels\\solo\\070_waste\\bitmaps\\waste_panel_column",
                "levels\\solo\\070_waste\\bitmaps\\waste_panel_wall_mech",
                "levels\\solo\\070_waste\\bitmaps\\waste_panel_wall_horizontal",
                "levels\\solo\\100_citadel\\bitmaps\\panel_poop_ramp",
                "levels\\solo\\100_citadel\\bitmaps\\panel_wall_vert",
                "levels\\solo\\100_citadel\\bitmaps\\panel_light_ceiling",
                "levels\\solo\\100_citadel\\bitmaps\\panel_pipe_simple",
                "levels\\solo\\100_citadel\\bitmaps\\panel_floor_ele",
                "levels\\solo\\100_citadel\\bitmaps\\panel_floor_ele_parallax",
                "levels\\solo\\100_citadel\\bitmaps\\panel_glass",
                "levels\\solo\\100_citadel\\bitmaps\\panel_wall_alcove",
                "levels\\solo\\100_citadel\\bitmaps\\panel_wall_control_floor_2",
                "levels\\solo\\100_citadel\\bitmaps\\panel_bsp_160_floor",
                "levels\\solo\\100_citadel\\bitmaps\\panel_elevator_generator",
                "levels\\solo\\100_citadel\\bitmaps\\panel_elevator_generator_albedo",
                "levels\\solo\\100_citadel\\bitmaps\\decals\\decal_moss_mask",
                "levels\\solo\\100_citadel\\bitmaps\\panel_ceiling_corner",
                "levels\\solo\\100_citadel\\bitmaps\\panel_floor_ele_corner",
                "levels\\multi\\snowbound\\bitmaps\\snow_speckle_detail",
                "shaders\\default_bitmaps\\bitmaps\\gray_50_percent_linear",
                "ui\\halox\\common\\common_bitmaps\\emblems",
                "ui\\chud\\bitmaps\\objective_help",
                "ui\\chud\\bitmaps\\help_cap_left",
                "levels\\multi\\snowbound\\bitmaps\\cube_icecave_a_cubemap",
                "fx\\particles\\material\\_bitmaps\\bubbles",
                "objects\\levels\\multi\\salvation\\bitmaps\\plasma_large_red_mask",
                "levels\\dlc\\docks\\sky\\bitmaps\\water_base",
                "levels\\dlc\\docks\\sky\\bitmaps\\water_bump",
                "levels\\dlc\\docks\\bitmaps\\metal\\metal_detail_bump_a",
                "levels\\dlc\\shared\\decorators\\bitmaps\\ghosttown_board",
                "levels\\dlc\\fortress\\bitmaps\\waste_panel_pipe_illum",
                "levels\\dlc\\fortress\\bitmaps\\panel_platform_center",
                "levels\\dlc\\fortress\\bitmaps\\panel_platform_center_glow",
                "levels\\dlc\\fortress\\bitmaps\\glass_control",
                "levels\\dlc\\fortress\\bitmaps\\panel_wall_alcove_diffuse",
                "levels\\dlc\\fortress\\bitmaps\\panel_wall_hologram_illum",
                "levels\\dlc\\fortress\\bitmaps\\scanline_illum",
                "levels\\dlc\\fortress\\bitmaps\\panel_wall_hologram_illum2",
                "levels\\dlc\\ghosttown\\bitmaps\\brick_b_bump",
                "levels\\ui\\mainmenu\\objects\\matte_appearance\\bitmaps\\matte_appearance",
                "ui\\halox\\main_menu\\mainmenu_bkd",
                "ui\\halox\\main_menu\\bottom_gradient_ui",
                "ui\\halox\\main_menu\\halo3_logo_ui",
                "ui\\halox\\start_menu\\panes\\hq_service_record_file_share\\file_types_sm_ui",
                "ui\\halox\\start_menu\\panes\\hq_service_record_file_share\\file_types_lg_ui",
                "ui\\halox\\start_menu\\panes\\hq_service_record_file_share\\empty_screen_ui",
                "ui\\halox\\start_menu\\panes\\hq\\headquarters",
                "ui\\halox\\start_menu\\panes\\hq\\bunpro_uui",
                "ui\\halox\\common\\common_bitmaps\\dark_gradient_ui",
                "ui\\halox\\start_menu\\panes\\hq_service_record_file_share\\meter_border_short_ui",
                "ui\\halox\\start_menu\\panes\\hq_service_record_file_share\\game_types_sm_ui",
                "ui\\halox\\start_menu\\panes\\hq_service_record_file_share\\game_types_lg_ui",
                "ui\\halox\\start_menu\\panes\\hq_service_record_file_share\\bunpro_button_ui",
                "ui\\halox\\start_menu\\panes\\hq_service_record_file_share\\bnet_pro_alert_ui",
                "ui\\halox\\start_menu\\panes\\hq_service_record_file_share\\meter_border_ui",
                "ui\\halox\\start_menu\\panes\\hq_service_record_file_share\\empty_lg_ui",
                "ui\\halox\\pregame_lobby\\selection\\network",
                "ui\\halox\\start_menu\\panes\\settings_appearance_colors\\color_swatches_disabled",
                "ui\\halox\\start_menu\\panes\\settings_appearance_colors\\color_swatches",
                "ui\\halox\\start_menu\\panes\\settings_appearance_emblem\\emblem_foregrounds_ui",
                "ui\\halox\\start_menu\\panes\\settings_appearance_emblem\\emblem_backgrounds_ui",
                "ui\\halox\\pregame_lobby\\channel_ui",
                "ui\\halox\\common\\roster\\roster_unfocused_ui",
                "ui\\halox\\pregame_lobby\\progress_ui",
                "ui\\halox\\pregame_lobby\\nightmap_background_ui",
                "ui\\halox\\pregame_lobby\\map_images\\unknown_film_ui",
                "ui\\halox\\pregame_lobby\\selection\\films_ui",
                "ui\\halox\\pregame_lobby\\maximum_party_size\\hilite_ui",
                "ui\\halox\\pregame_lobby\\maximum_party_size\\disabledots_ui",
                "ui\\halox\\pregame_lobby\\maximum_party_size\\lock_new_ui",
                "ui\\halox\\game_browser\\browser_player_unfocused_ui",
                "ui\\halox\\matchmaking\\state_plate_ui",
                "ui\\halox\\common\\common_bitmaps\\third_column_short",
                "ui\\halox\\carnage_report\\medals_med_ui",
                "ui\\halox\\carnage_report\\medals_hilite_ui",
                "ui\\halox\\carnage_report\\weapon_scematics",
                "ui\\halox\\carnage_report\\medals_bkd_ui",
                "ui\\halox\\pregame_lobby\\advanced_screen\\warthog_01_ui",
                "ui\\halox\\campaign\\skulls_med_ui",
                "ui\\halox\\campaign\\glowin_red_eyes_ui",
                "ui\\halox\\campaign\\secondary_skulls_med_ui",
                "ui\\halox\\campaign\\skulls_lg_ui",
                "ui\\halox\\campaign\\secondary_skulls_lg_ui",
                "@@@fake_lightmap_primary_dxt5_@@@",
                "@@@fake_lightmap_intensity_dxt5_@@@",
                "levels\\dlc\\spacecamp\\bitmaps\\mc_detail_bumpy_zbump",
                "levels\\dlc\\spacecamp\\bitmaps\\mc_metal_floor_plate_trim_b_zbump"
            });


            if (args.Count < 1)
                return false;

            var mapsDir = new DirectoryInfo(args[0]);

            if (args.Count > 1)
                ResourceCachesHaloOnline.ResourceCacheNames[ResourceLocation.Textures] = args[1];

            var converted = new List<(CachedTag tag, long resourcesSize)>();

            foreach (var file in mapsDir.GetFiles("*.map"))
            {
                Console.WriteLine($"{file.Name}...");

                var srcCache = GameCache.Open(file);
                if (srcCache.TagCache == null || srcCache.TagCache.Count == 0)
                    continue;

                var resourceStreams = new Dictionary<ResourceLocation, Stream>();
                using (var srcStream = srcCache.OpenCacheRead())
                using (var dstStream = Cache.OpenCacheReadWrite())
                {
                    long totalResourcesSize = 0;
                    foreach (var tag in srcCache.TagCache.NonNull().Where(x => x.IsInGroup("bitm")))
                    {
                        CachedTag existingTag;
                        if (Cache.TagCache.TryGetTag(tag.ToString(), out existingTag))
                            continue;

                        if (tag.Name.Contains("lightmap") || tag.Name.Contains("cubemap") || ignore.Contains(tag.Name))
                            continue;

                        Console.WriteLine($"Converting {tag}...");

                        var portTag = new PortTagCommand(Cache, srcCache);
                        portTag.SetFlags(PortTagCommand.PortingFlags.Default);

                        var convertedTag = portTag.ConvertTag(dstStream, srcStream, resourceStreams, tag);

                        if (convertedTag == null)
                        {
                            Console.WriteLine($"[ERROR]: Failed to convert tag '{tag}'");
                            continue;
                        }

                        var resourcesSize = GetTagResourcesSize(dstStream, Cache, convertedTag);
                        totalResourcesSize += resourcesSize;

                        converted.Add((convertedTag, resourcesSize));
                    }

                    foreach (var stream in resourceStreams)
                        stream.Value.Close();
                    Cache.SaveStrings();
                    Cache.SaveTagNames();
                }

                using (var convertedDump = File.CreateText("converted.txt"))
                {
                    long totalResourcesSize = 0;
                    foreach (var entry in converted)
                    {
                        totalResourcesSize += entry.resourcesSize;
                        convertedDump.WriteLine($"{entry.resourcesSize / 1024.0 / 1024.0:0.000} MB \t\t{entry.tag.Name}.{entry.tag.Group.Tag}");
                    }

                    convertedDump.WriteLine($"Total: {totalResourcesSize / 1024.0 / 1024.0} MB");
                }
            }

            return true;
        }

        long GetTagResourcesSize(Stream stream, GameCache cache, CachedTag tag)
        {
            var definition = cache.Deserialize(stream, tag);

            return PageableResourceCollector.Collect(cache, definition as TagStructure)
                .Sum(x => x.Page.CompressedBlockSize);
        }

        class PageableResourceCollector
        {
            private readonly GameCache _cache;
            private readonly List<PageableResource> _pageableResources = new List<PageableResource>();

            public PageableResourceCollector(GameCache cache, TagStructure tagStructure)
            {
                _cache = cache;
                VisitTagStructure(tagStructure);
            }

            public static IEnumerable<PageableResource> Collect(GameCache cache, TagStructure tagStructure)
            {
                return new PageableResourceCollector(cache, tagStructure)._pageableResources;
            }

            private void VisitData(object data)
            {
                switch (data)
                {
                    case PageableResource pageableResource:
                        VisitPageableResource(pageableResource);
                        break;

                    case TagStructure tagStructure:
                        VisitTagStructure(tagStructure);
                        break;
                    case IList collection:
                        VisitCollection(collection);
                        break;
                }
            }

            private void VisitCollection(IList collection)
            {
                foreach (var element in collection)
                    VisitData(element);
            }

            private void VisitPageableResource(PageableResource pageableResource)
            {
                _pageableResources.Add(pageableResource);
            }

            private void VisitTagStructure(TagStructure tagStructure)
            {
                foreach (var field in tagStructure.GetTagFieldEnumerable(_cache.Version, _cache.Platform))
                {
                    var data = field.GetValue(tagStructure);
                    VisitData(data);
                }
            }
        }
    }
}
