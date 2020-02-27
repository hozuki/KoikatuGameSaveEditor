using System.Collections.Generic;
using JetBrains.Annotations;

namespace KGSE.Utilities {
    internal static class EnumerableHelper {

        [NotNull]
        public static IEnumerable<(int Index, T Value)> Enumerate<T>([NotNull] this IEnumerable<T> e) {
            var counter = 0;

            foreach (var v in e) {
                yield return (counter, v);
                counter += 1;
            }
        }

    }
}
