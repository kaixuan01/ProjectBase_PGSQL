﻿using System.Runtime.CompilerServices;
using Utils.Enums;

namespace DBL.Tools
{
    public delegate void LogEventHandler(LogLevelEnums level, string message, Exception ex);

    public static class LogHelper
    {
        public static event LogEventHandler OnLogEvent;

        public static void RaiseLogEvent(LogLevelEnums level, string message, Exception? ex = null, [CallerMemberName] string methodName = "")
        {
            // Format the log message
            var formattedMessage = $"DBL ({methodName}) | {message}";

            OnLogEvent?.Invoke(level, formattedMessage, ex);
        }
    }
}