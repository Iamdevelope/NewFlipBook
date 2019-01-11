using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJW.Book
{
    [SerializeField]
    public class GameData
    {
        public Vector3 CameraPosition { get; set; }
        public Quaternion CameraRotation { get; set; }
        //public UserData UserData { get; set; }
        public bool CloseAllPanel { get; set; }
        public CameraPathBezierAnimator.modes CameraPathBezierAnimatorModes { get; set; }
        public CameraPathBezier.viewmodes CameraPathBezierViewModes { get; set; }
        public string CurrentTargerPointName { get; set; }
        public bool IsTarget { get; set; }
    }
    /// <summary>
    /// 游戏初始化
    /// </summary>
    public class GameInit : MonoBehaviour
    {
        public void Start()
        {
            if (GameCore.GameData == null) return;
            Debug.Log(GameCore.GameData.CameraPosition + "\n" + GameCore.GameData.CurrentTargerPointName + "\n" + GameCore.GameData.IsTarget);
            //Camera.main.transform.position = GameCore.GameData.CameraPosition;
            //Camera.main.transform.rotation = GameCore.GameData.CameraRotation;
            //FindObjectOfType<StartAnim>().isArrived = GameCore.GameData.IsTarget;

            //FindObjectOfType<StartAnim>().ToTargetPoint(GameCore.GameData.CurrentTargerPointName);
            //FindObjectOfType<StartAnim>().currentTargetPointName.atPointNumber = FindObjectOfType<StartAnim>().currentTargetPointName.bezier.controlPoints.Length - 1;
        }
        public void SaveSceneData()
        {
            GameCore.GameData = new GameData();
            GameData temp = new GameData();
            temp.CameraPosition = Camera.main.transform.position;
            temp.CameraRotation = Camera.main.transform.rotation;
            temp.CloseAllPanel = true;
            temp.CameraPathBezierAnimatorModes = FindObjectOfType<StartAnim>().currentTargetPointName.mode;
            temp.CameraPathBezierViewModes = FindObjectOfType<StartAnim>().currentTargetPointName.bezier.mode;
            temp.CurrentTargerPointName = FindObjectOfType<StartAnim>().currentTargetPointName.gameObject.name;
            temp.IsTarget = FindObjectOfType<StartAnim>().isArrived;
            GameCore.GameData = temp;
        }
    }
}