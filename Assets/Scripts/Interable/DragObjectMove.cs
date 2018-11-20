using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PJW.Common
{
    /// <summary>
    /// 拖拽物体移动
    /// </summary>
    public class DragObjectMove : MonoBehaviour
    {
        private Vector3 position;
        public IEnumerator OnMouseDown()
        {
            Vector3 screen = Camera.main.WorldToScreenPoint(transform.position);
            position = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screen.z));
            while (Input.GetMouseButton(0))
            {
                transform.position = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screen.z)) + position);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}