using UnityEngine;
using MyCommon;

namespace PJW.Book{
    //翻书
    public class FlipBook : MonoSingleton<FlipBook> {
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
        private void Update(){
            if ((GameCore.Instance.GeneratePage.currentpage <= 1 && !isTitlePage))
            {
                isTitlePage = true;
                firstPage.enabled = true;
            }
            else if ((GameCore.Instance.GeneratePage.currentpage > (GameCore.Instance.GeneratePage.pagesnumber) - 3 && !isTitlePage))
            {
                isTitlePage = true;
                endPage.enabled = true;
            }
            else if ((GameCore.Instance.GeneratePage.currentpage > 1 && GameCore.Instance.GeneratePage.currentpage < GameCore.Instance.GeneratePage.pagesnumber - 2)
                && isTitlePage)
            {
                isTitlePage = false;
                firstPage.enabled = false;
                endPage.enabled = false;
            }
        }
    }
}