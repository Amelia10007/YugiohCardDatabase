using System;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace YugiohCardDatabase
{
    /// <summary>
    /// JSONとオブジェクトとの相互変換機能を提供する．
    /// </summary>
    public static class Json
    {
        private static readonly Encoding encoding = Encoding.UTF8;

        private static readonly DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings()
        {
            UseSimpleDictionaryFormat = true,
            SerializeReadOnlyTypes = true
        };

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">変換対象の型．この型はDataContract属性を持っている必要がある．</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="InvalidDataContractException"></exception>
        /// <exception cref="SerializationException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static string Serialize<T>(T source)
        {
            var serializer = new DataContractJsonSerializer(typeof(T), settings);
            using var stream = new MemoryStream();
            using (var writer = JsonReaderWriterFactory.CreateJsonWriter(
                stream: stream,
                encoding: encoding,
                ownsStream: true,
                indent: false,
                indentChars: " "))
            {
                serializer.WriteObject(writer, source);
                writer.Flush();
            }
            return encoding.GetString(stream.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">変換対象の型．この型はDataContract属性を持っている必要がある．</typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        /// <exception cref="EncoderFallbackException"></exception>
        public static T Deserialize<T>(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(T), settings);
            var bytes = encoding.GetBytes(json);
            using var stream = new MemoryStream(bytes);
            return (T)serializer.ReadObject(stream);
        }
    }
}
