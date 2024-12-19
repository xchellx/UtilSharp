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

namespace UtilSharp.IO
{
    /// <summary>
    /// Purpose: Reserves overloads that may clash<br/>
    /// AW = <see cref="AdvancedBinaryWriter"/><br/>
    /// CS = Write CString<br/>
    /// Usage: <see cref="AW_CS._"/>
    /// </summary>
    public enum AW_CS { _ }

    /// <summary>
    /// Purpose: Reserves overloads that may clash<br/>
    /// AW = <see cref="AdvancedBinaryWriter"/><br/>
    /// FC = Write FourCC<br/>
    /// Usage: <see cref="AW_FC._"/>
    /// </summary>
    public enum AW_FC { _ }
}
