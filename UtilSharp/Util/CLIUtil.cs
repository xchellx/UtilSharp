/*
 * MIT License
 * 
 * Copyright (c) 2023 Yonder
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
using System.IO;

namespace UtilSharp.Util
{
    public static class CLIUtil
    {
        public static bool Ask(string message, TextWriter console, ConsoleKey wanted, string? choiceStr = null)
        {
            console.Write(message);
            if (choiceStr != null)
            {
                console.Write(' ');
                console.Write(choiceStr);
            }
            console.Write(' ');
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            console.WriteLine(keyInfo.KeyChar);
            return keyInfo.Key != wanted;
        }


        public static bool CheckFileArgument(string path, Func<string, string> preSanitizer,
            Func<string, string> postSanitizer, out string sanitizedPath, bool isOutput, bool forceOverwrite,
            TextWriter console, string failTypeMsg, string notFileMsg, string notExistMsg, string noOvrwrtMsg)
        {
            sanitizedPath = preSanitizer(path);

            IOUtil.FileType pathType;
            try
            {
                pathType = IOUtil.GetFileType(sanitizedPath);
            }
            catch (Exception e)
            {
                console.WriteLine(failTypeMsg, e);
                return true;
            }

            if (pathType == IOUtil.FileType.Directory)
            {
                console.WriteLine(notFileMsg);
                return true;
            }
            else if (pathType == IOUtil.FileType.NotExist)
            {
                if (!isOutput)
                {
                    console.WriteLine(notExistMsg);
                    return true;
                }
                else
                {
                    sanitizedPath = postSanitizer(sanitizedPath);
                    return false;
                }
            }
            else
            {
                if (isOutput && !forceOverwrite)
                {
                    if (Ask(notExistMsg, console, ConsoleKey.Y, "[y/any]"))
                    {
                        console.WriteLine(noOvrwrtMsg);
                        return true;
                    }
                }

                sanitizedPath = postSanitizer(sanitizedPath);
                return false;
            }
        }

        public static bool CheckDirectoryArgument(string path, Func<string, string> preSanitizer,
            Func<string, string> postSanitizer, out string sanitizedPath, bool isOutput, bool forceCreate,
            TextWriter console, string failTypeMsg, string notDirMsg, string notExistMsg, string noCreateMsg,
            string failCreateMsg)
        {
            sanitizedPath = preSanitizer(path);

            IOUtil.FileType pathType;
            try
            {
                pathType = IOUtil.GetFileType(sanitizedPath);
            }
            catch (Exception e)
            {
                console.WriteLine(failTypeMsg, e);
                return true;
            }

            if (pathType == IOUtil.FileType.File)
            {
                console.WriteLine(notDirMsg);
                return true;
            }
            else if (pathType == IOUtil.FileType.NotExist)
            {
                if (isOutput)
                {
                    if (!forceCreate)
                    {
                        if (Ask(notExistMsg, console, ConsoleKey.Y, "[y/any]"))
                        {
                            console.WriteLine(noCreateMsg);
                            return true;
                        }
                    }

                    try
                    {
                        Directory.CreateDirectory(sanitizedPath);
                    }
                    catch (Exception e)
                    {
                        console.WriteLine(failCreateMsg, e);
                        return true;
                    }

                    sanitizedPath = postSanitizer(sanitizedPath);
                    return false;
                }
                else
                {
                    console.WriteLine(notExistMsg);
                    return true;
                }
            }
            else
            {
                sanitizedPath = postSanitizer(sanitizedPath);
                return false;
            }
        }
    }
}
