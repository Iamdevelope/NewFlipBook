
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
        public string XMLFile { get; set; }
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
        /// <summary>
        /// 通过书本文件夹生成对应的JSON文件,通过Resources进行加载
        /// </summary>
        /// <param name="bookFile">书的JSON文件路径</param>
        /// <param name="callBack">回调函数</param>
        public static void GetBookContentByFile(string bookFile,Action callBack)
        {
            books = new Books();
            bookTypes.Clear();
            string[] allBookType = Directory.GetFiles(Application.dataPath + "/Resources/AllBookImage/");
            if (allBookType.Length > 0)
            {
                for (int i = 0; i < allBookType.Length; i++)
                {
                    BookType bt = new BookType();
                    bt.BookTypeName = allBookType[i].Split('/')[6].Split('.')[0];
                    bt.ClassTypes = new List<ClassType>();
                    string[] allClassType = Directory.GetFiles(allBookType[i].Split('.')[0] + "/");
                    if (allClassType.Length > 0)
                    {
                        for (int j = 0; j < allClassType.Length; j++)
                        {
                            ClassType ct = new ClassType();
                            ct.ClassTypeName = allClassType[j].Split('/')[7].Split('.')[0];
                            ct.Book = new List<Book>();
                            Texture[] textures = Resources.LoadAll<Texture>("AllBookImage/" + bt.BookTypeName + "/" + ct.ClassTypeName + "/");
                            if (textures.Length > 0)
                            {
                                for (int k = 0; k < textures.Length; k++)
                                {
                                    Book b = new Book();
                                    b.Name = textures[k].name;
                                    b.XMLFile = Application.dataPath + "/XMLFiles/" + b.Name + ".json";
                                    b.BookImage = "AllBookImage/" + bt.BookTypeName + "/" + ct.ClassTypeName + "/" + b.Name;
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
            if (!Directory.Exists(GameCore.Instance.LocalXMLPath))
                Directory.CreateDirectory(GameCore.Instance.LocalXMLPath);
            CreateBookJSON(bookFile, books,callBack);

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