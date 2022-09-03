import test from 'ava';

import { FetchBannerlordVersionManager } from '../lib/FetchBannerlordVersionManager';

test('sort', async (t) => {
  const fetcher = await FetchBannerlordVersionManager.createAsync();

  const path = "./../test/";
  const dllName = 'TaleWorlds.Library.dll';

  const version = fetcher.getVersion(path, dllName);
  if (version !== 'e1.8.0') {
    t.fail();
    return;
  }

  const versionType = fetcher.getVersionType(path, dllName);
  if (versionType !== 4) {
    t.fail();
    return;
  }

  const changeSet = fetcher.getChangeSet(path, dllName);
  if (changeSet !== 321460) {
    t.fail();
    return;
  }

  await fetcher.dispose();

  t.pass();

});