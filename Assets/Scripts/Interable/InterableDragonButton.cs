using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace PJW.Book
{
    /// <summary>
    /// 恐龙按钮
    /// </summary>
    public class InterableDragonButton : InterableObject{
        private GameObject content;
        private RawImage image;
        private string bookName;
        private DragonContent[] dragons;
        public override void GenerateEvent(string bookName)
        {
            content = GameObject.Find("Content");
            this.bookName = bookName;
            image = content.GetComponent<RawImage>();
            content.SetActive(false);
        }

        public void ClickDragonButton(string name)
        {
            GameCore.Instance.PlaySoundBySoundName(SoundManager.CLICK_02);
            Texture sprite = Resources.Load("Sprites/" + bookName + "/" + name) as Texture;
            image.texture = sprite;
            content.SetActive(true);
            content.transform.localScale = Vector3.zero;
            content.transform.DOScale(Vector3.one, 0.6f);
        }
        public override void OnEnable()
        {
            if (dragons == null)
                dragons = FindObjectsOfType<DragonContent>();
            foreach (var item in dragons)
            {
                item.isActive = true;
            }
        }
        public override void OnDisable()
        {
            if (dragons == null)
                dragons = FindObjectsOfType<DragonContent>();
            foreach (var item in dragons)
            {
                item.isActive = false;
            }
        }
    }
}