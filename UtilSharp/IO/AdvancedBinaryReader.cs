/*
 * MIT License
 * 
 * Copyright (c) 2024 Yonder
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using UtilSharp.Extensions;
using UtilSharp.Util;
using System;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Text;

namespace UtilSharp.IO
{
    public class AdvancedBinaryReader : BinaryReader
    {
        public Encoding Encoding { get; } = IOUtil.DefaultEncoding;

        public bool LeaveOpen { get; } = false;

        public bool CanSeek => BaseStream.CanSeek;

        public bool CanRead => BaseStream.CanRead;

        public bool CanWrite => BaseStream.CanWrite;

        public long Length {
            get => CanSeek ? BaseStream.Length : throw new IOException("Stream is not seekable");
            set => throw new IOException("Stream is not writable");
        }

        public long Position
        {
            get => CanSeek ? BaseStream.Position : throw new IOException("Stream is not seekable");
            set
            {
                if (CanSeek)
                    BaseStream.Position = value;
                else
                    throw new IOException("Stream is not seekable");
            }
        }

        public AdvancedBinaryReader(Stream input) : base(input, IOUtil.DefaultEncoding)
        {
        }

        public AdvancedBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
        {
            Encoding = encoding;
        }

        public AdvancedBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
            Encoding = encoding;
            LeaveOpen = true;
        }

        public long Seek(long offset, SeekOrigin origin) => CanSeek
            ? BaseStream.Seek(offset, origin)
            : throw new IOException("Stream is not seekable");

        public override byte[] ReadBytes(int count)
        {
            byte[] bytesRead = base.ReadBytes(count);
            if (bytesRead.Length != 0)
                return bytesRead;
            else
                throw new EndOfStreamException();
        }

        public long Skip(long offset) => Seek(offset, SeekOrigin.Current);

        public string ReadEncodedCharacter()
        {
            char[] cdat = ReadChars(1);
            return new string(cdat);
        }

        public string ReadCString(bool seekBackOnNull = false) => ReadCString(true, seekBackOnNull, int.MaxValue, 0);

        public string ReadCString(bool nullTerm, bool seekBackOnNull = false, int length = int.MaxValue, int byteLen = 0)
        {
            if (seekBackOnNull && !CanSeek)
                throw new IOException("Stream is not seekable");
            else if (length < 0)
                throw new ArgumentException("Invalid string length specified", nameof(length));
            else if (!nullTerm && (byteLen == 0 || length == int.MaxValue))
                throw new ArgumentException("Must specify a length or byte length for a non-null terminated string",
                    nameof(length));
            else if (length == 0 && byteLen == 0)
                return string.Empty;
            else
            {
                string nullChar = IOUtil.GetNullChar(Encoding);

                StringBuilder str = new();
                long byteEnd = byteLen != 0 ? Position + byteLen : 0;
                while (str.Length != length)
                {
                    string c = ReadEncodedCharacter();
                    if (byteLen != 0 && Position > byteEnd)
                        throw new EndOfStreamException($"Read past expected end of byte boundry {byteEnd} for char");
                    if ((nullTerm && c == nullChar) || (byteLen != 0 && Position == byteEnd))
                        break;
                    else
                        str.Append(c);
                }
                if (seekBackOnNull)
                    Seek(-1, SeekOrigin.Current);

                return str.ToString();
            }
        }

        public long Align(long bits)
        {
            long aPos = Position.Align(bits);
            if (aPos > Length)
                throw new EndOfStreamException();
            Position = aPos;
            return aPos;
        }

        public string ReadFourCC() => Encoding.ASCII.GetString(ReadBytes(4));
    }
}
