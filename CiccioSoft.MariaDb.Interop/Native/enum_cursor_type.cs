namespace CiccioSoft.MariaDb.Interop.Native
{
    internal enum enum_cursor_type
    {
        CURSOR_TYPE_NO_CURSOR = 0,
        CURSOR_TYPE_READ_ONLY = 1,
        CURSOR_TYPE_FOR_UPDATE = 2,
        CURSOR_TYPE_SCROLLABLE = 4,
    }
}
