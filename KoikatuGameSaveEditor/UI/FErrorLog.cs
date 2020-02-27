using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace KGSE.UI {
    public partial class FErrorLog : Form {

        public FErrorLog() {
            InitializeComponent();
        }

        public static void ShowModal([CanBeNull] IWin32Window owner, [NotNull] ErrorReport report) {
            using (var f = new FErrorLog()) {
                f.ErrorReport = report;
                f.ShowDialog(owner);
            }
        }

        private void FErrorLog_Load(object sender, EventArgs e) {
            var report = ErrorReport;

            Trace.Assert(report != null);

            lv.Columns.Add("Family Name");
            lv.Columns.Add("Given Name");
            lv.Columns.Add("Description");

            foreach (var entry in report.Entries) {
                var lvi = lv.Items.Add(entry.LastName);
                lvi.SubItems.Add(entry.FirstName);
                lvi.SubItems.Add(entry.Description);
            }

            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        [CanBeNull]
        private ErrorReport ErrorReport { get; set; }

    }
}
