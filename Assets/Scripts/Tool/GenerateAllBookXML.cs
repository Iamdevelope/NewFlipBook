using PJW.Book;
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
        /// <param name="bookFile">书的XML文件路径</param>
        /// <param name="bookType">书的类型</param>
        /// <param name="callBack">回调函数</param>
        public static void GetBookContentByFile(string bookFile,string bookType, string classType, Action callBack)
        {
            books.Clear();

            Texture[] jiankang = Resources.LoadAll<Texture>("AllBookImage/" + bookType + "/" + classType + "/");

            if (jiankang != null)
            {
                foreach (var item in jiankang)
                {
                    if (item.ToString().EndsWith(".meta"))
                        continue;
                    Book book = new Book();
                    string pageName = item.ToString().Split('(')[0].TrimEnd();
                    book.ClassType = classType;
                    book.Name = pageName;
                    book.BookType = bookType;
                    book.XMLFile = pageName + ".xml";
                    book.BookImage = pageName + ".jpg";
                    books.Add(book);
                }
            }
            string savePath = GameCore.Instance.LocalXMLPath + bookType + "/" + classType + "/";
            Debug.Log(savePath);
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            GenerateAllBookHelper.CreateBookXML(bookFile, bookType, classType, books, callBack);
        }
    }
}