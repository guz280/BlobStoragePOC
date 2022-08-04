using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBlobStoragePOCConsole
{
	static class SASImplementation
	{
		public static async Task<bool> UploadAsync(string blobSasUrl, string blobName, Stream stream)
		{
			try
			{
				Uri uri = new Uri(blobSasUrl);
				BlobContainerClient container = new BlobContainerClient(uri);
								
				//Return a reference to a blob to be created in the container.
				BlobClient blob = container.GetBlobClient(blobName);
				// Open a stream for the file we want to upload
				await using (Stream? data = stream)//.OpenReadStream())
				{
					// Upload the file async
					var r = await blob.UploadAsync(data);
				}

				//blob.Upload(BinaryData.FromStream(stream));

				return true;
			}
			catch (Exception)
			{
				return false;
			}
			
		}


		public static async Task<Response<BlobDownloadResult>> DownloadAsync(string blobSasUrl, string blobName)
		{
			try
			{
				Uri uri = new Uri(blobSasUrl);
				BlobContainerClient container = new BlobContainerClient(uri);

				//Return a reference to a blob to be created in the container.
				BlobClient blob = container.GetBlobClient(blobName);

				//BlobDownloadInfo download = blob.Download();
				bool fileExists = await blob.ExistsAsync();
				if (fileExists)
				{
					var data = await blob.OpenReadAsync();
					Stream blobContent = data;

					// Download the file details async
					Response<BlobDownloadResult> contentResult = await blob.DownloadContentAsync();

					// Add data to variables in order to return a BlobDto
					string name = blobName;
					string contentType = contentResult.Value.Details.ContentType;

					return contentResult;
				}

				return null;
			}
			catch (Exception)
			{
				return null;
			}

		}


		public static async Task<bool> Delete(string blobSasUrl, string blobName)
		{
			try
			{
				Uri uri = new Uri(blobSasUrl);
				BlobContainerClient container = new BlobContainerClient(uri);

				//Return a reference to a blob to be created in the container.
				BlobClient blob = container.GetBlobClient(blobName);

				blob.Delete();

				return true;
			}
			catch (Exception)
			{

				throw;
			}

		}

	}
}
