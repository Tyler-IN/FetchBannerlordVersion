namespace FetchBannerlordVersion.DependencyInjection;

public class FetchBannerlordVersion : IFetchBannerlordVersion
{
    public int GetChangeSet(string gameFolderPath, string libAssembly) => Fetcher.GetChangeSet(gameFolderPath, libAssembly);

    public string GetVersion(string gameFolderPath, string libAssembly) => Fetcher.GetVersion(gameFolderPath, libAssembly);

    public VersionType GetVersionType(string gameFolderPath, string libAssembly) => Fetcher.GetVersionType(gameFolderPath, libAssembly);
}