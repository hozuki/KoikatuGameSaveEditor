using JetBrains.Annotations;

namespace KGSE.Entities.Internal {
    internal struct IdatChunk {

        public IdatChunk([NotNull] byte[] type, [NotNull] byte[] data, [NotNull] byte[] crc) {
            Type = type;
            Data = data;
            Crc = crc;
        }

        [NotNull]
        public readonly byte[] Type;

        [NotNull]
        public readonly byte[] Data;

        [NotNull]
        public readonly byte[] Crc;

    }
}
