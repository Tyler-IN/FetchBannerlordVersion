const test = require("ava");

const { Bannerlord, boot, terminate } = require("../lib/dotnet");

test.beforeEach("init", async (t) => {
  await boot();
});

test.afterEach.always("cleanup", (t) => {
  terminate();
});

test("api available", async (t) => {
  if ("GetVersion" in Bannerlord.ModuleManager === false) {
    t.fail();
  }

  if ("GetVersionType" in Bannerlord.ModuleManager === false) {
    t.fail();
  }

  if ("GetChangeSet" in Bannerlord.ModuleManager === false) {
    t.fail();
  }

  t.pass();
});
