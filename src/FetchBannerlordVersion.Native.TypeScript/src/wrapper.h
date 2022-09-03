#ifndef FETCHBLVERSION_WRAPPER_GUARD
#define FETCHBLVERSION_WRAPPER_GUARD

#include <napi.h>
#include "FetchBannerlordVersion.Native.h"
#include "utils.h"

using namespace Napi;
using namespace FetchBannerlordVersion::Native::Utils;

namespace FetchBannerlordVersion
{
    namespace Native
    {
        namespace Wrapper
        {

            const Number getChangeSetWrapped(const CallbackInfo &info)
            {
                const auto env = info.Env();
                const auto gameFolderPath = info[0].As<String>();
                const auto libAssembly = info[1].As<String>();
                const auto result = FetchBannerlordVersion::Native::get_change_set(escapeString(gameFolderPath).c_str(), escapeString(libAssembly).c_str());
                return Number::New(env, result);
            }

            const String getVersionWrapped(const CallbackInfo &info)
            {
                const auto env = info.Env();
                const auto gameFolderPath = info[0].As<String>();
                const auto libAssembly = info[1].As<String>();
                const auto result = FetchBannerlordVersion::Native::get_version(escapeString(gameFolderPath).c_str(), escapeString(libAssembly).c_str());
                return String::New(env, unescapeString(result));
            }

            const Number getVersionTypeWrapped(const CallbackInfo &info)
            {
                const auto env = info.Env();
                const auto gameFolderPath = info[0].As<String>();
                const auto libAssembly = info[1].As<String>();
                const auto result = FetchBannerlordVersion::Native::get_version_type(escapeString(gameFolderPath).c_str(), escapeString(libAssembly).c_str());
                return Number::New(env, result);
            }

            const Object Init(Env env, Object exports)
            {
                exports.Set("getChangeSet", Function::New(env, getChangeSetWrapped));

                exports.Set("getVersion", Function::New(env, getVersionWrapped));

                exports.Set("getVersionType", Function::New(env, getVersionTypeWrapped));

                return exports;
            }

        }
    }
}
#endif
