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
using System.Linq;
using System.Text;

namespace UtilSharp.IO
{
    public class AdvancedBinaryWriter : BinaryWriter
    {
        public Encoding Encoding { get; } = IOUtil.DefaultEncoding;

        public bool LeaveOpen { get; } = false;

        public bool CanSeek => BaseStream.CanSeek;

        public bool CanRead => BaseStream.CanRead;

        public bool CanWrite => BaseStream.CanWrite;

        public long Length
        {
            get => CanSeek ? BaseStream.Length : throw new IOException("Stream is not seekable");
            set
            {
                if (CanSeek && CanWrite)
                    BaseStream.SetLength(value);
                else
                    throw new IOException("Stream is not seekable and not writable");
            }
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

        public AdvancedBinaryWriter(Stream input) : base(input, IOUtil.DefaultEncoding)
        {
        }

        public AdvancedBinaryWriter(Stream input, Encoding encoding) : base(input, encoding)
        {
            Encoding = encoding;
        }

        public AdvancedBinaryWriter(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
            Encoding = encoding;
            LeaveOpen = true;
        }

        public long Seek(long offset, SeekOrigin origin) => CanSeek
            ? BaseStream.Seek(offset, origin)
            : throw new IOException("Stream is not seekable");

        public long Skip(long offset) => Seek(offset, SeekOrigin.Current);

        public void Write(string str, AW_CS _, bool nullTerm = true)
        {
            Write(Encoding.GetBytes(str, 0, str.Length));
            if (nullTerm)
                Write(IOUtil.GetNullCharBytes(Encoding));
        }

        public long Align(long bits)
        {
            long pad = Position.Pad(bits);
            if (Position + pad > Length)
                Write(Enumerable.Repeat((byte) 0, (int) ((Position + pad) - Length)).ToArray());
            return Position;
        }

        public void Write(string str, AW_FC _)
        {
            if (str?.Length == 4)
                Write(Encoding.ASCII.GetBytes(str));
            else
                throw new ArgumentException("Must be of length 4", nameof(str));
        }
    }
}
