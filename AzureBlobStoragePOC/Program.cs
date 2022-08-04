using Azure.Storage.Blobs;
using AzureBlobStoragePOCConsole;
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
			// common 
			var filePath = @"C:\development\POC\BlobStoragePOC\BlobStoragePOC\AzureBlobStoragePOC\Capture.PNG";
			byte[] file = File.ReadAllBytes(filePath);
			Stream stream = new MemoryStream(file);


			// using DefaultEndpointsProtocol connection string (Key1 or Key2) - This has full access to also delete
			//string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=novicd;AccountKey=CkY1PNN0ND5rLbdxg30SHev/gnJymqmqCYtucHJ+R81nVIaWyQzPFlgV9qDOfHD6R+r7M7pc0dnN+AStiJuA9w==;EndpointSuffix=core.windows.net";
			//string storageContainerName = "services-customerdocuments";
			//string blobFilename = "test2";

			//var upload = await Upload.UploadAsync(stream, blobFilename, storageConnectionString, storageContainerName);
			//var download = await Download.DownloadAsync(blobFilename, storageConnectionString, storageContainerName);

			// ---------------------------------------------------------------------------------------
			// ---------------------------------------------------------------------------------------
			// ---------------------------------------------------------------------------------------
			// ---------------------------------------------------------------------------------------




			// Using SAS - gives you more control of the access to give
			string blobSasUrl = "https://novicd.blob.core.windows.net/services-customerdocuments?sp=rcl&st=2022-08-03T10:07:31Z&se=2023-08-03T18:07:31Z&spr=https&sv=2021-06-08&sr=c&sig=zPF%2BbrsIRlP1oWhPQ3B6yvlLS%2BmO2%2BBg0KaeTjg5DVY%3D";
			string blobName = "Trial1";

			var sasUpload = await SASImplementation.UploadAsync(blobSasUrl, blobName, stream);
			var sasDownloadContent = await SASImplementation.DownloadAsync(blobSasUrl, blobName);
			// save to disk
			File.WriteAllBytes("TaghnaLiGibnaLura.png", sasDownloadContent.Value.Content.ToArray());

			var delete = await SASImplementation.Delete(blobSasUrl, blobName);


			var sasGetList = await SASImplementation.GetList(blobSasUrl);



			Console.ReadLine();
		}
	}
}
