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
				Response<BlobContentInfo> r = null;

				// https://stackoverflow.com/questions/17043631/using-stream-read-vs-binaryreader-read-to-process-binary-streams
				// use a stream when you have(only) byte[] to move.As is common in a lot of streaming scenarios.
				// use BinaryWriter and BinaryReader when you have any other basic type(including simple byte) of
				// data to process.
				// Their main purpose is conversion of the built-in framework types to byte[].


				using (Stream? data = stream)//.OpenReadStream())
				{
					// Upload the file async
					r = await blob.UploadAsync(data, overwrite: false);
					//data.Dispose();
				}

				if (r.GetRawResponse().Status == 201 && r.GetRawResponse().IsError == false)
					return true;



				//var r2 = await blob.UploadAsync(BinaryData.FromStream(stream));

				return false;
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
				return false;
			}

		}




		public static async Task<bool> GetList(string blobSasUrl, string blobName)
		{
			try
			{
				Uri uri = new Uri(blobSasUrl);
				BlobContainerClient container = new BlobContainerClient(uri);

				//Return a reference to a blob to be created in the container.
				var blob1 = container.GetBlobs();


				BlobClient blob = container.GetBlobClient(blobName);

				//BlobDownloadInfo download = blob.Download();
				bool fileExists = await blob.ExistsAsync();

				//var r = await container.ExistsAsync();

				return true;
			}
			catch (Exception)
			{

				throw;
			}

		}

	}
}
