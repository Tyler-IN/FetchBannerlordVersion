import { boot, BootStatus, getBootStatus, terminate } from "./dotnet";
import dotnet from "./dotnet";
import VersionType = dotnet.FetchBannerlordVersion.VersionType

export class FetchBannerlordVersionManager {
    static async createAsync(): Promise<FetchBannerlordVersionManager> {
        const lib = new FetchBannerlordVersionManager();
        await lib.init();
        return lib;
    }

    private constructor() { }

    private async init(): Promise<void> {
        const status = getBootStatus();
        if (status == BootStatus.Standby) {
            await boot();
        }
    }

    getChangeSet(gameFolderPath: string, libAssembly: string): number {
        return dotnet.FetchBannerlordVersion.getChangeSet(gameFolderPath, libAssembly);
    }

    getVersion(gameFolderPath: string, libAssembly: string): string {
        return dotnet.FetchBannerlordVersion.getVersion(gameFolderPath, libAssembly);
    }

    getVersionType(gameFolderPath: string, libAssembly: string): VersionType {
        return dotnet.FetchBannerlordVersion.getVersionType(gameFolderPath, libAssembly);
    }

    async dispose(): Promise<void> {
        await terminate();
    }
}
