using Serilog;
using System.Runtime.CompilerServices;
using Utils.Enums;

namespace E_commerce.Tools
{
    public class LogHelper
    {
        public static void FormatMainLogMessage(LogLevelEnums level, string message, Exception? ex = null, [CallerMemberName] string methodName = "")
        {
            // Format the log message
            var formattedMessage = $"Main ({methodName}) | {message}";

            LogMessage(level, formattedMessage, ex);
        }

        public static void LogMessage(LogLevelEnums level, string message, Exception? ex = null)
        {
            switch (level)
            {
                case LogLevelEnums.Verbose:
                    if (ex != null)
                        Log.Verbose(ex, message);
                    else
                        Log.Verbose(message);
                    break;
                case LogLevelEnums.Debug:
                    if (ex != null)
                        Log.Debug(ex, message);
                    else
                        Log.Debug(message);
                    break;
                case LogLevelEnums.Information:
                    if (ex != null)
                        Log.Information(ex, message);
                    else
                        Log.Information(message);
                    break;
                case LogLevelEnums.Warning:
                    if (ex != null)
                        Log.Warning(ex, message);
                    else
                        Log.Warning(message);
                    break;
                case LogLevelEnums.Error:
                    if (ex != null)
                        Log.Error(ex, message);
                    else
                        Log.Error(message);
                    break;
                case LogLevelEnums.Fatal:
                    if (ex != null)
                        Log.Fatal(ex, message);
                    else
                        Log.Fatal(message);
                    break;
                default:
                    if (ex != null)
                        Log.Information(ex, message);
                    else
                        Log.Information(message);
                    break;
            }
        }

    }
}
