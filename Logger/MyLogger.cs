using Serilog;

namespace backend.Loger
{
    public class MyLogger: IMyLogger
    {
        public void LogInformation(string message)
        {
            Log(message, "INFO");
        }

        public void LogWarning(string message)
        {
            Log(message, "WARN");
        }

        public void LogError(string message)
        {
            Log(message, "ERROR");
        }

        public void LogSuccess(string message)
        {
            Log(message, "SUCCESS");
        }


        private void Log(string message, string logLevel)
        {
            Directory.CreateDirectory("Logs");

            string fileName = $"log-{DateTime.Now:ddMMyy}.txt";
            string filePath = Path.Combine("Logs", fileName);

            string logLine = $"{DateTime.Now:HH:mm:ss} [{logLevel}] {message}";

            File.AppendAllText(filePath, logLine + Environment.NewLine);
        }



    }
}

