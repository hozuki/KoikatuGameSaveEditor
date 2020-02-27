using System.IO;
using JetBrains.Annotations;
using KGSE.Entities;

namespace KGSE.IO {
    public static class SaveDataIo {

        [NotNull]
        public static SaveData FromByteArray([NotNull] byte[] data) {
            using (var memory = new MemoryStream(data)) {
                return Read(memory);
            }
        }

        [NotNull]
        public static SaveData Read([NotNull] Stream stream) {
            var reader = new KkSaveDataReader();
            var saveData = reader.ReadSaveData(stream);

            var s = new SaveData(saveData);

            return s;
        }

        public static void Write([NotNull] SaveData saveData, [NotNull] Stream stream) {
            var writer = new KkSaveDataWriter();
            writer.WriteSaveData(saveData.InternalSaveData, stream);
        }

        [NotNull]
        public static byte[] ToByteArray([NotNull] SaveData saveData) {
            using (var memory = new MemoryStream()) {
                Write(saveData, memory);
                return memory.ToArray();
            }
        }

    }
}
