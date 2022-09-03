#ifndef SRC_FETCHBLVERSION_BINDINGS_H_
#define SRC_FETCHBLVERSION_BINDINGS_H_

namespace FetchBannerlordVersion {
    namespace Native {

#ifdef __cplusplus
        extern "C" {
#endif
            // We work with ANSI strings. Escape any non ASII symbol (> 127) as a unicode codepoint (\u0000)
            int get_change_set(const char* p_game_folder_path, const char* p_lib_assembly);
            const char* get_version(const char* p_game_folder_path, const char* p_lib_assembly);
            int get_version_type(const char* p_game_folder_path, const char* p_lib_assembly);
#ifdef __cplusplus
        }
#endif

    }
}

#endif