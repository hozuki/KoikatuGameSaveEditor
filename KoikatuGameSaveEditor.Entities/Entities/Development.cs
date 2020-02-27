using System;
using System.Diagnostics;
using JetBrains.Annotations;
using KGSE.Entities.Internal;
using KGSE.Entities.Internal.Known;

namespace KGSE.Entities {
    public sealed class Development : PercentagePropertyBag {

        internal Development([NotNull] KkCharacter character) {
            _character = character;
            Breasts = CreateProperty(KnownPercentageKeys.Mune);
            Crotch = CreateProperty(KnownPercentageKeys.Kokan);
            Anal = CreateProperty(KnownPercentageKeys.Anal);
            Butts = CreateProperty(KnownPercentageKeys.Siri);
            Nipples = CreateProperty(KnownPercentageKeys.Tikubi);
            VaginaInsertion = CreateProperty(KnownPercentageKeys.KokanPiston);
            AnalInsertion = CreateProperty(KnownPercentageKeys.AnalPiston);
            Serving = CreateProperty(KnownPercentageKeys.Houshi);

            RegisterEventHandlers();
        }

        ~Development() {
            UnregisterEventHandlers();
        }

        [NotNull]
        public PercentageProperty Breasts { get; }

        [NotNull]
        public PercentageProperty Crotch { get; }

        [NotNull]
        public PercentageProperty Anal { get; }

        [NotNull]
        public PercentageProperty Butts { get; }

        [NotNull]
        public PercentageProperty Nipples { get; }

        [NotNull]
        public PercentageProperty VaginaInsertion { get; }

        [NotNull]
        public PercentageProperty AnalInsertion { get; }

        [NotNull]
        public PercentageProperty Serving { get; }

        public override ValueProperty<Percentage>[] GetAllProperties() {
            return new ValueProperty<Percentage>[] {
                Breasts,
                Crotch,
                Anal,
                Butts,
                Nipples,
                VaginaInsertion,
                AnalInsertion,
                Serving,
            };
        }

        private void RegisterEventHandlers() {
            var properties = GetAllProperties();

            foreach (var property in properties) {
                property.ValueChanged += OnValueChanged;
            }
        }

        private void UnregisterEventHandlers() {
            var properties = GetAllProperties();

            foreach (var property in properties) {
                property.ValueChanged -= OnValueChanged;
            }
        }

        private void OnValueChanged(object sender, EventArgs e) {
            var s = (PercentageProperty)sender;
            Trace.Assert(Percentage.P0 <= s.Value && s.Value <= Percentage.P100);
            _character.SetPercentage(s.Key, (int)s.Value);
        }

        [NotNull]
        private PercentageProperty CreateProperty([NotNull] string key) {
            var v = _character.GetPercentage(key);

            Debug.Assert(v != null, nameof(v) + " != null");
            return new PercentageProperty(key, (Percentage)v.Value);
        }

        [NotNull]
        private readonly KkCharacter _character;

    }
}
