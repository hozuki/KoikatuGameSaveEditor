using System;
using System.Diagnostics;
using JetBrains.Annotations;
using KGSE.Entities.Internal;
using KGSE.Entities.Internal.Known;

namespace KGSE.Entities {
    public sealed class Answers : BooleanPropertyBag {

        internal Answers([NotNull] KkCharacter character) {
            _character = character;
            LikesAnimals = CreateProperty(KnownAnswerKeys.Animal);
            LikesEating = CreateProperty(KnownAnswerKeys.Eat);
            CooksByHerself = CreateProperty(KnownAnswerKeys.Cook);
            LikesExercising = CreateProperty(KnownAnswerKeys.Exercise);
            StudiesHard = CreateProperty(KnownAnswerKeys.Study);
            Chic = CreateProperty(KnownAnswerKeys.Fashionable);
            AcceptsBlackCoffee = CreateProperty(KnownAnswerKeys.BlackCoffee);
            LikesSpicyFood = CreateProperty(KnownAnswerKeys.Spicy);
            LikesSweetFood = CreateProperty(KnownAnswerKeys.Sweet);

            RegisterEventHandlers();
        }

        ~Answers() {
            UnregisterEventHandlers();
        }

        [NotNull]
        public BooleanProperty LikesAnimals { get; }

        [NotNull]
        public BooleanProperty LikesEating { get; }

        [NotNull]
        public BooleanProperty CooksByHerself { get; }

        [NotNull]
        public BooleanProperty LikesExercising { get; }

        [NotNull]
        public BooleanProperty StudiesHard { get; }

        [NotNull]
        public BooleanProperty Chic { get; }

        [NotNull]
        public BooleanProperty AcceptsBlackCoffee { get; }

        [NotNull]
        public BooleanProperty LikesSpicyFood { get; }

        [NotNull]
        public BooleanProperty LikesSweetFood { get; }

        public override ValueProperty<bool>[] GetAllProperties() {
            return new ValueProperty<bool>[] {
                LikesAnimals,
                LikesEating,
                CooksByHerself,
                LikesExercising,
                StudiesHard,
                Chic,
                AcceptsBlackCoffee,
                LikesSpicyFood,
                LikesSweetFood,
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
            _character.SetAnswer(s.Key, s.Value);
        }

        [NotNull]
        private BooleanProperty CreateProperty([NotNull] string key) {
            var v = _character.GetAnswer(key);

            Debug.Assert(v != null, nameof(v) + " != null");
            return new BooleanProperty(key, v.Value);
        }

        [NotNull]
        private readonly KkCharacter _character;

    }
}
