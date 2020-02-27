using System.IO;
using JetBrains.Annotations;
using KGSE.Entities.Internal;

namespace KGSE.IO {
    internal sealed class KkSaveDataWriter : KkSaveDataIoBase {

        public void WriteSaveData([NotNull] KkSaveData saveData, [NotNull] Stream stream) {
            BinaryIo.WriteUtf8String(stream, saveData.Version);
            BinaryIo.WriteUtf8String(stream, saveData.SchoolName);
            stream.Write(saveData.Unknown1);

            var characterWriter = new KkCharacterWriter();

            foreach (var chara in saveData.Characters) {
                characterWriter.WriteCharacter(chara, stream);
            }
        }

    }
}
