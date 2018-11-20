using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJW.Book
{
    /// <summary>
    /// 游戏中角色行为
    /// </summary>
    public class InterableAnimal : InterableObject
    {
        public const string HUBOSHI = "huboshi";
        public const string CONTENT = "content";
        public const string CLICKED = "clicked";
        private Animator anim;
        private float startX;
        private float endX;
        private float clam;
        private bool isFirst = true;
        protected override void Start()
        {
            anim = GetComponent<Animator>();
        }
        public override void OnMouseDown()
        {
            if (gameObject.transform.parent.name == HUBOSHI)
                transform.Find("book").gameObject.SetActive(false);
            anim.SetBool(CONTENT, true);
        }
        private void Update()
        {
            if (GameCore.CurrentObject) return;
            if (Input.GetMouseButton(0))
            {
                if (isFirst)
                {
                    isFirst = false;
                    startX = Input.mousePosition.x;
                }
                endX = Input.mousePosition.x;
                clam = endX - startX;
                transform.Rotate(new Vector3(0, -clam * 0.2f, 0));
                startX = Input.mousePosition.x;
            }
            if (Input.GetMouseButtonUp(0))
                isFirst = true;
        }

        /// <summary>
        /// 当介绍动画播放结束后
        /// </summary>
        public void ContentOver()
        {
            if (gameObject.transform.parent.name == HUBOSHI)
                transform.Find("book").gameObject.SetActive(true);
            anim.SetBool(CONTENT, false);
        }
        public void Enter()
        {
            Quaternion q = Quaternion.Euler(Vector3.zero);
            transform.rotation = Quaternion.FromToRotation(transform.rotation.eulerAngles, Vector3.zero);
            anim.SetBool(CLICKED, true);
            anim.SetBool(CONTENT, false);
        }
        /// <summary>
        /// 当被选中动画播放结束，进入游戏主场景
        /// </summary>
        public void EnterSelect()
        {
            anim.SetBool(CLICKED, false);
            GameCore.Instance.CloseCurrentUIPanel();
            GameCore.Instance.isSuccessLogin = true;
        }
    }
}