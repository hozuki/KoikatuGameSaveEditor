using System.Diagnostics;
using JetBrains.Annotations;

namespace KGSE.Entities {
    [DebuggerDisplay("Boolean Property: [\"{Key}\" = {Value}]")]
    public sealed class BooleanProperty : ValueProperty<bool> {

        internal BooleanProperty([NotNull] string key, bool value)
            : base(key, value) {
        }

    }
}
