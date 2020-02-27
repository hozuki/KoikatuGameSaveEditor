using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KGSE.Localization {
    public sealed class TranslationManager {

        private TranslationManager() {
            _translations = new Dictionary<string, Translation> {
                [DefaultTranslation.Value.Code] = DefaultTranslation.Value
            };
            _current = DefaultTranslation.Value;
        }

        [NotNull]
        public static TranslationManager Instance {
            get {
                if (_instance == null) {
                    _instance = new TranslationManager();
                }

                return _instance;
            }
        }

        [NotNull]
        public IReadOnlyDictionary<string, Translation> Translations {
            [DebuggerStepThrough]
            get => _translations;
        }

        [NotNull]
        public Translation Current {
            get => _current;
            set => _current = value ?? throw new ArgumentNullException(nameof(value));
        }

        internal void AddTranslation([NotNull] Translation translation) {
            if (_translations.ContainsKey(translation.Code)) {
                throw new InvalidOperationException($"Translation '{translation.Code}' already exists.");
            }

            _translations.Add(translation.Code, translation);
        }

        [CanBeNull]
        private static TranslationManager _instance;

        [NotNull]
        private readonly Dictionary<string, Translation> _translations;

        [NotNull]
        private Translation _current;

    }
}
