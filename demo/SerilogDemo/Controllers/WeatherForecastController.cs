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
            _logger.LogInformation("����һ����Ϣ�������־��Ϣ");
            _logger.LogWarning("����һ�����漶�����־��Ϣ");
            _logger.LogError("����һ�����󼶱����־��Ϣ");
            _logger.LogDebug("����һ�����Լ������־��Ϣ");

            var e = new Exception("0�쳣��");
            // ӛ��e�`ӍϢ
            Log.Logger.Error(e, "���Լ����{Message}",e.Message);

            var result= Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
            _logger.LogInformation("��Ӧ��{@Result}", result);
            _logger.LogInformation("��Ӧ2��{@result}", result);

            _logger.LogInformation("Hello world");//Ĭ�ϼ�¼��Default�ļ���
            _logger.LogInformation("{seriPos}��Hello world", LogType.Order);//��¼��Order�ļ���

            seriLogger.LogInformation("Hello world");//Ĭ�ϼ�¼��Default�ļ���
            seriLogger.LogInformation(LogType.Order, "Hello world");//��¼��Order�ļ���

            SerilogHelper.Info("Hello world");//Ĭ�ϼ�¼��Default�ļ���
            SerilogHelper.Info(LogType.Order, "Hello world");//��¼��Order�ļ���

            return result;
        }
    }
}
