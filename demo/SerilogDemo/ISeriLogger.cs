using static SerilogDemo.SerilogHelper;

namespace SerilogDemo
{
    public interface ISeriLogger
    {
        void LogVerbose(string msg, params object[] args);
        void LogVerbose(LogType logType, string msg, params object[] args);
        void LogInformation(string msg, params object[] args);
        void LogInformation(LogType logType, string msg, params object[] args);

        void LogDebug(string msg, params object[] args);
        void LogDebug(LogType logType, string msg, params object[] args);
        void LogDebug(Exception err, string msg);
        void LogDebug(LogType logType, Exception err, string msg);

        void LogWarning(string msg, params object[] args);
        void LogWarning(LogType logType, string msg, params object[] args);

        void LogError(string msg, params object[] args);
        void LogError(LogType logType, string msg, params object[] args);
        void LogError(Exception err, string msg);
        void LogError(LogType logType, Exception err, string msg);
        void LogError(Exception err, string msg, params object[] args);
        void LogError(LogType logType, Exception err, string msg, params object[] args);
    }
}
