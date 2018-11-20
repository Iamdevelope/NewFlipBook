using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PJW.Common
{
    public class GenerateXML
    {
        private static List<BookInfo> bookInfos = new List<BookInfo>();
        private static bool getSprites = false;
        private static bool getsounds = false;
        private static bool getobjects = false;
        private static bool getvideos = false;
        private static bool getuiSprites = false;

        /// <summary>
        /// 通过书本文件夹生成对应的XML文件,通过Resources进行加载
        /// </summary>
        /// <param name="bookFile"></param>
        public static void GetBookContentByFile(string bookFile, Action callBack)
        {
            //如果集合中保存了前面残留下的数据，则将其清空
            if (bookInfos.Count > 0)
                bookInfos.Clear();
            string path = bookFile.Split('.')[0];
            string bookName = path;
            object[] textures = Resources.LoadAll("Prefabs/Books/" + bookName + "/Textures/");
            object[] sprites = Resources.LoadAll("Prefabs/Books/" + bookName + "/Sprites/");
            object[] sounds = Resources.LoadAll("Prefabs/Books/" + bookName + "/Sound/");
            object[] objects = Resources.LoadAll("Prefabs/Books/" + bookName + "/Object/");
            object[] videos = Resources.LoadAll("Prefabs/Books/" + bookName + "/Video/");
            object[] uiSprites = Resources.LoadAll("Prefabs/Books/" + bookName + "/UIButtonSprite/");
            //对书页的页面贴图进行排序
            textures = textures.OrderBy(s => int.Parse(Regex.Match(s.ToString(), @"\d+").Value)).ToArray();
            sprites = sprites.OrderBy(s => int.Parse(Regex.Match(s.ToString(), @"\d+").Value)).ToArray();
            sounds = sounds.OrderBy(s => int.Parse(Regex.Match(s.ToString(), @"\d+").Value)).ToArray();
            objects = objects.OrderBy(s => int.Parse(Regex.Match(s.ToString(), @"\d+").Value)).ToArray();
            videos = videos.OrderBy(s => int.Parse(Regex.Match(s.ToString(), @"\d+").Value)).ToArray();
            uiSprites = uiSprites.OrderBy(s => int.Parse(Regex.Match(s.ToString(), @"\d+").Value)).ToArray();


            #region PC端采用的方式
            //#if UNITY_ANDROID
            //            path = Application.dataPath + "!/assets/Resources/Prefabs/Books/" + path;
            //#else
            //            path = Application.dataPath + "/Resources/Prefabs/Books/" + path;
            //#endif
            //            string texturePath = path + "/Textures/";
            //            string spritesPath = path + "/Sprites/";
            //            string soundPath = path + "/Sound/";
            //            string objectPath = path + "/Object/";
            //            string videoPath = path + "/Video/";
            //            string uiBtnSprite = path + "/UIButtonSprite/";
            //            

            //FileInfo[] textures = null;
            //FileInfo[] sprites = null;
            //FileInfo[] sounds = null;
            //FileInfo[] objects = null;
            //FileInfo[] videos = null;
            //FileInfo[] uiSprites = null;

            //判断当前路径是存在的
            //if (Directory.Exists(texturePath))
            //{
            //    DirectoryInfo directoryInfo = new DirectoryInfo(texturePath);
            //    textures = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            //}
            //if (Directory.Exists(spritesPath))
            //{
            //    DirectoryInfo directoryInfo = new DirectoryInfo(spritesPath);
            //    sprites = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            //}
            //if (Directory.Exists(soundPath))
            //{
            //    DirectoryInfo directoryInfo = new DirectoryInfo(soundPath);
            //    sounds = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            //}
            //if (Directory.Exists(objectPath))
            //{
            //    DirectoryInfo directoryInfo = new DirectoryInfo(objectPath);
            //    objects = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            //}
            //if (Directory.Exists(videoPath))
            //{
            //    DirectoryInfo directoryInfo = new DirectoryInfo(videoPath);
            //    videos = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            //}
            //if (Directory.Exists(uiBtnSprite))
            //{
            //    DirectoryInfo directoryInfo = new DirectoryInfo(uiBtnSprite);
            //    uiSprites = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            //}
            #endregion

            if (textures != null)
            {
                foreach (var item in textures)
                {

                    getSprites = false;
                    getsounds = false;
                    getobjects = false;
                    getvideos = false;
                    getuiSprites = false;

                    BookInfo bookInfo = new BookInfo();
                    if (item.ToString().EndsWith(".meta"))
                        continue;
                    string pageName = item.ToString().Split('(')[0].TrimEnd();
                    bookInfo.pageName = pageName;
                    if (sprites != null)
                    {
                        for (int i = 0; i < sprites.Length; i++)
                        {
                            if (pageName.Equals(sprites[i].ToString().Split('_')[0])) getSprites = false;
                            if (getSprites) continue;
                            //判断当前页面是否具有精灵物体
                            if (pageName.Equals(sprites[i].ToString().Split('_')[0]))
                            {
                                getSprites = true;
                                //记录当前页面
                                bookInfo.spriteCount = int.Parse(sprites[i].ToString().Split('_')[0]);
                                bookInfo.spriteName.Add(sprites[i].ToString().Split('(')[0].TrimEnd());
                            }
                        }
                    }
                    if (objects != null)
                    {
                        for (int i = 0; i < objects.Length; i++)
                        {
                            if (pageName.Equals(objects[i].ToString().Split('_')[0])) getobjects = false;
                            if (getobjects) continue;
                            if (pageName.Equals(objects[i].ToString().Split('_')[0]))
                            {
                                getobjects = true;
                                bookInfo.count = int.Parse(objects[i].ToString().Split('_')[0]);
                                bookInfo.objectName.Add(objects[i].ToString().Split('(')[0].TrimEnd());
                            }
                        }
                    }
                    if (sounds != null)
                    {
                        for (int i = 0; i < sounds.Length; i++)
                        {
                            if (getsounds) continue;
                            if (pageName.Equals(sounds[i].ToString().Split('(')[0].TrimEnd()))
                            {
                                getsounds = true;
                                bookInfo.sound = sounds[i].ToString().Split('(')[0].TrimEnd();
                            }
                        }
                    }
                    if (videos != null)
                    {
                        for (int i = 0; i < videos.Length; i++)
                        {
                            if (pageName.Equals(videos[i].ToString().Split('_')[0])) getvideos = false;
                            if (getvideos) continue;
                            if (pageName.Equals(videos[i].ToString().Split('_')[0]))
                            {
                                getvideos = true;
                                bookInfo.videoCount = int.Parse(videos[i].ToString().Split('_')[0]);
                                bookInfo.videoName.Add(videos[i].ToString().Split('(')[0].TrimEnd());
                            }
                        }
                    }
                    if (uiSprites != null)
                    {
                        for (int i = 0; i < uiSprites.Length; i++)
                        {
                            if (pageName.Equals(uiSprites[i].ToString().Split('_')[0])) getuiSprites = false;
                            if (getuiSprites) continue;
                            if (pageName.Equals(uiSprites[i].ToString().Split('_')[0]))
                            {
                                getuiSprites = true;
                                bookInfo.uiButtonSpriteCount = int.Parse(uiSprites[i].ToString().Split('_')[0]);
                                bookInfo.uiSprite.Add(uiSprites[i].ToString().Split('(')[0].TrimEnd());
                            }
                        }
                    }
                    bookInfos.Add(bookInfo);
                }

                string savePath = Application.persistentDataPath + "/Books/XMLContent/";
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);
                GenerateXMLHelper.CreateBookXML(savePath + "/" + bookFile, bookInfos, bookFile, callBack);
            }
        }
    }
}