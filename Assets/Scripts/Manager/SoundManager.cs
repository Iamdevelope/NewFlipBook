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
        [HideInInspector]
        public AudioSource source;
        public Stack<AudioClip> clipStack = new Stack<AudioClip>();
        private AudioClip currentClip;
        public void Init()
        {
            GameCore.Instance.SoundManager = this;
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

        public void Reset()
        {
            source.clip = null;
            currentClip = null;
        }
    }
}