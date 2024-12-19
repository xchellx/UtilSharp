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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UtilSharp.Util
{
    public static class IOUtil
    {
        // System.Text.Encoding.CodePages is required via Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
        public static Encoding DefaultEncoding => Encoding.UTF8;

        // System.Text.Encoding.CodePages is required via Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
        public static Encoding Windows1252 { get; } = Encoding.GetEncoding(1252);

        public static Encoding ShiftJIS { get; } = Encoding.GetEncoding(932);

        public static bool DefaultIsLittleEndian => BitConverter.IsLittleEndian;

        public static string Mask { get; } = "*";

        public static EnumerationOptions EnumOpts { get; } = new()
        {
            AttributesToSkip = FileAttributes.System | FileAttributes.Encrypted | FileAttributes.Offline,
            IgnoreInaccessible = true,
            MatchCasing = MatchCasing.CaseInsensitive,
            MatchType = MatchType.Simple,
            RecurseSubdirectories = false,
            ReturnSpecialDirectories = false
        };

        public static EnumerationOptions EnumOptsRecur { get; } = new()
        {
            AttributesToSkip = FileAttributes.System | FileAttributes.Encrypted | FileAttributes.Offline,
            IgnoreInaccessible = true,
            MatchCasing = MatchCasing.CaseInsensitive,
            MatchType = MatchType.Simple,
            RecurseSubdirectories = true,
            ReturnSpecialDirectories = false
        };

        public static FileType GetFileType(FileSystemInfo path)
        {
            try
            {
                return GetFileType(path.Attributes);
            }
            catch (FileNotFoundException)
            {
                return FileType.NotExist;
            }
            catch (DirectoryNotFoundException)
            {
                return FileType.NotExist;
            }
        }

        public static FileType GetFileType(string path)
        {
            try
            {
                return GetFileType(File.GetAttributes(path));
            }
            catch (FileNotFoundException)
            {
                return FileType.NotExist;
            }
            catch (DirectoryNotFoundException)
            {
                return FileType.NotExist;
            }
        }

        public static FileType GetFileType(FileAttributes attributes)
        {
            try
            {
                return (attributes & FileAttributes.Directory) == FileAttributes.Directory
                    ? FileType.Directory
                    : FileType.File;
            }
            catch (FileNotFoundException)
            {
                return FileType.NotExist;
            }
            catch (DirectoryNotFoundException)
            {
                return FileType.NotExist;
            }
        }

        public static string? GetDirectoryName(string? path)
        {
            if (path == null || path.Length == 0)
                return null;

            string? fullPath = Path.GetFullPath(path);
            return Path.GetDirectoryName(fullPath) ?? Path.GetPathRoot(fullPath);
        }

        public static string? GetDirectoryName(FileInfo path)
        {
            if (path == null || path.Length == 0)
                return null;

            return path.Directory?.FullName ?? Path.GetPathRoot(path.FullName);
        }

        public static DirectoryInfo? GetDirectory(string path) => GetDirectoryName(path) is string s ? new(s) : null;

        public static DirectoryInfo? GetDirectory(FileInfo path) => GetDirectoryName(path) is string s ? new(s) : null;

        private static readonly Dictionary<Encoding, string?> getNullCharCache = [];

        private static readonly byte[] nullCharASCII = [0x00];

        public static byte[] GetNullCharBytes(Encoding enc)
        {
            string nullChar = GetNullChar(enc);
            return enc.GetBytes(nullChar, 0, nullChar.Length);
        }

        public static string GetNullChar(Encoding enc)
        {
            string? nullChar = getNullCharCache.GetValueOrDefault(enc);
            if (nullChar == null)
            {
                nullChar = enc.GetString(Encoding.Convert(Encoding.ASCII, enc, nullCharASCII, 0, 1));
                getNullCharCache.TryAdd(enc, nullChar);
            }
            return nullChar;
        }

        public static void WalkDirectory(string rootDir, VisitorDel<string, string> visitor)
            => WalkDirectoryCore(new DirectoryInfo(rootDir), visitor);

        public static void WalkDirectory(string rootDir, VisitorDel<DirectoryInfo, FileInfo> visitor)
            => WalkDirectoryCore(new DirectoryInfo(rootDir), visitor);

        public static void WalkDirectory(DirectoryInfo rootDir, VisitorDel<string, string> visitor)
            => WalkDirectoryCore(rootDir, visitor);

        public static void WalkDirectory(DirectoryInfo rootDir, VisitorDel<DirectoryInfo, FileInfo> visitor)
            => WalkDirectoryCore(rootDir, visitor);

        private static void WalkDirectoryCore<D, F>(DirectoryInfo rootDir, VisitorDel<D, F> visitor)
            where D : class
            where F : class
        {
            if (typeof(D) != typeof(string) && !typeof(FileSystemInfo).IsAssignableFrom(typeof(D)))
                throw new ArgumentException(
                    $"Generic type is not of {typeof(string).Name} nor {typeof(FileSystemInfo).Name}", nameof(D));
            else if (typeof(F) != typeof(string) && !typeof(FileSystemInfo).IsAssignableFrom(typeof(F)))
                throw new ArgumentException(
                    $"Generic type is not of {typeof(string).Name} nor {typeof(FileSystemInfo).Name}", nameof(F));

            //var fsCompare = Comparer<FileSystemInfo>.Create((x, y) => x.FullName.CompareTo(y.FullName));
            Stack<DirectoryInfo> dirStack = new();
            dirStack.Push(rootDir);
            Queue<VisitorQueueable<D, F>> visitorQueue = new();
            while (dirStack.TryPop(out DirectoryInfo? dir))
            {
                foreach (DirectoryInfo subDir in dir.EnumerateDirectories(Mask, EnumOpts)/*.Order(fsCompare)*/)
                    dirStack.Push(subDir);

                foreach (FileInfo file in dir.EnumerateFiles(Mask, EnumOpts)/*.Order(fsCompare)*/)
                    visitorQueue.Enqueue(new VisitorQueueable<D, F>(dir, file, visitor));
            }

            while (visitorQueue.TryDequeue(out VisitorQueueable<D, F>? visitorQueueable))
                visitorQueueable.Invoke();
        }

        public enum FileType : int
        {
            NotExist,
            File,
            Directory
        }

        public delegate void VisitorDel<D, F>(D dir, F file);

        private class VisitorQueueable<D, F>
            where D : class
            where F : class
        {
            private readonly DirectoryInfo dir;
            private readonly FileInfo file;
            private readonly VisitorDel<D, F> visitor;

            internal VisitorQueueable(DirectoryInfo dir, FileInfo file, VisitorDel<D, F> visitor)
            {
                if (typeof(D) != typeof(string) && !typeof(FileSystemInfo).IsAssignableFrom(typeof(D)))
                    throw new ArgumentException(
                        $"Generic type is not of {typeof(string).Name} nor {typeof(FileSystemInfo).Name}", nameof(D));
                else if (typeof(F) != typeof(string) && !typeof(FileSystemInfo).IsAssignableFrom(typeof(F)))
                    throw new ArgumentException(
                        $"Generic type is not of {typeof(string).Name} nor {typeof(FileSystemInfo).Name}", nameof(F));

                this.dir = dir;
                this.file = file;
                this.visitor = visitor;
            }

            internal void Invoke() => visitor.Invoke(DResult(dir), FResult(file));

            private static D DResult(FileSystemInfo e) => (D) (object) (typeof(D) == typeof(string) ? e.FullName : e);

            private static F FResult(FileSystemInfo e) => (F) (object) (typeof(F) == typeof(string) ? e.FullName : e);
        }
    }
}
