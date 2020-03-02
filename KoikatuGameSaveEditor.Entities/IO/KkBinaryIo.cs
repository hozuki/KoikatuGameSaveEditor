using System;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;

namespace KGSE.IO {
    // DESIGNED FOR SINGLE-THREADED I/O!
    internal sealed class KkBinaryIo {

        public byte ReadByte([NotNull] Stream stream) {
            stream.ReadExact(_readBuffer, 0, 1);
            return _readBuffer[0];
        }

        public void WriteByte([NotNull] Stream stream, byte b) {
            _writeBuffer[0] = b;
            stream.Write(_writeBuffer, 0, 1);
        }

        public uint ReadUInt32BE([NotNull] Stream stream) {
            stream.ReadExact(_readBuffer, 0, 4);

            var u = BitConverter.ToUInt32(_readBuffer, 0);

            if (BitConverter.IsLittleEndian) {
                u = SwapEndian(u);
            }

            return u;
        }

        public void WriteUInt32BE([NotNull] Stream stream, uint value) {
            if (BitConverter.IsLittleEndian) {
                value = SwapEndian(value);
            }

            var bytes = BitConverter.GetBytes(value);

            stream.Write(bytes);
        }

        public uint ReadUInt32LE([NotNull] Stream stream) {
            stream.ReadExact(_readBuffer, 0, 4);

            var u = BitConverter.ToUInt32(_readBuffer, 0);

            if (!BitConverter.IsLittleEndian) {
                u = SwapEndian(u);
            }

            return u;
        }

        public int ReadInt32BE([NotNull] Stream stream) {
            return unchecked((int)ReadUInt32BE(stream));
        }

        public void WriteInt32BE([NotNull] Stream stream, int value) {
            WriteUInt32BE(stream, unchecked((uint)value));
        }

        public int ReadInt32LE([NotNull] Stream stream) {
            return unchecked((int)ReadUInt32LE(stream));
        }

        public void WriteInt32LE([NotNull] Stream stream, int value) {
            WriteUInt32LE(stream, unchecked((uint)value));
        }

        public void WriteUInt32LE([NotNull] Stream stream, uint value) {
            if (!BitConverter.IsLittleEndian) {
                value = SwapEndian(value);
            }

            var bytes = BitConverter.GetBytes(value);

            stream.Write(bytes);
        }

        public void WriteInt32LE([NotNull] byte[] buffer, int value) {
            WriteUInt32LE(buffer, unchecked((uint)value));
        }

        public void WriteUInt32LE([NotNull] byte[] buffer, uint value) {
            Trace.Assert(buffer.Length >= 4);

            if (!BitConverter.IsLittleEndian) {
                value = SwapEndian(value);
            }

            var bytes = BitConverter.GetBytes(value);

            Array.Copy(bytes, buffer, bytes.Length);
        }

        public ulong ReadUInt64BE([NotNull] Stream stream) {
            stream.ReadExact(_readBuffer, 0, 8);

            var u = BitConverter.ToUInt64(_readBuffer, 0);

            if (BitConverter.IsLittleEndian) {
                u = SwapEndian(u);
            }

            return u;
        }

        public void WriteUInt64BE([NotNull] Stream stream, ulong value) {
            if (BitConverter.IsLittleEndian) {
                value = SwapEndian(value);
            }

            var bytes = BitConverter.GetBytes(value);

            stream.Write(bytes);
        }

        public ulong ReadUInt64LE([NotNull] Stream stream) {
            stream.ReadExact(_readBuffer, 0, 8);

            var u = BitConverter.ToUInt64(_readBuffer, 0);

            if (!BitConverter.IsLittleEndian) {
                u = SwapEndian(u);
            }

            return u;
        }

        public long ReadInt64BE([NotNull] Stream stream) {
            return unchecked((long)ReadUInt64BE(stream));
        }

        public void WriteInt64BE([NotNull] Stream stream, long value) {
            WriteUInt64BE(stream, unchecked((ulong)value));
        }

        public long ReadInt64LE([NotNull] Stream stream) {
            return unchecked((long)ReadUInt64LE(stream));
        }

        public void WriteInt64LE([NotNull] Stream stream, long value) {
            WriteUInt64LE(stream, unchecked((ulong)value));
        }

        public void WriteUInt64LE([NotNull] Stream stream, ulong value) {
            if (!BitConverter.IsLittleEndian) {
                value = SwapEndian(value);
            }

            var bytes = BitConverter.GetBytes(value);

            stream.Write(bytes);
        }

        public static ushort ReadUInt16LE([NotNull] byte[] span) {
            Trace.Assert(span.Length >= 2);

            var u = BitConverter.ToUInt16(span, 0);

            if (!BitConverter.IsLittleEndian) {
                u = SwapEndian(u);
            }

            return u;
        }

        public static void WriteUInt16LE([NotNull] byte[] span, ushort value, int offset) {
            Trace.Assert(span.Length >= 2 + offset);

            if (!BitConverter.IsLittleEndian) {
                value = SwapEndian(value);
            }

            var bytes = BitConverter.GetBytes(value);

            Array.Copy(bytes, 0, span, offset, bytes.Length);
        }

        public string ReadUtf8String([NotNull] Stream stream) {
            var length = ReadByte(stream);

            if (length == 0) {
                return string.Empty;
            }

            var buffer = new byte[length];

            stream.ReadExact(buffer, 0, length);

            var str = Utf8.Encoding.GetString(buffer);

            return str;
        }

        public void WriteUtf8String([NotNull] Stream stream, [NotNull] string str) {
            if (str is null) {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0) {
                WriteByte(stream, 0);
                return;
            }

            var bytes = Utf8.Encoding.GetBytes(str);

            if (bytes.Length > 0xff) {
                throw new ArgumentException("Strings with length longer than 255 bytes (in UTF-8 encoding) are not supported.", nameof(str));
            }

            WriteByte(stream, (byte)bytes.Length);

            stream.Write(bytes);
        }

        private static ushort SwapEndian(ushort value) {
            var v0 = value & 0x00ff;
            var v1 = value & 0xff00;

            return (ushort)((v1 >> 8) | (v0 << 8));
        }

        private static uint SwapEndian(uint value) {
            var v0 = value & 0x000000ff;
            var v1 = value & 0x0000ff00;
            var v2 = value & 0x00ff0000;
            var v3 = value & 0xff000000;

            return (v3 >> 24) | (v2 >> 8) | (v1 << 8) | (v0 << 24);
        }

        private static ulong SwapEndian(ulong value) {
            var v0 = value & 0x00000000000000ff;
            var v1 = value & 0x000000000000ff00;
            var v2 = value & 0x0000000000ff0000;
            var v3 = value & 0x00000000ff000000;
            var v4 = value & 0x000000ff00000000;
            var v5 = value & 0x0000ff0000000000;
            var v6 = value & 0x00ff000000000000;
            var v7 = value & 0xff00000000000000;

            return (v7 >> 56) | (v6 >> 40) | (v5 >> 24) | (v4 >> 8) | (v3 << 8) | (v2 << 24) | (v1 << 40) | (v0 << 56);
        }

        [AssertionMethod]
        [ContractAnnotation("false => halt")]
        private static void AssertSuccessful(
            [AssertionCondition(AssertionConditionType.IS_TRUE)]
            bool condition
        ) {
            if (!condition) {
                throw new ApplicationException();
            }
        }

        private readonly byte[] _readBuffer = new byte[8];
        private readonly byte[] _writeBuffer = new byte[8];

    }
}
