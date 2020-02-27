using JetBrains.Annotations;
using MsgPack;

namespace KGSE.Entities.Internal.Packed {
    internal sealed class Coordinate {

        [NotNull]
        public MessagePackObjectDictionary Clothes { get; internal set; }

        [NotNull]
        public MessagePackObjectDictionary Accessory { get; internal set; }

        public bool MakeupEnabled { get; internal set; }

        [NotNull]
        public MessagePackObjectDictionary Makeup { get; internal set; }

    }
}
