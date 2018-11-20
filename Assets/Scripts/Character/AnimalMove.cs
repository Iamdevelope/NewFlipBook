using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PJW.Book
{
    /// <summary>
    /// 动物移动控制
    /// </summary>
    public class AnimalMove : MonoBehaviour
    {
        private Animator anim;
        private CharacterCamera characterCamera;
        public float moveSpeed;
        public float rotationSpeed;
        public bool isMove { get; private set; }

        private CharacterController controller;
        
        private void Start()
        {
            characterCamera = GameCore.CharacterCamera.GetComponent<CharacterCamera>();
            controller = GetComponent<CharacterController>();
            anim = GetComponentInChildren<Animator>();
        }
        private void FixedUpdate()
        {
            
            float z = Input.GetAxis("Horizontal");
            float x = Input.GetAxis("Vertical");

            if (x != 0)
            {
                isMove = true;
                transform.Translate(new Vector3(0, 0, x * moveSpeed * Time.deltaTime * 0.1f));
            }
            if (Mathf.Abs(x) < 0.6f)
            {
                isMove = false;
                anim.SetBool("Idle", true);
            }
            else
                anim.SetBool("Idle", false);
            if (z != 0)
            {
                Quaternion q = Quaternion.AngleAxis(z, transform.up);
                transform.rotation *= Quaternion.Lerp(transform.rotation, q, 300 * Time.deltaTime);
                //transform.Rotate(transform.up, z);
                characterCamera.CameraRotate(z);
            }
        }
    }
}