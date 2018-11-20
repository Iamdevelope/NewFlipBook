using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PJW.Common
{
    public class GenerateAllBookXML
    {
        public static List<Book> books = new List<Book>();
        /// <summary>
        /// 通过书本文件夹生成对应的XML文件,通过Resources进行加载
        /// </summary>
        /// <param name="bookFile"></param>
        public static void GetBookContentByFile(string bookFile,Action callBack)
        {
            string path = bookFile.Split('.')[0];
            string bookName = path;
            Texture[] images = Resources.LoadAll<Texture>("AllBookImage/");
            if (images != null)
            {
                foreach (var item in images)
                {
                    if (item.ToString().EndsWith(".meta"))
                        continue;
                    Book book = new Book();
                    string pageName = item.ToString().Split('(')[0].TrimEnd();
                    book.Name = pageName;
                    book.xmlFile = pageName + ".xml";
                    book.bookImage = pageName + ".jpg";
                    books.Add(book);
                }
            }
            string savePath = Application.persistentDataPath + "/Books/XMLContent/";
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            GenerateAllBookHelper.CreateBookXML(savePath + "/" + bookFile, books, bookFile, callBack);
        }
    }
}