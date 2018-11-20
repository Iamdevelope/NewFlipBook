using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace PJW.Book
{

    public class UIPlayer : MonoBehaviour
    {
        private VideoPlayer videoPlayer;
        private RawImage image;
        private PlayVideoController controller;

        void OnEnable()
        {
            if (controller == null)
            {
                controller = GetComponent<PlayVideoController>();
                videoPlayer = GetComponent<VideoPlayer>();
                image = GetComponent<RawImage>();
            }
        }

        void Update()
        {
            if (controller == null) return;
            if (videoPlayer.clip != null)
                image.texture = videoPlayer.texture;
        }
    }
}
