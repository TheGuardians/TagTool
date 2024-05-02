(global short player_1_count -1)
(global short player_2_count -1)
(global short player_3_count -1)

(global long player_1_anim -1)
(global long player_2_anim -1)
(global long player_3_anim -1)

(global short player_1_index -1)
(global short player_2_index -1)
(global short player_3_index -1)

(script extern long (pdm_get_player_animation_count (short idx)))
(script extern animationgraph (pdm_get_player_animation_tag (short idx)))
(script extern stringid (pdm_get_player_animation_str (short type) (short idx) (long anim)))
(script extern real (pdm_get_animation_offset (short idx) (long anim)))
(script extern anytag (pdm_get_animation_weapon (short idx) (long anim) (boolean primary)))
(script extern void (pdm_show_ui_place (short place) (short idx)))
(script extern void (pdm_respawn_player_offset (short idx) (cutscenecamerapoint camera) (real dis) (real hor) (real vert) (real yaw) (real pitch)))
(script extern void (pdm_event (string event)))
(script extern short player_get_local_idx)
(script extern short (player_get_idx_by_place (short place)))
(script extern void (hide_all_players (boolean hidden)))
(script extern void (delete_all_objects (boolean delete_modifiers)))
(script extern void (give_unit_weapon (unit player) (anytag primary) (anytag secondary)))
(script extern object (player_get_by_idx (short idx)))
(script extern short lobby_size)
(script extern effect (pdm_get_respawn_effect (short place)))
(script extern real (pdm_get_biped_offset (string field_type) (short place)))
(script extern long (pdm_get_biped_spawn_delay (short place)))
(script extern long pdm_get_tick_delay)
(script extern boolean pdm_has_forge_object)
(script extern void respawn_map_objects)
(script extern void mp_hide_scoreboard)
(script extern void player_reset_sprint)
(script extern void (pdm_forge_override_allowed (boolean allowed)))
(script extern void (mp_sync_global (string global)))

(script static void podium_init
	; Allow forge map variants to place their own podium spots to override the cutscene camera position
	(pdm_forge_override_allowed true)
	; Tell the engine that podium is starting so that the player scoreboard will not be displayed when the match ends automatically so that we can display it after
	(pdm_event "podium_init")
)

(script static void podium
	; Sync player places so clients dont need to look this up
	(set player_1_index (player_get_idx_by_place 1))
	(set player_2_index (player_get_idx_by_place 2))
	(set player_3_index (player_get_idx_by_place 3))
	(mp_sync_global player_1_index)
	(mp_sync_global player_2_index)
	(mp_sync_global player_3_index)

	; Set up and sync the player animation index to be used for this podium so that all clients are in sync
	(set player_1_count (pdm_get_player_animation_count player_1_index))
	(set player_2_count (pdm_get_player_animation_count player_2_index))
	(set player_3_count (pdm_get_player_animation_count player_3_index))
	(set player_1_anim (random_range 0 player_1_count))
	(set player_2_anim (random_range 0 player_2_count))
	(set player_3_anim (random_range 0 player_3_count))
	(mp_sync_global player_1_anim)
	(mp_sync_global player_2_anim)
	(mp_sync_global player_3_anim)
	
	; Add additional time to podium if there are more players
	(if (>= (lobby_size) 3)
		(game_finished_wait_time_add 2)
	)
	(if (>= (lobby_size) 2)
		(game_finished_wait_time_add 2)
	)
	(if (>= (lobby_size) 1)
		(game_finished_wait_time_add 18)
	)
	(sleep 100)
	; Fade out and prepare all the players to be ready for podium
	(wake prepare_players)
	(sleep 11)
	; Start the podium cutscene
	(mp_wake_script podium_start)
)

(script dormant void prepare_players
	(mp_wake_script fade_out_players)
	(sleep 1)
	; Hide all objects so that nothing is in the way for podium
	(if (pdm_has_forge_object)
		(respawn_map_objects)
		(delete_all_objects true)
	)
	(sleep 5)
	; Respawn the player in front of the camera so that they dont keep any states they were previously in when the game ended.
	(pdm_respawn_player_offset player_1_index "podium_camera" (- (pdm_get_biped_offset "x" 0) (pdm_get_animation_offset player_1_index player_1_anim)) (pdm_get_biped_offset "y" 0) (pdm_get_biped_offset "z" 0) (pdm_get_biped_offset "i" 0) 90)
	(pdm_respawn_player_offset player_2_index "podium_camera" (- (pdm_get_biped_offset "x" 1) (pdm_get_animation_offset player_2_index player_2_anim)) (pdm_get_biped_offset "y" 1) (pdm_get_biped_offset "z" 1) (pdm_get_biped_offset "i" 1) 90)
	(pdm_respawn_player_offset player_3_index "podium_camera" (- (pdm_get_biped_offset "x" 2) (pdm_get_animation_offset player_3_index player_3_anim)) (pdm_get_biped_offset "y" 2) (pdm_get_biped_offset "z" 2) (pdm_get_biped_offset "i" 2) 90)
	(sleep 5)
	; Give the player the weapon they need for the animation that was chosen
	(give_unit_weapon (unit (player_get_by_idx player_1_index)) (pdm_get_animation_weapon player_1_index player_1_anim true) (pdm_get_animation_weapon player_1_index player_1_anim false))
	(give_unit_weapon (unit (player_get_by_idx player_2_index)) (pdm_get_animation_weapon player_2_index player_2_anim true) (pdm_get_animation_weapon player_2_index player_2_anim false))
	(give_unit_weapon (unit (player_get_by_idx player_3_index)) (pdm_get_animation_weapon player_3_index player_3_anim true) (pdm_get_animation_weapon player_3_index player_3_anim false))
	(mp_wake_script hide_players)
)

(script dormant void fade_out_players
	; Fade out the screens and disable all inputs and controls
	(mp_hide_scoreboard)
	(fade_out 0 0 0 0)
	(chud_cinematic_fade 0 0)
	(chud_show_messages false)
	(sound_class_set_gain "ui" 0 0)
	(player_enable_input false)
	(player_disable_movement true)
	(player_camera_control false)
	(player_reset_sprint)
	(pdm_event "podium_start")
)

(script dormant void hide_players
	; Hide all players so that no one is still visible during podium
	(hide_all_players true)
)

(script dormant void podium_start
	(print "Podium Started")
	(sound_class_set_gain "ui" 1 0)
	(sleep 1)
	(camera_control true)
	(camera_set "podium_camera" 0)
	(fade_in 0 0 0 40)
	(sleep (pdm_get_tick_delay))
	; Start animating the players for podium
	(if (>= (lobby_size) 3)
		(begin
			(sleep (pdm_get_biped_spawn_delay 2))
			(wake player_third)
			(sleep (pdm_get_biped_spawn_delay 1))
		)
	)
	(if (>= (lobby_size) 2)
		(begin
			(wake player_second)
			(sleep (pdm_get_biped_spawn_delay 0))
		)
	)
	(if (>= (lobby_size) 1)
		(begin
			(wake player_first)
		)
	)
	(sleep 300)
	; Hide the podium screen and show the scoreboard
	(pdm_show_ui_place -1 -1)
	(sleep 25)
	(sound_impulse_start "sound\ui\scoreboard\appear_whoosh" "none" 1)
	(sleep_forever)
)

(script dormant void player_third
	(sleep 1)
	; Enable Ground fitting so the player biped will sit directly on the ground rather than floating slightly above the surface
	(biped_force_ground_fitting_on (unit (player_get_by_idx player_3_index)) true)
	; Play the intial podium animation on the player
	(custom_animation (unit (player_get_by_idx player_3_index)) (pdm_get_player_animation_tag player_3_index) (pdm_get_player_animation_str 0 player_3_index player_3_anim) false)
	; Start the spawning effect on the player model
	(effect_new_on_object_marker (pdm_get_respawn_effect 2) (player_get_by_idx player_3_index) "body")
	(sleep 2)
	; Display the podium screen ui for this player
	(pdm_show_ui_place 3 -1)
	(sound_impulse_start "sound\podium\third_place" "none" 1)
	; Reveal the player after the animations and effects have been set so the viewer doesnt see any of the idle animations
	(object_hide (player_get_by_idx player_3_index) false)
	; Wait until the initial animation is done then start the final looping animation for the player model
	; Loop the idle animation incase the player emotes so that they return to the idle animation
	(sleep_until 
		(begin
			(sleep (unit_get_custom_animation_time (unit (player_get_by_idx player_3_index))))
			(custom_animation_loop (unit (player_get_by_idx player_3_index)) (pdm_get_player_animation_tag player_3_index) (pdm_get_player_animation_str 1 player_3_index player_3_anim) true)
			(sleep 1)
		)
	-1)
	(sleep_forever)
)

(script dormant void player_second
	(sleep 1)
	; Enable Ground fitting so the player biped will sit directly on the ground rather than floating slightly above the surface
	(biped_force_ground_fitting_on (unit (player_get_by_idx player_2_index)) true)
	; Play the intial podium animation on the player
	(custom_animation (unit (player_get_by_idx player_2_index)) (pdm_get_player_animation_tag player_2_index) (pdm_get_player_animation_str 0 player_2_index player_2_anim) false)
	; Start the spawning effect on the player model
	(effect_new_on_object_marker (pdm_get_respawn_effect 1) (player_get_by_idx player_2_index) "body")
	(sleep 2)
	; Display the podium screen ui for this player
	(pdm_show_ui_place 2 -1)
	(sound_impulse_start "sound\podium\second_place" "none" 1)
	; Reveal the player after the animations and effects have been set so the viewer doesnt see any of the idle animations
	(object_hide (player_get_by_idx player_2_index) false)
	; Wait until the initial animation is done then start the final looping animation for the player model
	; Loop the idle animation incase the player emotes so that they return to the idle animation
	(sleep_until 
		(begin
			(sleep (unit_get_custom_animation_time (unit (player_get_by_idx player_2_index))))
			(custom_animation_loop (unit (player_get_by_idx player_2_index)) (pdm_get_player_animation_tag player_2_index) (pdm_get_player_animation_str 1 player_2_index player_2_anim) true)
			(sleep 1)
		)
	-1)
	(sleep_forever)
)

(script dormant void player_first
	(sleep 1)
	; Enable Ground fitting so the player biped will sit directly on the ground rather than floating slightly above the surface
	(biped_force_ground_fitting_on (unit (player_get_by_idx player_1_index)) true)
	; Play the intial podium animation on the player
	(custom_animation (unit (player_get_by_idx player_1_index)) (pdm_get_player_animation_tag player_1_index) (pdm_get_player_animation_str 0 player_1_index player_1_anim) false)
	; Start the spawning effect on the player model
	(effect_new_on_object_marker (pdm_get_respawn_effect 0) (player_get_by_idx player_1_index) "body")
	(sleep 2)
	; Display the podium screen ui for this player
	(pdm_show_ui_place 1 player_1_index)
	(sound_impulse_start "sound\podium\first_place" "none" 1)
	; Reveal the player after the animations and effects have been set so the viewer doesnt see any of the idle animations
	(object_hide (player_get_by_idx player_1_index) false)
	; Wait until the initial animation is done then start the final looping animation for the player model
	; Loop the idle animation incase the player emotes so that they return to the idle animation
	(sleep_until 
		(begin
			(sleep (unit_get_custom_animation_time (unit (player_get_by_idx player_1_index))))
			(custom_animation_loop (unit (player_get_by_idx player_1_index)) (pdm_get_player_animation_tag player_1_index) (pdm_get_player_animation_str 1 player_1_index player_1_anim) true)
			(sleep 1)
		)
	-1)
	(sleep_forever)
)