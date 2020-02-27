using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using KGSE.Entities.Internal;

namespace KGSE.Entities {
    public sealed class SaveData {

        internal SaveData([NotNull] KkSaveData data) {
            _data = data;

            var characters = new List<CharacterBase>();

            foreach (var chara in data.Characters) {
                CharacterBase c;

                switch (chara.Gender) {
                    case Gender.Male:
                        c = new MaleCharacter(chara);
                        break;
                    case Gender.Female:
                        c = new FemaleCharacter(chara);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(chara.Gender), chara.Gender, null);
                }

                characters.Add(c);
            }

            _characters = characters;
        }

        [NotNull]
        public string SchoolName {
            get => _data.SchoolName;
            set => _data.SchoolName = value;
        }

        [NotNull, ItemNotNull]
        public IReadOnlyList<CharacterBase> Characters {
            [DebuggerStepThrough]
            get => _characters;
        }

        [NotNull]
        internal KkSaveData InternalSaveData {
            [DebuggerStepThrough]
            get => _data;
        }

        [NotNull]
        private readonly KkSaveData _data;

        [NotNull, ItemNotNull]
        private readonly IReadOnlyList<CharacterBase> _characters;

    }
}
