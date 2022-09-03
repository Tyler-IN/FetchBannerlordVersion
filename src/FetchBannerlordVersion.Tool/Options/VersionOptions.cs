using CommandLine;

namespace FetchBannerlordVersion.Options
{
    [Verb("getversion")]
    internal sealed record VersionOptions
    {
        [Option('f', "dir", Required = true)]
        public string Directory { get; init; } = default!;

        [Option('l', "lib", Required = false)]
        public string Library { get; init; } = "TaleWorlds.Library.dll";
    }
}