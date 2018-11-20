using DG.Tweening;
using PJW.Book;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookDummyGeneratePage : MonoBehaviour {
    private Material m_Material;
    float startX, endX;
    private GameObject currentPageObj;
    public bool isRight;
    private float t;
    private bool isDown;
    private BookDummy generatePage;
    void Start()
    {
        generatePage = GameCore.Instance.BookDummy;
        m_Material = GetComponent<MeshRenderer>().material;
        currentPageObj = this.gameObject;
    }
    public void OnMouseDown()
    {
        BookDummyFlipBook.Instance.index++;
        //当正在翻页时，禁止翻页
        if (BookDummyFlipBook.Instance.isFlip) return;
        startX = Input.mousePosition.x;

        if (generatePage.showObject.Count > 0)
        {
            foreach (var item in generatePage.showObject)
            {
                item.SetActive(false);
            }
        }
        if (generatePage.spriteMap.ContainsKey(generatePage.currentpage + 1))
            generatePage.spriteMap[generatePage.currentpage + 1].SetActive(false);
        if (generatePage.uiBtnSprites.Count > 0)
        {
            foreach (var item in generatePage.uiBtnSprites)
            {
                item.SetActive(false);
            }
        }
    }
    private void OnMouseDrag()
    {
        if (BookDummyFlipBook.Instance.index > 1) return;
        if (BookDummyFlipBook.Instance.isFlip) return;
        endX = Input.mousePosition.x;
        if (!isRight)
        {
            if (endX - startX > 0)
                Turning((endX - startX) * 0.5f);
        }
        else
        {
            if (endX - startX < 0)
                Turning(Mathf.Abs(endX - startX) * 0.5f);
        }
    }
    private void OnMouseUp()
    {
        isDown = false;
        //松开鼠标时，判断是否需要进行页面材质的切换
        endX = Input.mousePosition.x;
        if (BookDummyFlipBook.Instance.isFlip) return;
        BookDummyFlipBook.Instance.isFlip = true;
        //从左往右翻页的时候
        if (endX - startX > 0)
        {
            if (m_Material.GetFloat("_Angle") < 60)
            {
                generatePage.ShowHideObject(0);
                MaterialOfAngleRotate(1, 0, 0.5f, false);
                StartCoroutine(BackMaterial(1));
            }
            else
            {
                MaterialOfAngleRotate(1, 178, 0.5f, true);
            }
        }
        else
        {
            if (m_Material.GetFloat("_Angle") < 60)
            {
                generatePage.ShowHideObject(0);
                MaterialOfAngleRotate(0, 0, 0.5f, false);
                StartCoroutine(BackMaterial(-1));
            }
            else
            {
                MaterialOfAngleRotate(0, 180, 0.5f, true);
            }
        }

    }
    /// <summary>
    /// 时刻改变材质的值，达到旋转效果
    /// </summary>
    /// <param name="angle">旋转的角度</param>
    public void Turning(float angle)
    {
        if (angle > 180) return;
        if (m_Material.GetFloat("_Angle") < 0 || m_Material.GetFloat("_Angle") > 181) return;
        MaterialOfAngleRotate(0, angle, 0, false, false);

    }
    /// <summary>
    /// 得到向左翻页时，页面索引
    /// </summary>
    /// <returns></returns>
    private int GetRightPos()
    {
        return generatePage.currentpage;
    }
    private IEnumerator BackMaterial(int v)
    {
        yield return new WaitForSeconds(1);
        BookDummyFlipBook.Instance.isFlip = false;
    }
    /// <summary>
    /// 书页材质旋转效果
    /// </summary>
    /// <param name="dic">翻页的方向,0表示从右往左，1表示从左往右</param>
    /// <param name="rotate">旋转的角度</param>
    /// <param name="time">所需要的时间</param>
    /// <param name="isCanHide">是否需要进行书页之间的操作</param>
    public void MaterialOfAngleRotate(int dic, float rotate, float time = 0, bool isCanHide = false, bool isflowerFlip = true)
    {
        //当书面翻转结束，进行书页之间的切换显示隐藏
        m_Material.DOFloat(rotate, "_Angle", time).OnComplete(() =>
        {
            if (isflowerFlip)
            {
                BookDummyFlipBook.Instance.index = 0;
                startX = 0;
                endX = 0;
            }
            if (!isCanHide) return;

            if (!ViewController.Instance.is3DView)
            {
                BookDummyFlipBook.Instance.isFlip = true;
                ViewController.Instance.ChangeCommonView();
            }
            BookDummyFlipBook.Instance.isFlip = false;
            generatePage.ShowHideObject(t);
            CanvasController.Instance.UpdatePageNum(generatePage.currentpage);
            if (!generatePage.notGenerateSoundManager)
            {
                generatePage.soundManager.clipStack.Push(GetAudioClip());
                generatePage.soundManager.PlayAudioClip();
            }
            else
            {
                GameCore.Instance.PlaySound(GetAudioClip());
            }
        });
    }
    /// <summary>
    /// 得到当前页面该播放的声音资源
    /// </summary>
    /// <returns></returns>
    private AudioClip GetAudioClip()
    {
        AudioClip audioClip = null;
        foreach (var item in generatePage.bookSoundClip)
        {
            if (generatePage.currentpage.ToString().Equals(item.name))
            {
                audioClip = item;
            }
        }
        return audioClip;
    }
    /// <summary>
    /// 由右向左翻页，判断是否需要进行视野的切换
    /// </summary>
    private void RightFlipPage()
    {
        if (generatePage.currentpage <= 0) return;
        CanFlipPage();
    }
    /// <summary>
    /// 由左向右翻页，判断是否需要进行视野的切换
    /// </summary>
    private void LeftFlipPage()
    {
        if (generatePage.currentpage >= generatePage.pagesnumber) return;
        CanFlipPage();
    }
    /// <summary>
    /// 翻页
    /// </summary>
    private void CanFlipPage()
    {
        //如果当前已经处于3D视角，则加载3D物体时，设置其显示出来的等待时长
        if (ViewController.Instance.is3DView)
            t = 0;
        else
            t = 4;
        if (IsChangeView())
            ViewController.Instance.is3DView = true;
        else
            ViewController.Instance.is3DView = false;
    }
    /// <summary>
    /// 判断是否需要改变视野
    /// </summary>
    /// <returns></returns>
    private bool IsChangeView()
    {
        return generatePage.bookMap.ContainsKey(generatePage.currentpage);
    }
}
