using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

namespace PJW.Common
{
    public class BookInfo
    {
        public string pageName { get; set; }
        public int count { get; set; }
        public int spriteCount { get; set; }
        public int videoCount { get; set; }
        public int uiButtonSpriteCount { get; set; }
        public string sound { get; set; }


        public List<string> uiSprite = new List<string>();
        public List<string> objectName = new List<string>();
        public List<string> spriteName = new List<string>();
        public List<string> videoName = new List<string>();
    }
    /// <summary>
    /// 自动生成XML文件
    /// </summary>
    public class GenerateXMLHelper
    {
        /// <summary>  
        /// 创建图书信息XML  
        /// </summary>  
        /// <param name="fileName"></param>  
        public static void CreateBookXML(string fileName,List<BookInfo> bookList,string bookName,Action callBack)
        {
            FileStream fileStream = null;
            if (!File.Exists(fileName))
                fileStream = File.Create(fileName);
            else
                fileStream = new FileStream(fileName, FileMode.Create);
            try
            {
                //fileStream = new FileStream(fileName, FileMode.Create);
                //XmlWriter writer = XmlWriter.Create(fileName);
                XmlTextWriter writer = new XmlTextWriter(fileStream, Encoding.UTF8);
                if (bookList != null && bookList.Count > 0)
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("book");    //创建父节点

                    writer.WriteStartElement("Pages");
                    writer.WriteAttributeString("bookName", bookName.Split('.')[0]);
                    foreach (BookInfo book in bookList)
                    {
                        writer.WriteStartElement("page");    //创建子节点  
                        writer.WriteAttributeString("pageName", book.pageName);    //添加属性  
                        writer.WriteAttributeString("count", book.count.ToString());
                        writer.WriteAttributeString("spriteCount", book.spriteCount.ToString());
                        writer.WriteAttributeString("sound", book.sound);
                        writer.WriteAttributeString("videoCount", book.videoCount.ToString());
                        writer.WriteAttributeString("uiButtonSpriteCount", book.uiButtonSpriteCount.ToString());

                        if (book.count > 0)
                        {
                            for (int i = 0; i < book.objectName.Count; i++)
                            {
                                writer.WriteStartElement("gameObject");
                                writer.WriteAttributeString("objectName", book.objectName[i]);
                                writer.WriteEndElement();
                            }
                        }
                        if (book.spriteCount > 0)
                        {
                            for (int i = 0; i < book.spriteName.Count; i++)
                            {
                                writer.WriteStartElement("sprite");
                                writer.WriteAttributeString("spriteName", book.spriteName[i]);
                                writer.WriteEndElement();
                            }
                        }
                        if (book.uiButtonSpriteCount > 0)
                        {
                            for (int i = 0; i < book.uiSprite.Count; i++)
                            {
                                writer.WriteStartElement("uiButtonSprite");
                                writer.WriteAttributeString("uiButtonSpriteName", book.uiSprite[i]);
                                writer.WriteEndElement();
                            }
                        }
                        if (book.videoCount > 0)
                        {
                            for (int i = 0; i < book.videoName.Count; i++)
                            {
                                writer.WriteStartElement("video");
                                writer.WriteAttributeString("videoName", book.videoName[i]);
                                writer.WriteEndElement();
                            }
                        }
                        
                        writer.WriteEndElement();    //子节点结束  
                    }
                    writer.WriteEndElement();    //父节点结束  
                }
                writer.WriteEndDocument();
                writer.Close();
                
                fileStream.Close();
                fileStream.Dispose();
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