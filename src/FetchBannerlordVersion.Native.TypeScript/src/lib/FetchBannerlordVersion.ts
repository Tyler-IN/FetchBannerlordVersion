import { IFetchBannerlordVersion } from "./types";

const fetcher: IFetchBannerlordVersion = require('./../../fetchblversion.node');

export class FetchBannerlordVersion {
  private constructor() { }

  static getChangeSet(gameFolderPath: string, libAssembly: string): number {
    return fetcher.getChangeSet(gameFolderPath, libAssembly);
  }

  static getVersion(gameFolderPath: string, libAssembly: string): string {
    return fetcher.getVersion(gameFolderPath, libAssembly);
  }

  static getVersionType(gameFolderPath: string, libAssembly: string): number {
    return fetcher.getVersionType(gameFolderPath, libAssembly);
  }
}
