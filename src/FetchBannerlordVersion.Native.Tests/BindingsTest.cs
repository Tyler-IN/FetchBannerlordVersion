using NUnit.Framework;

using System;
using System.Runtime.InteropServices;

namespace FetchBannerlordVersion.Native.Tests
{
    public class BindingsTest
    {
        private const string DllPath = "..\\..\\..\\..\\FetchBannerlordVersion.Native\\bin\\Release\\net7.0\\win-x64\\native\\FetchBannerlordVersion.Native.dll";

        [DllImport(DllPath, EntryPoint = "get_change_set")]
        private static extern int GetChangeSet(IntPtr p_game_folder_path, IntPtr p_lib_assembly);

        [DllImport(DllPath, EntryPoint = "get_version")]
        private static extern IntPtr GetVersion(IntPtr p_game_folder_path, IntPtr p_lib_assembly);

        [DllImport(DllPath, EntryPoint = "get_version_type")]
        private static extern int GetVersionType(IntPtr p_game_folder_path, IntPtr p_lib_assembly);

        [Test]
        public void Test()
        {
            const string path = "./Data";
            const string dllName = "TaleWorlds.Library.dll";

            var escapedPath = Utils.EscapeNonASCII(path);
            var escapedDllName = Utils.EscapeNonASCII(dllName);
            var pPath = Marshal.StringToHGlobalAnsi(escapedPath);
            var pDllName = Marshal.StringToHGlobalAnsi(escapedDllName);

            var changeSet = GetChangeSet(pPath, pDllName);
            Assert.AreEqual(321460, changeSet);

            var pVersion = GetVersion(pPath, pDllName);
            var versionUnescaped = Marshal.PtrToStringAnsi(pVersion);
            var version = Utils.UnescapeNonASCII(versionUnescaped);
            Assert.AreEqual("e1.8.0", version);

            var versionType = GetVersionType(pPath, pDllName);
            Assert.AreEqual(4, versionType);
        }
    }
}