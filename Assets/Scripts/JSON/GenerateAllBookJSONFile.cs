
using PJW.Book;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using System.Text;

namespace PJW.Json
{

    [SerializeField]
    public class Book
    {
        public string Name { get; set; }
        public string ConfigFile { get; set; }
        public string BookImage { get; set; }

    }
    [SerializeField]
    public class BookType
    {
        public string BookTypeName { get; set; }
        public List<ClassType> ClassTypes { get; set; }
    }
    [SerializeField]
    public class ClassType
    {
        public string ClassTypeName { get; set; }
        public List<Book> Book { get; set; }
    }
    [SerializeField]
    public class Books
    {
        public List<BookType> BookTypes { get; set; }
    }

    public class GenerateAllBookJSONFile
    {
        public static Books books;
        public static List<BookType> bookTypes = new List<BookType>();
        private static int num;
        /// <summary>
        /// 通过书本文件夹生成对应的JSON文件,通过Resources进行加载
        /// </summary>
        /// <param name="bookFile">书的JSON文件路径</param>
        /// <param name="callBack">回调函数</param>
        public static void GetBookContentByFile(string bookFile,Action callBack)
        {
            if (Application.platform == RuntimePlatform.Android && Application.platform != RuntimePlatform.WindowsEditor)
                num = 10;
            else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
                num = 9;
            books = new Books();
            bookTypes.Clear();
            Debug.Log(" the num is " + num);
            string temp = GameCore.Instance.LocalConfigPath + "/AllBookImage";
            try
            {
                string[] allBookType = Directory.GetDirectories(temp);
                if (allBookType.Length > 0)
                {
                    for (int i = 0; i < allBookType.Length; i++)
                    {
                        allBookType[i] = allBookType[i].Replace('\\', '/');
                        BookType bt = new BookType();
                        bt.BookTypeName = allBookType[i].Split('/')[num].Split('.')[0];
                        bt.ClassTypes = new List<ClassType>();
                        string[] allClassType = Directory.GetDirectories(temp + "/" + bt.BookTypeName);
                        if (allClassType.Length > 0)
                        {
                            for (int j = 0; j < allClassType.Length; j++)
                            {
                                allClassType[j] = allClassType[j].Replace('\\', '/');
                                ClassType ct = new ClassType();
                                ct.ClassTypeName = allClassType[j].Split('/')[num+1].Split('.')[0];
                                ct.Book = new List<Book>();
                                string[] textNames = Directory.GetFiles(temp + "/" + bt.BookTypeName + "/" + ct.ClassTypeName);
                                if (textNames.Length > 0)
                                {
                                    for (int k = 0; k < textNames.Length; k++)
                                    {
                                        if (textNames[k].EndsWith(".meta")) continue;
                                        textNames[k] = textNames[k].Replace('\\', '/');
                                        Book b = new Book();
                                        b.Name = textNames[k].Split('/')[num + 2].Split('.')[0];
                                        b.ConfigFile = GameCore.Instance.BookOfConfig + b.Name + ".json";
                                        b.BookImage = textNames[k];
                                        ct.Book.Add(b);
                                    }
                                }
                                bt.ClassTypes.Add(ct);
                            }
                        }
                        bookTypes.Add(bt);
                        books.BookTypes = bookTypes;
                    }
                }
                if (!Directory.Exists(GameCore.Instance.BookOfConfig))
                    Directory.CreateDirectory(GameCore.Instance.BookOfConfig);
                CreateBookJSON(bookFile, books, callBack);
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }

        }
        /// <summary>  
        /// 创建图书信息XML  
        /// </summary>  
        /// <param name="fileName"></param>  
        public static void CreateBookJSON(string fileName, Books books,Action callBack)
        {
            Debug.Log(fileName);
            if (!File.Exists(fileName))
                File.Create(fileName).Dispose();
            try
            {
                StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8);
                string str = JsonMapper.ToJson(books);
                writer.Write(str);
                writer.Close();
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