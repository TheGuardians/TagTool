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
        public HaloOnlineCacheContext CacheContext { get; }
        public CachedTagInstance TagInstance { get; }
        public Sound Sound { get; }
        public FileInfo SoundFile { get; }
        public WindowsMediaPlayer Player { get; private set; }

        public SoundControl()
        {
            InitializeComponent();
        }

        public SoundControl(HaloOnlineCacheContext cacheContext, CachedTagInstance tagInstance, Sound sound) :
            this()
        {
            CacheContext = cacheContext;
            TagInstance = tagInstance;
            Sound = sound;
            SoundFile = new FileInfo(Path.Combine(Application.StartupPath, "temp", $"{TagInstance.Index:X4}.mp3"));
        }

        protected override void OnLoad(EventArgs e)
        {
            stopButton_Click(this, e);

            if (!SoundFile.Directory.Exists)
                SoundFile.Directory.Create();

            if (SoundFile.Exists)
                SoundFile.Delete();

            using (var fileStream = SoundFile.Create())
                CacheContext.ExtractResource(Sound.Resource.HaloOnlinePageableResource, fileStream);

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
                using (var fileStream = SoundFile.Create())
                    CacheContext.ExtractResource(Sound.Resource.HaloOnlinePageableResource, fileStream);

            var destSoundFile = new FileInfo(sfd.FileName);

            if (!destSoundFile.Directory.Exists)
                destSoundFile.Directory.Create();

            SoundFile.CopyTo(destSoundFile.FullName);
        }
    }
}