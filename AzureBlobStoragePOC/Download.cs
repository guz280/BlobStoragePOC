using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobStoragePOC.Models;
using System.IO;
using System.Threading.Tasks;

namespace AzureBlobStoragePOC
{
	static class Download
	{
		public static async Task<BlobDto> DownloadAsync(string blobFilename, string  storageConnectionString, string storageContainerName)
		{
            try
            {
                BlobContainerClient client = new BlobContainerClient(storageConnectionString, storageContainerName);

                // Get a reference to the blob uploaded earlier from the API in the container from configuration settings
                BlobClient file = client.GetBlobClient(blobFilename);

                // Check if the file exists in the container
                if (await file.ExistsAsync())
                {
                    var data = await file.OpenReadAsync();
                    Stream blobContent = data;

                    // Download the file details async
                    var content = await file.DownloadContentAsync();

                    // Add data to variables in order to return a BlobDto
                    string name = blobFilename;
                    string contentType = content.Value.Details.ContentType;

                    // Create new BlobDto with blob data from variables
                    return new BlobDto { Content = blobContent, Name = name, ContentType = contentType };
                }

                return null;
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                // Log error to console
                return null;
            }
        }
	}
}
