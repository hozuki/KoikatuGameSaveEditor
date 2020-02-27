using JetBrains.Annotations;

namespace KGSE.IO {
    internal abstract class KkSaveDataIoBase {

        protected KkSaveDataIoBase() {
            BinaryIo = new KkBinaryIo();
        }

        [NotNull]
        private protected readonly KkBinaryIo BinaryIo;

    }
}
