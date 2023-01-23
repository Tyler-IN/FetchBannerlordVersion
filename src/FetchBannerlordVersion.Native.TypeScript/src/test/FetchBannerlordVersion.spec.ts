import test from 'ava';

import { FetchBannerlordVersion } from '../lib/FetchBannerlordVersion';

test('sort', async (t) => {
  const path = __dirname;
  const dllName = 'TaleWorlds.Library.dll';

  const version = FetchBannerlordVersion.getVersion(path, dllName);
  if (version !== 'e1.8.0') {
    t.fail();
    return;
  }

  const versionType = FetchBannerlordVersion.getVersionType(path, dllName);
  if (versionType !== 4) {
    t.fail();
    return;
  }

  const changeSet = FetchBannerlordVersion.getChangeSet(path, dllName);
  if (changeSet !== 321460) {
    t.fail();
    return;
  }

  t.pass();
});