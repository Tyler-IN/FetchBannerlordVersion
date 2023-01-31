using BUTR.NativeAOT.Shared;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FetchBannerlordVersion.Native.Tests
{
    internal static partial class Utils2
    {
        public const string DllPath = "../../../../../src/FetchBannerlordVersion.Native/bin/Release/net7.0/win-x64/native/FetchBannerlordVersion.Native.dll";


        static unsafe Utils2()
        {
            Allocator.SetCustom(&alloc, &dealloc);
        }

        [LibraryImport(DllPath), UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
        private static unsafe partial void* alloc(nuint size);
        [LibraryImport(DllPath), UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
        private static unsafe partial void dealloc(void* ptr);
        [LibraryImport(DllPath), UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
        private static unsafe partial int alloc_alive_count();

        public static unsafe void LibrarySetAllocator() => Allocator.SetCustom(&alloc, &dealloc);
        public static int LibraryAliveCount() => alloc_alive_count();

        public static unsafe ReadOnlySpan<char> ToSpan(param_string* value) => new SafeStringMallocHandle((char*) value, false).ToSpan();

        public static unsafe string GetResult(return_value_string* ret)
        {
            using var result = SafeStructMallocHandle.Create(ret, true);
            using var str = result.ValueAsString();
            return str.ToSpan().ToString();
        }
        public static unsafe bool GetResult(return_value_bool* ret)
        {
            using var result = SafeStructMallocHandle.Create(ret, true);
            return result.ValueAsBool();
        }
        public static unsafe int GetResult(return_value_int32* ret)
        {
            using var result = SafeStructMallocHandle.Create(ret, true);
            return result.ValueAsInt32();
        }
        public static unsafe uint GetResult(return_value_uint32* ret)
        {
            using var result = SafeStructMallocHandle.Create(ret, true);
            return result.ValueAsUInt32();
        }
        public static unsafe void GetResult(return_value_void* ret)
        {
            using var result = SafeStructMallocHandle.Create(ret, true);
            result.ValueAsVoid();
        }
    }
}