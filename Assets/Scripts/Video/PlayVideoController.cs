using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

namespace PJW.Book
{
    /// <summary>
    /// 视频播放控制
    /// </summary>
    public class PlayVideoController : MonoBehaviour
    {
        private VideoPlayer video;
        private Slider slider;
        private double videoLength;
        private Button playController;
        private Button exitBtn;
        public Image image;
        public Sprite player;
        public Sprite pause;
        public Text time;
        public bool isPlay = true;
        private int currentHour;
        private int currentMinute;
        private int currentSecond;

        private void OnEnable()
        {
            image.DOFade(0, 0);
            if (video == null)
            {
                video = GetComponent<VideoPlayer>();
                videoLength = video.clip.length;
            }
            if (exitBtn == null)
            {
                exitBtn = transform.Find("Exit").GetComponent<Button>();
                exitBtn.onClick.AddListener(ExitClickEvent);
            }
            if (slider == null)
                slider = GetComponentInChildren<Slider>();
            if (playController == null)
            {
                playController = GameObject.Find("PlayController").GetComponent<Button>();
                playController.onClick.AddListener(PlayerControllerEvent);
            }
        }
        private void Update()
        {
            if (isPlay)
                SetVideoSlider(video.time);
        }
        /// <summary>
        /// 设置视频进度
        /// </summary>
        /// <param name="f"></param>
        private void SetVideoSlider(double f)
        {
            slider.value = (float)(f / video.clip.length);
            currentHour = (int)(video.time / 3600);
            currentMinute = (int)(video.time - currentHour * 3600) / 60;
            currentSecond = (int)(video.time - currentHour * 3600 - currentMinute * 60);
            time.text = string.Format("{0:D2}:{1:D2}:{2:D2}", currentHour, currentMinute, currentSecond);
        }
        /// <summary>
        /// 视频播放与暂停控制
        /// </summary>
        private void PlayerControllerEvent()
        {
            GameCore.Instance.PlaySoundBySoundName();
            if (isPlay)
            {
                isPlay = false;
                video.Pause();
                image.DOFade(1, 0.6f);
                image.sprite = pause;
            }
            else
            {
                isPlay = true;
                video.Play();
                image.DOFade(0, 0.6f);
                image.sprite = player;
            }
        }
        /// <summary>
        /// 关闭事件
        /// </summary>
        public void ExitClickEvent()
        {
            GameCore.Instance.PlaySoundBySoundName();
            if (transform.GetComponent<VideoPlayer>())
                transform.GetComponent<VideoPlayer>().Stop();
            transform.DOScale(0, 0.3f);
        }
    }
}