using System;
using Azure.Storage.Blobs;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using XamarinLogic.Utils;

namespace XamarinLogic.Services
{
	public class Azure
	{

		static Azure Instance { get; set; }

		public static void Init()
		{
			if(Ext.HasValue(Instance))
			{
				return;
			}

			Instance = new Azure();
			Initializing.SetResult(Instance);
		}

		static TaskCompletionSource<Azure> Initializing { get; } = new TaskCompletionSource<Azure>();

		public static Task<Azure> GetAsync()
		{
			return Initializing.Task;
		}



		string ConnectionString => Secret.Secrets.Seek.AZURE_CONNECTION_STRING;
		BlobServiceClient ServiceClient { get; set; }
		BlobContainerClient Container { get; set; }
		BlobClient BlobClient { get; set; }
		protected Azure()
		{
			ServiceClient = new BlobServiceClient(ConnectionString);
			Container = ServiceClient.GetBlobContainerClient("seek-resources");
			BlobClient = Container.GetBlobClient("seek_website_meta.json");
		}

		public async Task<WebsiteMeta> GetWebsiteMeta()
		{
			using (var stream = new MemoryStream())
			{
				await BlobClient.DownloadToAsync(stream);
				stream.Position = 0;//resetting stream's position to 0
				var serializer = new JsonSerializer();

				using (var sr = new StreamReader(stream))
				{
					using (var jsonTextReader = new JsonTextReader(sr))
					{
						var result = serializer.Deserialize<WebsiteMeta>(jsonTextReader);
						return result;
					}
				}
			}
		}

	}

	// models

	public class WebsiteMeta
	{
		public string url { get; set; }
		public bool shouldUsePayment { get; set; } = true;
	}
}