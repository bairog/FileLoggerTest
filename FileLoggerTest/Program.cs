using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FileLoggerTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            ILogger logger = host.Services.GetService<ILogger<Program>>();
            logger.LogWarning("Test warning");

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.
             ConfigureLogging((ctx, logging) =>
             {
                 // clear default logging providers
                 logging.ClearProviders();

                 //����������� ������� Karambolo.Extensions.Logging.File ��� ������ ���� � ���� (������� ������ �� LogLevel.Warning � ����)
                 logging.AddFile("Logs/app.log", options =>
                 {
                     //���������� � ����� �����
                     options.Append = true;
                     //������ ������ ������� ������ �� loglevel.warning � ����
                     options.MinLevel = LogLevel.Warning;
                     //������ ������ ����� ���� �� ����� 5�� (����� ����� ������ ����� ���� app1.log, ����� app2.log � �. �.)
                     options.FileSizeLimitBytes = 5 * 1024 * 1024;
                     //����������� �� ������������� ���������� ����� ������ - ���� ��� 1000 ����
                     options.MaxRollingFiles = 1000;
                 });

                 //����������� ������� NReco.Logging.File ��� ������ ���� � ����(������� ������ �� LogLevel.Warning � ����)
                 //logging.AddFile("Logs/app.log", options =>
                 //{
                 //    //���������� � ����� �����
                 //    options.Append = true;
                 //    //������ ������ ������� ������ �� LogLevel.Warning � ����
                 //    options.MinLevel = LogLevel.Warning;
                 //    //������ ������ ����� ���� �� ����� 5�� (����� ����� ������ ����� ���� app1.log, ����� app2.log � �. �.)
                 //    options.FileSizeLimitBytes = 5 * 1024 * 1024;
                 //    //����������� �� ������������� ���������� ����� ������ - ���� ��� 1000 ����
                 //    options.MaxRollingFiles = 1000;
                 //});


                 // add built-in providers manually, as needed 
                 logging.AddConsole();
                 logging.AddDebug();
                 logging.AddEventLog();
                 logging.AddEventSourceLogger();
             }).
             // UseIIS(). /// Uses when Inprocess IIS
             UseIISIntegration(). /// Uses when outoftheprocess IIS
            CaptureStartupErrors(true). /// adding logs
            UseStartup<Startup>();
                });
    }
}
