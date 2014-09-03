using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobStorage
{
	public class WebcastStorage
	{
		private static readonly CloudBlobContainer BlobContainer;
		private const string ContainerName = "cloudcastswebcasts";

		static WebcastStorage()
		{
			var connectionString = ConfigurationManager.ConnectionStrings["BlobCloudStorage"].ConnectionString;

			var account = CloudStorageAccount.Parse(connectionString);
			var blobClient = account.CreateCloudBlobClient();
			BlobContainer = blobClient.GetContainerReference(ContainerName);
			if (!BlobContainer.Exists())
			{
				BlobContainer.Create();
				var permissions = new BlobContainerPermissions
				{
					PublicAccess = BlobContainerPublicAccessType.Blob
				};
				BlobContainer.SetPermissions(permissions);
			}
		}

		public string UploadFromStream(Stream fileStream, string fileName, Webcast webcast)
		{
			var blob = BlobContainer.GetBlockBlobReference(fileName);
			blob.Properties.ContentType = "video/x-ms-wmv";
			blob.UploadFromStream(fileStream);
			var webcastUri = blob.Uri.AbsoluteUri;

			blob.Metadata.Add("Id", webcast.Id);
			blob.Metadata.Add("Title", webcast.Title);
			blob.Metadata.Add("Descirption", webcast.Descirption);
			blob.Metadata.Add("Presenter", webcast.Presenter);
			blob.Metadata.Add("DateAdded", webcast.DateAdded.ToString(CultureInfo.InvariantCulture));
			blob.Metadata.Add("PhotoUri", webcast.PhotoUri);
			blob.Metadata.Add("WebcastUri", webcastUri);

			blob.SetMetadata();
			return webcastUri;
		}

		public List<Webcast> GetWebcasts()
		{
			var casts = new List<Webcast>();
			var blobItems = BlobContainer.ListBlobs();

			foreach (var blobItem in blobItems)
			{
				var blob = BlobContainer.GetBlockBlobReference(blobItem.Uri.AbsoluteUri);
				blob.FetchAttributes();

				var cast = CreateWebcast(blob);
				casts.Add(cast);
			}
			return casts;
		}

		private static Webcast CreateWebcast(CloudBlockBlob blob)
		{
			var cast = new Webcast
			{
				Id = blob.Metadata["Id"],
				Title = blob.Metadata["Title"],
				Descirption = blob.Metadata["Descirption"],
				Presenter = blob.Metadata["Presenter"],
				DateAdded = DateTime.Parse(blob.Metadata["DateAdded"]),
				PhotoUri = blob.Metadata["PhotoUri"],
				WebcastUri = blob.Metadata["WebcastUri"],
			};
			return cast;
		}

		public Webcast GetWebcast(string uri)
		{
			var blob = BlobContainer.GetBlockBlobReference(uri);
			blob.FetchAttributes();

			return CreateWebcast(blob);
		}
	}
}