import test from 'ava';

import { FetchBannerlordVersion } from '../lib/FetchBannerlordVersion';

test('sort', async (t) => {
  const fetcher = await FetchBannerlordVersion.createAsync();

  const path = __dirname;
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
});