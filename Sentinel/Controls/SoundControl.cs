using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagTool.Cache;
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

        protected override void OnLoad(EventArgs e) // endianness for Gen3 probably needs updating
        {
            stopButton_Click(this, e);

            if (!SoundFile.Directory.Exists)
                SoundFile.Directory.Create();

            if (SoundFile.Exists)
                SoundFile.Delete();

            var resourceDefinition = Cache.ResourceCache.GetSoundResourceDefinition(Sound.Resource);
            if (resourceDefinition != null)
            { 
                var dataReference = resourceDefinition.Data;

                using (var fileStream = SoundFile.Create())
                    fileStream.Write(dataReference.Data, 0, dataReference.Data.Length);
            }

            base.OnLoad(e);
        }

        private void playButton_Click(object sender, EventArgs e)
        {
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

                    using (var fileStream = SoundFile.Create())
                        fileStream.Write(dataReference.Data, 0, dataReference.Data.Length);
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
    }
}