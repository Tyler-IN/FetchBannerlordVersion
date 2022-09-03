import { IFetchBannerlordVersion } from "./types";

const fetcher: IFetchBannerlordVersion = require('./../../fetchblversion.node');

export class FetchBannerlordVersion implements IFetchBannerlordVersion {
    static async createAsync(): Promise<FetchBannerlordVersion> {
        const lib = new FetchBannerlordVersion();
        return lib;
    }

    private constructor() { }

    getChangeSet(gameFolderPath: string, libAssembly: string): number {
      return fetcher.getChangeSet(gameFolderPath, libAssembly);
    }

    getVersion(gameFolderPath: string, libAssembly: string): string {
      return fetcher.getVersion(gameFolderPath, libAssembly);
    }

    getVersionType(gameFolderPath: string, libAssembly: string): number {
      return fetcher.getVersionType(gameFolderPath, libAssembly);
    }

    async dispose(): Promise<void> {
    }
}
