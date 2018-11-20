using PJW.Book;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Scene2Back : MonoBehaviour {
    public GameObject CameraObject;
    private GenerateBookStore bookStore;
    [HideInInspector]
    public int pos;
    private GameObject right;
    private GameObject left;
    private float startX;
    private float endX;
    private void Start()
    {
        right = FindObjectOfType<Right>().gameObject;
        left = FindObjectOfType<Left>().gameObject;
        bookStore = FindObjectOfType<GenerateBookStore>();
        //CameraObject = Camera.main.gameObject;

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
            if (endX - startX < 0)
                CameraAnimation(1);
            else if (endX - startX > 0)
                CameraAnimation(-1);
        }
    }

    /// <summary>
    /// 摄像机移动
    /// </summary>
    /// <param name="dic">移动的方向，1表示右，-1表示左</param>
    public void CameraAnimation(int dic)
    {
        Debug.Log("这里有这么多本书：" + bookStore.bookNum);
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

    private void HideButton(Image image,float endValue)
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
        GameCore.Instance.SoundManager.Reset();
        SceneManager.LoadScene("Main");
    }
}
