using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

namespace PJW.Book
{
    /// <summary>
    /// 书本中的按钮
    /// </summary>
    public class InterablePageButton : InterableObject
    {
        private VideoClip movie;
        private GameObject playObject;
        private VideoPlayer video;
        public override void GenerateEvent(VideoPlayer game, List<VideoClip> movies)
        {
            string name = gameObject.name.Split('_')[0];
            foreach (var item in movies)
            {
                if (gameObject.name.Equals(item.name))
                {
                    playObject = game.gameObject;
                    movie = item;
                    video = game;
                }
            }
        }
        public override void GenerateEvent(VideoPlayer game, VideoClip clip)
        {
            playObject = game.gameObject;
            movie = clip;
            video = game;
        }
        public void ClickHandler(BaseEventData data)
        {
            playObject.GetComponent<RectTransform>().DOScale(1, 0.3f).OnComplete(() =>
            {
                video.clip = movie;
                video.Play();
            });
        }
    }
}