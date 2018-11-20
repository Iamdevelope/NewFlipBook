using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyCommon;
using UnityEngine.UI;
using UnityEngine.Video;

namespace PJW.Book
{

    /// <summary>
    /// 可交互对象
    /// </summary>
    public class InterableObject : MonoSingleton<InterableObject>,ICanReset
    {
        protected Vector3 position;
        /// <summary>
        /// 可以交互
        /// </summary>
        public bool isTrigger = true;
        protected virtual void Start() { }
        /// <summary>
        /// 点击物体触发的事件
        /// </summary>
        public virtual void OnMouseDown() { }
        public virtual void OnMouseUp() { }
        /// <summary>
        /// 碰到物体触发的事件
        /// </summary>
        public virtual void OnMouseEnter() { }
        /// <summary>
        /// 从物体身上移除时所触发的事件
        /// </summary>
        public virtual void OnMouseExit() { }
        /// <summary>
        /// 物体被显示时触发的事件
        /// </summary>
        public virtual void OnEnable() { }
        /// <summary>
        /// 物体隐藏时触发的事件
        /// </summary>
        public virtual void OnDisable() { }
        /// <summary>
        /// 对象被创建时触发的事件
        /// </summary>
        public virtual void GenerateEvent() { }
        /// <summary>
        /// 对象被创建时触发的事件
        /// </summary>
        public virtual void GenerateEvent(VideoPlayer game,List<VideoClip> movies) { }
        public virtual void GenerateEvent(VideoPlayer game,VideoClip clip) { }
        public virtual void GenerateEvent(string bookName) { }
        public virtual void MoveLeave() { }
        public virtual void Move() { }


        public virtual void Reset()
        {
            
        }
    }
}