using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PJW.Book;
using PJW.Book.UI;
using System;

namespace PJW.Book
{
    public class StartAnim : MonoBehaviour
    {

        public const string JIANKANG_JINGLINGWU = "jiankang_jinglingwu";
        public const string KEXUE_KEJICHENG = "kexue_kejicheng";
        public const string YISHU_HAITAN = "yishu_haidaochuan";
        public const string YUYAN_MOGUBAO = "yuyan_mogubao";
        public const string SHEHUI_CUNZHUANG = "shehui_cunzhuang";
        public const string HOME = "home";

        public bool isArrived;
        private CameraPathBezierAnimator jiankang_jinglingwu;
        private CameraPathBezierAnimator kexue_kejicheng;
        private CameraPathBezierAnimator yishu_haitan;
        private CameraPathBezierAnimator yuyan_mogubao;
        private CameraPathBezierAnimator shehui_cunzhuang;
        [HideInInspector]
        public CameraPathBezierAnimator currentTargetPointName;
        [HideInInspector]
        public string currentTargetName;
        private void Awake()
        {
            jiankang_jinglingwu = transform.Find(JIANKANG_JINGLINGWU).GetComponent<CameraPathBezierAnimator>();
            kexue_kejicheng = transform.Find(KEXUE_KEJICHENG).GetComponent<CameraPathBezierAnimator>();
            yishu_haitan = transform.Find(YISHU_HAITAN).GetComponent<CameraPathBezierAnimator>();
            yuyan_mogubao = transform.Find(YUYAN_MOGUBAO).GetComponent<CameraPathBezierAnimator>();
            shehui_cunzhuang = transform.Find(SHEHUI_CUNZHUANG).GetComponent<CameraPathBezierAnimator>();

            jiankang_jinglingwu.AnimationFinished += ArrayTargetPoint;
            kexue_kejicheng.AnimationFinished += ArrayTargetPoint;
            yishu_haitan.AnimationFinished += ArrayTargetPoint;
            yuyan_mogubao.AnimationFinished += ArrayTargetPoint;
            shehui_cunzhuang.AnimationFinished += ArrayTargetPoint;


            jiankang_jinglingwu.AnimationPointReached += GoBackFromTargetPoint;
            kexue_kejicheng.AnimationPointReached += GoBackFromTargetPoint;
            yishu_haitan.AnimationPointReached += GoBackFromTargetPoint;
            yuyan_mogubao.AnimationPointReached += GoBackFromTargetPoint;
            shehui_cunzhuang.AnimationPointReached += GoBackFromTargetPoint;
            
        }
        /// <summary>
        /// 点击返回时，设置摄像机模式为用户控制
        /// </summary>
        private void GoBackFromTargetPoint()
        {
            if (currentTargetPointName.mode == CameraPathBezierAnimator.modes.reverse && currentTargetPointName.atPointNumber <= 1)
                currentTargetPointName.bezier.mode = CameraPathBezier.viewmodes.usercontrolled;
        }
        /// <summary>
        /// 摄像机到达目标点
        /// </summary>
        private void ArrayTargetPoint()
        {
            if (isArrived || (currentTargetPointName.mode == CameraPathBezierAnimator.modes.reverse))
            {
                return;
            }
            GameCore.Instance.OpenNextUIPanel(FindObjectOfType<WecomePanel>().gameObject, currentTargetName);

            isArrived = true;
        }
        /// <summary>
        /// 到目标地点
        /// </summary>
        /// <param name="pointName"></param>
        public void ToTargetPoint(string pointName)
        {
            switch (pointName)
            {
                case JIANKANG_JINGLINGWU:
                    currentTargetPointName = jiankang_jinglingwu;
                    currentTargetName = "精灵屋";
                    break;
                case KEXUE_KEJICHENG:
                    currentTargetPointName = kexue_kejicheng;
                    currentTargetName = "科技城";
                    break;
                case YISHU_HAITAN:
                    currentTargetPointName = yishu_haitan;
                    currentTargetName = "海滩";
                    break;
                case YUYAN_MOGUBAO:
                    currentTargetPointName = yuyan_mogubao;
                    currentTargetName = "蘑菇堡";
                    break;
                case SHEHUI_CUNZHUANG:
                    currentTargetPointName = shehui_cunzhuang;
                    currentTargetName = "村庄";
                    break;
                case HOME:
                    currentTargetPointName.mode = CameraPathBezierAnimator.modes.reverse;
                    currentTargetPointName.Play();
                    break;
                default:
                    break;
            }

            if (!isArrived) return;
            isArrived = false;
            if (currentTargetPointName != null && !isArrived)
            {
                currentTargetPointName.mode = CameraPathBezierAnimator.modes.once;
                currentTargetPointName.bezier.mode = CameraPathBezier.viewmodes.followpath;
            }
            
            if (currentTargetPointName != null && !isArrived)
            {
                currentTargetPointName.Play();
            }
        }
        /// <summary>
        /// 回到起始点
        /// </summary>
        public void GoStartPoint()
        {

            GameCore.Instance.CloseCurrentUIPanel();
            if (currentTargetPointName == null) return;
            currentTargetPointName.mode = CameraPathBezierAnimator.modes.reverse;
            currentTargetPointName.bezier.mode = CameraPathBezier.viewmodes.reverseFollowpath;
            currentTargetPointName.Play();
            StartCoroutine(WaitTimeArrivedHome(isArrived ? currentTargetPointName.pathTime : currentTargetPointName.usedAnimationTime));
            isArrived = false;
        }

        private IEnumerator WaitTimeArrivedHome(float pathTime)
        {
            yield return new WaitForSeconds(pathTime);
            isArrived = true;
        }

        private void OnGUI()
        {
            if (GameCore.Instance.isSuccessLogin)
            {
                if (GUI.Button(new Rect(50, 50, 100, 100), "精灵屋"))
                {
                    ToTargetPoint(JIANKANG_JINGLINGWU);
                }
                if (GUI.Button(new Rect(50, 160, 100, 100), "科技城"))
                {
                    ToTargetPoint(KEXUE_KEJICHENG);
                }
                if (GUI.Button(new Rect(50, 270, 100, 100), "海滩"))
                {
                    ToTargetPoint(YISHU_HAITAN);
                }
                if (GUI.Button(new Rect(50, 380, 100, 100), "蘑菇屋"))
                {
                    ToTargetPoint(YUYAN_MOGUBAO);
                }
                if (GUI.Button(new Rect(50, 490, 100, 100), "村庄"))
                {
                    ToTargetPoint(SHEHUI_CUNZHUANG);
                }
                if (GUI.Button(new Rect(50, 600, 100, 100), "回家"))
                {
                    GoStartPoint();
                }
            }
        }
    }
}
