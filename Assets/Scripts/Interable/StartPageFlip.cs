using UnityEngine;
using DG.Tweening;
using System;

namespace PJW.Book
{
    /// <summary>
    /// 书本的封面的翻页效果
    /// </summary>
    public class StartPageFlip : InterableObject
    {
        [Tooltip("是否是封面")]
        public bool isStartPage;
        private float startX, endX;
        private float t = 0;
        //是否已经翻页结束
        public bool isRotate;
        [HideInInspector]
        public bool canFlip;
        public void Turning(float angle)
        {
            if (angle >= 0) return;
            if (transform.rotation.eulerAngles.z > 1 && transform.rotation.eulerAngles.z < 180) return;
            ObjectRotate(new Vector3(0, 0, angle), 0);
        }
        public override void OnMouseDown()
        {
            startX = Input.mousePosition.x;
            if (!isStartPage)
            {
                if (GameCore.Instance.GeneratePage.showObject.Count > 0)
                {
                    foreach (var item in GameCore.Instance.GeneratePage.showObject)
                    {
                        item.SetActive(false);
                    }
                }
                if (GameCore.Instance.GeneratePage.spriteMap.ContainsKey(GameCore.Instance.GeneratePage.currentpage + 1))
                    GameCore.Instance.GeneratePage.spriteMap[GameCore.Instance.GeneratePage.currentpage + 1].SetActive(false);
                if (GameCore.Instance.GeneratePage.uiBtnSprites.Count > 0)
                {
                    foreach (var item in GameCore.Instance.GeneratePage.uiBtnSprites)
                    {
                        item.SetActive(false);
                    }
                }
            }
        }
        private void OnMouseDrag()
        {
            if (isStartPage)
            {
                if (canFlip)
                {
                    endX = Input.mousePosition.x;
                    if (endX - startX > 0 && isRotate)
                        Turning(-180 + (endX - startX) * 0.5f);
                    else if (!isRotate)
                        Turning((endX - startX) * 0.5f);
                }
            }
            else
            {
                endX = Input.mousePosition.x;
                if (endX - startX > 0 && isRotate)
                    Turning(-180 + (endX - startX) * 0.5f);
                else if (!isRotate)
                    Turning((endX - startX) * 0.5f);
                if (GameCore.Instance.GeneratePage.showObject.Count > 0)
                {
                    foreach (var item in GameCore.Instance.GeneratePage.showObject)
                    {
                        item.SetActive(false);
                    }
                }
                if (GameCore.Instance.GeneratePage.spriteMap.ContainsKey(GameCore.Instance.GeneratePage.currentpage))
                    GameCore.Instance.GeneratePage.spriteMap[GameCore.Instance.GeneratePage.currentpage].SetActive(false);
                if (GameCore.Instance.GeneratePage.uiBtnSprites.Count > 0)
                {
                    foreach (var item in GameCore.Instance.GeneratePage.uiBtnSprites)
                    {
                        item.SetActive(false);
                    }
                }
            }
        }
        public override void OnMouseUp()
        {
            endX = Input.mousePosition.x;
            if (Mathf.Abs(endX - startX) <= 0.1f) return;
            if (isRotate)
            {
                if (transform.rotation.eulerAngles.z > 240 &&
                transform.rotation.eulerAngles.z < 360)
                {
                    //transform.DORotate(new Vector3(0, 0, 0), 1f);
                    ObjectRotate(Vector3.zero, 1);
                    isRotate = false;
                }
                else if (transform.rotation.eulerAngles.z > 180 &&
                    transform.rotation.eulerAngles.z <= 240)
                {
                    //transform.DORotate(new Vector3(0, 0, -180), 1f);
                    ObjectRotate(new Vector3(0, 0, -180), 1);
                    isRotate = true;
                    if (isStartPage)
                    {
                        ViewController.Instance.anim.SetBool("startRead", true);
                    }
                    else
                    {
                        ViewController.Instance.OverAnim();
                    }
                }
                else
                {
                    ObjectRotate(new Vector3(0, 0, -180), 0);
                    isRotate = true;
                    if (isStartPage)
                    {
                        ViewController.Instance.anim.SetBool("startRead", true);
                    }
                    else
                    {
                        ViewController.Instance.OverAnim();
                    }
                }
            }
            else if(endX-startX<0)
            {
                if (transform.rotation.eulerAngles.z > 300 &&
                transform.rotation.eulerAngles.z < 360)
                {
                    //transform.DORotate(new Vector3(0, 0, 0), 1f);
                    ObjectRotate(Vector3.zero, 1);
                    isRotate = false;
                }
                else if (transform.rotation.eulerAngles.z > 180 &&
                    transform.rotation.eulerAngles.z <= 300)
                {
                    //transform.DORotate(new Vector3(0, 0, -180), 1f);
                    ObjectRotate(new Vector3(0, 0, -180), 1);
                    isRotate = true;
                    if (isStartPage)
                    {
                        ViewController.Instance.anim.SetBool("startRead", true);
                    }
                    else
                    {
                        ViewController.Instance.OverAnim();
                    }
                }
                else
                {
                    ObjectRotate(new Vector3(0, 0, -180), 1);
                    isRotate = true;
                    if (isStartPage)
                    {
                        ViewController.Instance.anim.SetBool("startRead", true);
                    }
                    else
                    {
                        ViewController.Instance.OverAnim();
                    }
                }
            }
        }
        private void ObjectRotate(Vector3 angle,float time)
        {
            transform.DORotate(angle,time);
        }
    }
}