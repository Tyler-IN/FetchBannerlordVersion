using DotNetJS;

using Microsoft.JSInterop;

using System.IO;

[assembly: JSNamespace("FetchBannerlordVersion.WASM", "FetchBannerlordVersion")]

namespace FetchBannerlordVersion.WASM
{
    public static class Program
    {
        public static void Main() { }

        [JSInvokable]
        public static int GetChangeSet(string gameFolderPath, string libAssembly) =>
            Fetcher.GetChangeSet(Path.GetFullPath(gameFolderPath), libAssembly);

        [JSInvokable]
        public static string GetVersion(string gameFolderPath, string libAssembly) =>
            Fetcher.GetVersion(Path.GetFullPath(gameFolderPath), libAssembly);

        [JSInvokable]
        public static VersionType GetVersionType(string gameFolderPath, string libAssembly) =>
            Fetcher.GetVersionType(Path.GetFullPath(gameFolderPath), libAssembly);
    }
}