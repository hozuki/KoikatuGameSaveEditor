using System.Windows.Forms;
using JetBrains.Annotations;

namespace KGSE.UI {
    internal static class AppCommon {

        static AppCommon() {
            Title = Application.ProductName;
        }

        [NotNull]
        public static readonly string Title;

    }
}
