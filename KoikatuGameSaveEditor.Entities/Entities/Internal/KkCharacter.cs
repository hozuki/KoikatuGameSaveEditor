using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using KGSE.Entities.Internal.Packed;
using KGSE.IO;
using KGSE.Utilities;
using MsgPack;

namespace KGSE.Entities.Internal {
    internal sealed class KkCharacter {

        [NotNull]
        public string MasterVersion { get; internal set; }

        public bool IsWithCard { get; internal set; }

        [CanBeNull]
        public byte[] PrimaryCardImageData { get; internal set; }

        public int ProductNumber { get; internal set; }

        [NotNull]
        public string Marker { get; internal set; }

        [NotNull]
        public string Unknown1 { get; internal set; }

        [NotNull]
        public byte[] SecondaryCardImageData { get; internal set; }

        [NotNull]
        public CharaList InfoList { get; internal set; }

        // Custom

        [NotNull]
        public MessagePackObjectDictionary Face { get; internal set; }

        [NotNull]
        public MessagePackObjectDictionary Body { get; internal set; }

        [NotNull]
        public MessagePackObjectDictionary Hair { get; internal set; }

        // Coordinates

        [NotNull, ItemNotNull]
        public IReadOnlyList<Coordinate> Coordinates { get; internal set; }

        // Parameters

        internal MessagePackObjectDictionary Parameters { get; set; }

        [NotNull]
        public string FirstName {
            get => Parameters["firstname"].AsString();
            set => Parameters["firstname"] = value;
        }

        [NotNull]
        public string LastName {
            get => Parameters["lastname"].AsString();
            set => Parameters["lastname"] = value;
        }

        [NotNull]
        public string Nickname {
            get => Parameters["nickname"].AsString();
            set => Parameters["nickname"] = value;
        }

        [CanBeNull]
        public int? Intelligence {
            get => GetInt32StringProperty("intelligence");
            set => SetInt32StringProperty("intelligence", value);
        }

        [CanBeNull]
        public int? Physical {
            get => GetInt32StringProperty("physical");
            set => SetInt32StringProperty("physical", value);
        }

        [CanBeNull]
        public int? Hentai {
            get => GetInt32StringProperty("hentai");
            set => SetInt32StringProperty("hentai", value);
        }

        public Gender Gender {
            get => (Gender)Parameters["sex"].AsInt32();
            set => Parameters["sex"] = (int)value;
        }

        [NotNull]
        public MessagePackObjectDictionary Answers {
            // This is not my typo...
            get => Parameters["awnser"].AsDictionary();
        }

        [CanBeNull]
        public bool? GetAnswer([NotNull] string key) {
            return GetBooleanPreference(Answers, key);
        }

        public void SetAnswer([NotNull] string key, bool preference) {
            // Not my typo...
            SetBooleanPreference(Answers, key, preference, "awnser");
        }

        [NotNull]
        public MessagePackObjectDictionary Denials {
            get => Parameters["denial"].AsDictionary();
        }

        [CanBeNull]
        public bool? GetDenial([NotNull] string key) {
            return GetBooleanPreference(Denials, key);
        }

        public void SetDenial([NotNull] string key, bool preference) {
            SetBooleanPreference(Denials, key, preference, "denial");
        }

        [NotNull]
        public MessagePackObjectDictionary Attributes {
            get => Parameters["attribute"].AsDictionary();
        }

        [CanBeNull]
        public bool? GetAttribute([NotNull] string key) {
            return GetBooleanPreference(Attributes, key);
        }

        public void SetAttribute([NotNull] string key, bool preference) {
            SetBooleanPreference(Attributes, key, preference, "attribute");
        }

        public int Personality {
            get => Parameters["personality"].AsInt32();
            set => Parameters["personality"] = value;
        }

        public int WeakPoint {
            get => Parameters["weakPoint"].AsInt32();
            set => Parameters["weakPoint"] = value;
        }

        // Status

        [NotNull]
        internal MessagePackObjectDictionary Status { get; set; }

        // BepInEx Extra

        [CanBeNull]
        public MessagePackObjectDictionary Extra { get; internal set; }

        // Additional

        [CanBeNull]
        public IDictionary<string, byte[]> Percentages { get; internal set; }

        // Maps percentage from float16 to levels (0 to 10, integers)
        [CanBeNull]
        public int? GetPercentage([NotNull] string key) {
            var p = Percentages;

            if (p is null) {
                return null;
            }

            if (!p.ContainsKey(key)) {
                return null;
            }

            var k = p[key];

            if (k is null || k.Length == 0) {
                throw new InvalidOperationException("Cannot get value from a placeholder.");
            }

            var a = k.SubArray(2);

            var v = KkBinaryIo.ReadUInt16LE(a);

            foreach (var (i, level) in PercentageKeyMap.Enumerate()) {
                if (v < level) {
                    return i > 0 ? i - 1 : 0;
                }
            }

            return 10;
        }

        // value: the "percentage index", i.e. 0 -> 0%, 1 -> 10%, ..., 10 -> 100%.
        public void SetPercentage([NotNull] string key, int value) {
            var p = Percentages;

            if (p is null) {
                throw new InvalidOperationException("Percentages are not loaded.");
            }

            if (!p.ContainsKey(key)) {
                throw new KeyNotFoundException($"Key '{key}' is not found in percentage dictionary. Cannot add new properties.");
            }

            if (value < 0 || value > 10) {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Value should be an integer from 0 to 10.");
            }

            var k = p[key];

            if (k is null || k.Length == 0) {
                throw new InvalidOperationException("Cannot set value to a placeholder.");
            }

            var mapped = PercentageKeyMap[value];

            KkBinaryIo.WriteUInt16LE(k, mapped, 2);
        }

        [CanBeNull]
        public byte[] ExtraData { get; internal set; }

        [CanBeNull]
        public int? Intimacy { get; internal set; }

        [CanBeNull]
        public int? EventAfterDay { get; internal set; }

        [CanBeNull]
        public bool? IsFirstGirlfriend { get; internal set; }

        [CanBeNull]
        public byte[] Unknown2 { get; internal set; }

        [CanBeNull]
        public byte[] UnknownMark { get; internal set; }

        [CanBeNull]
        public string DearName { get; internal set; }

        [CanBeNull]
        public int? Feeling { get; internal set; }

        [CanBeNull]
        public int? LoveGauge { get; internal set; }

        [CanBeNull]
        public int? HCount { get; internal set; }

        [CanBeNull]
        public bool? IsClubMember { get; internal set; }

        [CanBeNull]
        public bool? IsLover { get; internal set; }

        [CanBeNull]
        public bool? IsAngry { get; internal set; }

        [CanBeNull]
        public byte[] Unknown3 { get; internal set; }

        // Intelligence field for females. Do not read from Parameters.
        [CanBeNull]
        public int? Intelligence2 { get; internal set; }

        [CanBeNull]
        public int? Strength { get; internal set; }

        [CanBeNull]
        public bool? HasDate { get; internal set; }

        [CanBeNull]
        public int? Ero { get; internal set; }

        [CanBeNull]
        public byte[] Unknown6 { get; internal set; }

        [CanBeNull]
        public byte[] Unknown7 { get; internal set; }

        [CanBeNull]
        public byte[] BeforeAdditional { get; internal set; }

        [CanBeNull]
        public byte[] AfterAdditional { get; internal set; }

        [CanBeNull]
        public IDictionary<string, int> AdditionalValueMap { get; internal set; }

        [CanBeNull, ItemNotNull]
        public string[] AdditionalKeys { get; internal set; }

        // ------

        [CanBeNull]
        private int? GetInt32StringProperty([NotNull] string storageKey) {
            if (!Parameters.ContainsKey(storageKey)) {
                return null;
            }

            var s = Parameters[storageKey].AsString();
            return int.Parse(s);
        }

        private void SetInt32StringProperty([NotNull] string storageKey, int? value) {
            if (!Parameters.ContainsKey(storageKey)) {
                throw new KeyNotFoundException($"There is no property called '{storageKey}'. Cannot add new properties.");
            }

            if (value is null) {
                throw new ArgumentNullException(nameof(value));
            }

            Parameters[storageKey] = value.Value.ToString();
        }

        [CanBeNull]
        private static bool? GetBooleanPreference([NotNull] MessagePackObjectDictionary dictionary, [NotNull] string key) {
            if (dictionary.ContainsKey(key)) {
                return dictionary[key].AsBoolean();
            } else {
                return null;
            }
        }

        private static void SetBooleanPreference([NotNull] MessagePackObjectDictionary dictionary, [NotNull] string key, bool preference, [NotNull] string groupName) {
            if (!dictionary.ContainsKey(key)) {
                throw new KeyNotFoundException($"There is no {groupName} called '{key}'.");
            }

            dictionary[key] = preference;
        }

        // Actually these are half-precision floating point numbers
        [NotNull]
        private static readonly ushort[] PercentageKeyMap = {
            0x3000,
            0x4120,
            0x41a0,
            0x41f4,
            0x4220,
            0x4248,
            0x4270,
            0x428c,
            0x42a0,
            0x42b4,
            0x42c8
        };

    }
}
