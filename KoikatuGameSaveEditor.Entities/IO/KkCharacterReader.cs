using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;
using KGSE.Entities;
using KGSE.Entities.Internal;
using KGSE.Entities.Internal.Known;
using KGSE.Entities.Internal.Packed;
using KGSE.Utilities;
using MsgPack;
using MsgPack.Serialization;

namespace KGSE.IO {
    internal sealed class KkCharacterReader : KkCharacterIoBase {

        [NotNull]
        public KkCharacter ReadCharacter([NotNull] Stream stream, bool withCard = true, bool skipAdditional = true, [NotNull] string masterVersion = DefaultMasterVersion) {
            var r = new KkCharacter();

            r.MasterVersion = masterVersion;
            r.IsWithCard = withCard;

            if (withCard) {
                // Main card image (the displayed one)
                r.PrimaryCardImageData = ReadPngFileData(stream);
            }

            // Header
            r.ProductNumber = BinaryIo.ReadInt32LE(stream);
            r.Marker = BinaryIo.ReadUtf8String(stream);

            // Version?
            r.Unknown1 = BinaryIo.ReadUtf8String(stream);

            // Secondary card image
            var secondaryCardImageSize = BinaryIo.ReadInt32LE(stream);
            r.SecondaryCardImageData = ReadPngFileData(stream);

            // List info
            var listInfoSize = BinaryIo.ReadInt32LE(stream);
            var listInfoData = stream.ReadExact(listInfoSize);

            {
                var serializer = MessagePackSerializer.Get<CharaList>();
                r.InfoList = serializer.UnpackSingleObject(listInfoData);
            }

            int charaDataSize;

            {
                var charaDataSizeLong = BinaryIo.ReadUInt64LE(stream);

                Trace.Assert(charaDataSizeLong < int.MaxValue);

                charaDataSize = (int)charaDataSizeLong;
            }

            var charaData = stream.ReadExact(charaDataSize);

            foreach (var entry in r.InfoList.Entries) {
                byte[] part;

                switch (entry.Name) {
                    case KnownEntryNames.Custom:
                        part = GetCharaDataBytes(charaData, entry);
                        ReadCustomPart(r, part);
                        break;
                    case KnownEntryNames.Coordinate:
                        part = GetCharaDataBytes(charaData, entry);
                        ReadCoordinatePart(r, part);
                        break;
                    case KnownEntryNames.Parameter:
                        part = GetCharaDataBytes(charaData, entry);
                        ReadParametersPart(r, part);
                        break;
                    case KnownEntryNames.Status:
                        part = GetCharaDataBytes(charaData, entry);
                        ReadStatusPart(r, part);
                        break;
                    case KnownEntryNames.Extra:
                        part = GetCharaDataBytes(charaData, entry);
                        ReadExtraPart(r, part);
                        break;
                    default:
                        throw new FormatException($"Unknown entry name: '{entry.Name}'.");
                }
            }

            // If we are in a real save game, there is more information...
            if (!withCard) {
                {
                    var len1 = BinaryIo.ReadByte(stream);

                    if (len1 == 4) {
                        var marker = stream.ReadExact(len1);

                        if (marker.ElementsEqual(KkexMarker)) {
                            var version = BinaryIo.ReadInt32LE(stream);
                            var len2 = BinaryIo.ReadInt32LE(stream);
                            var exData = stream.ReadExact(len2);

                            using (var memory = new MemoryStream()) {
                                BinaryIo.WriteByte(memory, len1);
                                memory.Write(marker);
                                BinaryIo.WriteInt32LE(memory, version);
                                BinaryIo.WriteInt32LE(memory, len2);
                                memory.Write(exData);

                                r.ExtraData = memory.ToArray();
                            }
                        }
                    } else {
                        stream.Position -= 1;
                    }
                }

                r.Unknown2 = stream.ReadExact(4);
                r.UnknownMark = stream.ReadExact(4);

                r.DearName = BinaryIo.ReadUtf8String(stream);

                r.Feeling = BinaryIo.ReadInt32LE(stream);
                r.LoveGauge = BinaryIo.ReadInt32LE(stream);
                r.HCount = BinaryIo.ReadInt32LE(stream);
                r.IsClubMember = BinaryIo.ReadByte(stream) != 0;
                r.IsLover = BinaryIo.ReadByte(stream) != 0;
                r.IsAngry = BinaryIo.ReadByte(stream) != 0;

                r.Unknown3 = stream.ReadExact(1);

                r.Intelligence2 = BinaryIo.ReadInt32LE(stream);

                switch (r.Gender) {
                    case Gender.Male:
                        r.Strength = BinaryIo.ReadInt32LE(stream);
                        r.HasDate = false;
                        break;
                    case Gender.Female:
                        r.HasDate = BinaryIo.ReadByte(stream) != 0;
                        stream.ReadExact(3); // No used
                        r.Strength = 0;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(r.Gender), r.Gender, null);
                }

                r.Ero = BinaryIo.ReadInt32LE(stream);

                var percentages = new Dictionary<string, byte[]>();

                if (skipAdditional) {
                    percentages[KnownPercentageKeys.Mune] = ByteArray.Empty;
                    percentages[KnownPercentageKeys.Kokan] = ByteArray.Empty;
                    percentages[KnownPercentageKeys.Anal] = ByteArray.Empty;
                    percentages[KnownPercentageKeys.Siri] = ByteArray.Empty;
                    percentages[KnownPercentageKeys.Tikubi] = ByteArray.Empty;
                    percentages[KnownPercentageKeys.KokanPiston] = ByteArray.Empty;
                    percentages[KnownPercentageKeys.AnalPiston] = ByteArray.Empty;
                    percentages[KnownPercentageKeys.Houshi] = ByteArray.Empty;
                    r.Unknown6 = ByteArray.Empty;
                    r.Unknown7 = ByteArray.Empty;
                } else {
                    r.Unknown6 = stream.ReadExact(14);
                    percentages[KnownPercentageKeys.Mune] = stream.ReadExact(4);
                    percentages[KnownPercentageKeys.Kokan] = stream.ReadExact(4);
                    percentages[KnownPercentageKeys.Anal] = stream.ReadExact(4);
                    percentages[KnownPercentageKeys.Siri] = stream.ReadExact(4);
                    percentages[KnownPercentageKeys.Tikubi] = stream.ReadExact(4);
                    r.Unknown7 = stream.ReadExact(14);
                    percentages[KnownPercentageKeys.KokanPiston] = stream.ReadExact(4);
                    percentages[KnownPercentageKeys.AnalPiston] = stream.ReadExact(4);
                    percentages[KnownPercentageKeys.Houshi] = ByteArray.Empty; // This value is set later.
                }

                r.Percentages = percentages;

                ReadAdditionalData(stream, r);
            }

            return r;
        }

        [NotNull]
        private byte[] ReadPngFileData([NotNull] Stream stream) {
            var signature = stream.ReadExact(8);

            Trace.Assert(signature.ElementsEqual(PngHeaderSignature));

            var ihdr = stream.ReadExact(25);

            // Read IDAT chunks
            var idatChunks = new List<IdatChunk>();
            var idatDataLength = BinaryIo.ReadInt32BE(stream);

            while (idatDataLength > 0) {
                var idatType = stream.ReadExact(4);

                Trace.Assert(idatType.ElementsEqual(BlockIdat));

                var idatData = stream.ReadExact(idatDataLength);
                var idatCrc = stream.ReadExact(4);

                idatChunks.Add(new IdatChunk(idatType, idatData, idatCrc));

                idatDataLength = BinaryIo.ReadInt32BE(stream);
            }

            // Read IEND chunk
            var iendDataLength = idatDataLength;
            var iendType = stream.ReadExact(4);

            Trace.Assert(iendType.ElementsEqual(BlockIend));

            var iendCrc = stream.ReadExact(4);

            // Combine chunks
            byte[] result;

            using (var memory = new MemoryStream()) {
                memory.Write(signature);
                memory.Write(ihdr);

                foreach (var chunk in idatChunks) {
                    BinaryIo.WriteInt32BE(memory, chunk.Data.Length);
                    memory.Write(chunk.Type);
                    memory.Write(chunk.Data);
                    memory.Write(chunk.Crc);
                }

                idatChunks.Clear();

                BinaryIo.WriteInt32BE(memory, iendDataLength);
                memory.Write(iendType);
                memory.Write(iendCrc);

                result = memory.ToArray();
            }

            return result;
        }

        [NotNull]
        private static byte[] GetCharaDataBytes([NotNull] byte[] data, [NotNull] CharaListEntry entry) {
            return data.SubArray(entry.Position, entry.Size);
        }

        private void ReadCustomPart([NotNull] KkCharacter c, [NotNull] byte[] part) {
            using (var memory = new MemoryStream(part, false)) {
                var length = BinaryIo.ReadInt32LE(memory);
                var data = memory.ReadExact(length);
                c.Face = Unpacking.UnpackDictionary(data).Value;

                length = BinaryIo.ReadInt32LE(memory);
                data = memory.ReadExact(length);
                c.Body = Unpacking.UnpackDictionary(data).Value;

                length = BinaryIo.ReadInt32LE(memory);
                data = memory.ReadExact(length);
                c.Hair = Unpacking.UnpackDictionary(data).Value;
            }
        }

        private void ReadCoordinatePart([NotNull] KkCharacter c, [NotNull] byte[] part) {
            var unpacked = Unpacking.UnpackArray(part).Value;
            var coordinates = new List<Coordinate>();

            foreach (var o in unpacked) {
                var coordinateData = o.AsBinary();
                var coordinate = new Coordinate();

                using (var memory = new MemoryStream(coordinateData, false)) {
                    var length = BinaryIo.ReadInt32LE(memory);
                    var data = memory.ReadExact(length);
                    coordinate.Clothes = Unpacking.UnpackDictionary(data).Value;

                    length = BinaryIo.ReadInt32LE(memory);
                    data = memory.ReadExact(length);
                    coordinate.Accessory = Unpacking.UnpackDictionary(data).Value;

                    var makeupEnabled = BinaryIo.ReadByte(memory);
                    coordinate.MakeupEnabled = makeupEnabled != 0;

                    length = BinaryIo.ReadInt32LE(memory);
                    data = memory.ReadExact(length);
                    coordinate.Makeup = Unpacking.UnpackDictionary(data).Value;
                }

                coordinates.Add(coordinate);
            }

            c.Coordinates = coordinates;
        }

        private void ReadParametersPart([NotNull] KkCharacter c, [NotNull] byte[] part) {
            c.Parameters = Unpacking.UnpackDictionary(part).Value;
        }

        private void ReadStatusPart([NotNull] KkCharacter c, [NotNull] byte[] part) {
            c.Status = Unpacking.UnpackDictionary(part).Value;
        }

        private void ReadExtraPart([NotNull] KkCharacter c, [NotNull] byte[] part) {
            c.Extra = Unpacking.UnpackDictionary(part).Value;
        }

        private void ReadAdditionalData([NotNull] Stream data, [NotNull] KkCharacter c) {
            var chunk = data.ReadRest();
            var start = ByteArray.Find(chunk, IdleKey);

            if (start < 0) {
                c.BeforeAdditional = chunk;
                c.EventAfterDay = 0;
                c.IsFirstGirlfriend = false;
                c.Intimacy = 0;
                c.AfterAdditional = ByteArray.Empty;

                return;
            }

            c.BeforeAdditional = chunk.SubArray(0, start - 1);

            var additionalKeys = new List<string>();
            var additionalValues = new List<int>();

            using (var stream = new MemoryStream(chunk, start + IdleKey.Length, chunk.Length - (start + IdleKey.Length), false)) {
                {
                    var idleValue = BinaryIo.ReadInt32LE(stream);
                    additionalKeys.Add("Idle");
                    additionalValues.Add(idleValue);
                }

                while (true) {
                    var len = BinaryIo.ReadByte(stream);

                    if (len == 0) {
                        stream.Position -= 1;
                        break;
                    }

                    // Go back one byte so we can reuse ReadUtf8String()
                    stream.Position -= 1;

                    var key = BinaryIo.ReadUtf8String(stream);
                    var value = BinaryIo.ReadInt32LE(stream);

                    additionalKeys.Add(key);
                    additionalValues.Add(value);
                }

                {
                    c.AdditionalKeys = additionalKeys.ToArray();

                    var d = new Dictionary<string, int>();
                    var count = additionalKeys.Count;

                    for (var i = 0; i < count; i += 1) {
                        d.Add(additionalKeys[i], additionalValues[i]);
                    }

                    c.AdditionalValueMap = d;
                }

                {
                    var levelBytes = stream.ReadExact(4);
                    Debug.Assert(c.Percentages != null, "c.Percentages != null");
                    c.Percentages[KnownPercentageKeys.Houshi] = levelBytes;
                }

                var stringComparer = StringComparer.Ordinal;

                if (stringComparer.Compare(c.MasterVersion, "0.0.7") >= 0) {
                    c.EventAfterDay = BinaryIo.ReadInt32LE(stream);
                    c.IsFirstGirlfriend = BinaryIo.ReadByte(stream) != 0;
                }

                if (stringComparer.Compare(c.MasterVersion, "1.0.1") >= 0) {
                    c.Intimacy = BinaryIo.ReadInt32LE(stream);
                }

                c.AfterAdditional = stream.ReadRest();
            }
        }

        private const string DefaultMasterVersion = "0.0.0";

        [NotNull]
        private static readonly byte[] PngHeaderSignature = { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a };

        [NotNull]
        private static readonly byte[] BlockIdat = { (byte)'I', (byte)'D', (byte)'A', (byte)'T' };

        [NotNull]
        private static readonly byte[] BlockIend = { (byte)'I', (byte)'E', (byte)'N', (byte)'D' };

        [NotNull]
        private static readonly byte[] KkexMarker = { (byte)'K', (byte)'K', (byte)'E', (byte)'x' };

        [NotNull]
        private static readonly byte[] IdleKey = { (byte)'I', (byte)'d', (byte)'l', (byte)'e' };

    }
}
