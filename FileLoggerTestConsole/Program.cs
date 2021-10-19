using System;
using System.IO;

namespace FileLoggerTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Logs", "app.log");
            var LogFileContent = ReadLogFile(filePath);

            Console.WriteLine(LogFileContent);
            Console.ReadKey();
        }


        internal static string ReadLogFile(string filePath)
        {
            try
            {
                using (FileStream stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (IOException ex)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return ex.ToString();
            }
        }
    }
}
