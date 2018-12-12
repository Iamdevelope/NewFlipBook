using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJW.Book
{
    [RequireComponent(typeof(Rigidbody))]
    /// <summary>
    /// 角色控制
    /// </summary>
    public class CharacterControllerByETC : MonoBehaviour
    {
        private Animator anim;
        private Rigidbody c_Rigidbody;
        
        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
            c_Rigidbody = GetComponent<Rigidbody>();

            c_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        private void Update()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (h != 0 || v != 0)
            {
                Move(new Vector2(h,v));
            }
        }
        public void Move(Vector2 vector2)
        {
            Vector3 temp = Physics.gravity * 2 - Physics.gravity;
            c_Rigidbody.AddForce(temp);
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            anim.SetFloat("turn", c_Rigidbody.velocity.y);
            
        }
    }
}