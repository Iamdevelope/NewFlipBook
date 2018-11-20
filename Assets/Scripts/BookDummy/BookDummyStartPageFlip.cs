using DG.Tweening;
using PJW.Book;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookDummyStartPageFlip : InterableObject {
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
        if (angle <= 0) return;
        //if (transform.rotation.eulerAngles.z > 1 && transform.rotation.eulerAngles.z < 180) return;
        ObjectRotate(new Vector3(0, 0, angle), 0);
    }
    public override void OnMouseDown()
    {
        startX = Input.mousePosition.x;
        if (!isStartPage)
        {
            if (GameCore.Instance.BookDummy.showObject.Count > 0)
            {
                foreach (var item in GameCore.Instance.BookDummy.showObject)
                {
                    item.SetActive(false);
                }
            }
            if (GameCore.Instance.BookDummy.spriteMap.ContainsKey(GameCore.Instance.BookDummy.currentpage + 1))
                GameCore.Instance.BookDummy.spriteMap[GameCore.Instance.BookDummy.currentpage + 1].SetActive(false);
            if (GameCore.Instance.BookDummy.uiBtnSprites.Count > 0)
            {
                foreach (var item in GameCore.Instance.BookDummy.uiBtnSprites)
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
                    Turning((Mathf.Abs(endX - startX)) * 0.5f);
            }
        }
        else
        {
            endX = Input.mousePosition.x;
            if (endX - startX > 0 && isRotate)
                Turning(-180 + (endX - startX) * 0.5f);
            else if (!isRotate)
                Turning((endX - startX) * 0.5f);
            if (GameCore.Instance.BookDummy.showObject.Count > 0)
            {
                foreach (var item in GameCore.Instance.BookDummy.showObject)
                {
                    item.SetActive(false);
                }
            }
            if (GameCore.Instance.BookDummy.spriteMap.ContainsKey(GameCore.Instance.BookDummy.currentpage))
                GameCore.Instance.BookDummy.spriteMap[GameCore.Instance.BookDummy.currentpage].SetActive(false);
            if (GameCore.Instance.BookDummy.uiBtnSprites.Count > 0)
            {
                foreach (var item in GameCore.Instance.BookDummy.uiBtnSprites)
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
                ObjectRotate(new Vector3(0, 0, 180), 1);
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
                ObjectRotate(new Vector3(0, 0, 180), 0);
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
        else if (endX - startX < 0)
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
                ObjectRotate(new Vector3(0, 0, 180), 1);
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
                ObjectRotate(new Vector3(0, 0, 180), 1);
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
    private void ObjectRotate(Vector3 angle, float time)
    {
        Debug.Log(angle);
        transform.DOLocalRotate(angle, time);
    }
}