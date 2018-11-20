using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

namespace PJW.Book
{
    public class PageTurning : MonoBehaviour
    {
        private Material m_Material;
        float startX, endX;
        private GameObject currentPageObj;
        public bool isRight;
        private float t;
        private bool isDown;
        private GeneratePage generatePage;
        void Start()
        {
            generatePage = GameCore.Instance.GeneratePage;
            m_Material = GetComponent<MeshRenderer>().material;
            currentPageObj = this.gameObject;
        }
        public void OnMouseDown()
        {
            FlipBook.Instance.index++;
            //当正在翻页时，禁止翻页
            if (FlipBook.Instance.isFlip) return;
            startX = Input.mousePosition.x;
            
            if (generatePage.showObject.Count > 0)
            {
                foreach (var item in generatePage.showObject)
                {
                    item.SetActive(false);
                }
            }
            if (generatePage.spriteMap.ContainsKey(generatePage.currentpage+1))
                generatePage.spriteMap[generatePage.currentpage+1].SetActive(false);
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
            if (FlipBook.Instance.index > 1) return;
            if (FlipBook.Instance.isFlip) return;
            //当鼠标拖动页面时，进行页面内容的更改
            if (!isDown)
            {
                if (GetRightPos() > 0 && GetRightPos() <= generatePage.pagestextures.Length)
                {
                    if (isRight && (generatePage.currentpage >= 0 && generatePage.currentpage < generatePage.pagesnumber))
                        generatePage.FlipPage(1);
                    else if (generatePage.currentpage >= 0 && generatePage.currentpage < generatePage.pagesnumber)
                        generatePage.FlipPage(-1);
                }
                isDown = true;
            }
            endX = Input.mousePosition.x;
            if (!isRight)
            {
                if (endX - startX > 0)
                    Turning(1,(endX - startX) * 0.5f);
            }
            else
            {
                if(endX-startX<0)
                    Turning(0,Mathf.Abs(endX - startX) * 0.5f);
            }
        }
        private void OnMouseUp()
        {
            isDown = false;
            //松开鼠标时，判断是否需要进行页面材质的切换
            endX = Input.mousePosition.x;
            if (FlipBook.Instance.isFlip) return;
            FlipBook.Instance.isFlip = true;
            //从左往右翻页的时候
            if (endX - startX > 0)
            {
                if (m_Material.GetFloat("_Angle") < 60)
                {
                    generatePage.ShowHideObject(0);
                    MaterialOfAngleRotate(1,0,0.5f,false);
                    StartCoroutine(BackMaterial(1));
                }
                else
                {
                    MaterialOfAngleRotate(1,178,0.5f,true);
                }
            }
            else
            {
                if (m_Material.GetFloat("_Angle") < 60)
                {
                    generatePage.ShowHideObject(0);
                    MaterialOfAngleRotate(0, 0, 0.5f,false);
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
        public void Turning(int dic,float angle)
        {
            if (angle > 180) return;
            if (m_Material.GetFloat("_Angle") < 0 || m_Material.GetFloat("_Angle") > 181) return;
            MaterialOfAngleRotate(dic, angle, 0, false, false);

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
            FlipBook.Instance.isFlip = false;
            if (GetRightPos() > 2 && GetRightPos() < generatePage.pagestextures.Length)
                generatePage.FlipPage(v);
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
                    FlipBook.Instance.index = 0;
                    startX = 0;
                    endX = 0;
                }
                if (!isCanHide) return;
                //表示由右向左翻书，这个时候应该将右边书页列表中的当前的对象隐藏
                if (dic == 0)
                {
                    //将书页位置上下调整
                    generatePage.rightPages[1].transform.localPosition = new Vector3(1.377f, 0, 0);
                    generatePage.rightPages[0].transform.localPosition = new Vector3(1.377f, -0.01f, 0);
                    //Reset shader中的_Angle数值，使其恢复初始位置
                    //generatePage.rightPages[0].GetComponentInChildren<MeshRenderer>().material.SetFloat("_Angle", 0);
                    GetComponent<MeshRenderer>().material.SetFloat("_Angle", 0);
                    //反转右边数组
                    Array.Reverse(generatePage.rightPages);
                    //当左边的页面还没有显示，则将其显示
                    if (generatePage.currentpage < 4)
                        generatePage.leftPages[0].SetActive(true);
                    //当翻到结尾时，将右边的书页慢慢隐藏
                    if (generatePage.currentpage > generatePage.pagesnumber - 5 && generatePage.currentpage < generatePage.pagesnumber)
                        generatePage.rightPages[1].SetActive(false);
                    if (generatePage.currentpage >= 3)
                    {
                        //调整左边的页面位置
                        generatePage.leftPages[1].transform.localPosition = new Vector3(-1.377f, 0, 0);
                        generatePage.leftPages[0].transform.localPosition = new Vector3(-1.377f, -0.01f, 0);
                    }
                    //反转左边页面的数组
                    Array.Reverse(generatePage.leftPages);
                    if (generatePage.currentpage < 3)
                    {
                        generatePage.leftPages[1].transform.localPosition = new Vector3(-1.377f, 0, 0);
                        generatePage.leftPages[0].transform.localPosition = new Vector3(-1.377f, -0.01f, 0);
                    }

                    generatePage.currentpage += 1;
                    LeftFlipPage();
                }
                else if (dic == 1)
                {
                    //将书页位置上下调整
                    generatePage.leftPages[1].transform.localPosition = new Vector3(-1.377f, 0, 0);
                    generatePage.leftPages[0].transform.localPosition = new Vector3(-1.377f, -0.01f, 0);
                    //将shader中的_Angle数值恢复初始值
                    //generatePage.leftPages[0].GetComponentInChildren<MeshRenderer>().material.SetFloat("_Angle", 0);
                    GetComponent<MeshRenderer>().material.SetFloat("_Angle", 0);
                    //反转左边数组
                    Array.Reverse(generatePage.leftPages);
                    //当左边没有页面时，还要向左边翻页，则将左边页面隐藏
                    if (generatePage.currentpage < 4)
                    {
                        Array.Reverse(generatePage.leftPages);
                        generatePage.leftPages[0].SetActive(false);
                    }
                    if (generatePage.currentpage > generatePage.pagesnumber - 5 && generatePage.currentpage < generatePage.pagesnumber)
                        generatePage.rightPages[1].SetActive(true);
                    //调整右边页面的位置
                    generatePage.rightPages[1].transform.localPosition = new Vector3(1.377f, 0, 0);
                    generatePage.rightPages[0].transform.localPosition = new Vector3(1.377f, -0.01f, 0);
                    //反转右边页面数组
                    Array.Reverse(generatePage.rightPages);
                    generatePage.currentpage -= 1;
                    RightFlipPage();
                }

                if (!ViewController.Instance.is3DView)
                {
                    FlipBook.Instance.isFlip = true;
                    ViewController.Instance.ChangeCommonView();
                }
                FlipBook.Instance.isFlip = false;
                generatePage.ShowHideObject(t);
                CanvasController.Instance.UpdatePageNum(generatePage.currentpage);
                if (!generatePage.notGenerateSoundManager)
                {
                    Debug.Log("播放下一页的声音");
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
}