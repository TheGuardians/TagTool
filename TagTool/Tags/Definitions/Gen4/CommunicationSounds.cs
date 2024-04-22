using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "communication_sounds", Tag = "coms", Size = 0xCF0)]
    public class CommunicationSounds : TagStructure
    {
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Silence;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag AlertHolding;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Enemy1Enemy;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Enemy2Enemy;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Enemy3Enemy;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EnemyManyEnemies;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EnemyVehicle;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EnemyManyVehicles;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EnemyTank;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EnemyManyTanks;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EnemyBanshee;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EnemyFalcon;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EnemyGhost;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EnemyWraith;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EnemyScorpion;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EnemyMongoose;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag AlertBackup;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocDummy;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhBfghill;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhBfgramp;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhBfgwater;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhCliffarch;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhCovybridge;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhCovycore;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhCovytower;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhHillside;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhPowerarch;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhPowerhouse;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhPowerinside;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhPowerpipe;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhRock;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhSpartanbarn;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhSpartanbarnroof;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhSpartancore;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhSpartancoreroof;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhSpartandam;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhSpartanhigh;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhSpartantower;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhStationbridge;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhUpperdeck;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhWaterbridge;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocPhWaterside;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSlBeachside;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSlBfg;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSlCave;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSlCliffside;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSlCovbase;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSlCovgen;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSlCovside;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSlCrack;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSlMiddle;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSlSpartanbase;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSlSpartancore;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSlSpartangen;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSlSpartanside;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfAftConnector;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfAftVault;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfCentralCatwalk;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfCentralThing;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfCoilAccess;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfForwardCatwalk;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfForwardCompartment;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfInSpace;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfNinjaRail;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfPortCatwalk;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfPortFloor;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfPortMancannon;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfPortObservation;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfPortSide;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfSpine;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfStarboardCatwalk;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfStarboardFloor;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfStarboardMancannon;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfStarboardObservation;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocAfStarboardSide;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbAtrium;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbAtriumFloor;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbAtriumRamp;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbBackHallway;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbBackPocket;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbBackStairs;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbBlueLift;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbBreakRoom;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbCicEntrance;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbCicHall;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbCicRamp;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbCommandInformationCenter;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbElbow;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbGreenLift;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbGreenLiftEntrance;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbHangar;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbHangarBridge;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbHighBalcony;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbHighBridge;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbHighPerch;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbHighStairs;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbLockerRoom;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbLounge;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbLoungeBalcony;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbLowerElevatorHall;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbLowBridge;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbMidlevel;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbMidBridge;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbMountainviewRoom;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbSmallRamp;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbSwordRoom;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbUpperElevatorHall;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbWaitingRoom;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeAir;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeBackDoor;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeBackPath;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeBelowDeck;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeBighouseHigh;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeBighouseMid;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeBighouseObservation;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeBlockhouse;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeBlockhouseBridge;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeBlockhouseRoof;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeBridge;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeCenter;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeCommsRoof;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeCommsShack;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeCommsStairs;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeDeck;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeDestroyedHouse;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeEastCliff;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeHighBridge;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeLockerRoom;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeLockerStairs;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeMainRing;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeMud;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeOffTheEdge;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSePatio;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSePipesHigh;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSePipesLow;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSePipeStairs;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeRidge;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeRoad;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeRockGarden;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeShowers;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeSpillway;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeSpillwayArch;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeSpillwayDebris;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeStaffroom;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeStaffroomRoof;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeStoneArch;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeStoneBridge;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeStorage;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeStorageRamp;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeWater;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeWaterfall;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeWestCliff;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeYard;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbCic;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbVestibule;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbProcessing;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbWindowbridge;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbGreathall;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSbVent;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsCatwalk;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsLeftstairs;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsRightstairs;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsLeftvent;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsRightvent;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsTunnel;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsMainlift;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsLiftaccess;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsLiftroom;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsLeftplatform;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsRightplatform;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsHighbalconyright;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsHighbalconyleft;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsObservation;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsRightroom;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsLeftroom;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsBigplatform;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsLeftelevator;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsRightelevator;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsTopwalkway;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsMidwalkway;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsLowwalkway;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsLowbalconyleft;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsLowbalconyright;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsLowmiddle;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsHighmiddle;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsHighright;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsHighleft;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsFloor;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsOutside;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsLeftside;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsRightside;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsGantry;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsSabre;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsCenter;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocLsOutofbounds;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeSouthbridge;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeSoutharch;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeNorthbridge;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeNortharch;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeBighouselow;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeBighousesteps;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeBunkhouseroof;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeBunkhouse;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSePipes;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeTransformers;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeEastpath;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LocSeNorthside;
    }
}
