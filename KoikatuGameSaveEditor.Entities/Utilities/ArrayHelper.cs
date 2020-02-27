using System.Collections.Generic;
using JetBrains.Annotations;

namespace KGSE.Utilities {
    internal static class ArrayHelper {

        public static bool ElementsEqual<T>([NotNull, ItemCanBeNull] this T[] array1, [NotNull, ItemCanBeNull] T[] array2) {
            return ElementsEqual(array1, array2, EqualityComparer<T>.Default);
        }

        public static bool ElementsEqual<T>([NotNull, ItemCanBeNull] this T[] array1, [NotNull, ItemCanBeNull] T[] array2, [NotNull] IEqualityComparer<T> comparer) {
            if (array1.Length != array2.Length) {
                return false;
            }

            var length = array1.Length;

            for (var i = 0; i < length; i += 1) {
                var v1 = array1[i];
                var v2 = array2[i];

                if (!comparer.Equals(v1, v2)) {
                    return false;
                }
            }

            return true;
        }

    }
}
