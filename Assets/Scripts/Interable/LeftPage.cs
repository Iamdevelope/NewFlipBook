using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJW.Book
{
    /// <summary>
    /// 向左翻书
    /// </summary>
    public class LeftPage : InterableObject
    {
        float startX, endX;
        public override void OnMouseDown()
        {
            //if (isTrigger)
            //    BookEdit.Instance.LeftFlipPage();
            startX = Input.mousePosition.x;
        }
        public void OnMouseUp()
        {
            endX = Input.mousePosition.x;
            if (endX - startX < 0 && isTrigger)
            {
                //isTrigger = false;
                BookEdit.Instance.LeftFlipPage();
            }
        }
    }
}
