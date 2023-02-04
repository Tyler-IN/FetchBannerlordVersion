import { IFetchBannerlordVersion } from "./types";

const fetcher: IFetchBannerlordVersion & { allocAliveCount(): number; } = require('./../../fetchblversion.node');

export const allocAliveCount = (): number => {
  return fetcher.allocAliveCount();
}

export class FetchBannerlordVersion {
  /* istanbul ignore next */
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
