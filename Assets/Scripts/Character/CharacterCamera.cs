using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJW.Book
{
    /// <summary>
    /// 角色跟随
    /// </summary>
    public class CharacterCamera : MonoBehaviour
    {
        private GameObject target;
        private Vector3 dic;
        private Vector3 rotateAxis;
        private bool isFirst = true;
        private float mouseStartX;
        private float mouseEndX;
        private float f;
        private float mouseClam;
        private float startX;
        private float endX;
        private float clam;
        public float rotationSpeed;
        public float speed;
        private Transform children;
        private Vector3 cameraCurrentPos;
        private Quaternion cameraCurrentRotate;
        private float t;
        private int screenWidth;
        /// <summary>
        /// 对象被显示
        /// </summary>
        private void OnEnable()
        {
            cameraCurrentRotate = Quaternion.Euler(Vector3.zero);
            screenWidth = Screen.width;
            Debug.Log(" screen of width " + screenWidth);
            if (GameCore.CurrentObject == null) return;
            target = GameCore.CurrentObject;
            rotateAxis = new Vector3(0, target.transform.position.y, 0);
            children = transform.GetChild(0);
            dic = transform.position - target.transform.position;
        }
        private void Update()
        {
            if (target)
            {
                //将相机位置与角色位置始终保持在可视范围内
                transform.position = Vector3.Lerp(transform.position, target.transform.position + dic, rotationSpeed * Time.deltaTime);
                if (Input.GetMouseButtonDown(0))
                {
                    f = Input.mousePosition.x;
                    mouseStartX = Input.mousePosition.x;
                }
                //如果按下鼠标左键，可以对相机进行旋转观察角色
                if (Input.GetMouseButton(0))
                {
                    //鼠标在刚按下时，记录初始鼠标位置
                    if (isFirst)
                    {
                        isFirst = false;
                        startX = Input.mousePosition.x;
                    }
                    endX = Input.mousePosition.x;
                    mouseEndX = endX;
                    Debug.Log(mouseClam + " from update ");
                    if (endX - startX == 0)
                    {
                        f = Input.mousePosition.x;
                    }
                    //记录鼠标移动的距离
                    mouseClam = ((mouseEndX - f) / screenWidth) * 360;
                    //摄像机围绕角色进行旋转
                    CameraTotateAround(mouseClam * rotationSpeed * Time.deltaTime);
                    //更新鼠标位置
                    startX = Input.mousePosition.x;
                }
                //松开鼠标时，将摄像机位置重置
                if (Input.GetMouseButtonUp(0))
                {
                    mouseEndX = Input.mousePosition.x;
                    mouseClam = mouseEndX - mouseStartX;
                    Debug.Log("mouse of move " + mouseClam);
                    isFirst = true;
                    //记录松开鼠标时，相机的旋转角度与初始角度插值
                    t = children.eulerAngles.y - cameraCurrentRotate.eulerAngles.y;
                    t = t % 360;
                    Debug.Log(t);
                    //鼠标旋转方向为由左向右移动
                    if (mouseClam > 0)
                    {
                        t = t < 180 ? -t : 180 - t;
                    }
                    else
                    {
                        t = t < 180 ? t : t - 360;
                    }

                    ////如果旋转方向是顺时针方向
                    //if (t > 0)
                    //    t = t < 180 ? -t : 360 - t;
                    ////否则旋转方向为逆时针方向时
                    //else
                    //    t = t > -180 ? t : 360 + t;
                }
                if (t != 0)
                {
                    CameraTotateAround(Mathf.Abs(t) / speed * Time.deltaTime);
                }
            }
            if (Mathf.Abs(cameraCurrentRotate.eulerAngles.y - children.eulerAngles.y) < 1f)
            {
                t = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dic">1表示向右，-1表示向左</param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        /// <returns></returns>
        private float GetEulerAngles(int dic, float startAngle,float endAngle)
        {
            if (dic == 1)
            {

            }
            else if (dic == -1)
            {

            }
            return 0;
        }
        /// <summary>
        /// 当角色移动时，相机跟着旋转
        /// </summary>
        /// <param name="angle">相机需要旋转的角度</param>
        public void CameraRotate(float angle)
        {
            CameraTotateAround(angle);
            cameraCurrentPos = children.localPosition;
            cameraCurrentRotate = children.localRotation;
        }
        private void CameraTotateAround(float angle)
        {
            children.RotateAround(target.transform.position, Vector3.up, angle);
        }
    }
}