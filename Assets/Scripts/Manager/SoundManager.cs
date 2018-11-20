using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJW.Book
{
    /// <summary>
    /// 声音管理器
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        public const string CLICK_01 = "click01";
        public const string CLICK_02 = "click02";
        public const string CLICKDRAG = "ClickDrag";
        public const string INSCENE = "InScene";
        [HideInInspector]
        public AudioSource source;
        public Stack<AudioClip> clipStack = new Stack<AudioClip>();
        private AudioClip currentClip;
        public void Init()
        {

            source = GetComponent<AudioSource>();
        }
        public void PlayAudioClip()
        {
            if (clipStack.Count > 0)
            {
                source.clip = clipStack.Pop();
                currentClip = source.clip;
                source.Play();
            }
        }
        public void PlayAudioClip(string name)
        {
            PlayAudioClip(Resources.Load<AudioClip>("Sounds/" + name));
        }
        private void PlayAudioClip(AudioClip clip)
        {
            source.clip = clip;
            source.Play();
            StartCoroutine(PlayCurrentSound(clip.length));
        }

        private IEnumerator PlayCurrentSound(float t)
        {
            yield return new WaitForSeconds(t);
            source.clip = currentClip;
            source.Play();
        }
        /// <summary>
        /// 设置声音大小
        /// </summary>
        /// <param name="volume"></param>
        public void SetSoundSize(float volume)
        {
            source.volume = volume;
        }
        public void Reset()
        {
            source.clip = null;
            currentClip = null;
        }
    }
}