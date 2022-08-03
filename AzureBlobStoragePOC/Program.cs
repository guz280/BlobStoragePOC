using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureBlobStoragePOC
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=novicd;AccountKey=HN3/VCYYfjZprGYSL7zoiyM4unusN+O+wQ4AWdeRDDDi9CD7GThAtzq6uFtRTadqn2IiZR9P2tco+ASttx/lNw==;EndpointSuffix=core.windows.net";
			string storageContainerName = "services-customerdocuments";

			string blobFilename = "test1";


			var filePath = @"C:\development\POC\BlobStoragePOC\BlobStoragePOC\AzureBlobStoragePOC\image1.png";
			byte[] file = File.ReadAllBytes(filePath);
			Stream stream = new MemoryStream(file);

			var upload = await Upload.UploadAsync(stream, blobFilename, storageConnectionString, storageContainerName);


			var download = await Download.DownloadAsync(blobFilename, storageConnectionString, storageContainerName);

			
			Console.ReadLine();
		}
	}
}
