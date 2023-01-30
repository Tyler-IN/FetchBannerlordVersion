using BUTR.NativeAOT.Shared;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using static FetchBannerlordVersion.Native.Tests.Utils2;

namespace FetchBannerlordVersion.Native.Tests
{
    public partial class Tests
    {
        private const string DllPath = "../../../../../src/FetchBannerlordVersion.Native/bin/Release/net7.0/win-x64/native/FetchBannerlordVersion.Native.dll";


        [LibraryImport(DllPath), UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
        private static unsafe partial return_value_uint32* bfv_get_change_set(param_string* p_game_folder_path, param_string* p_lib_assembly);

        [LibraryImport(DllPath), UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
        private static unsafe partial return_value_string* bfv_get_version(param_string* p_game_folder_path, param_string* p_lib_assembly);

        [LibraryImport(DllPath), UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
        private static unsafe partial return_value_uint32* bfv_get_version_type(param_string* p_game_folder_path, param_string* p_lib_assembly);

        [Test]
        public unsafe void Test_Main()
        {
            Assert.DoesNotThrow(() =>
            {
                var path = Path.GetFullPath("./Data");
                var dllName = "TaleWorlds.Library.dll";

                using var path2 = Copy(path);
                using var dllName2 = Copy(dllName);

                var changeSet = GetResult(bfv_get_change_set((param_string*) path2.DangerousGetHandle(), (param_string*) dllName2.DangerousGetHandle()));
                Assert.That(changeSet, Is.EqualTo(321460));

                var version = GetResult(bfv_get_version((param_string*) path2.DangerousGetHandle(), (param_string*) dllName2.DangerousGetHandle()));
                Assert.That(version, Is.EqualTo("e1.8.0"));

                var versionType = GetResult(bfv_get_version_type((param_string*) path2.DangerousGetHandle(), (param_string*) dllName2.DangerousGetHandle()));
                Assert.That(versionType, Is.EqualTo(4));
            });

            Assert.That(DanglingAllocationsCount, Is.EqualTo(0));
        }
    }
}