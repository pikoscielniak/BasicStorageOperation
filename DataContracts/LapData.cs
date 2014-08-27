using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DataContracts
{
	[DataContract(Namespace = "http://somenamespace.com")]
	public class LapData
	{
		[DataMember(Order = 1)]
		public string PlayerName { get; set; }

		[DataMember(Order = 4)]
		public string LapId { get; set; }

		[DataMember(Order = 5)]
		public DateTime GameStarTime { get; set; }

		[DataMember(Order = 6)]
		public int LapTimeMs { get; set; }

		[DataMember(Order = 7)]
		public List<int> SectorTimesMs { get; set; }

		public LapData()
		{
			SectorTimesMs = new List<int>();
			GameStarTime = DateTime.UtcNow;
		}
	}
}