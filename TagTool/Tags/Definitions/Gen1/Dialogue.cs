using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "dialogue", Tag = "udlg", Size = 0x1010)]
    public class Dialogue : TagStructure
    {
        [TagField(Length = 0x2)]
        public byte[] Unknown;
        [TagField(Length = 0x2)]
        public byte[] Padding;
        [TagField(Length = 0xC)]
        public byte[] Padding1;
        /// <summary>
        /// vocalizations generated at intervals when nothing else is happening.
        /// </summary>
        /// <summary>
        /// played randomly and intermittently whenever we aren't in combat
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag IdleNoncombat;
        /// <summary>
        /// played randomly and intermittently whenever we're in combat
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag IdleCombat;
        /// <summary>
        /// played continuously while we are fleeing
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag IdleFlee;
        [TagField(Length = 0x10)]
        public byte[] Padding2;
        [TagField(Length = 0x10)]
        public byte[] Padding3;
        [TagField(Length = 0x10)]
        public byte[] Padding4;
        /// <summary>
        /// vocalizations generated automatically when damaged; interrupt everything except scripted dialogue.
        /// </summary>
        /// <summary>
        /// took body damage
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PainBodyMinor;
        /// <summary>
        /// took a significant amount of body damage
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PainBodyMajor;
        /// <summary>
        /// took shield damage
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PainShield;
        /// <summary>
        /// took damage from falling
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PainFalling;
        /// <summary>
        /// screaming in fear (falling to your death, explosive stuck to you)
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag ScreamFear;
        /// <summary>
        /// screaming in pain (being flamed)
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag ScreamPain;
        /// <summary>
        /// limb body part (arm or leg) was destroyed
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag MaimedLimb;
        /// <summary>
        /// head body part was destroyed
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag MaimedHead;
        /// <summary>
        /// died from minor damage, or was unprepared
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag DeathQuiet;
        /// <summary>
        /// died from violent trauma
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag DeathViolent;
        /// <summary>
        /// died from falling
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag DeathFalling;
        /// <summary>
        /// died in a horribly painful fashion (burnt to death)
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag DeathAgonizing;
        /// <summary>
        /// died instantly
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag DeathInstant;
        /// <summary>
        /// died and was blown up into the air
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag DeathFlying;
        [TagField(Length = 0x10)]
        public byte[] Padding5;
        /// <summary>
        /// hurt a friendly AI
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag DamagedFriend;
        /// <summary>
        /// hurt a friendly player
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag DamagedFriendPlayer;
        /// <summary>
        /// hurt an enemy
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag DamagedEnemy;
        /// <summary>
        /// hurt an enemy: comment to friend
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag DamagedEnemyCm;
        [TagField(Length = 0x10)]
        public byte[] Padding6;
        [TagField(Length = 0x10)]
        public byte[] Padding7;
        [TagField(Length = 0x10)]
        public byte[] Padding8;
        [TagField(Length = 0x10)]
        public byte[] Padding9;
        /// <summary>
        /// hurt by a friendly AI
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HurtFriend;
        /// <summary>
        /// hurt by a friendly AI: response from that friend
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HurtFriendRe;
        /// <summary>
        /// hurt by a friendly player
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HurtFriendPlayer;
        /// <summary>
        /// hurt by an enemy
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HurtEnemy;
        /// <summary>
        /// hurt by an enemy: response from the enemy that hurt us ('you like that?')
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HurtEnemyRe;
        /// <summary>
        /// hurt by an enemy: comment from a friend of ours
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HurtEnemyCm;
        /// <summary>
        /// hurt by an enemy with bullets
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HurtEnemyBullet;
        /// <summary>
        /// hurt by an enemy with needles
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HurtEnemyNeedler;
        /// <summary>
        /// hurt by an enemy with a plasma bolt
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HurtEnemyPlasma;
        /// <summary>
        /// hurt by an enemy with a sniper weapon
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HurtEnemySniper;
        /// <summary>
        /// a grenade is stuck to us
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HurtEnemyGrenade;
        /// <summary>
        /// hurt by an enemy with an explosive weapon
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HurtEnemyExplosion;
        /// <summary>
        /// hurt by an enemy with a melee weapon
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HurtEnemyMelee;
        /// <summary>
        /// hurt by an enemy with flamethrower
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HurtEnemyFlame;
        /// <summary>
        /// hurt by an enemy with a shotgun
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HurtEnemyShotgun;
        /// <summary>
        /// hurt by an enemy with a vehicle
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HurtEnemyVehicle;
        /// <summary>
        /// hurt by an enemy with a fixed weapon
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HurtEnemyMountedweapon;
        [TagField(Length = 0x10)]
        public byte[] Padding10;
        [TagField(Length = 0x10)]
        public byte[] Padding11;
        [TagField(Length = 0x10)]
        public byte[] Padding12;
        /// <summary>
        /// killed a friendly AI
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledFriend;
        /// <summary>
        /// killed a friendly AI: comment from a friend
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledFriendCm;
        /// <summary>
        /// killed a friendly player
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledFriendPlayer;
        /// <summary>
        /// killed a friendly player: comment from a friend
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledFriendPlayerCm;
        /// <summary>
        /// killed an enemy
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemy;
        /// <summary>
        /// killed an enemy: comment from a friend
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyCm;
        /// <summary>
        /// killed an enemy player
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyPlayer;
        /// <summary>
        /// killed an enemy player: comment from a friend
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyPlayerCm;
        /// <summary>
        /// killed an enemy covenant
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyCovenant;
        /// <summary>
        /// killed an enemy covenant: comment from a friend
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyCovenantCm;
        /// <summary>
        /// killed an enemy flood combat form
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyFloodcombat;
        /// <summary>
        /// killed an enemy flood combat form: comment from a friend
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyFloodcombatCm;
        /// <summary>
        /// killed an enemy flood carrier form
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyFloodcarrier;
        /// <summary>
        /// killed an enemy flood carrier form: comment from a friend
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyFloodcarrierCm;
        /// <summary>
        /// killed an enemy sentinel
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemySentinel;
        /// <summary>
        /// killed an enemy sentinel: comment from a friend
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemySentinelCm;
        /// <summary>
        /// killed an enemy with bullets
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyBullet;
        /// <summary>
        /// killed an enemy with needles
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyNeedler;
        /// <summary>
        /// killed an enemy with a plasma bolt
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyPlasma;
        /// <summary>
        /// killed an enemy with a sniper weapon
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemySniper;
        /// <summary>
        /// killed an enemy with a grenade
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyGrenade;
        /// <summary>
        /// killed an enemy with an explosive weapon
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyExplosion;
        /// <summary>
        /// killed an enemy with a melee weapon
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyMelee;
        /// <summary>
        /// killed an enemy with flamethrower
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyFlame;
        /// <summary>
        /// killed an enemy with a shotgun
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyShotgun;
        /// <summary>
        /// killed an enemy by hitting them with a vehicle
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyVehicle;
        /// <summary>
        /// killed an enemy with a fixed weapon
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KilledEnemyMountedweapon;
        /// <summary>
        /// we are on a killing spree
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag KillingSpree;
        [TagField(Length = 0x10)]
        public byte[] Padding13;
        [TagField(Length = 0x10)]
        public byte[] Padding14;
        [TagField(Length = 0x10)]
        public byte[] Padding15;
        /// <summary>
        /// responses to a friendly player killing an enemy nearby
        /// </summary>
        /// <summary>
        /// response to the player killing an enemy
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PlayerKillCm;
        /// <summary>
        /// response to the player killing an enemy with bullets
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PlayerKillBulletCm;
        /// <summary>
        /// response to the player killing an enemy with needles
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PlayerKillNeedlerCm;
        /// <summary>
        /// response to the player killing an enemy with a plasma bolt
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PlayerKillPlasmaCm;
        /// <summary>
        /// response to the player killing an enemy with a sniper weapon
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PlayerKillSniperCm;
        /// <summary>
        /// response to _anyone_ killing an enemy with a grenade
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag AnyoneKillGrenadeCm;
        /// <summary>
        /// response to the player killing an enemy with an explosive weapon
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PlayerKillExplosionCm;
        /// <summary>
        /// response to the player killing an enemy with a melee weapon
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PlayerKillMeleeCm;
        /// <summary>
        /// response to the player killing an enemy with flamethrower
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PlayerKillFlameCm;
        /// <summary>
        /// response to the player killing an enemy with a shotgun
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PlayerKillShotgunCm;
        /// <summary>
        /// response to the player killing an enemy by hitting them with a vehicle
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PlayerKillVehicleCm;
        /// <summary>
        /// response to the player killing an enemy with a fixed weapon
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PlayerKillMountedweaponCm;
        /// <summary>
        /// response to the player going on a killing spree
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PlayerKilllingSpreeCm;
        [TagField(Length = 0x10)]
        public byte[] Padding16;
        [TagField(Length = 0x10)]
        public byte[] Padding17;
        [TagField(Length = 0x10)]
        public byte[] Padding18;
        /// <summary>
        /// a friendly AI died
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag FriendDied;
        /// <summary>
        /// a friendly player died
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag FriendPlayerDied;
        /// <summary>
        /// a friend died from friendly fire
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag FriendKilledByFriend;
        /// <summary>
        /// friend died from player's friendly fire
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag FriendKilledByFriendlyPlayer;
        /// <summary>
        /// a friend died from enemy fire
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag FriendKilledByEnemy;
        /// <summary>
        /// friend died from an enemy player
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag FriendKilledByEnemyPlayer;
        /// <summary>
        /// a friend died from covenant fire
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag FriendKilledByCovenant;
        /// <summary>
        /// a friend died from the flood
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag FriendKilledByFlood;
        /// <summary>
        /// a friend died from sentinel fire
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag FriendKilledBySentinel;
        /// <summary>
        /// a friend was deliberately killed by an ally that we don't trust (e.g. player killed a marine)
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag FriendBetrayed;
        [TagField(Length = 0x10)]
        public byte[] Padding19;
        [TagField(Length = 0x10)]
        public byte[] Padding20;
        /// <summary>
        /// vocalizations that can be played even if a friend is talking
        /// </summary>
        /// <summary>
        /// see an enemy and we have not previously been in combat
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag NewCombatAlone;
        /// <summary>
        /// see a new enemy and we have recently been in combat
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag NewEnemyRecentCombat;
        /// <summary>
        /// see an enemy that we are currently looking for
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag OldEnemySighted;
        /// <summary>
        /// unexpectedly encounters enemy (behind or to the side)
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag UnexpectedEnemy;
        /// <summary>
        /// unexpectedly finds a dead body of a friend
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag DeadFriendFound;
        /// <summary>
        /// we decide that a former ally is now a traitor
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag AllianceBroken;
        /// <summary>
        /// we forgive a traitor and make him our friend again
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag AllianceReformed;
        /// <summary>
        /// throwing a grenade
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag GrenadeThrowing;
        /// <summary>
        /// see an enemy grenade
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag GrenadeSighted;
        /// <summary>
        /// alerted by a grenade bouncing near us
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag GrenadeStartle;
        /// <summary>
        /// in danger from an enemy grenade
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag GrenadeDangerEnemy;
        /// <summary>
        /// in danger from your own grenade
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag GrenadeDangerSelf;
        /// <summary>
        /// in danger from a friendly grenade (not our own)
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag GrenadeDangerFriend;
        [TagField(Length = 0x10)]
        public byte[] Padding21;
        [TagField(Length = 0x10)]
        public byte[] Padding22;
        /// <summary>
        /// vocalizations that require friends
        /// </summary>
        /// <summary>
        /// reply to a nearby friend who alerted us to an enemy
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag NewCombatGroupRe;
        /// <summary>
        /// reply to a distant friend who alerted us to an enemy
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag NewCombatNearbyRe;
        /// <summary>
        /// alert a friend who is in a noncombat state
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag AlertFriend;
        /// <summary>
        /// alerted by a friend when in a noncombat state
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag AlertFriendRe;
        /// <summary>
        /// alert friends that target was not at expected location
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag AlertLostContact;
        /// <summary>
        /// alert friends that target was not at expected location: response
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag AlertLostContactRe;
        /// <summary>
        /// friend is blocking us from moving or firing
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Blocked;
        /// <summary>
        /// friend is blocking us from moving or firing: response
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag BlockedRe;
        /// <summary>
        /// starting to search
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SearchStart;
        /// <summary>
        /// asking searchers whether they have found anything
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SearchQuery;
        /// <summary>
        /// asking searchers whether they have found anything: response
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SearchQueryRe;
        /// <summary>
        /// searcher reporting that an area is clear
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SearchReport;
        /// <summary>
        /// searcher giving up on search
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SearchAbandon;
        /// <summary>
        /// search coordinator giving up on search
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SearchGroupAbandon;
        /// <summary>
        /// starting to uncover target with friend
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag GroupUncover;
        /// <summary>
        /// starting to uncover target: response
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag GroupUncoverRe;
        /// <summary>
        /// our platoon starts to attack or advance
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Advance;
        /// <summary>
        /// our platoon starts to attack or advance: response
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag AdvanceRe;
        /// <summary>
        /// our platoon starts to defend or retreat
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Retreat;
        /// <summary>
        /// our platoon starts to defend or retreat: response
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag RetreatRe;
        /// <summary>
        /// telling friends to seek cover
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Cover;
        [TagField(Length = 0x10)]
        public byte[] Padding23;
        [TagField(Length = 0x10)]
        public byte[] Padding24;
        [TagField(Length = 0x10)]
        public byte[] Padding25;
        [TagField(Length = 0x10)]
        public byte[] Padding26;
        /// <summary>
        /// vocalizations that don't require friends
        /// </summary>
        /// <summary>
        /// sighted a new friendly player
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SightedFriendPlayer;
        /// <summary>
        /// shooting at an enemy
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Shooting;
        /// <summary>
        /// shooting from a vehicle at an enemy
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag ShootingVehicle;
        /// <summary>
        /// shooting at an enemy while berserk
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag ShootingBerserk;
        /// <summary>
        /// shooting at a group of enemies
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag ShootingGroup;
        /// <summary>
        /// shooting at a traitorous player
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag ShootingTraitor;
        /// <summary>
        /// taunting the enemy
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Taunt;
        /// <summary>
        /// taunted by an enemy: response
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag TauntRe;
        /// <summary>
        /// fleeing in panic
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Flee;
        /// <summary>
        /// fleeing in panic: response
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag FleeRe;
        /// <summary>
        /// fleeing in panic because our leaders are all dead
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag FleeLeaderDied;
        /// <summary>
        /// unable to flee because a leader is nearby
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag AttemptedFlee;
        /// <summary>
        /// unable to flee because a leader is nearby - response
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag AttemptedFleeRe;
        /// <summary>
        /// target was not at expected location
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag LostContact;
        /// <summary>
        /// stops hiding and pursues target
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag HidingFinished;
        /// <summary>
        /// enters vehicle
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag VehicleEntry;
        /// <summary>
        /// exits vehicle
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag VehicleExit;
        /// <summary>
        /// excited while in vehicle (big air, etc)
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag VehicleWoohoo;
        /// <summary>
        /// scared while in vehicle (imminent crash)
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag VehicleScared;
        /// <summary>
        /// riding in a vehicle and the driver collides with something
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag VehicleCollision;
        /// <summary>
        /// saw something suspicious but not sure it was an enemy
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PartiallySighted;
        /// <summary>
        /// decided that a suspicious sighting was nothing after all
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag NothingThere;
        /// <summary>
        /// pleading for the player to spare our pitiable lives
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Pleading;
        [TagField(Length = 0x10)]
        public byte[] Padding27;
        [TagField(Length = 0x10)]
        public byte[] Padding28;
        [TagField(Length = 0x10)]
        public byte[] Padding29;
        [TagField(Length = 0x10)]
        public byte[] Padding30;
        [TagField(Length = 0x10)]
        public byte[] Padding31;
        [TagField(Length = 0x10)]
        public byte[] Padding32;
        /// <summary>
        /// vocalizations that interrupt our talking
        /// </summary>
        /// <summary>
        /// surprised by an enemy, noise, body or weapon impact
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Surprise;
        /// <summary>
        /// went berserk
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Berserk;
        /// <summary>
        /// attacked an enemy in melee
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag MeleeAttack;
        /// <summary>
        /// dove away from danger or into cover
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Dive;
        /// <summary>
        /// leapt out of corner to uncover a suspected target
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag UncoverExclamation;
        /// <summary>
        /// begin a leap attack
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag LeapAttack;
        /// <summary>
        /// arise and return to life
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Resurrection;
        [TagField(Length = 0x10)]
        public byte[] Padding33;
        [TagField(Length = 0x10)]
        public byte[] Padding34;
        [TagField(Length = 0x10)]
        public byte[] Padding35;
        [TagField(Length = 0x10)]
        public byte[] Padding36;
        /// <summary>
        /// vocalizations that immediately follow combat
        /// </summary>
        /// <summary>
        /// all enemies defeated
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Celebration;
        /// <summary>
        /// post-combat checking an enemy's dead body
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag CheckBodyEnemy;
        /// <summary>
        /// post-combat checking a friend's dead body
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag CheckBodyFriend;
        /// <summary>
        /// post-combat shooting an enemy's dead body
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag ShootingDeadEnemy;
        /// <summary>
        /// post-combat shooting an enemy player's dead body
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag ShootingDeadEnemyPlayer;
        [TagField(Length = 0x10)]
        public byte[] Padding37;
        [TagField(Length = 0x10)]
        public byte[] Padding38;
        [TagField(Length = 0x10)]
        public byte[] Padding39;
        [TagField(Length = 0x10)]
        public byte[] Padding40;
        /// <summary>
        /// vocalizations that immediately follow combat
        /// </summary>
        /// <summary>
        /// post-combat all friends killed
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Alone;
        /// <summary>
        /// post-combat we were not hurt
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Unscathed;
        /// <summary>
        /// post-combat we were badly hurt
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SeriouslyWounded;
        /// <summary>
        /// post-combat replying to a friend who was badly hurt
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SeriouslyWoundedRe;
        /// <summary>
        /// post-combat our friends took heavy casualties
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Massacre;
        /// <summary>
        /// post-combat reply: our friends took heavy casualties
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag MassacreRe;
        /// <summary>
        /// post-combat our friends kicked alien ass
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Rout;
        /// <summary>
        /// post-combat reply: our friends kicked alien ass
        /// </summary>
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag RoutRe;
        [TagField(Length = 0x10)]
        public byte[] Padding41;
        [TagField(Length = 0x10)]
        public byte[] Padding42;
        [TagField(Length = 0x10)]
        public byte[] Padding43;
        [TagField(Length = 0x10)]
        public byte[] Padding44;
        [TagField(Length = 0x2F0)]
        public byte[] Padding45;
    }
}

