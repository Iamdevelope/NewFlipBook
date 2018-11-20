using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

namespace PJW.Common
{
    public class Book
    {
        public string Name { get; set; }
        public string xmlFile { get; set; }
        public string bookImage { get; set; }

    }
    public class GenerateAllBookHelper
    {
        /// <summary>  
        /// 创建图书信息XML  
        /// </summary>  
        /// <param name="fileName"></param>  
        public static void CreateBookXML(string fileName, List<Book> bookList, string bookName, Action callBack)
        {
            if (!File.Exists(fileName))
                File.Create(fileName).Dispose();
            try
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Create);
                //XmlWriter writer = XmlWriter.Create(fileName);
                XmlTextWriter writer = new XmlTextWriter(fileStream, Encoding.UTF8);
                if (bookList != null && bookList.Count > 0)
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Book");    //创建父节点
                    foreach (var item in bookList)
                    {
                        writer.WriteStartElement("book");
                        writer.WriteAttributeString("Name", item.Name);
                        writer.WriteAttributeString("xmlFile", item.xmlFile);
                        writer.WriteAttributeString("bookImage", item.bookImage);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();    //父节点结束  
                }
                writer.WriteEndDocument();
                writer.Close();
                fileStream.Close();
                if (callBack != null)
                    callBack();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

        }
    }
}