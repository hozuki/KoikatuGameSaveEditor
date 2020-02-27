using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using KGSE.Entities.Internal;
using KGSE.Utilities;

namespace KGSE.IO {
    internal sealed class KkSaveDataReader : KkSaveDataIoBase {

        [NotNull]
        public KkSaveData ReadSaveData([NotNull] Stream stream) {
            var r = new KkSaveData();

            r.Version = BinaryIo.ReadUtf8String(stream);
            r.SchoolName = BinaryIo.ReadUtf8String(stream);

            r.Unknown1 = stream.ReadExact(17);

            var charaDataLump = stream.ReadRest();

            byte[] charaHeader;

            do {
                var charaHeaderIndex = ByteArray.Find(charaDataLump, CharacterMarkerKoikatu);

                if (charaHeaderIndex == 0) {
                    charaHeader = CharacterMarkerKoikatu;
                    break;
                }

                charaHeaderIndex = ByteArray.Find(charaDataLump, CharacterMarkerKoikatuParty);

                if (charaHeaderIndex == 0) {
                    charaHeader = CharacterMarkerKoikatuParty;
                    break;
                }

                throw new FormatException("Cannot find any known character header.");
            } while (false);

            var characters = new List<KkCharacter>();
            var counter = 0;

            var charaDataParts = ByteArray.Split(charaDataLump, charaHeader);
            var characterReader = new KkCharacterReader();

            foreach (var part in charaDataParts) {
                KkCharacter character;

                using (var memory = new MemoryStream()) {
                    memory.Write(charaHeader);
                    memory.Write(part);
                    memory.Position = 0;

                    character = characterReader.ReadCharacter(memory, false, counter == 0, r.Version);
                }

                characters.Add(character);

                counter += 1;
            }

            r.Characters = characters;

            return r;
        }

        [NotNull]
        private static readonly byte[] CharacterMarkerKoikatu = { 0x64, 0x00, 0x00, 0x00, 0x12, 0xe3, 0x80, 0x90, (byte)'K', (byte)'o', (byte)'i', (byte)'K', (byte)'a', (byte)'t', (byte)'u', (byte)'C', (byte)'h', (byte)'a', (byte)'r', (byte)'a', 0xe3, 0x80, 0x91 };

        [NotNull]
        private static readonly byte[] CharacterMarkerKoikatuParty = { 0x64, 0x00, 0x00, 0x00, 0x14, 0xe3, 0x80, 0x90, (byte)'K', (byte)'o', (byte)'i', (byte)'K', (byte)'a', (byte)'t', (byte)'u', (byte)'C', (byte)'h', (byte)'a', (byte)'r', (byte)'a', (byte)'S', (byte)'P', 0xe3, 0x80, 0x91 };

    }
}
