using System.Diagnostics;
using JetBrains.Annotations;
using KGSE.Entities.Internal;

namespace KGSE.Entities {
    [DebuggerDisplay("Male Character: {LastName} {FirstName}")]
    public sealed class MaleCharacter : CharacterBase {

        internal MaleCharacter([NotNull] KkCharacter character)
            : base(character) {
        }

        public override Gender Gender => Gender.Male;

        // 0 to 100
        // WTF not Intelligence?
        public int Intelligence {
            get => InternalCharacter.Intelligence2 ?? 0;
            set => InternalCharacter.Intelligence2 = value;
        }

        // 0 to 100
        // WTF not Physical?
        public int Strength {
            get => InternalCharacter.Strength ?? 0;
            set => InternalCharacter.Strength = value;
        }

        // 0 to 100
        // WTF not Hentai?
        public int Hentai {
            get => InternalCharacter.Ero ?? 0;
            set => InternalCharacter.Ero = value;
        }

    }
}
