using CommandLine;

namespace FetchBannerlordVersion.Options
{
    [Verb("getchangeset")]
    internal class ChangeSetOptions
    {
        [Option('f', "dir", Required = true)]
        public string Directory { get; set; } = default!;
    }
}