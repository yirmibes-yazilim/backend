namespace backend.Loger
{
    public interface IMyLogger
    {

        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogSuccess(string message);
    }
}
