using System;
using System.Diagnostics;
using JetBrains.Annotations;
using KGSE.Entities.Internal;
using KGSE.Entities.Internal.Known;

namespace KGSE.Entities {
    // The translated names are chosen according to the wiki:
    // https://wiki.anime-sharing.com/hgames/index.php?title=Koikatu/Gameplay/Traits
    public sealed class Traits : BooleanPropertyBag {

        internal Traits([NotNull] KkCharacter character) {
            _character = character;
            SmallBladder = CreateProperty(KnownAttributeKeys.Hinnyo);
            Starved = CreateProperty(KnownAttributeKeys.Harapeko);
            Insensitive = CreateProperty(KnownAttributeKeys.Donkan);
            Simple = CreateProperty(KnownAttributeKeys.Choroi);
            Slutty = CreateProperty(KnownAttributeKeys.Bitch);
            Promiscuous = CreateProperty(KnownAttributeKeys.Mutturi);
            Bookworm = CreateProperty(KnownAttributeKeys.Dokusyo);
            LikesMusic = CreateProperty(KnownAttributeKeys.Ongaku);
            Lively = CreateProperty(KnownAttributeKeys.Kappatu);
            Submissive = CreateProperty(KnownAttributeKeys.Ukemi);
            Friendly = CreateProperty(KnownAttributeKeys.Friendly);
            Neat = CreateProperty(KnownAttributeKeys.Kireizuki);
            Lazy = CreateProperty(KnownAttributeKeys.Taida);
            Elusive = CreateProperty(KnownAttributeKeys.Sinsyutu);
            Loner = CreateProperty(KnownAttributeKeys.Hitori);
            Sporty = CreateProperty(KnownAttributeKeys.Undo);
            Diligent = CreateProperty(KnownAttributeKeys.Majime);
            LikesGirls = CreateProperty(KnownAttributeKeys.LikeGirls);

            RegisterEventHandlers();
        }

        ~Traits() {
            UnregisterEventHandlers();
        }

        [NotNull]
        public BooleanProperty SmallBladder { get; }

        [NotNull]
        public BooleanProperty Starved { get; }

        [NotNull]
        public BooleanProperty Insensitive { get; }

        [NotNull]
        public BooleanProperty Simple { get; }

        [NotNull]
        public BooleanProperty Slutty { get; }

        [NotNull]
        public BooleanProperty Promiscuous { get; }

        [NotNull]
        public BooleanProperty Bookworm { get; }

        [NotNull]
        public BooleanProperty LikesMusic { get; }

        [NotNull]
        public BooleanProperty Lively { get; }

        [NotNull]
        public BooleanProperty Submissive { get; }

        [NotNull]
        public BooleanProperty Friendly { get; }

        [NotNull]
        public BooleanProperty Neat { get; }

        [NotNull]
        public BooleanProperty Lazy { get; }

        [NotNull]
        public BooleanProperty Elusive { get; }

        [NotNull]
        public BooleanProperty Loner { get; }

        [NotNull]
        public BooleanProperty Sporty { get; }

        [NotNull]
        public BooleanProperty Diligent { get; }

        [NotNull]
        public BooleanProperty LikesGirls { get; }

        public override ValueProperty<bool>[] GetAllProperties() {
            return new ValueProperty<bool>[] {
                SmallBladder,
                Starved,
                Insensitive,
                Simple,
                Slutty,
                Promiscuous,
                Bookworm,
                LikesMusic,
                Lively,
                Submissive,
                Friendly,
                Neat,
                Lazy,
                Elusive,
                Loner,
                Sporty,
                Diligent,
                LikesGirls,
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
            _character.SetAttribute(s.Key, s.Value);
        }

        [NotNull]
        private BooleanProperty CreateProperty([NotNull] string key) {
            var v = _character.GetAttribute(key);

            Debug.Assert(v != null, nameof(v) + " != null");
            return new BooleanProperty(key, v.Value);
        }

        [NotNull]
        private readonly KkCharacter _character;

    }
}
