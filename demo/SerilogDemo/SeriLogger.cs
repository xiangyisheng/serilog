using static SerilogDemo.SerilogHelper;

namespace SerilogDemo
{

    public class SeriLogger : ISeriLogger
    {
        readonly string seriCustomProperty = SerilogHelper.seriCustomProperty;


        public void LogVerbose(string msg, params object[] args)
        {
            Serilog.Log.Verbose(msg, args);
        }

        public void LogVerbose(LogType logType, string msg, params object[] args)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Verbose(msg, args);
        }

        public void LogInformation(string msg, params object[] args)
        {
            Serilog.Log.Information(msg, args);
        }

        public void LogInformation(LogType logType, string msg, params object[] args)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Information(msg, args);
        }

        public void LogDebug(string msg, params object[] args)
        {
            Serilog.Log.Debug(msg, args);
        }

        public void LogDebug(LogType logType, string msg, params object[] args)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Debug(msg, args);
        }

        public void LogDebug(Exception err, string msg)
        {
            Serilog.Log.Debug(err, msg);
        }

        public void LogDebug(LogType logType, Exception err, string msg)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Debug(err, msg);
        }

        public void LogError(string msg, params object[] args)
        {
            Serilog.Log.Error(msg, args);
        }

        public void LogError(LogType logType, string msg, params object[] args)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Error(msg, args);
        }

        public void LogError(Exception err, string msg)
        {
            Serilog.Log.Error(err, msg);
        }

        public void LogError(LogType logType, Exception err, string msg)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Error(err, msg);
        }

        public void LogError(Exception err, string msg, params object[] args)
        {
            Serilog.Log.Error(err, msg, args);
        }

        public void LogError(LogType logType, Exception err, string msg, params object[] args)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Error(err, msg, args);
        }

        public void LogWarning(string msg, params object[] args)
        {
            Serilog.Log.Warning(msg, args);
        }

        public void LogWarning(LogType logType, string msg, params object[] args)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Warning(msg, args);
        }

        public void Verbose(string msg, params object[] args)
        {
            Serilog.Log.Verbose(msg, args);
        }

        public void Verbose(LogType logType, string msg, params object[] args)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Verbose(msg, args);
        }
    }
}
