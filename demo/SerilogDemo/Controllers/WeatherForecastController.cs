using Microsoft.AspNetCore.Mvc;
using Serilog;
using static SerilogDemo.SerilogHelper;

namespace SerilogDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ISeriLogger seriLogger;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, ISeriLogger seriLogger)
        {
            _logger = logger;
            this.seriLogger = seriLogger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("这是一个信息级别的日志消息");
            _logger.LogWarning("这是一个警告级别的日志消息");
            _logger.LogError("这是一个错误级别的日志消息");
            _logger.LogDebug("这是一个调试级别的日志消息");

            var e = new Exception("0异常啦");
            // 記錄錯誤訊息
            Log.Logger.Error(e, "我自己造的{Message}",e.Message);

            var result= Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
            _logger.LogInformation("响应：{@Result}", result);
            _logger.LogInformation("响应2：{@result}", result);

            _logger.LogInformation("Hello world");//默认记录到Default文件夹
            _logger.LogInformation("{seriPos}：Hello world", LogType.Order);//记录到Order文件夹

            seriLogger.LogInformation("Hello world");//默认记录到Default文件夹
            seriLogger.LogInformation(LogType.Order, "Hello world");//记录到Order文件夹

            SerilogHelper.Info("Hello world");//默认记录到Default文件夹
            SerilogHelper.Info(LogType.Order, "Hello world");//记录到Order文件夹

            return result;
        }
    }
}
