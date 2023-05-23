using Azure.Storage.Blobs;
using SaritasaT.DTOs.Item;
using System.IO;

namespace SaritasaT.Services
{
    public interface IBlobService
    {
        Task<BlobResponse> UploadFile(IFormFile file);
        Task<List<Blob>> GetAllFiles();
        Task<Blob?> DownloadFile(string fileName);
        Task<BlobResponse> DeleteFile(string fileName);
    }
    public class BlobService : IBlobService
    {
        private readonly string _connectionString;
        private readonly BlobContainerClient _containerClient;
        public BlobService()
        {
            _connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_BLOB_KEY");
            _containerClient = new BlobContainerClient(_connectionString, Environment.GetEnvironmentVariable("ASPNETCORE_BLOB_CONTAINER"));
        }

        public async Task<List<Blob>> GetAllFiles()
        {
           List <Blob> files= new List<Blob>();
            await foreach(var file in _containerClient.GetBlobsAsync())
            {
                string uri = _containerClient.Uri.ToString();
                var name=file.Name;
                var fullUri = $"{uri}/{name}";
                files.Add(new Blob
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }
            return files;
        }

        public async Task<Blob?> DownloadFile(string fileName)
        {
            BlobClient file = _containerClient.GetBlobClient(fileName);
            if(await file.ExistsAsync())
            {
                var data = await file.OpenReadAsync();
                Stream blobContent = data;
                var content=await file.DownloadContentAsync();

                string name = fileName;
                string contentType = content.Value.Details.ContentType;

                return new Blob { ContentType = contentType, Name = name ,Content=blobContent};
            }
            return null;
        }

        public async Task<BlobResponse> UploadFile(IFormFile file)
        {
            try
            {
                BlobResponse response = new();
                BlobClient client = _containerClient.GetBlobClient(file.FileName);
                using (Stream? data = file.OpenReadStream())
                {
                    await client.UploadAsync(data);
                }
                response.Status = $"File {file.FileName} Uploaded Successfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = file.Name;
                return response;
            }
            catch
            {
                throw;
            }
        }
        public async Task<BlobResponse> DeleteFile(string fileName)
        {
            BlobClient file=_containerClient.GetBlobClient(fileName);
            await file.DeleteAsync();
            return new BlobResponse { Error = false, Status = $"File {fileName} Deleted Successfully" };
        }
    }
}
