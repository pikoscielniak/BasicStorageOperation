using System;

namespace BlobStorage
{
	public class Webcast
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Descirption { get; set; }
		public string Presenter { get; set; }
		public DateTime DateAdded { get; set; }
		public string WebcastUri { get; set; }
		public string PhotoUri { get; set; }
	}
}