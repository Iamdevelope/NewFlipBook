using UnityEngine;
using UnityEngine.EventSystems;

namespace PJW.Book.UI
{
    /// <summary>
    /// 鼠标拖拽UI
    /// </summary>
    public class DragUIMove : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IEndDragHandler
    {
        public RectTransform canvas;
        private RectTransform imgRect;
        private InterablePageOfObject interable;
        Vector2 offset = new Vector3();
        private bool UICanMove = false; //判断当前的拖拽是否可以让整个UI移动，鼠标在其他按钮上时拖拽，避免造成错误。
        void Awake()
        {
            imgRect = GetComponent<RectTransform>();
            interable = GetComponent<InterablePageOfObject>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Vector2 mouseDown = eventData.position;
            Vector2 mouseUguiPos = new Vector2();
            bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, mouseDown, eventData.enterEventCamera, out mouseUguiPos);
            if (isRect)
            {
                offset = imgRect.anchoredPosition - mouseUguiPos;
                UICanMove = true;
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (interable != null && !interable.notMove)
                interable.notMove = true;
            if (UICanMove)
            {
                Vector2 mouseDrag = eventData.position;
                Vector2 uguiPos = new Vector2();
                bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, mouseDrag, eventData.enterEventCamera, out uguiPos);

                if (isRect)
                {
                    imgRect.anchoredPosition = offset + uguiPos;
                }
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (interable != null)
                interable.notMove = false;
            offset = Vector2.zero;
            UICanMove = false;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            offset = Vector2.zero;
            UICanMove = false;
        }
    }
}
