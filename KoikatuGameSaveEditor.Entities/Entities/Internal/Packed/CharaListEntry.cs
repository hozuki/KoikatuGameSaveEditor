using System;
using System.Diagnostics;
using JetBrains.Annotations;
using MsgPack.Serialization;

namespace KGSE.Entities.Internal.Packed {
    [DebuggerDisplay("Info list entry '{" + nameof(Name) + "}'")]
    public sealed class CharaListEntry : ICloneable<CharaListEntry>, ICloneable {

        [MessagePackMember(0, Name = "name", NilImplication = NilImplication.Prohibit)]
        [NotNull]
        public string Name { get; set; }

        [MessagePackMember(1, Name = "version", NilImplication = NilImplication.Prohibit)]
        [NotNull]
        public string Version { get; set; }

        [MessagePackMember(2, Name = "pos", NilImplication = NilImplication.Prohibit)]
        public int Position { get; set; }

        [MessagePackMember(3, Name = "size", NilImplication = NilImplication.Prohibit)]
        public int Size { get; set; }

        public CharaListEntry Clone() {
            return new CharaListEntry {
                Name = Name,
                Version = Version,
                Position = Position,
                Size = Size
            };
        }

        object ICloneable.Clone() {
            return Clone();
        }

    }
}
