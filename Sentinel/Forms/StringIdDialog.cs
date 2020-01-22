using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Sentinel.Forms
{
    public partial class StringIdDialog : Form
    {
        public GameCache Cache { get; }

        public StringId Value =>
            stringIdTreeView.SelectedNode?.Tag != null ?
                (StringId)stringIdTreeView.SelectedNode?.Tag :
                StringId.Invalid;

        private static string[] SetNames => new[]
        {
            "global",
            "gui",
            "gui_alert",
            "gui_dialog",
            "game_start",
            "game_engine",
            "achievement",
            "saved_game",
            "runtime"
        };

        public StringIdDialog()
        {
            InitializeComponent();
        }

        public StringIdDialog(GameCache cache) :
            this()
        {
            Cache = cache;
        }

        protected override void OnLoad(EventArgs e)
        {
            LoadStringIdTreeView();
            base.OnLoad(e);
        }

        private void LoadStringIdTreeView(string filter = null)
        {
            stringIdTreeView.Nodes.Clear();
            okButton.Enabled = false;

            stringIdTreeView.Nodes.Add(new TreeNode
            {
                Name = "<null>",
                Text = "<null>",
                Tag = StringId.Invalid
            });

            var stringIdSets = new Dictionary<string, List<StringId>>();

            for (var stringIndex = 0; stringIndex < Cache.StringTable.Count; stringIndex++)
            {
                var stringId = Cache.StringTable.GetStringId(stringIndex);

                if (stringId == StringId.Invalid)
                    continue;

                var stringIdResolver = Cache.StringTable.Resolver;
                var set = SetNames[stringIdResolver.GetSet(stringId)];

                if (!stringIdSets.ContainsKey(set))
                    stringIdSets[set] = new List<StringId>();

                stringIdSets[set].Add(stringId);
            }

            foreach (var entry in stringIdSets)
            {
                var set = entry.Key;

                var setNode = new TreeNode
                {
                    Name = set,
                    Text = set
                };

                foreach (var stringId in stringIdSets[set])
                {
                    var stringValue = Cache.StringTable.GetString(stringId);

                    if (stringValue == null || (filter != null && !stringValue.Contains(filter)))
                        continue;

                    setNode.Nodes.Add(new TreeNode
                    {
                        Name = stringValue,
                        Text = stringValue,
                        Tag = stringId
                    });
                }

                stringIdTreeView.Nodes.Add(setNode);
            }
        }

        private void stringIdTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            okButton.Enabled = stringIdTreeView.SelectedNode?.Tag is StringId;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            stringIdTreeView.SelectedNode = null;
            okButton.Enabled = false;
            LoadStringIdTreeView(searchTextBox.Text);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}