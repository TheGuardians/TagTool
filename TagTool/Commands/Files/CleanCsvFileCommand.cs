using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands;

namespace TagTool.Commands.Files
{
    class CleanCsvFileCommand : Command
    {
        public HaloOnlineCacheContext CacheContext { get; }

        public CleanCsvFileCommand(HaloOnlineCacheContext cacheContext) :
            base(CommandFlags.Inherit,

                "CleanCsvFile",
                "Removes any unfound tag indices from a tag conversion .csv file.",

                "CleanCsvFile <csv file>",

                "Removes any unfound tag indices from a tag conversion .csv file.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var csvFile = new FileInfo(args[0]);

            if (!csvFile.Exists)
                throw new FileNotFoundException(csvFile.FullName);

            string versionStringLine = null;
            string versionTimeLine = null;
            string[] tagLines = null;

            using (var csvStream = csvFile.OpenRead())
            using (var csvReader = new StreamReader(csvStream))
            {
                versionStringLine = csvReader.ReadLine();
                versionTimeLine = csvReader.ReadLine();

                tagLines = csvReader.ReadToEnd().Replace("\r\n", "\n").Split('\n');
            }

            using (var csvStream = csvFile.Create())
            using (var csvWriter = new StreamWriter(csvStream))
            {
                csvWriter.WriteLine(versionStringLine);
                csvWriter.WriteLine(versionTimeLine);

                foreach (var tagLine in tagLines)
                {
                    var tagIndices = tagLine.Split(',').ToList();

                    if (!int.TryParse(tagIndices[0], NumberStyles.HexNumber, null, out int tagIndex))
                        continue;

                    if (tagIndex >= CacheContext.TagCache.Index.Count || CacheContext.TagCache.Index[tagIndex] == null)
                        continue;

                    var first = true;
                    foreach (var index in tagIndices)
                    {
                        csvWriter.Write(first ? "" : ",");

                        if (first)
                            first = false;

                        csvWriter.Write("0x" + index);
                    }
                    csvWriter.WriteLine();
                }
            }

            return true;
        }
    }
}
