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

				  "UpdateMapFiles",

				  "Updates the game's .map files to contain valid scenario indices.")
		{
			CacheContext = cacheContext;
		}

		public override object Execute(List<string> args)
		{
			if (args.Count != 0)
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
					reader.BaseStream.Position = 0x2DEC;
					var mapID = reader.ReadInt32();

					if (!scnrIndices.ContainsKey(mapID)) // verify that the map contains a valid scenario index
						continue;

					// write our mapID
					var scnrIndex = scnrIndices[mapID];
					writer.BaseStream.Position = 0x2DF0;
					writer.Write(scnrIndex);

					scnrIndices.Remove(mapID); // remove scenario entries once the map file has been updated...

					Console.WriteLine($"Scenario tag index for {mapFile.Name}: {scnrIndex:X8}");
				}
			}

			// generate map files for scenarios that were unable to have a map file updated
			// *WARNING: requires at least one existing HO .map be in the CacheContext.Directory
			// *WARNING: map file name, and name(s) within the file come from the scenario tagname
			// *TODO: Handle campaign & menu .maps; this is only currently setup for multiplayer maps. 
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
					reader.BaseStream.Position = 0x0000;
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
						bMapName.CopyTo(mapFileData, 0x01A4);
						bMapPath.CopyTo(mapFileData, 0x02C8);

						// replace the scenario index and map ID in our map data
						BitConverter.GetBytes(mapID).CopyTo(mapFileData, 0x2DEC);
						BitConverter.GetBytes(scnrIndex).CopyTo(mapFileData, 0x2DF0);

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