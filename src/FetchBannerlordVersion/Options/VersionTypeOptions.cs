using CommandLine;

namespace FetchBannerlordVersion.Options
{
    [Verb("getversiontype")]
    internal class VersionTypeOptions
    {
        [Option('f', "dir", Required = true)]
        public string Directory { get; set; } = default!;

        [Option('l', "lib", Required = false)]
        public string Library { get; set; } = "TaleWorlds.Library.dll";
    }
}