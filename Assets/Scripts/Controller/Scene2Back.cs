
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using PJW.Book.UI;

namespace PJW.Book
{
    public class Scene2Back : MonoBehaviour
    {
        public GameObject CameraObject;
        private NewGenerateBookstore bookStore;
        [HideInInspector]
        public int pos;
        private GameObject right;
        private GameObject left;
        private float startX;
        private float endX;
        private ClassPanel classPanel;
        private BasePanel[] allPanels;
        private void Start()
        {
            right = FindObjectOfType<Right>().gameObject;
            left = FindObjectOfType<Left>().gameObject;
            bookStore = FindObjectOfType<NewGenerateBookstore>();
            allPanels = FindObjectsOfType<BasePanel>();
            for (int i = 0; i < allPanels.Length; i++)
            {
                allPanels[i].Init();
            }
            classPanel = FindObjectOfType<ClassPanel>();
            if (GameCore.GameData != null)
            {
                //打开的书籍类型是根据上一个场景进的是哪个地方决定
                string name = GameCore.GameData.CurrentTargerPointName.Split('_')[0];
                classPanel.ClassButton(name);
            }

            ShowHideButton();
        }

        public void ShowHideButton()
        {
            Reset();
            Debug.Log(bookStore.bookNum);
            HideButton(left.GetComponent<Image>(), 0);
            HideButton(right.transform.GetChild(0).GetComponent<Image>(), 0);
            left.GetComponent<Button>().enabled = false;
            right.GetComponent<Button>().enabled = false;
            if (bookStore.bookNum > 3)
            {
                right.GetComponent<Button>().enabled = true;
                HideButton(right.transform.GetChild(0).GetComponent<Image>(), 1);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {

                startX = Input.mousePosition.x;
                endX = Input.mousePosition.x;
            }
            if (Input.GetMouseButtonUp(0))
            {
                endX = Input.mousePosition.x;
                if (Mathf.Abs(endX - startX) < 0.5f) return;
                if (endX - startX < 0)
                    CameraAnimation(1);
                else if (endX - startX > 0)
                    CameraAnimation(-1);
            }
        }
        /// <summary>
        /// 切换不同类型的书时，摄像机进行入场动画过度
        /// </summary>
        public void CameraStartOrOverAnimation(bool isStart)
        {
            if (isStart)
            {
                CameraObject.transform.DOLocalMoveY(CameraObject.transform.localPosition.y + 20, 1f);
            }
            else
            {
                CameraObject.transform.DOLocalMoveY(CameraObject.transform.localPosition.y - 20, 1f);
            }
        }
        /// <summary>
        /// 摄像机移动
        /// </summary>
        /// <param name="dic">移动的方向，1表示右，-1表示左</param>
        public void CameraAnimation(int dic)
        {

            if (bookStore.bookNum <= 3) return;
            pos += dic;
            if (pos <= 0)
            {
                left.GetComponent<Button>().enabled = false;
                HideButton(left.GetComponent<Image>(), 0);
            }
            else
            {
                left.GetComponent<Button>().enabled = true;
                HideButton(left.GetComponent<Image>(), 1);
            }
            if (pos >= (bookStore.bookNum % 3 == 0 ? bookStore.bookNum / 3 - 1 : bookStore.bookNum / 3))
            {
                right.GetComponent<Button>().enabled = false;
                HideButton(right.transform.GetChild(0).GetComponent<Image>(), 0);
            }
            else
            {
                right.GetComponent<Button>().enabled = true;
                HideButton(right.transform.GetChild(0).GetComponent<Image>(), 1);
            }
            if (pos < 0 || pos > (bookStore.bookNum % 3 == 0 ? bookStore.bookNum / 3 - 1 : bookStore.bookNum / 3))
            {
                pos -= dic;
                return;
            }


            switch (dic)
            {
                case 1:
                    CameraObject.transform.DOLocalMoveX(CameraObject.transform.localPosition.x + 18.24f, 0.6f);
                    break;
                case -1:
                    CameraObject.transform.DOLocalMoveX(CameraObject.transform.localPosition.x - 18.24f, 0.6f);
                    break;
                default:
                    break;
            }
        }

        private void HideButton(Image image, float endValue)
        {
            image.DOFade(endValue, 0.3f);
        }

        public void ExitBackHandle()
        {
            Cursor.visible = true;
            GameCore.Instance.PlaySoundBySoundName();
            if (GameCore.Instance.asset != null)
            {
                GameCore.Instance.asset.Unload(true);
                GameCore.Instance.asset = null;
            }
            GameCore.allBooksName.Clear();
            Globe.nextSceneName = "Main";
            SceneManager.LoadScene("Loading");
        }

        public void Reset()
        {
            CameraObject.transform.DOLocalMoveX(0, 0);
            pos = 0;
        }
    }
}