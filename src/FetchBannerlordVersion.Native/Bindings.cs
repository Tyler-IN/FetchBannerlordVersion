using System;
using System.Runtime.InteropServices;

namespace FetchBannerlordVersion.Native
{
    public class Bindings
    {
        [UnmanagedCallersOnly(EntryPoint = "get_change_set")]
        public static int GetChangeSet(IntPtr p_game_folder_path, IntPtr p_lib_assembly)
        {
            try
            {
                var gameFolderPath = Utils.UnescapeNonASCII(Marshal.PtrToStringAnsi(p_game_folder_path));
                var libAssembly = Utils.UnescapeNonASCII(Marshal.PtrToStringAnsi(p_lib_assembly));

                return Fetcher.GetChangeSet(gameFolderPath, libAssembly);
            }
            catch
            {
                return -1;
            }
        }

        [UnmanagedCallersOnly(EntryPoint = "get_version")]
        public static IntPtr GetVersion(IntPtr p_game_folder_path, IntPtr p_lib_assembly)
        {
            try
            {
                var gameFolderPath = Utils.UnescapeNonASCII(Marshal.PtrToStringAnsi(p_game_folder_path));
                var libAssembly = Utils.UnescapeNonASCII(Marshal.PtrToStringAnsi(p_lib_assembly));

                var version = Fetcher.GetVersion(gameFolderPath, libAssembly);
                return Marshal.StringToHGlobalAnsi(version);
            }
            catch
            {
                return IntPtr.Zero;
            }
        }

        [UnmanagedCallersOnly(EntryPoint = "get_version_type")]
        public static int GetVersionType(IntPtr p_game_folder_path, IntPtr p_lib_assembly)
        {
            try
            {
                var gameFolderPath = Utils.UnescapeNonASCII(Marshal.PtrToStringAnsi(p_game_folder_path));
                var libAssembly = Utils.UnescapeNonASCII(Marshal.PtrToStringAnsi(p_lib_assembly));

                return (int) Fetcher.GetVersionType(gameFolderPath, libAssembly);
            }
            catch
            {
                return -1;
            }
        }
    }
}