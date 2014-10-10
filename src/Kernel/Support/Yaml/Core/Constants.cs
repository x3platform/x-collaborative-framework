//  This file is part of X3Platform.Yaml - A .NET library for YAML.
//  Copyright (c) 2008, 2009, 2010, 2011, 2012, 2013 Antoine Aubry and contributors
    
//  Permission is hereby granted, free of charge, to any person obtaining a copy of
//  this software and associated documentation files (the "Software"), to deal in
//  the Software without restriction, including without limitation the rights to
//  use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//  of the Software, and to permit persons to whom the Software is furnished to do
//  so, subject to the following conditions:
    
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
    
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.

using System;
using X3Platform.Yaml.Core.Tokens;

namespace X3Platform.Yaml.Core
{
	/// <summary>
	/// Defines constants thar relate to the YAML specification.
	/// </summary>
	internal static class Constants
	{
		public static readonly TagDirective[] DefaultTagDirectives = new[]
		{
			new TagDirective("!", "!"),
			new TagDirective("!!", "tag:yaml.org,2002:"),
		};
		
		public const int MajorVersion = 1;
		public const int MinorVersion = 1;
		
		public const char HandleCharacter = '!';
		public const string DefaultHandle = "!";
	}
}