using DotNetJS;

using Microsoft.JSInterop;

[assembly: JSNamespace("FetchBannerlordVersion.WASM", "FetchBannerlordVersion")]

namespace FetchBannerlordVersion.WASM
{
    public static class Program
    {
        public static void Main() { }

        [JSInvokable]
        public static int GetChangeSet(string gameFolderPath, string libAssembly) =>
            Fetcher.GetChangeSet(gameFolderPath, libAssembly);

        [JSInvokable]
        public static string GetVersion(string gameFolderPath, string libAssembly) =>
            Fetcher.GetVersion(gameFolderPath, libAssembly);

        [JSInvokable]
        public static VersionType GetVersionType(string gameFolderPath, string libAssembly) =>
            Fetcher.GetVersionType(gameFolderPath, libAssembly);
    }
}