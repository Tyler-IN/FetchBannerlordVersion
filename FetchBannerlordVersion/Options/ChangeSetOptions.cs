using CommandLine;

namespace FetchBannerlordVersion.Options
{
    [Verb("getchangeset")]
    internal class ChangeSetOptions
    {
        [Option('f', "dir", Required = true)]
        public string Directory { get; set; } = default!;

        [Option('l', "lib", Required = false)]
        public string Library { get; set; } = "TaleWorlds.Library.dll";
    }
}