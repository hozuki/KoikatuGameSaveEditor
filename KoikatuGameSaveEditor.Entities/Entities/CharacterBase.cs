using System.Diagnostics;
using JetBrains.Annotations;
using KGSE.Entities.Internal;

namespace KGSE.Entities {
    [DebuggerDisplay("Character: {LastName} {FirstName}")]
    public abstract class CharacterBase {

        private protected CharacterBase([NotNull] KkCharacter character) {
            InternalCharacter = character;
        }

        public abstract Gender Gender { get; }

        [NotNull]
        public string FirstName {
            get => InternalCharacter.FirstName;
            set => InternalCharacter.FirstName = value;
        }

        [NotNull]
        public string LastName {
            get => InternalCharacter.LastName;
            set => InternalCharacter.LastName = value;
        }

        [NotNull]
        public string Nickname {
            get => InternalCharacter.Nickname;
            set => InternalCharacter.Nickname = value;
        }

        [CanBeNull]
        public byte[] GetPrimaryCardImageData() {
            return (byte[])InternalCharacter.PrimaryCardImageData?.Clone();
        }

        [NotNull]
        public byte[] GetSecondaryCardImageData() {
            return (byte[])InternalCharacter.SecondaryCardImageData.Clone();
        }

        [NotNull]
        internal KkCharacter InternalCharacter { get; }

    }
}
