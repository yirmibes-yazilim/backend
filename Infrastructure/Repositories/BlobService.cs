using Azure.Storage.Blobs;
using backend.Application.Services;

namespace backend.Infrastructure.Repositories
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _container;
        public BlobService(IConfiguration config, BlobContainerClient container)
        {
            _container = container;
        }
        public async Task<string> UploadAsync(IFormFile file)
        {
            await _container.CreateIfNotExistsAsync();
            var extension = Path.GetExtension(file.FileName);
            var blobName = $"{Guid.NewGuid()}{extension}";
            var blob = _container.GetBlobClient(blobName);
            using var stream = file.OpenReadStream();
            await blob.UploadAsync(stream, overwrite: true);
            return blob.Uri.ToString();
        }

        public async Task DeleteAsync(string blobName)
        {
            await  _container.DeleteBlobIfExistsAsync(blobName);
        }
    }
}
