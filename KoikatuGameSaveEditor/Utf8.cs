using System.Text;
using JetBrains.Annotations;

namespace KGSE {
    public static class Utf8 {

        static Utf8() {
            Encoding = new UTF8Encoding(false);
        }

        [NotNull]
        public static readonly Encoding Encoding;

    }
}
