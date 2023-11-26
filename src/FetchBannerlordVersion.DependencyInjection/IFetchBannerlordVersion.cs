namespace FetchBannerlordVersion.DependencyInjection;

public interface IFetchBannerlordVersion
{
    int GetChangeSet(string gameFolderPath, string libAssembly);
    string GetVersion(string gameFolderPath, string libAssembly);
    VersionType GetVersionType(string gameFolderPath, string libAssembly);
}