using PJW.Book;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyCommon;

public class BookDummyFlipBook : MonoSingleton<BookDummyFlipBook> {
    //是否是封面
    private bool isTitlePage;
    public bool isFlip;
    public BoxCollider firstPage;
    public BoxCollider endPage;
    [HideInInspector]
    public int index;
    [HideInInspector]
    public int rightIndex;
    [HideInInspector]
    public int leftIndex;
    private void Update()
    {
        if ((GameCore.Instance.BookDummy.currentpage <= 1 && !isTitlePage))
        {
            isTitlePage = true;
            firstPage.enabled = true;
        }
        else if ((GameCore.Instance.BookDummy.currentpage > (GameCore.Instance.BookDummy.pagesnumber) - 3 && !isTitlePage))
        {
            isTitlePage = true;
            endPage.enabled = true;
        }
        else if ((GameCore.Instance.BookDummy.currentpage > 1 && GameCore.Instance.BookDummy.currentpage < GameCore.Instance.BookDummy.pagesnumber - 2)
            && isTitlePage)
        {
            isTitlePage = false;
            firstPage.enabled = false;
            endPage.enabled = false;
        }
    }
}
