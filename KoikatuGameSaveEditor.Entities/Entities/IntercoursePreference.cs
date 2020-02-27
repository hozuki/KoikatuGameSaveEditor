using System;
using System.Diagnostics;
using JetBrains.Annotations;
using KGSE.Entities.Internal;
using KGSE.Entities.Internal.Known;

namespace KGSE.Entities {
    // It represents accepting not denial...
    // Again it is not my naming. Go tell Illusion staff members.
    public sealed class IntercoursePreference : BooleanPropertyBag {

        internal IntercoursePreference([NotNull] KkCharacter character) {
            _character = character;
            Kissing = CreateProperty(KnownDenialKeys.Kiss);
            Caress = CreateProperty(KnownDenialKeys.Aibu);
            Anal = CreateProperty(KnownDenialKeys.Anal);
            Vibrator = CreateProperty(KnownDenialKeys.Massage);
            RawInsertion = CreateProperty(KnownDenialKeys.NotCondom);

            RegisterEventHandlers();
        }

        ~IntercoursePreference() {
            UnregisterEventHandlers();
        }

        [NotNull]
        public BooleanProperty Kissing { get; }

        [NotNull]
        public BooleanProperty Caress { get; }

        [NotNull]
        public BooleanProperty Anal { get; }

        [NotNull]
        public BooleanProperty Vibrator { get; }

        [NotNull]
        public BooleanProperty RawInsertion { get; }

        public override ValueProperty<bool>[] GetAllProperties() {
            return new ValueProperty<bool>[] {
                Kissing,
                Caress,
                Anal,
                Vibrator,
                RawInsertion,
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
            var s = (BooleanProperty)sender;
            _character.SetDenial(s.Key, s.Value);
        }

        [NotNull]
        private BooleanProperty CreateProperty([NotNull] string key) {
            var v = _character.GetDenial(key);

            Debug.Assert(v != null, nameof(v) + " != null");
            return new BooleanProperty(key, v.Value);
        }

        [NotNull]
        private readonly KkCharacter _character;

    }
}
