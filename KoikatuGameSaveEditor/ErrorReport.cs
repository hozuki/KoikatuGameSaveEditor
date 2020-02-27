using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace KGSE {
    public class ErrorReport : IErrorReport {

        public ErrorReport() {
            _entries = new List<ErrorEntry>();
        }

        public void Add(ErrorEntry entry) {
            _entries.Add(entry);
        }

        [NotNull, ItemNotNull]
        public IReadOnlyList<ErrorEntry> Entries {
            [DebuggerStepThrough]
            get => _entries;
        }

        [NotNull, ItemNotNull]
        private readonly List<ErrorEntry> _entries;

    }
}
