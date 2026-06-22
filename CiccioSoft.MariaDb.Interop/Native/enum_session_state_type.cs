namespace CiccioSoft.MariaDb.Interop.Native
{
    internal enum enum_session_state_type
    {
        SESSION_TRACK_SYSTEM_VARIABLES = 0,
        SESSION_TRACK_SCHEMA,
        SESSION_TRACK_STATE_CHANGE,
        SESSION_TRACK_GTIDS,
        SESSION_TRACK_TRANSACTION_CHARACTERISTICS,
        SESSION_TRACK_TRANSACTION_STATE,
    }
}
