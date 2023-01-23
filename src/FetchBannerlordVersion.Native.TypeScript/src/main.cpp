#include "Bannerlord.FetchVersion.h"
#include <napi.h>

using namespace Napi;
using namespace Bannerlord::FetchVersion;

Object InitAll(const Env env, const Object exports)
{
  return Init(env, exports);
}

NODE_API_MODULE(NODE_GYP_MODULE_NAME, InitAll)
