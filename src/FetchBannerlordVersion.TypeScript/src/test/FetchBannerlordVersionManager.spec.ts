import test from 'ava';

import { FetchBannerlordVersionManager } from '../lib/FetchBannerlordVersionManager';

test('Main', async (t) => {
  const fetcher = await FetchBannerlordVersionManager.createAsync();

  const path = "./../test/";
  const dllName = 'TaleWorlds.Library.dll';

  const changeSet = fetcher.getChangeSet(path, dllName);
  t.is(changeSet, 321460);
  
  const version = fetcher.getVersion(path, dllName);
  t.is(version, 'e1.8.0');

  const versionType = fetcher.getVersionType(path, dllName);
  t.is(versionType, 4);

  await fetcher.dispose();

  t.pass();
});