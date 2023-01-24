using BUTR.NativeAOT.Shared;

using System.Runtime.CompilerServices;

namespace FetchBannerlordVersion.Native.Tests;

public static class Utils2
{
    public static unsafe ReadOnlySpan<char> ToSpan(char* value) => new SafeStringMallocHandle(value).ToSpan();
    public static unsafe ReadOnlySpan<char> ToSpan(param_string* value) => new SafeStringMallocHandle((char*) value).ToSpan();
    public static unsafe (string Error, string Result) GetResult(return_value_string* ret)
    {
        var result = Unsafe.AsRef<return_value_string>(ret);
        return (ToSpan(result.Error).ToString(), ToSpan(result.Value).ToString());
    }
    public static unsafe (string Error, bool Result) GetResult(return_value_bool* ret)
    {
        var result = Unsafe.AsRef<return_value_bool>(ret);
        return (ToSpan(result.Error).ToString(), result.Value);
    }
    public static unsafe (string Error, int Result) GetResult(return_value_int32* ret)
    {
        var result = Unsafe.AsRef<return_value_int32>(ret);
        return (new string(result.Error), result.Value);
    }
    public static unsafe (string Error, uint Result) GetResult(return_value_uint32* ret)
    {
        var result = Unsafe.AsRef<return_value_uint32>(ret);
        return (new string(result.Error), result.Value);
    }
    public static unsafe (string Error, string Result) GetResult(return_value_void* ret)
    {
        var result = Unsafe.AsRef<return_value_void>(ret);
        return (new string(result.Error), string.Empty);
    }
}