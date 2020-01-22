using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagTool.Audio;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Tags.Definitions;
using System.IO;
using WMPLib;
using System.Runtime.InteropServices;

namespace Sentinel.Controls
{
    public partial class SoundControl : UserControl
    {
        public GameCache Cache { get; }
        public CachedTag Tag { get; }
        public Sound Sound { get; }
        public FileInfo SoundFile { get; }
        public WindowsMediaPlayer Player { get; private set; }

        public SoundControl()
        {
            InitializeComponent();
        }

        public SoundControl(GameCache cache, CachedTag tag, Sound sound) :
            this()
        {
            Cache = cache;
            Tag = tag;
            Sound = sound;
            SoundFile = new FileInfo(Path.Combine(Application.StartupPath, "temp", $"{Tag.Index:X4}.mp3"));
        }

        protected override void OnLoad(EventArgs e)
        {
            stopButton_Click(this, e); //todo: fix

            if (!SoundFile.Directory.Exists)
                SoundFile.Directory.Create();

            if (SoundFile.Exists)
                SoundFile.Delete();

            var resourceDefinition = Cache.ResourceCache.GetSoundResourceDefinition(Sound.Resource);
            if (resourceDefinition != null)
            {
                var dataReference = resourceDefinition.Data;
                byte[] soundData = dataReference.Data;

                if (Cache is GameCacheGen3)
                {
                    soundData = ConvertGen3SoundData(Cache, Sound, dataReference.Data);
                }

                using (var fileStream = SoundFile.Create())
                    fileStream.Write(soundData, 0, soundData.Length);
            }

            base.OnLoad(e);
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (Player != null)
                stopButton_Click(sender, e);

            Player = new WindowsMediaPlayer();
            Player.URL = SoundFile.FullName;
            Player.controls.play();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (Player == null)
                return;

            Player.controls.stop();
            Player.currentPlaylist.clear();
            Marshal.FinalReleaseComObject(Player);
            Player = null;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "MP3 Files (*.mp3)|*.mp3";

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            if (!SoundFile.Directory.Exists)
                SoundFile.Directory.Create();

            if (!SoundFile.Exists)
            {
                var resourceDefinition = Cache.ResourceCache.GetSoundResourceDefinition(Sound.Resource);
                if (resourceDefinition != null)
                {
                    var dataReference = resourceDefinition.Data;
                    byte[] soundData = dataReference.Data;

                    if (Cache is GameCacheGen3)
                    {
                        soundData = ConvertGen3SoundData(Cache, Sound, dataReference.Data);
                    }

                    using (var fileStream = SoundFile.Create())
                        fileStream.Write(soundData, 0, soundData.Length);
                }
            }

            var destSoundFile = new FileInfo(sfd.FileName);

            if (!destSoundFile.Directory.Exists)
                destSoundFile.Directory.Create();

            if (destSoundFile.Exists)
                destSoundFile.Delete();

            if (SoundFile.Exists)
                SoundFile.CopyTo(destSoundFile.FullName);
        }

        private byte[] ConvertGen3SoundData(GameCache cache, Sound definition, byte[] soundData)
        {
            // todo: use PortingContextFactory here (currently inaccessible)
            SoundCacheFileGestalt blamSoundGestalt = null;
            foreach (var tag in cache.TagCache.TagTable)
            {
                if (tag.Group.Tag == "ugh!")
                {
                    using (var stream = cache.OpenCacheRead())
                        blamSoundGestalt = cache.Deserialize<SoundCacheFileGestalt>(stream, tag);
                    break;
                }
            }

            // ExportSoundCommand code
            using (MemoryStream soundDataStream = new MemoryStream())
            {
                using (EndianWriter output = new EndianWriter(soundDataStream, EndianFormat.BigEndian))
                {
                    if (blamSoundGestalt != null)
                    {
                        for (int pitchRangeIndex = definition.SoundReference.PitchRangeIndex; pitchRangeIndex < definition.SoundReference.PitchRangeIndex + definition.SoundReference.PitchRangeCount; pitchRangeIndex++)
                        {
                            var relativePitchRangeIndex = pitchRangeIndex - definition.SoundReference.PitchRangeIndex;
                            var permutationCount = blamSoundGestalt.GetPermutationCount(pitchRangeIndex);

                            for (int i = 0; i < permutationCount; i++)
                            {
                                BlamSound blamSound = SoundConverter.ConvertGen3Sound(cache, blamSoundGestalt, definition, relativePitchRangeIndex, i, soundData);

                                output.WriteBlock(blamSound.Data);
                            }
                        }
                    }
                }

                return soundDataStream.ToArray();
            }
        }
    }
}