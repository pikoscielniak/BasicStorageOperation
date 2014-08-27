using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace DataContracts
{
	public static class SerializationUtils
	{
		public static string SerializeToString<T>(T objectToSerialize)
		{
			using (var memStm = new MemoryStream())
			{
				var serializer = new DataContractSerializer(typeof(T));
				var sb = new StringBuilder();
				using (var writer = XmlWriter.Create(sb))
				{
					serializer.WriteObject(writer, objectToSerialize);
					writer.Flush();
					return sb.ToString();
				}
			}
		}

		public static T Deserialize<T>(string rawXml)
		{
			using (var reader = XmlReader.Create(new StreamReader(rawXml)))
			{
				var formatter = new DataContractSerializer(typeof(T));
				return (T)formatter.ReadObject(reader);
			}
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