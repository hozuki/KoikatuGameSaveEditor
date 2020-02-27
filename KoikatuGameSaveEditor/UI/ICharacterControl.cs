using JetBrains.Annotations;
using KGSE.Entities;

namespace KGSE.UI {
    public interface ICharacterControl {

        [CanBeNull]
        CharacterBase Character { get; }

        void LoadFromCharacter();

        void SaveToCharacter();

        void ValidateInput([NotNull] IErrorReport report);

    }
}
