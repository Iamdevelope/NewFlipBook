﻿using PJW.Book;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDontDestoryOnLoad : MonoBehaviour {
    private static bool isDontDestroyOnLoad;
    private static GameObject temp;
    public GameObject messageObject;
    private void Start()
    {
        if (!isDontDestroyOnLoad)
        {
            isDontDestroyOnLoad = true;
            temp = new GameObject("GameCore");
            GameObject message = Instantiate(messageObject);
            temp.AddComponent<SoundManager>();
            temp.AddComponent<GameCore>();
            temp.AddComponent<AudioSource>();
            DontDestroyOnLoad(temp);
            DontDestroyOnLoad(message);
        }
        temp.GetComponent<GameCore>().Init();
    }
}
