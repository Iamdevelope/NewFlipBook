using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace PJW.Book
{

    /// <summary>
    /// 这是个测试的
    /// </summary>
    public class InterableTest : InterableObject
    {

        public override void OnMouseDown()
        {
            Debug.Log(gameObject.name);
        }
        public override void OnMouseEnter()
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        public override void OnMouseExit()
        {
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            GetComponent<MeshRenderer>().material.color = Color.white;
            transform.DOScale(Vector3.zero, 0.6f);
        }
        public override void OnEnable()
        {
            transform.DOScale(Vector3.one, 0.6f);
        }

        public override void MoveLeave()
        {
            gameObject.transform.DOLocalMove(new Vector3(100, transform.position.y, transform.position.z), 1);
        }
        public override void Move()
        {
            transform.DOLocalMove(position, 1f);
        }
        
    }
}
