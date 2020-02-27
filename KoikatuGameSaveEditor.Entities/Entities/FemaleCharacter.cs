using System.Diagnostics;
using JetBrains.Annotations;
using KGSE.Entities.Internal;

namespace KGSE.Entities {
    [DebuggerDisplay("Female Character: {LastName} {FirstName}")]
    public sealed class FemaleCharacter : CharacterBase {

        internal FemaleCharacter([NotNull] KkCharacter character)
            : base(character) {
            Answers = new Answers(character);
            IntercoursePreferences = new IntercoursePreference(character);
            Traits = new Traits(character);
            Development = new Development(character);
        }

        public override Gender Gender => Gender.Female;

        public Personality Personality {
            get => (Personality)InternalCharacter.Personality;
            set => InternalCharacter.Personality = (int)value;
        }

        public WeakPoint WeakPoint {
            get => (WeakPoint)InternalCharacter.WeakPoint;
            set => InternalCharacter.WeakPoint = (int)value;
        }

        [NotNull]
        public Answers Answers { get; }

        [NotNull]
        public IntercoursePreference IntercoursePreferences { get; }

        [NotNull]
        public Traits Traits { get; }

        public int Feeling {
            get => InternalCharacter.Feeling ?? 0;
            set => InternalCharacter.Feeling = value;
        }

        public bool IsLover {
            get => InternalCharacter.IsLover ?? false;
            set => InternalCharacter.IsLover = value;
        }

        public int EroticDegree {
            get => InternalCharacter.LoveGauge ?? 0;
            set => InternalCharacter.LoveGauge = value;
        }

        public int IntercourseCount {
            get => InternalCharacter.HCount ?? 0;
            set => InternalCharacter.HCount = value;
        }

        public int Intimacy {
            get => InternalCharacter.Intimacy ?? 0;
            set => InternalCharacter.Intimacy = value;
        }

        public bool IsAngry {
            get => InternalCharacter.IsAngry ?? false;
            set => InternalCharacter.IsAngry = value;
        }

        public bool IsClubMember {
            get => InternalCharacter.IsClubMember ?? false;
            set => InternalCharacter.IsClubMember = value;
        }

        public bool HasDate {
            get => InternalCharacter.HasDate ?? false;
            set => InternalCharacter.HasDate = value;
        }

        [NotNull]
        public Development Development { get; }

    }
}
