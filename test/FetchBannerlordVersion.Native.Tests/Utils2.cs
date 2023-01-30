using BUTR.NativeAOT.Shared;

using Microsoft.Win32.SafeHandles;

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FetchBannerlordVersion.Native.Tests
{
    public static partial class Utils2
    {
        private const string DllPath = "../../../../../src/FetchBannerlordVersion.Native/bin/Release/net7.0/win-x64/native/FetchBannerlordVersion.Native.dll";


        public sealed unsafe class SafeStringMallocHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            public static implicit operator ReadOnlySpan<char>(SafeStringMallocHandle handle) => MemoryMarshal.CreateReadOnlySpanFromNullTerminated((char*) handle.handle.ToPointer());

            private readonly bool _isExternal;

            public SafeStringMallocHandle() : base(true) { }
            public SafeStringMallocHandle(char* ptr, bool isExternal = false) : base(true)
            {
                handle = new IntPtr(ptr);
                _isExternal = isExternal;
                if (isExternal)
                {
                    var b = false;
                    DangerousAddRef(ref b);
                }
            }

            protected override bool ReleaseHandle()
            {
                if (handle != IntPtr.Zero)
                {
                    if (_isExternal)
                        dealloc(handle.ToPointer());
                    else
                        Dealloc(handle.ToPointer());
                }
                return true;
            }

            public ReadOnlySpan<char> ToSpan() => this;
        }

        private unsafe class SafeStructMallocHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            public static SafeStructMallocHandle<TStruct> Create<TStruct>(TStruct* ptr, bool isExternal = true) where TStruct : unmanaged => new(ptr, isExternal);

            private readonly bool _isExternal;

            protected SafeStructMallocHandle() : base(true) { }
            protected SafeStructMallocHandle(IntPtr handle, bool isExternal = true) : base(true)
            {
                this.handle = handle;
                _isExternal = isExternal;
                if (isExternal)
                {
                    var b = false;
                    DangerousAddRef(ref b);
                }
            }

            protected override bool ReleaseHandle()
            {
                if (handle != IntPtr.Zero)
                {
                    if (_isExternal)
                        dealloc(handle.ToPointer());
                    else
                        Dealloc(handle.ToPointer());
                }
                return true;
            }
        }

        private sealed unsafe class SafeStructMallocHandle<TStruct> : SafeStructMallocHandle where TStruct : unmanaged
        {
            public static implicit operator TStruct*(SafeStructMallocHandle<TStruct> handle) => (TStruct*) handle.handle.ToPointer();

            private readonly bool _isExternal;
            public TStruct* Value => this;

            public bool IsNull => Value == null;

            public SafeStructMallocHandle() : base(IntPtr.Zero) { }
            public SafeStructMallocHandle(TStruct* param, bool isExternal = true) : base(new IntPtr(param), isExternal) { }

            public void ValueAsVoid()
            {
                if (typeof(TStruct) != typeof(return_value_void))
                    throw new Exception();

                var ptr = (return_value_void*) Value;
                if (ptr->Error is null)
                {
                    return;
                }

                using var hError = new SafeStringMallocHandle(ptr->Error, true);
                throw new NativeCallException(new string(hError));
            }

            public SafeStringMallocHandle ValueAsString()
            {
                if (typeof(TStruct) != typeof(return_value_string))
                    throw new Exception();

                var ptr = (return_value_string*) Value;
                if (ptr->Error is null)
                {
                    return new SafeStringMallocHandle(ptr->Value, true);
                }

                using var hError = new SafeStringMallocHandle(ptr->Error, true);
                throw new NativeCallException(new string(hError));
            }

            public bool ValueAsBool()
            {
                if (typeof(TStruct) != typeof(return_value_bool))
                    throw new Exception();

                var ptr = (return_value_bool*) Value;
                if (ptr->Error is null)
                {
                    return ptr->Value == 1;
                }

                using var hError = new SafeStringMallocHandle(ptr->Error, true);
                throw new NativeCallException(new string(hError));
            }

            public uint ValueAsUInt32()
            {
                if (typeof(TStruct) != typeof(return_value_uint32))
                    throw new Exception();

                var ptr = (return_value_uint32*) Value;
                if (ptr->Error is null)
                {
                    return ptr->Value;
                }

                using var hError = new SafeStringMallocHandle(ptr->Error, true);
                throw new NativeCallException(new string(hError));
            }

            public int ValueAsInt32()
            {
                if (typeof(TStruct) != typeof(return_value_int32))
                    throw new Exception();

                var ptr = (return_value_int32*) Value;
                if (ptr->Error is null)
                {
                    return ptr->Value;
                }

                using var hError = new SafeStringMallocHandle(ptr->Error, true);
                throw new NativeCallException(new string(hError));
            }

            public void* ValueAsPointer()
            {
                if (typeof(TStruct) != typeof(return_value_ptr))
                    throw new Exception();

                var ptr = (return_value_ptr*) Value;
                if (ptr->Error is null)
                {
                    return ptr->Value;
                }

                using var hError = new SafeStringMallocHandle(ptr->Error, true);
                throw new NativeCallException(new string(hError));
            }
        }


        [LibraryImport(DllPath), UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
        private static unsafe partial void* alloc(nuint size);
        [LibraryImport(DllPath), UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
        private static unsafe partial void dealloc(void* ptr);

        private static readonly ConcurrentDictionary<nuint, object?> _pointers = new();
        private static unsafe void* Alloc(nuint size)
        {
            var ptr = alloc(size);
            if (!_pointers.TryAdd(new UIntPtr(ptr), null)) throw new Exception("Alloc: Allocation returned an existing living address!");
            return ptr;
        }
        private static unsafe void Dealloc(void* ptr)
        {
            var ptr2 = new UIntPtr(ptr);
            if (!_pointers.TryRemove(ptr2, out _)) throw new Exception("Dealloc: Allocation not found!");
            dealloc(ptr);
        }
        public static int DanglingAllocationsCount()
        {
            return _pointers.Count;
        }

        public static unsafe SafeStringMallocHandle Copy(in ReadOnlySpan<char> str)
        {
            var size = (uint) ((str.Length + 1) * 2);

            var dst = (char*) Alloc(new UIntPtr(size));
            str.CopyTo(new Span<char>(dst, str.Length));
            dst[str.Length] = '\0';
            return new SafeStringMallocHandle(dst);
        }

        public static unsafe ReadOnlySpan<char> ToSpan(param_string* value) => new SafeStringMallocHandle((char*) value).ToSpan();

        public static unsafe string GetResult(return_value_string* ret)
        {
            using var result = SafeStructMallocHandle.Create(ret);
            using var str = result.ValueAsString();
            return str.ToSpan().ToString();
        }
        public static unsafe bool GetResult(return_value_bool* ret)
        {
            using var result = SafeStructMallocHandle.Create(ret);
            return result.ValueAsBool();
        }
        public static unsafe int GetResult(return_value_int32* ret)
        {
            using var result = SafeStructMallocHandle.Create(ret);
            return result.ValueAsInt32();
        }
        public static unsafe uint GetResult(return_value_uint32* ret)
        {
            using var result = SafeStructMallocHandle.Create(ret);
            return result.ValueAsUInt32();
        }
        public static unsafe void GetResult(return_value_void* ret)
        {
            using var result = SafeStructMallocHandle.Create(ret);
            result.ValueAsVoid();
        }
    }
}