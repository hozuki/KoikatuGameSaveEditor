using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using KGSE.IO;

namespace KGSE.Utilities {
    internal static class ByteArray {

        static ByteArray() {
            Empty = Array.Empty<byte>();
        }

        [NotNull]
        public static byte[] Empty { get; }

        [NotNull, ItemNotNull]
        public static byte[][] Split([NotNull] byte[] data, [NotNull] byte[] pattern) {
            return Split(data, pattern, true);
        }

        [NotNull, ItemNotNull]
        public static byte[][] Split([NotNull] byte[] data, [NotNull] byte[] pattern, bool removeEmptyEntries) {
            if (data.Length == 0) {
                return Array.Empty<byte[]>();
            }

            if (pattern.Length == 0) {
                throw new ArgumentException("Pattern cannot be empty.", nameof(pattern));
            }

            var lps = ComputeLpsArray(pattern);

            var lastIndex = 0;
            var nextIndex = Find(data, pattern, lastIndex, lps);

            if (nextIndex < 0) {
                return new[] { (byte[])data.Clone() };
            }

            var result = new List<byte[]>();

            do {
                var subArray = SubArray(data, lastIndex, nextIndex - lastIndex, false);

                if (subArray.Length == 0) {
                    if (!removeEmptyEntries) {
                        result.Add(subArray);
                    }
                } else {
                    result.Add(subArray);
                }

                lastIndex = nextIndex + pattern.Length;
                nextIndex = Find(data, pattern, lastIndex, lps);
            } while (nextIndex >= 0);

            // Don't forget the last one.
            if (lastIndex < data.Length) {
                var subArray = SubArray(data, lastIndex, data.Length - lastIndex, false);
                result.Add(subArray);
            }

            return result.ToArray();
        }

        public static int Find([NotNull] byte[] data, [NotNull] byte[] pattern) {
            return Find(data, pattern, 0);
        }

        public static int Find([NotNull] byte[] data, [NotNull] byte[] pattern, int startIndex) {
            var lps = ComputeLpsArray(pattern);
            return Find(data, pattern, startIndex, lps);
        }

        [NotNull]
        public static byte[] SubArray([NotNull] this byte[] array, int index) {
            return SubArray(array, index, array.Length - index);
        }

        [NotNull]
        public static byte[] SubArray([NotNull] this byte[] array, int index, int count) {
            return SubArray(array, index, count, true);
        }

        [NotNull]
        public static byte[] Join([NotNull, ItemNotNull] params byte[][] data) {
            byte[] result;

            using (var memory = new MemoryStream()) {
                foreach (var d in data) {
                    memory.Write(d);
                }

                result = memory.ToArray();
            }

            return result;
        }

        // Search for first byte pattern (KMP)
        private static int Find([NotNull] byte[] data, [NotNull] byte[] pattern, int startIndex, int[] lps) {
            var dataLength = data.Length;
            var patternLength = pattern.Length;

            var dataIndex = startIndex;
            var patternIndex = 0;

            while (dataIndex < dataLength) {
                if (pattern[patternIndex] == data[dataIndex]) {
                    patternIndex += 1;
                    dataIndex += 1;
                }

                if (patternIndex == patternLength) {
                    return dataIndex - patternIndex;
                }

                if (dataIndex < dataLength && pattern[patternIndex] != data[dataIndex]) {
                    if (patternIndex > 0) {
                        patternIndex = lps[patternIndex - 1];
                    } else {
                        dataIndex += 1;
                    }
                }
            }

            return -1;
        }

        [NotNull]
        private static byte[] SubArray([NotNull] this byte[] array, int index, int count, bool throwOnOverRange) {
            if (count < 0) {
                throw new ArgumentOutOfRangeException(nameof(count), count, null);
            } else if (count == 0) {
                return Empty;
            }

            if (array.Length == 0) {
                throw new ArgumentException("Array cannot be empty.", nameof(array));
            }

            if (index >= array.Length) {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var subLength = Math.Min(array.Length - index, count);

            if (subLength < 0) {
                if (throwOnOverRange) {
                    throw new ArgumentOutOfRangeException(nameof(index), "Range exceeds array boundary.");
                } else {
                    subLength = 0;
                }
            }

            if (subLength == 0) {
                return Empty;
            }

            var result = new byte[subLength];

            Array.Copy(array, index, result, 0, subLength);

            return result;
        }

        // Compute LPS (longest prefix suffix) indices
        [NotNull]
        private static int[] ComputeLpsArray([NotNull] byte[] pattern) {
            var patternLength = pattern.Length;
            var lps = new int[patternLength];

            var len = 0;
            var i = 1;
            lps[0] = 0;

            while (i < patternLength) {
                if (pattern[i] == pattern[len]) {
                    len += 1;
                    lps[i] = len;
                    i += 1;
                } else {
                    if (len != 0) {
                        len = lps[len - 1];
                    } else {
                        lps[i] = len;
                        i += 1;
                    }
                }
            }

            return lps;
        }

    }
}
