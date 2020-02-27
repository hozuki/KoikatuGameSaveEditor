using System;
using JetBrains.Annotations;
using KGSE.Utilities;
using MsgPack.Serialization;

namespace KGSE.Entities.Internal.Packed {
    public sealed class CharaList : ICloneable<CharaList>, ICloneable {

        [MessagePackMember(0, Name = "lstInfo", NilImplication = NilImplication.Prohibit)]
        [NotNull, ItemNotNull]
        public CharaListEntry[] Entries { get; set; }

        public CharaList Clone() {
            var entries = new CharaListEntry[Entries.Length];

            foreach (var (i, e) in Entries.Enumerate()) {
                entries[i] = e.Clone();
            }

            return new CharaList {
                Entries = entries
            };
        }

        object ICloneable.Clone() {
            return Clone();
        }

    }
}
