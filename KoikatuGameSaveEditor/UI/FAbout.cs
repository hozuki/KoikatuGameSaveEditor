using System.Diagnostics;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace KGSE.UI {
    public partial class FAbout : Form {

        public FAbout() {
            InitializeComponent();
        }

        public static void ShowModal([CanBeNull] IWin32Window owner) {
            using (var f = new FAbout()) {
                f.ShowDialog(owner);
            }
        }

        private void FAbout_Load(object sender, System.EventArgs e) {
            label2.Text = $"version {Application.ProductVersion}";
            toolTip1.SetToolTip(linkLabel1, GitHubRepoAddress);
            toolTip1.SetToolTip(linkLabel2, BasedOnRepoAddress);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            OpenUrl(GitHubRepoAddress);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            OpenUrl(BasedOnRepoAddress);
        }

        private static void OpenUrl([NotNull] string url) {
            var startInfo = new ProcessStartInfo(url) {
                UseShellExecute = true,
                Verb = "Open"
            };

            Process.Start(startInfo);
        }

        private const string GitHubRepoAddress = "https://github.com/hozuki/KoikatuGameSaveEditor";
        private const string BasedOnRepoAddress = "https://github.com/kiletw/KoikatuSaveDataEdit";

    }
}
