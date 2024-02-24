using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ShipShareAPI.Application.Interfaces.Services;
using ShipShareAPI.Persistence.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Persistence.Concretes.Services
{
    public class UploadImageToStorageService : IUploadImageToStorageService
    {
        private readonly AzureOptions _azureOptions;

        public UploadImageToStorageService(IOptions<AzureOptions> azureOptions)
        {
            _azureOptions = azureOptions.Value;
        }
        public string UploadImageToStorage(IFormFile formFile)
        {
            var fileExtension = Path.GetExtension(formFile.FileName);
            using var memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            memoryStream.Position = 0;
            BlobContainerClient client = new(_azureOptions.ConnectionString,_azureOptions.Container);
            var uniqueName = Guid.NewGuid().ToString() + fileExtension;
            var blobClient = client.GetBlobClient(uniqueName);
            blobClient.Upload(memoryStream, new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders()
                {
                    ContentType = "image/bitmap"
                }
            },cancellationToken : default);
            var imageUrl = blobClient.Uri.ToString();
            return imageUrl;
        }
    }
}
