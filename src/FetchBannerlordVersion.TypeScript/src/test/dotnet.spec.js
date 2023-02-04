const test = require("ava");

const { FetchBannerlordVersion, boot, terminate } = require("../lib/dotnet");

test.beforeEach("init", async (t) => {
  await boot();
});

test.afterEach.always("cleanup", (t) => {
  terminate();
});

test("api available", async (t) => {
  if ("getVersion" in FetchBannerlordVersion === false) {
    t.fail();
  }

  if ("getVersionType" in FetchBannerlordVersion === false) {
    t.fail();
  }

  if ("getChangeSet" in FetchBannerlordVersion === false) {
    t.fail();
  }

  t.pass();
});
