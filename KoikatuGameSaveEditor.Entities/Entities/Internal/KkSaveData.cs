using System.Collections.Generic;
using JetBrains.Annotations;

namespace KGSE.Entities.Internal {
    internal sealed class KkSaveData {

        [NotNull]
        public string Version { get; internal set; }

        [NotNull]
        public string SchoolName { get; internal set; }

        [NotNull]
        public byte[] Unknown1 { get; internal set; }

        [NotNull, ItemNotNull]
        public IReadOnlyList<KkCharacter> Characters { get; internal set; }

    }
}
