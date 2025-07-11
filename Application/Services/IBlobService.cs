namespace backend.Application.Services
{
    public interface IBlobService
    {
        Task<string> UploadAsync(IFormFile file);
        Task DeleteAsync(string blobName);
    }
}
