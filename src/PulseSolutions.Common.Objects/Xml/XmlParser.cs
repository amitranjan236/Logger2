
using System;
using System.IO;
using System.Xml.Serialization;

namespace PulseSolutions.Common.Objects.Xml
{
    /// <summary>
    /// XmlParser class. Holds member funtion for Serializing and Deserialing of objects.
    /// </summary>
    public static class XmlParser
    {
       
        /// <summary>
        /// Serializes a manage object of type T to xml string
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="item">instance of object to serialize</param>
        /// <returns>xml string of type T </returns>
        public static string Serialize<T>(T item)
        {
            string empty = string.Empty;
            if ((object)item == null)
                return empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                new XmlSerializer(typeof(T)).Serialize(stringWriter, (object)item);
                empty = stringWriter.ToString();
            }
            return empty;
        }

        /// <summary>
        /// Deserializes xml to object of type T
        /// </summary>
        /// <typeparam name="T">Type/Class </typeparam>
        /// <param name="xml">raw xml</param>
        /// <returns>Object of type T</returns>
        public static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                throw new Exception("Invalid or blank xml.");
            T obj;
            using (StringReader stringReader = new StringReader(xml))
                obj = (T)new XmlSerializer(typeof(T)).Deserialize(stringReader);
            return obj;
        }
    }
}
