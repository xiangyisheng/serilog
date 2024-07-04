using Serilog;
using Serilog.Events;
using Serilog.Filters;
using static SerilogDemo.SerilogHelper;

namespace SerilogDemo
{
    /// <summary>
    /// https://blog.csdn.net/gaiyi09/article/details/130036732
    /// </summary>
    public class SerilogHelper
    {
        public enum LogType
        {
            Default, Order, Goods
        }

        #region Serilog 相关设置
        internal static string LogFilePath(string fileName) => $@"Logs/{fileName}/{DateTime.Now.Year}.{DateTime.Now.Month}/log_.log";
        internal static readonly string seriCustomProperty = "seriPos";
        internal static readonly string logOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}][{ProcessName}({ProcessId})][{ThreadName}({ThreadId})][{EnvironmentName}.{MachineName}][{RequestId}] {Message:lj}{NewLine}{Exception}";
        internal static readonly long? fileSize = 31457280L;
        /// <summary>
        /// 初始化serilog
        /// </summary>
        public static void InitSerilog()
        {
            /*
            WriteTo.File，可同步或异步，异步需要引用async包
            path：默认路径是程序的bin目录+path参数，当然也可以写绝对路径，只需要写入参数就可以了
            rollingInterval：创建文件的类别，可以是分钟，小时，天，月。 此参数可以让创建的log文件名 + 时间。例如log_20220202_001.log
            fileSizeLimitBytes：文件大小限制，类型long,1024=1KB，31457280=30MB
            rollOnFileSizeLimit：达到文件大小限制后，是否继续创建新文件，如log_20220202_001.log
            outputTemplate：日志模板，可以自定义
            retainedFileCountLimit：设置日志文件个数最大值，默认31，意思就是只保留最近的31个日志文件,等于null时永远保留文件
            restrictedToMinimumLevel：最小写入级别，接收器Sink的级别必须高于Logger的级别，比如Logger的默认级别为 Information，即便 Sink 重写日志级别为 LogEventLevel.Debug，也只能看到 Information
            */


            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Information()
            //    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
            //    .Enrich.FromLogContext()
            //    .Enrich.WithMachineName()
            //    .Enrich.WithEnvironmentName()
            //    .Enrich.WithProcessName()
            //    .Enrich.WithProcessId()
            //    .Enrich.WithThreadName()
            //    .Enrich.WithThreadId()
            //    .WriteTo.Console()
            //    .WriteTo.File(@"../log-.txt",
            //        rollingInterval: RollingInterval.Day,
            //        rollOnFileSizeLimit: true,
            //        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}][{ProcessName}({ProcessId})][{ThreadName}({ThreadId})][{EnvironmentName}.{MachineName}][{RequestId}] {Message:lj}{NewLine}{Exception}")
            //    .WriteTo.Seq("http://localhost:5341")
            //    .CreateLogger();

            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Default", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
                .Enrich.FromLogContext()//记录相关上下文信息 
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithProcessName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadName()
                .Enrich.WithThreadId()
                .WriteTo.Seq("http://localhost:5341")
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Verbose, theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code, outputTemplate: logOutputTemplate)
                //利用过滤器对输出文件按文件夹分类
                //如果使用依赖注入ILogger记录日志，当需要记录到指定文件夹，需要传入seriPos，否则只记录到默认Default文件夹
                //比如：Log.Information("{seriPos}:Test", LogType.Order);//LogType自定义枚举
                .WriteTo.Logger(lg =>
                {
                    lg.Filter.ByIncludingOnly(e =>
                    {
                        try
                        {
                            if (e.Properties.TryGetValue(seriCustomProperty, value: out var value))
                            {
                                ScalarValue scalarValue = value as ScalarValue;
                                LogType arg = (LogType)scalarValue.Value;
                                if (arg != LogType.Default)
                                    return false;
                            }
                            return true;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    });
                    lg.WriteTo.File(LogFilePath(LogType.Default.ToString()), rollingInterval: RollingInterval.Day, fileSizeLimitBytes: fileSize, rollOnFileSizeLimit: true, outputTemplate: logOutputTemplate);
                })
                .WriteToFilePath(new List<LogType> { LogType.Order, LogType.Goods })
                .CreateLogger();
        }
        #endregion

        /*****************************日志级别*****************************/
        // FATAL(致命错误) > ERROR（一般错误） > Warning（警告） > Information（一般信息） > DEBUG（调试信息）>Verbose（详细模式，即全部）

        #region Info
        public static void Info(string msg, params object[] args)
        {
            Serilog.Log.Information(msg, args);
        }

        public static void Info(LogType logType, string msg, params object[] args)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Information(msg, args);
        }
        #endregion

        #region Debug
        public static void Debug(string msg, params object[] args)
        {
            Serilog.Log.Debug(msg, args);
        }

        public static void Debug(LogType logType, string msg, params object[] args)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Debug(msg, args);
        }

        public static void Debug(Exception err, string msg)
        {
            Serilog.Log.Debug(err, msg);
        }

        public static void Debug(LogType logType, Exception err, string msg)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Debug(err, msg);
        }
        #endregion

        #region Warning
        public static void Warning(string msg, params object[] args)
        {
            Serilog.Log.Warning(msg, args);
        }

        public static void Warning(LogType logType, string msg, params object[] args)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Warning(msg, args);
        }
        #endregion

        #region Error
        public static void Error(string msg, params object[] args)
        {
            Serilog.Log.Error(msg, args);
        }

        public static void Error(LogType logType, string msg, params object[] args)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Error(msg, args);
        }

        public static void Error(Exception err, string msg)
        {
            Serilog.Log.Error(err, msg);
        }

        public static void Error(LogType logType, Exception err, string msg)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Error(err, msg);
        }

        public static void Error(Exception err, string msg, params object[] args)
        {
            Serilog.Log.Error(err, msg, args);
        }

        public static void Error(LogType logType, Exception err, string msg, params object[] args)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Error(err, msg, args);
        }

        #endregion

        #region Fatal
        public static void Fatal(string msg, params object[] args)
        {
            Serilog.Log.Fatal(msg, args);
        }

        public static void Fatal(LogType logType, string msg, params object[] args)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Fatal(msg, args);
        }

        public static void Fatal(Exception err, string msg)
        {
            Serilog.Log.Fatal(err, msg);
        }

        public static void Fatal(LogType logType, Exception err, string msg)
        {
            Serilog.Log.ForContext(seriCustomProperty, logType).Fatal(err, msg);
        }
        #endregion

        #region Verbose
        public static void Verbose(string msg, params object[] args)
        {
            Serilog.Log.Verbose(msg, args);
        }

        public static void Verbose(Exception err, string msg)
        {
            Serilog.Log.Verbose(err, msg);
        }
        #endregion
    }


    public static class LoggerConfigurationExtiensions
    {
        /// <summary>
        /// 遍历集合项，将Log输出到对应LogType文件夹
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="logType">logType作为子Log文件夹名</param>
        /// <returns></returns>
        public static LoggerConfiguration WriteToFilePath(this LoggerConfiguration logger, List<LogType> logType)
        {
            logType.ForEach(q =>
            {
                logger.WriteTo.Logger(lg =>
                {
                    lg.Filter.ByIncludingOnly(Matching.WithProperty<LogType>(SerilogHelper.seriCustomProperty, p => p == q));
                    lg.WriteTo.File(SerilogHelper.LogFilePath(q.ToString()), rollingInterval: RollingInterval.Day, fileSizeLimitBytes: SerilogHelper.fileSize, rollOnFileSizeLimit: true,
                        outputTemplate: SerilogHelper.logOutputTemplate);
                });
            });
            return logger;
        }
    }
}

