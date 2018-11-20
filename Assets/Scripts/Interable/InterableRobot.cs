using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJW.Book
{
    /// <summary>
    /// 机器人
    /// </summary>
    public class InterableRobot :  InterableObject{
        private Animator anim;
        private int index;
        private bool isPlayAnim;
        public override void GenerateEvent()
        {
            anim = GetComponentInChildren<Animator>();
        }
        public override void OnEnable()
        {
            transform.DOScale(1, 0.6f);
            if (anim == null)
                anim = GetComponentInChildren<Animator>();
        }
        public override void OnDisable()
        {
            transform.DOScale(Vector3.zero, 0.6f);
        }
        public override void OnMouseEnter()
        {
            Debug.Log(gameObject.name);
            if (!isPlayAnim)
            {
                isPlayAnim = true;
                anim.SetBool("dance", true);
                //anim.Play("Dance");
            }
        }
        public override void OnMouseExit()
        {
            anim.SetBool("dance", false);
            isPlayAnim = false;
        }
    }
}
