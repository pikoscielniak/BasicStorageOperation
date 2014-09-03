using System.Configuration;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobStorage
{
	public class PhotoStorage
	{
		private static readonly CloudBlobContainer BlobContainer;
		private const string ContainerName = "cloudcastsphotos";

		static PhotoStorage()
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

		public string UploadFromStream(Stream fileStream, string fileName)
		{
			var blob = BlobContainer.GetBlockBlobReference(fileName);
			blob.Properties.ContentType = "image/pjpeg";
			blob.UploadFromStream(fileStream);
			return blob.Uri.AbsoluteUri;
		}
	}
}