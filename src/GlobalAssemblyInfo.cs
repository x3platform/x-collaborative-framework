// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// Filename     :GlobalAssemblyInfo.cs
//
// Abstract     :GlobalAssemblyInfo 全局信息
//
// Author       :Elane
//
// Date			:2010-01-01
//
// =============================================================================

using System;
using System.Reflection;
using System.Runtime.InteropServices;

// 禁止编译器警告 CS1699
#pragma warning disable 1699

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: CLSCompliant(true)]

[assembly: AssemblyProduct("X3 Platform Suite")]
[assembly: AssemblyCompany("X3 Platform")]

[assembly: AssemblyCopyright("Copyright (C) X3Platform")]
[assembly: AssemblyTrademark("X3Platform")]

#if DEBUG
[assembly: AssemblyFileVersion("2.0.0.0")]
#else
[assembly: AssemblyFileVersion("2.0.0.0")]
#endif

#if NONSTRONGNAME
[assembly: AssemblyKeyFile("")]
#else
[assembly: AssemblyKeyFile("..\\..\\Licence.snk")]
#endif

[assembly: AssemblyKeyName("")]