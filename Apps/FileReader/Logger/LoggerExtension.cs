using System.Runtime.CompilerServices;
using Serilog;

namespace Logger;

public static class LoggerExtension
{
    public static ILogger Here(this ILogger logger,
        [CallerMemberName] string memberNane = "",
        [CallerFilePath] string sourceFilepath = "",
        [CallerLineNumber] int sourceLineNumber = 0) {
        return logger
            .ForContext("MemberName", memberNane)
            .ForContext("FilePath", sourceFilepath)
            .ForContext("LineNumber", sourceLineNumber);
    }
}