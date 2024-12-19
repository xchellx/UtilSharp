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

using UtilSharp.Util;
using System;
using System.IO;
using System.Text;

namespace UtilSharp.IO
{
    public class EndianBinaryReader(Stream input, bool? isLittleEndian = null, Encoding? encoding = null,
        bool leaveOpen = false) : AdvancedBinaryReader(input, encoding ?? IOUtil.DefaultEncoding, leaveOpen)
    {
        public bool IsLittleEndian { get; set; } = isLittleEndian ?? IOUtil.DefaultIsLittleEndian;

        private byte[] ReadForEndianness(int bytesToRead, bool isLittleEndian)
        {
            byte[] bytesRead = ReadBytes(bytesToRead);
            if (isLittleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(bytesRead);
            return bytesRead;
        }

        public override bool ReadBoolean() => ReadBoolean(IsLittleEndian);

        public override ushort ReadUInt16() => ReadUInt16(IsLittleEndian);

        public override short ReadInt16() => ReadInt16(IsLittleEndian);

        public override uint ReadUInt32() => ReadUInt32(IsLittleEndian);

        public override int ReadInt32() => ReadInt32(IsLittleEndian);

        public override ulong ReadUInt64() => ReadUInt64(IsLittleEndian);

        public override long ReadInt64() => ReadInt64(IsLittleEndian);

        public override float ReadSingle() => ReadSingle(IsLittleEndian);

        public override double ReadDouble() => ReadDouble(IsLittleEndian);

        public bool ReadBoolean(bool isLittleEndian) => BitConverter.ToBoolean(ReadForEndianness(sizeof(bool),
            isLittleEndian), 0);

        public ushort ReadUInt16(bool isLittleEndian) => BitConverter.ToUInt16(ReadForEndianness(sizeof(ushort),
            isLittleEndian), 0);

        public short ReadInt16(bool isLittleEndian) => BitConverter.ToInt16(ReadForEndianness(sizeof(short),
            isLittleEndian), 0);

        public uint ReadUInt32(bool isLittleEndian) => BitConverter.ToUInt32(ReadForEndianness(sizeof(uint),
            isLittleEndian), 0);

        public int ReadInt32(bool isLittleEndian) => BitConverter.ToInt32(ReadForEndianness(sizeof(int),
            isLittleEndian), 0);

        public ulong ReadUInt64(bool isLittleEndian) => BitConverter.ToUInt64(ReadForEndianness(sizeof(ulong),
            isLittleEndian), 0);

        public long ReadInt64(bool isLittleEndian) => BitConverter.ToInt64(ReadForEndianness(sizeof(long),
            isLittleEndian), 0);

        public float ReadSingle(bool isLittleEndian) => BitConverter.ToSingle(ReadForEndianness(sizeof(float),
            isLittleEndian), 0);

        public double ReadDouble(bool isLittleEndian) => BitConverter.ToDouble(ReadForEndianness(sizeof(double),
            isLittleEndian), 0);
    }
}
