using System;
using System.Runtime.InteropServices;

namespace MariaDbClient.Native;

/// <summary>
/// Dichiarazioni P/Invoke dirette per libmariadb.
/// <para>
/// Tutte le signature usano esclusivamente tipi blittabili:
/// <c>byte*</c> per i puntatori <c>char*</c> C, <c>IntPtr</c> per gli
/// handle opachi (<c>MYSQL*</c>, <c>MYSQL_RES*</c>, <c>MYSQL_STMT*</c>),
/// tipi primitivi integrali per gli scalari.
/// Nessun <c>MarshalAs</c>, nessuna <c>string</c> nelle firme.
/// </para>
/// <para>
/// I nomi dei metodi coincidono esattamente con i simboli C della libreria.
/// </para>
/// </summary>
public static unsafe class MariaDbImports
{
    // Nome della libreria nativa.
    // Su Linux  → libmariadb.so.3
    // Su Windows → libmariadb.dll
    // Su macOS  → libmariadb.dylib
    // Il runtime .NET risolve automaticamente prefisso/estensione per piattaforma.
    private const string Lib = "libmariadb";

    // ================================================================
    //  LIBRERIA / THREAD
    // ================================================================

    /// <summary>int mysql_library_init(int argc, char **argv, char **groups)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int mysql_library_init(int argc, byte** argv, byte** groups);

    /// <summary>void mysql_library_end(void)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void mysql_library_end();

    /// <summary>int mysql_thread_init(void)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int mysql_thread_init();

    /// <summary>void mysql_thread_end(void)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void mysql_thread_end();

    // ================================================================
    //  INIZIALIZZAZIONE / CONNESSIONE
    // ================================================================

    /// <summary>MYSQL *mysql_init(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr mysql_init(IntPtr mysql);

    /// <summary>
    /// MYSQL *mysql_real_connect(MYSQL *mysql,
    ///     const char *host, const char *user, const char *passwd,
    ///     const char *db, unsigned int port,
    ///     const char *unix_socket, unsigned long clientflag)
    /// </summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr mysql_real_connect(
        IntPtr       mysql,
        byte*        host,
        byte*        user,
        byte*        passwd,
        byte*        db,
        uint         port,
        byte*        unixSocket,
        ClientFlags  clientFlag);

    /// <summary>void mysql_close(MYSQL *sock)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void mysql_close(IntPtr mysql);

    /// <summary>int mysql_ping(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int mysql_ping(IntPtr mysql);

    /// <summary>int mysql_select_db(MYSQL *mysql, const char *db)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int mysql_select_db(IntPtr mysql, byte* db);

    /// <summary>unsigned long mysql_thread_id(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong mysql_thread_id(IntPtr mysql);

    // ================================================================
    //  OPZIONI
    // ================================================================

    /// <summary>int mysql_options(MYSQL *mysql, enum mysql_option option, const void *arg)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int mysql_options(IntPtr mysql, MysqlOption option, void* arg);

    /// <summary>int mysql_get_optionv(MYSQL*, enum mysql_option, void *arg, ...)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int mysql_get_option(IntPtr mysql, MysqlOption option, void* arg);

    // ================================================================
    //  QUERY SEMPLICI
    // ================================================================

    /// <summary>int mysql_query(MYSQL *mysql, const char *q)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int mysql_query(IntPtr mysql, byte* q);

    /// <summary>int mysql_real_query(MYSQL *mysql, const char *q, unsigned long length)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int mysql_real_query(IntPtr mysql, byte* q, nuint length);

    /// <summary>int mysql_send_query(MYSQL *mysql, const char *q, unsigned long length)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int mysql_send_query(IntPtr mysql, byte* q, nuint length);

    /// <summary>int mysql_read_query_result(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int mysql_read_query_result(IntPtr mysql);

    // ================================================================
    //  RESULT SET
    // ================================================================

    /// <summary>MYSQL_RES *mysql_store_result(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr mysql_store_result(IntPtr mysql);

    /// <summary>MYSQL_RES *mysql_use_result(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr mysql_use_result(IntPtr mysql);

    /// <summary>void mysql_free_result(MYSQL_RES *result)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void mysql_free_result(IntPtr result);

    /// <summary>MYSQL_ROW mysql_fetch_row(MYSQL_RES *result)</summary>
    /// <returns>Puntatore a array di byte* (char**), NULL se fine risultati.</returns>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte** mysql_fetch_row(IntPtr result);

    /// <summary>unsigned long *mysql_fetch_lengths(MYSQL_RES *result)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    // public static extern nuint* mysql_fetch_lengths(IntPtr result);
    public static extern uint* mysql_fetch_lengths(IntPtr result);

    /// <summary>unsigned int mysql_num_fields(MYSQL_RES *res)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint mysql_num_fields(IntPtr result);

    /// <summary>my_ulonglong mysql_num_rows(MYSQL_RES *res)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong mysql_num_rows(IntPtr result);

    /// <summary>MYSQL_FIELD *mysql_fetch_field(MYSQL_RES *result)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern MysqlFieldNative* mysql_fetch_field(IntPtr result);

    /// <summary>MYSQL_FIELD *mysql_fetch_fields(MYSQL_RES *result)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern MysqlFieldNative* mysql_fetch_fields(IntPtr result);

    /// <summary>MYSQL_FIELD *mysql_fetch_field_direct(MYSQL_RES *res, unsigned int fieldnr)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern MysqlFieldNative* mysql_fetch_field_direct(IntPtr result, uint fieldNr);

    /// <summary>void mysql_data_seek(MYSQL_RES *result, my_ulonglong offset)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void mysql_data_seek(IntPtr result, ulong offset);

    /// <summary>MYSQL_ROW_OFFSET mysql_row_tell(MYSQL_RES *res)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr mysql_row_tell(IntPtr result);

    /// <summary>MYSQL_ROW_OFFSET mysql_row_seek(MYSQL_RES *result, MYSQL_ROW_OFFSET offset)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr mysql_row_seek(IntPtr result, IntPtr offset);

    // ================================================================
    //  STATO / INFORMAZIONI QUERY
    // ================================================================

    /// <summary>my_ulonglong mysql_affected_rows(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong mysql_affected_rows(IntPtr mysql);

    /// <summary>my_ulonglong mysql_insert_id(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong mysql_insert_id(IntPtr mysql);

    /// <summary>unsigned int mysql_warning_count(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint mysql_warning_count(IntPtr mysql);

    /// <summary>const char *mysql_info(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* mysql_info(IntPtr mysql);

    /// <summary>const char *mysql_stat(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* mysql_stat(IntPtr mysql);

    // ================================================================
    //  ERRORI
    // ================================================================

    /// <summary>const char *mysql_error(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* mysql_error(IntPtr mysql);

    /// <summary>unsigned int mysql_errno(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint mysql_errno(IntPtr mysql);

    /// <summary>const char *mysql_sqlstate(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* mysql_sqlstate(IntPtr mysql);

    // ================================================================
    //  METADATI SERVER
    // ================================================================

    /// <summary>const char *mysql_get_server_info(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* mysql_get_server_info(IntPtr mysql);

    /// <summary>const char *mysql_get_host_info(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* mysql_get_host_info(IntPtr mysql);

    /// <summary>const char *mysql_get_client_info(void)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* mysql_get_client_info();

    /// <summary>unsigned long mysql_get_client_version(void)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong mysql_get_client_version();

    /// <summary>unsigned long mysql_get_server_version(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong mysql_get_server_version(IntPtr mysql);

    /// <summary>unsigned int mysql_get_proto_info(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint mysql_get_proto_info(IntPtr mysql);

    // ================================================================
    //  TRANSAZIONI
    // ================================================================

    /// <summary>my_bool mysql_autocommit(MYSQL *mysql, my_bool auto_mode)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte mysql_autocommit(IntPtr mysql, byte autoMode);

    /// <summary>my_bool mysql_commit(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte mysql_commit(IntPtr mysql);

    /// <summary>my_bool mysql_rollback(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte mysql_rollback(IntPtr mysql);

    // ================================================================
    //  ESCAPE
    // ================================================================

    /// <summary>
    /// unsigned long mysql_real_escape_string(
    ///     MYSQL *mysql, char *to, const char *from, unsigned long length)
    /// </summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern nuint mysql_real_escape_string(
        IntPtr mysql, byte* to, byte* from, nuint length);

    /// <summary>
    /// unsigned long mysql_real_escape_string_quote(
    ///     MYSQL *mysql, char *to, const char *from, unsigned long length, char quote)
    /// </summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern nuint mysql_real_escape_string_quote(
        IntPtr mysql, byte* to, byte* from, nuint length, byte quote);

    // ================================================================
    //  MULTI-STATEMENT
    // ================================================================

    /// <summary>int mysql_next_result(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int mysql_next_result(IntPtr mysql);

    /// <summary>my_bool mysql_more_results(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte mysql_more_results(IntPtr mysql);

    // ================================================================
    //  PREPARED STATEMENTS
    // ================================================================

    /// <summary>MYSQL_STMT *mysql_stmt_init(MYSQL *mysql)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr mysql_stmt_init(IntPtr mysql);

    /// <summary>int mysql_stmt_prepare(MYSQL_STMT *stmt, const char *query, unsigned long length)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int mysql_stmt_prepare(IntPtr stmt, byte* query, nuint length);

    /// <summary>int mysql_stmt_execute(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int mysql_stmt_execute(IntPtr stmt);

    /// <summary>int mysql_stmt_fetch(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int mysql_stmt_fetch(IntPtr stmt);

    /// <summary>
    /// int mysql_stmt_fetch_column(MYSQL_STMT *stmt, MYSQL_BIND *bind_arg,
    ///     unsigned int column, unsigned long offset)
    /// </summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int mysql_stmt_fetch_column(
        IntPtr stmt, MysqlBindNative* bindArg, uint column, nuint offset);

    /// <summary>my_bool mysql_stmt_bind_param(MYSQL_STMT *stmt, MYSQL_BIND *bnd)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte mysql_stmt_bind_param(IntPtr stmt, MysqlBindNative* bnd);

    /// <summary>my_bool mysql_stmt_bind_result(MYSQL_STMT *stmt, MYSQL_BIND *bnd)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte mysql_stmt_bind_result(IntPtr stmt, MysqlBindNative* bnd);

    /// <summary>my_bool mysql_stmt_close(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte mysql_stmt_close(IntPtr stmt);

    /// <summary>my_bool mysql_stmt_reset(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte mysql_stmt_reset(IntPtr stmt);

    /// <summary>my_bool mysql_stmt_free_result(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte mysql_stmt_free_result(IntPtr stmt);

    /// <summary>my_ulonglong mysql_stmt_affected_rows(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong mysql_stmt_affected_rows(IntPtr stmt);

    /// <summary>my_ulonglong mysql_stmt_insert_id(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong mysql_stmt_insert_id(IntPtr stmt);

    /// <summary>unsigned int mysql_stmt_field_count(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint mysql_stmt_field_count(IntPtr stmt);

    /// <summary>unsigned long mysql_stmt_param_count(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern nuint mysql_stmt_param_count(IntPtr stmt);

    /// <summary>int mysql_stmt_store_result(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int mysql_stmt_store_result(IntPtr stmt);

    /// <summary>my_ulonglong mysql_stmt_num_rows(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong mysql_stmt_num_rows(IntPtr stmt);

    /// <summary>MYSQL_RES *mysql_stmt_result_metadata(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr mysql_stmt_result_metadata(IntPtr stmt);

    /// <summary>MYSQL_RES *mysql_stmt_param_metadata(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr mysql_stmt_param_metadata(IntPtr stmt);

    /// <summary>const char *mysql_stmt_error(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* mysql_stmt_error(IntPtr stmt);

    /// <summary>unsigned int mysql_stmt_errno(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint mysql_stmt_errno(IntPtr stmt);

    /// <summary>const char *mysql_stmt_sqlstate(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* mysql_stmt_sqlstate(IntPtr stmt);

    /// <summary>int mysql_stmt_next_result(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int mysql_stmt_next_result(IntPtr stmt);

    /// <summary>void mysql_stmt_data_seek(MYSQL_STMT *stmt, my_ulonglong offset)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void mysql_stmt_data_seek(IntPtr stmt, ulong offset);

    /// <summary>my_ulonglong mysql_stmt_row_tell(MYSQL_STMT *stmt)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong mysql_stmt_row_tell(IntPtr stmt);

    /// <summary>my_ulonglong mysql_stmt_row_seek(MYSQL_STMT *stmt, my_ulonglong offset)</summary>
    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong mysql_stmt_row_seek(IntPtr stmt, ulong offset);

    // ================================================================
    //  UTILITÀ: conversione byte* → string managed (helper interno)
    // ================================================================

    /// <summary>
    /// Converte un puntatore <c>byte*</c> restituito dalla libreria C
    /// in una stringa managed UTF-8. Restituisce null se il puntatore è nullo.
    /// </summary>
    public static string? PtrToStringUtf8(byte* ptr)
    {
        if (ptr == null) return null;
        int len = 0;
        while (ptr[len] != 0) len++;
        return System.Text.Encoding.UTF8.GetString(ptr, len);
    }
}
