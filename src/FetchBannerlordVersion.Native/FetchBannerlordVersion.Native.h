
#ifndef SRC_BINDINGS_H_
#define SRC_BINDINGS_H_

#ifndef __cplusplus

#include <stdlib.h>
#include <stdint.h>

#else

#include <memory>
#include <string>
#include <cstdint>
#include <cuchar>

namespace FetchBannerlordVersion::Native
{
    extern "C"
    {
#endif

#ifndef __cplusplus
        typedef char16_t wchar_t;
#endif
        typedef char16_t param_string;
        typedef char16_t param_json;
        typedef uint8_t param_bool;
        typedef int32_t param_int;
        typedef uint32_t param_uint;
        typedef void param_ptr;

        typedef struct return_value_void
        {
            param_string *const error;
        } return_value_void;
        typedef struct return_value_string
        {
            param_string *const error;
            param_string *const value;
        } return_value_string;
        typedef struct return_value_json
        {
            param_string *const error;
            param_json *const value;
        } return_value_json;
        typedef struct return_value_bool
        {
            param_string *const error;
            param_bool const value;
        } return_value_bool;
        typedef struct return_value_int32
        {
            param_string *const error;
            param_int const value;
        } return_value_int32;
        typedef struct return_value_uint32
        {
            param_string *const error;
            param_uint const value;
        } return_value_uint32;
        typedef struct return_value_ptr
        {
            param_string *const error;
            param_ptr *const value;
        } return_value_ptr;

    void __cdecl common_dealloc(const param_ptr* const ptr);
    void* const __cdecl common_alloc(size_t size);
    int32_t __cdecl common_alloc_alive_count();
    return_value_uint32* const __cdecl bfv_get_change_set(const param_string* const p_game_folder_path, const param_string* const p_lib_assembly);
    return_value_string* const __cdecl bfv_get_version(const param_string* const p_game_folder_path, const param_string* const p_lib_assembly);
    return_value_uint32* const __cdecl bfv_get_version_type(const param_string* const p_game_folder_path, const param_string* const p_lib_assembly);


#ifdef __cplusplus
    }
}
#endif

#endif
