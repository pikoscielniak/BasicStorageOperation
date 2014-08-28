using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Newtonsoft.Json;

namespace DataContracts
{
	public static class SerializationUtils
	{
		public static string SerializeToString(object objectToSerialize)
		{
			return JsonConvert.SerializeObject(objectToSerialize);
		}

		public static T Deserialize<T>(string raw)
		{
			return JsonConvert.DeserializeObject<T>(raw);
		}

		public static byte[] SerializeToByteArray<T>(T obj)
		{
			var serializer = new DataContractSerializer(typeof(T));
			var stream = new MemoryStream();
			using (var writer = XmlDictionaryWriter.CreateBinaryWriter(stream))
			{
				serializer.WriteObject(writer, obj);
			}
			return stream.ToArray();
		}

		public static T Deserialize<T>(byte[] data)
		{
			var serializer = new DataContractSerializer(typeof(T));
			using (var stream = new MemoryStream(data))
			using (var reader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max))
			{
				return (T)serializer.ReadObject(reader);
			}
		}
	}
}