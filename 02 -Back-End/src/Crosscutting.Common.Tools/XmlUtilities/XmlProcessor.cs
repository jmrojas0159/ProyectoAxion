using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Crosscutting.Common.Tools.XmlUtilities
{
    public class XmlProcessor
    {
        private XmlSerializer _serializer;
        public T Deserialize<T>(string item)
        {
            _serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(item))
            {
                return (T)_serializer.Deserialize(reader);
            }
        }

        public string Serialize<T>(T item)
        {
            _serializer = new XmlSerializer(item.GetType());
            var result = new StringBuilder();
            var settings = new XmlWriterSettings { OmitXmlDeclaration = true };

            using (var writer = XmlWriter.Create(result, settings))
            {
                _serializer.Serialize(writer, item);
            }

            return result.ToString();
        }
    }
}
