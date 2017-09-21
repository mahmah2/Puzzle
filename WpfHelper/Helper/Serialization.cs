using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DematicMenuPreview.Helper
{
	public class Serialization
	{
		public static T XmlStringToObject<T>(string xml)
		{
			using (Stream stream = new MemoryStream())
			{
				byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
				stream.Write(data, 0, data.Length);
				stream.Position = 0;
				DataContractSerializer deserializer = new DataContractSerializer(typeof(T));
				return (T)deserializer.ReadObject(stream);
			}
		}

		public static string ObjectToXmlString(object obj)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			using (StreamReader reader = new StreamReader(memoryStream))
			{
				DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
				serializer.WriteObject(memoryStream, obj);
				memoryStream.Position = 0;
				return reader.ReadToEnd();
			}
		}
	}
}
