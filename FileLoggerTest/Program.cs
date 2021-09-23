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

                 //регистрация сервиса Karambolo.Extensions.Logging.File для записи лога в файл (события уровня от LogLevel.Warning и выше)
                 logging.AddFile("Logs/app.log", options =>
                 {
                     //дописывать в конец файла
                     options.Append = true;
                     //писать только события уровня от loglevel.warning и выше
                     options.MinLevel = LogLevel.Warning;
                     //размер одного файла лога не более 5мб (далее будет создан новый файл app1.log, потом app2.log и т. д.)
                     options.FileSizeLimitBytes = 5 * 1024 * 1024;
                     //ограничение по максимальному количеству таких файлов - пока что 1000 штук
                     options.MaxRollingFiles = 1000;
                 });

                 //регистрация сервиса NReco.Logging.File для записи лога в файл(события уровня от LogLevel.Warning и выше)
                 //logging.AddFile("Logs/app.log", options =>
                 //{
                 //    //дописывать в конец файла
                 //    options.Append = true;
                 //    //писать только события уровня от LogLevel.Warning и выше
                 //    options.MinLevel = LogLevel.Warning;
                 //    //размер одного файла лога не более 5МБ (далее будет создан новый файл app1.log, потом app2.log и т. д.)
                 //    options.FileSizeLimitBytes = 5 * 1024 * 1024;
                 //    //ограничение по максимальному количеству таких файлов - пока что 1000 штук
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
