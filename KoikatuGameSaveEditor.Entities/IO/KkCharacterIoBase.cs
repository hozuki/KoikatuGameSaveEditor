using JetBrains.Annotations;

namespace KGSE.IO {
    internal abstract class KkCharacterIoBase {

        private protected KkCharacterIoBase() {
            BinaryIo = new KkBinaryIo();
        }

        [NotNull]
        private protected readonly KkBinaryIo BinaryIo;

    }
}
