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
    public class EndianBinaryWriter(Stream input, bool? isLittleEndian = null, Encoding? encoding = null,
        bool leaveOpen = false) : AdvancedBinaryWriter(input, encoding ?? IOUtil.DefaultEncoding, leaveOpen)
    {
        public bool IsLittleEndian { get; set; } = isLittleEndian ?? IOUtil.DefaultIsLittleEndian;

        private void WriteForEndianness(byte[] buffer, bool isLittleEndian)
        {
            if (isLittleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
            Write(buffer);
        }

        public override void Write(bool value) => Write(value, IsLittleEndian);

        public override void Write(ushort value) => Write(value, IsLittleEndian);

        public override void Write(short value) => Write(value, IsLittleEndian);

        public override void Write(uint value) => Write(value, IsLittleEndian);

        public override void Write(int value) => Write(value, IsLittleEndian);

        public override void Write(ulong value) => Write(value, IsLittleEndian);

        public override void Write(long value) => Write(value, IsLittleEndian);

        public override void Write(float value) => Write(value, IsLittleEndian);

        public override void Write(double value) => Write(value, IsLittleEndian);

        public void Write(bool value, bool isLittleEndian) => WriteForEndianness(BitConverter.GetBytes(value),
            isLittleEndian);

        public void Write(ushort value, bool isLittleEndian) => WriteForEndianness(BitConverter.GetBytes(value),
            isLittleEndian);

        public void Write(short value, bool isLittleEndian) => WriteForEndianness(BitConverter.GetBytes(value),
            isLittleEndian);

        public void Write(uint value, bool isLittleEndian) => WriteForEndianness(BitConverter.GetBytes(value),
            isLittleEndian);

        public void Write(int value, bool isLittleEndian) => WriteForEndianness(BitConverter.GetBytes(value),
            isLittleEndian);

        public void Write(ulong value, bool isLittleEndian) => WriteForEndianness(BitConverter.GetBytes(value),
            isLittleEndian);

        public void Write(long value, bool isLittleEndian) => WriteForEndianness(BitConverter.GetBytes(value),
            isLittleEndian);

        public void Write(float value, bool isLittleEndian) => WriteForEndianness(BitConverter.GetBytes(value),
            isLittleEndian);

        public void Write(double value, bool isLittleEndian) => WriteForEndianness(BitConverter.GetBytes(value),
            isLittleEndian);
    }
}
