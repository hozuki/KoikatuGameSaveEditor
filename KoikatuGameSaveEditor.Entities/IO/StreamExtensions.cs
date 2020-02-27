using System;
using System.IO;
using JetBrains.Annotations;

namespace KGSE.IO {
    internal static class StreamExtensions {

        [NotNull]
        public static byte[] ReadExact([NotNull] this Stream stream, int count) {
            var result = new byte[count];

            ReadExact(stream, result, 0, count);

            return result;
        }

        public static void ReadExact([NotNull] this Stream stream, [NotNull] byte[] buffer, int startIndex, int count) {
            if (startIndex < 0 || startIndex >= buffer.Length) {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            int totalToRead;

            if (count < 0) {
                throw new ArgumentOutOfRangeException(nameof(count));
            } else if (count == 0) {
                return;
            } else {
                totalToRead = Math.Max(Math.Min(count, buffer.Length - startIndex), 0);

                if (totalToRead == 0) {
                    throw new ArgumentException("Trying to read more than buffer size.");
                }
            }

            var index = startIndex;
            var remained = totalToRead;

            while (remained > 0) {
                var read = stream.Read(buffer, index, remained);

                remained -= read;
                index += read;
            }
        }

        [NotNull]
        public static byte[] ReadRest([NotNull] this Stream stream) {
            byte[] result;

            using (var memory = new MemoryStream()) {
                stream.CopyTo(memory);
                result = memory.ToArray();
            }

            return result;
        }

        public static void Write([NotNull] this Stream stream, [NotNull] byte[] data) {
            stream.Write(data, 0, data.Length);
        }

    }
}
