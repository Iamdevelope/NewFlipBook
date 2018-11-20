using System;
using System.IO;
using System.Xml.Serialization;

namespace PJW.Common
{
    /// <summary>
    /// XML序列化工具类
    /// </summary>
    public class XmlSerializeHelper
    {
        /// <summary>
        /// XML序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string XmlSerialize<T>(T obj)
        {
            using (StringWriter sw = new StringWriter())
            {
                Type t = obj.GetType();
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(sw, obj);
                sw.Close();
                return sw.ToString();
            }
        }
        /// <summary>
        /// XML文件反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strXML"></param>
        /// <returns></returns>
        public static T XmlDeSerialize<T>(string strXML) where T : class
        {
            try
            {
                using(StringReader sr=new StringReader(strXML))
                {
                    XmlSerializer serialize = new XmlSerializer(typeof(T));
                    return serialize.Deserialize(sr) as T;
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
