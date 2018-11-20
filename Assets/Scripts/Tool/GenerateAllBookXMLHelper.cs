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
        public string BookType { get; set; }
        public string XMLFile { get; set; }
        public string BookImage { get; set; }
        public string ClassType { get; set; }

    }
    public class GenerateAllBookHelper
    {
        /// <summary>  
        /// 创建图书信息XML  
        /// </summary>  
        /// <param name="fileName"></param>  
        public static void CreateBookXML(string fileName,string bookType, string classType, List<Book> bookList, Action callBack)
        {
            Debug.Log(fileName);
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
                    writer.WriteAttributeString("bookType", bookType);
                    writer.WriteAttributeString("classType", classType);
                    foreach (var item in bookList)
                    {
                        writer.WriteStartElement("book");
                        writer.WriteAttributeString("classType", item.ClassType);
                        writer.WriteAttributeString("Name", item.Name);
                        writer.WriteAttributeString("xmlFile", item.XMLFile);
                        writer.WriteAttributeString("bookImage", item.BookImage);
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