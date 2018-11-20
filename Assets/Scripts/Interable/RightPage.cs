using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJW.Book
{
    /// <summary>
    /// 向右翻书
    /// </summary>
    public class RightPage : InterableObject
    {
        float startX, endX;
        public override void OnMouseDown()
        {
            //if (isTrigger)
            //    BookEdit.Instance.RightFlipPage();
            startX = Input.mousePosition.x;
        }
        public void OnMouseUp()
        {
            endX = Input.mousePosition.x;
            if (endX - startX > 0 && isTrigger)
            {
                //isTrigger = false;
                BookEdit.Instance.RightFlipPage();
            }
        }
    }
}
