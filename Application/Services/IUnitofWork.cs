namespace backend.Application.Services
{
    public interface IUnitofWork : IDisposable
    {
        Task CommitAsync();
    }
}
