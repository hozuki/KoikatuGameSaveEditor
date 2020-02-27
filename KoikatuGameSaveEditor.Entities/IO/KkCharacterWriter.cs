using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using KGSE.Entities;
using KGSE.Entities.Internal;
using KGSE.Entities.Internal.Known;
using KGSE.Entities.Internal.Packed;
using KGSE.Utilities;
using MsgPack;
using MsgPack.Serialization;

namespace KGSE.IO {
    internal sealed class KkCharacterWriter : KkCharacterIoBase {

        // Write serialized character info, without the main PNG image.
        public void WriteCharacter([NotNull] KkCharacter character, [NotNull] Stream stream) {
            BinaryIo.WriteInt32LE(stream, character.ProductNumber);
            BinaryIo.WriteUtf8String(stream, character.Marker);
            BinaryIo.WriteUtf8String(stream, character.Unknown1);

            BinaryIo.WriteInt32LE(stream, character.SecondaryCardImageData.Length);
            stream.Write(character.SecondaryCardImageData);

            {
                // Recompute info list entry offsets and sizes

                var infoList = character.InfoList.Clone();
                var infoData = new Dictionary<string, byte[]>();

                infoData.Add(KnownEntryNames.Custom, PackCustomPart(character));
                infoData.Add(KnownEntryNames.Coordinate, PackCoordinatePart(character));
                infoData.Add(KnownEntryNames.Parameter, PackParametersPart(character));
                infoData.Add(KnownEntryNames.Status, PackStatusPart(character));

                if (character.Extra != null) {
                    infoData.Add(KnownEntryNames.Extra, PackExtraPart(character));
                }

                var infoOrder = infoList.Entries.Select(entry => entry.Name).ToArray();
                byte[] charaValues;
                var position = 0;

                using (var memory = new MemoryStream()) {
                    foreach (var (i, key) in infoOrder.Enumerate()) {
                        infoList.Entries[i].Position = position;

                        var data = infoData[key];

                        infoList.Entries[i].Size = data.Length;
                        memory.Write(data);

                        position += data.Length;
                    }

                    charaValues = memory.ToArray();
                }

                var infoListSerializer = MessagePackSerializer.Get<MessagePackObjectDictionary>();
                var infoListDict = Packing.PackToDictionary(infoList);
                var infoListPacked = infoListSerializer.PackSingleObject(infoListDict);

                BinaryIo.WriteInt32LE(stream, infoListPacked.Length);
                stream.Write(infoListPacked);

                BinaryIo.WriteUInt64LE(stream, (ulong)charaValues.Length);
                stream.Write(charaValues);
            }

            if (character.ExtraData != null) {
                stream.Write(character.ExtraData);
            }

            if (character.Unknown2 != null) {
                stream.Write(character.Unknown2);
            }

            if (character.UnknownMark != null) {
                stream.Write(character.UnknownMark);
            }

            if (character.DearName != null) {
                BinaryIo.WriteUtf8String(stream, character.DearName);
            }

            if (character.Feeling != null) {
                BinaryIo.WriteInt32LE(stream, character.Feeling.Value);
            }

            if (character.LoveGauge != null) {
                BinaryIo.WriteInt32LE(stream, character.LoveGauge.Value);
            }

            if (character.HCount != null) {
                BinaryIo.WriteInt32LE(stream, character.HCount.Value);
            }

            if (character.IsClubMember != null) {
                BinaryIo.WriteByte(stream, (byte)(character.IsClubMember.Value ? 1 : 0));
            }

            if (character.IsLover != null) {
                BinaryIo.WriteByte(stream, (byte)(character.IsLover.Value ? 1 : 0));
            }

            if (character.IsAngry != null) {
                BinaryIo.WriteByte(stream, (byte)(character.IsAngry.Value ? 1 : 0));
            }

            if (character.Unknown3 != null) {
                stream.Write(character.Unknown3);
            }

            if (character.Intelligence2 != null) {
                BinaryIo.WriteInt32LE(stream, character.Intelligence2.Value);
            } else {
                var intel = character.Parameters.ContainsKey("intelligence") ? character.Intelligence.Value : 0;
                BinaryIo.WriteInt32LE(stream, intel);
            }

            {
                var bstr = new byte[4];

                switch (character.Gender) {
                    case Gender.Male:
                        BinaryIo.WriteInt32LE(bstr, character.Strength ?? 0);
                        break;
                    case Gender.Female:
                        bstr[0] = (byte)(character.HasDate.HasValue ? (character.HasDate.Value ? 1 : 0) : 0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(character.Gender), character.Gender, null);
                }

                stream.Write(bstr);
            }

            if (character.Ero != null) {
                BinaryIo.WriteInt32LE(stream, character.Ero.Value);
            }

            if (character.Percentages != null) {
                var perc = character.Percentages;

                if (character.Unknown6 != null) {
                    stream.Write(character.Unknown6);
                }

                stream.Write(perc[KnownPercentageKeys.Mune]);
                stream.Write(perc[KnownPercentageKeys.Kokan]);
                stream.Write(perc[KnownPercentageKeys.Anal]);
                stream.Write(perc[KnownPercentageKeys.Siri]);
                stream.Write(perc[KnownPercentageKeys.Tikubi]);

                if (character.Unknown7 != null) {
                    stream.Write(character.Unknown7);
                }

                stream.Write(perc[KnownPercentageKeys.KokanPiston]);
                stream.Write(perc[KnownPercentageKeys.AnalPiston]);
            }

            WriteAdditional(character, stream);
        }

        [NotNull]
        private byte[] PackCustomPart([NotNull] KkCharacter c) {
            using (var memory = new MemoryStream()) {
                var serializer = MessagePackSerializer.Get<MessagePackObjectDictionary>();

                var packed = serializer.PackSingleObject(c.Face);
                BinaryIo.WriteInt32LE(memory, packed.Length);
                memory.Write(packed);

                packed = serializer.PackSingleObject(c.Body);
                BinaryIo.WriteInt32LE(memory, packed.Length);
                memory.Write(packed);

                packed = serializer.PackSingleObject(c.Hair);
                BinaryIo.WriteInt32LE(memory, packed.Length);
                memory.Write(packed);

                return memory.ToArray();
            }
        }

        [NotNull]
        private byte[] PackCoordinatePart([NotNull] KkCharacter c) {
            var o = new byte[c.Coordinates.Count][];
            var dictSerializer = MessagePackSerializer.Get<MessagePackObjectDictionary>();

            foreach (var (i, coordinate) in c.Coordinates.Enumerate()) {
                var memory = new MemoryStream();

                var p = dictSerializer.PackSingleObject(coordinate.Clothes);
                BinaryIo.WriteInt32LE(memory, p.Length);
                memory.Write(p);

                p = dictSerializer.PackSingleObject(coordinate.Accessory);
                BinaryIo.WriteInt32LE(memory, p.Length);
                memory.Write(p);

                BinaryIo.WriteByte(memory, (byte)(coordinate.MakeupEnabled ? 1 : 0));

                p = dictSerializer.PackSingleObject(coordinate.Makeup);
                BinaryIo.WriteInt32LE(memory, p.Length);
                memory.Write(p);

                o[i] = memory.ToArray();
            }

            var bytesSerializer = MessagePackSerializer.Get<byte[][]>();
            var packed = bytesSerializer.PackSingleObject(o);

            return packed;
        }

        [NotNull]
        private byte[] PackParametersPart([NotNull] KkCharacter c) {
            var serializer = MessagePackSerializer.Get<MessagePackObjectDictionary>();
            return serializer.PackSingleObject(c.Parameters);
        }

        [NotNull]
        private byte[] PackStatusPart([NotNull] KkCharacter c) {
            var serializer = MessagePackSerializer.Get<MessagePackObjectDictionary>();
            return serializer.PackSingleObject(c.Status);
        }

        [NotNull]
        private byte[] PackExtraPart([NotNull] KkCharacter c) {
            var serializer = MessagePackSerializer.Get<MessagePackObjectDictionary>();
            return serializer.PackSingleObject(c.Extra);
        }

        private void WriteAdditional([NotNull] KkCharacter c, [NotNull] Stream stream) {
            if (c.BeforeAdditional != null) {
                stream.Write(c.BeforeAdditional);
            }

            if (c.AdditionalKeys != null && c.AdditionalValueMap != null) {
                foreach (var key in c.AdditionalKeys) {
                    BinaryIo.WriteUtf8String(stream, key);
                    BinaryIo.WriteInt32LE(stream, c.AdditionalValueMap[key]);
                }
            }

            if (c.Percentages != null && c.Percentages.ContainsKey(KnownPercentageKeys.Houshi)) {
                stream.Write(c.Percentages[KnownPercentageKeys.Houshi]);
            }

            if (c.Gender == Gender.Female) {
                var stringComparer = StringComparer.Ordinal;

                if (stringComparer.Compare(c.MasterVersion, "0.0.7") >= 0) {
                    if (c.EventAfterDay != null) {
                        BinaryIo.WriteInt32LE(stream, c.EventAfterDay.Value);
                    }

                    if (c.IsFirstGirlfriend != null) {
                        BinaryIo.WriteByte(stream, (byte)(c.IsFirstGirlfriend.Value ? 1 : 0));
                    }
                }

                if (stringComparer.Compare(c.MasterVersion, "1.0.1") >= 0) {
                    if (c.Intimacy != null) {
                        BinaryIo.WriteInt32LE(stream, c.Intimacy.Value);
                    }
                }
            }

            if (c.AfterAdditional != null) {
                stream.Write(c.AfterAdditional);
            }
        }

    }
}
