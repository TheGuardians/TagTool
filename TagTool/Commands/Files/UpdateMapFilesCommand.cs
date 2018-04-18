using TagTool.Cache;
using TagTool.Commands;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TagTool.Commands.Files
{
	class UpdateMapFilesCommand : Command
	{
		public GameCacheContext CacheContext { get; }

		public UpdateMapFilesCommand(GameCacheContext cacheContext)
			: base(CommandFlags.Inherit,

				  "UpdateMapFiles",
				  "Updates the game's .map files to contain valid scenario indices.",

				  "UpdateMapFiles [gen]",

				  "Updates the game's .map files to contain valid scenario indices." +
				  "If 'gen' option is set, generates map files for scenario's that were unable to update a map file.")
		{
			CacheContext = cacheContext;
		}

		int FILE_START_OFFSET = 0x0000;
		int MAP_NAME_OFFSET = 0x01A4;
		int MAP_PATH_OFFSET = 0X01C8;
		int SCNR_INDEX_OFFSET = 0x2DEC;
		int MAP_ID_OFFSET = 0x2DF0;

		public override object Execute(List<string> args)
		{
			if (args.Count > 1)
				return false;

			// build a dictionary of (mapID, scnrIndex)
			var scnrIndices = new Dictionary<int, int>();
			using (var stream = CacheContext.OpenTagCacheRead())
			using (var reader = new BinaryReader(stream))
			{
				foreach (var tag in CacheContext.TagCache.Index.FindAllInGroup("scnr"))
				{
					if (tag.HeaderOffset == -1)
						continue;

					reader.BaseStream.Position = tag.HeaderOffset + tag.DefinitionOffset + 0x8;
					scnrIndices[reader.ReadInt32()] = tag.Index;
				}
			}

			// update the .map files
			foreach (var mapFile in CacheContext.Directory.GetFiles("*.map"))
			{
				using (var stream = mapFile.Open(FileMode.Open, FileAccess.ReadWrite))
				using (var reader = new BinaryReader(stream))
				using (var writer = new BinaryWriter(stream))
				{
					if (reader.ReadInt32() != new Tag("head").Value) // verify the map file is a HO .map
						continue;

					// scenario index
					reader.BaseStream.Position = SCNR_INDEX_OFFSET;
					var mapID = reader.ReadInt32();

					if (!scnrIndices.ContainsKey(mapID)) // verify that the map contains a valid scenario index
						continue;

					// write our mapID
					var scnrIndex = scnrIndices[mapID];
					writer.BaseStream.Position = MAP_ID_OFFSET;
					writer.Write(scnrIndex);

					scnrIndices.Remove(mapID); // remove scenario entries once the map file has been updated...

					Console.WriteLine($"Scenario tag index for {mapFile.Name}: {scnrIndex:X8}");
				}
			}

			// generate map files for scenarios that were unable to have a map file updated
			// *WARNING: requires at least one existing HO .map be in the CacheContext.Directory
			// *WARNING: map file name, and name(s) within the file come from the scenario tagname
			// *TODO: Handle campaign & menu .maps; this is only currently setup for multiplayer maps. 
			if (args[0].ToLower() == "gen")
				foreach (var mapFile in CacheContext.Directory.GetFiles("*.map"))
				{
					// don't use mainmenu.map for this because it is pretty different.
					if (mapFile.Name.ToLower().Contains("mainmenu"))
						continue;

					using (var stream = mapFile.Open(FileMode.Open, FileAccess.Read))
					using (var reader = new BinaryReader(stream))
					{
						if (reader.ReadInt32() != new Tag("head").Value) // verify the map file is a HO .map
							continue;

						// read the entire .map to use as a base
						reader.BaseStream.Position = FILE_START_OFFSET;
						var mapFileData = reader.ReadBytes((int)reader.BaseStream.Length);

						foreach (var scnr in scnrIndices)
						{
							// get byte[]'s for our map-name and map-path
							var mapID = scnr.Key;
							var scnrIndex = scnr.Value;
							var sMapPath = CacheContext.TagNames.ContainsKey(scnrIndex) ?
											CacheContext.TagNames[scnrIndex] :
											"0x" + scnrIndex.ToString("X4");    // levels\atlas\sc100\sc100
							var sMapName = sMapPath.Split('\\').Last();         // sc100
							var bMapName = new byte[0x0024];
							Encoding.ASCII.GetBytes(sMapName).CopyTo(bMapName, 0);
							var bMapPath = new byte[0x0100];
							Encoding.ASCII.GetBytes(sMapPath).CopyTo(bMapPath, 0);

							// replace the map name and path in our map data
							bMapName.CopyTo(mapFileData, MAP_NAME_OFFSET);
							bMapPath.CopyTo(mapFileData, MAP_PATH_OFFSET);

							// replace the scenario index and map ID in our map data
							BitConverter.GetBytes(mapID).CopyTo(mapFileData, SCNR_INDEX_OFFSET);
							BitConverter.GetBytes(scnrIndex).CopyTo(mapFileData, MAP_ID_OFFSET);

							// save our new map file
							using (var fs = new FileStream(Path.Combine(CacheContext.Directory.FullName, sMapName + ".map"),
															FileMode.Create, FileAccess.Write))
								fs.Write(mapFileData, 0, mapFileData.Length);

							Console.WriteLine($"Created map file: {sMapName}.map, with scenario: 0x{scnrIndex.ToString("X4")}, and map ID: {mapID}");
						}
					}
				}

			Console.WriteLine("Done!");

			return true;
		}
	}
}