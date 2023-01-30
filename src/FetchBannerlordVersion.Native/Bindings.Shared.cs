using BUTR.NativeAOT.Shared;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FetchBannerlordVersion.Native
{
    public static unsafe partial class Bindings
    {
        [UnmanagedCallersOnly(EntryPoint = "alloc", CallConvs = new [] { typeof(CallConvCdecl) })]
        public static void* Alloc(nuint size)
        {
            Logger.LogInput(size);
            try
            {
                var result = NativeMemory.Alloc(size);

                Logger.LogOutputPrimitive((int) result);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return null;
            }
        }

        [UnmanagedCallersOnly(EntryPoint = "dealloc", CallConvs = new [] { typeof(CallConvCdecl) })]
        public static void Dealloc(param_ptr* ptr)
        {
            Logger.LogInput(ptr);
            try
            {
                NativeMemory.Free(ptr);

                Logger.LogOutput();
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }
    }
}