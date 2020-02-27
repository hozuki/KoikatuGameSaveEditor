using System.Diagnostics;
using JetBrains.Annotations;

namespace KGSE.Entities {
    [DebuggerDisplay("Percentage Property: [\"{Key}\" = {Value}]")]
    public sealed class PercentageProperty : ValueProperty<Percentage> {

        internal PercentageProperty([NotNull] string key, Percentage value)
            : base(key, value) {
        }

    }
}
