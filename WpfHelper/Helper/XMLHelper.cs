using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace WPFHelper
{
	public class XMLHelper
	{
		public static string FormatXml(string xmlString)
		{
			try
			{
				XmlDocument doc = new XmlDocument();

				doc.LoadXml(xmlString);

				StringBuilder sb = new StringBuilder();

				System.IO.TextWriter tr = new System.IO.StringWriter(sb);

				XmlTextWriter wr = new XmlTextWriter(tr);

				wr.Formatting = Formatting.Indented;

				doc.Save(wr);

				wr.Close();

				return sb.ToString();
			}
			catch
			{
				return xmlString;
			}
		}

		public static string ConvertXMLbyXSLT(string xmlContent, string xsltContent)
		{
			try
			{
				string input = xmlContent;

				var xmlWriterSettings = new XmlWriterSettings();
				xmlWriterSettings.ConformanceLevel = ConformanceLevel.Fragment;
				var propInfo = xmlWriterSettings.GetType().GetProperty("OutputMethod");
				propInfo.SetValue(xmlWriterSettings, System.Xml.XmlOutputMethod.Xml, null);

				using (StringReader stringReader = new StringReader(input))
				using (XmlReader xReader = XmlReader.Create(stringReader))
				using (StringWriter sWriter = new StringWriter())
				using (XmlWriter xWriter = XmlWriter.Create(sWriter, xmlWriterSettings))
				{
					var xslt = new XslCompiledTransform();

					xslt.Load(new XmlTextReader(new StringReader(xsltContent)));

					xslt.Transform(xReader, xWriter);

					return sWriter.ToString();
				}
			}
			catch
			{
				return string.Empty;
			}
		}
	}
}
