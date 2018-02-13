using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagTool.Tags.Definitions
{
    /// <summary>
    /// Contains a list of vfiles.
    /// </summary>
    [TagStructure(Name = "vfiles_list", Tag = "vfsl", Size = 0x20, MinVersion = CacheVersion.HaloOnline106708)]
    public class VFilesList
    {
        /// <summary>
        /// The files in the list.
        /// </summary>
        public List<VFileInfo> Files;

        /// <summary>
        /// The data block containing the data for every file.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Attempts to find a file by its path.
        /// </summary>
        /// <param name="path">The path of the file to find.</param>
        /// <returns>The file if found, or <c>null</c> otherwise.</returns>
        public VFileInfo Find(string path)
        {
            return Files.FirstOrDefault(f => path == Path.Combine(f.Folder, f.Name));
        }

        /// <summary>
        /// Extracts the specified file.
        /// </summary>
        /// <param name="file">The file to extract.</param>
        /// <returns>The file data.</returns>
        public byte[] Extract(VFileInfo file)
        {
            if (file.Offset < 0 || file.Offset >= Data.Length)
                throw new ArgumentException("Invalid file");
            var result = new byte[file.Size];
            Buffer.BlockCopy(Data, file.Offset, result, 0, file.Size);
            return result;
        }

        /// <summary>
        /// Replaces the specified file.
        /// </summary>
        /// <param name="file">The file to replace.</param>
        /// <param name="newData">The data to replace it with.</param>
        public void Replace(VFileInfo file, byte[] newData)
        {
            // Replace the file's data in the data array
            var sizeDelta = newData.Length - file.Size;
            Data = ArrayUtil.Replace(Data, file.Offset, file.Size, newData);

            // Adjust file offsets
            foreach (var adjustFile in Files)
            {
                if (adjustFile.Offset > file.Offset)
                    adjustFile.Offset += sizeDelta;
            }
            file.Size = newData.Length;
        }

        /// <summary>
        /// Adds a new file to the tag.
        /// </summary>
        /// <param name="name">The name of the file to add.</param>
        /// <param name="folder">The folder the file is located in.</param>
        /// <param name="fileData">The file data.</param>
        public void Insert(string name, string folder, byte[] fileData)
        {
            // Insert a file of size 0 and then replace it
            var fileInfo = new VFileInfo
            {
                Name = name,
                Folder = folder,
                Offset = Data.Length,
                Size = 0
            };
            Files.Add(fileInfo);
            Replace(fileInfo, fileData);
        }

        /// <summary>
        /// Removes a file from the tag.
        /// </summary>
        /// <param name="file">The file to remove.</param>
        public void Remove(VFileInfo file)
        {
            // Replace the file with an empty byte array and then remove its entry
            Replace(file, new byte[0]);
            Files.Remove(file);
        }
    }

    /// <summary>
    /// Contains information about a vfile.
    /// </summary>
    [TagStructure(Size = 0x208)]
    public class VFileInfo
    {
        /// <summary>
        /// The name of the file (e.g. "hf2p_weapons_categories.ps").
        /// </summary>
        [TagField(Length = 0x100)] public string Name;

        /// <summary>
        /// The folder the file is located in (e.g. "ps\autogen\").
        /// </summary>
        [TagField(Length = 0x100)] public string Folder;

        /// <summary>
        /// The starting offset of the file from the file data block.
        /// </summary>
        public int Offset;

        /// <summary>
        /// The size of the file in bytes.
        /// </summary>
        public int Size;
    }
}