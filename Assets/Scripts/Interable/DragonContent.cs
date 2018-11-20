using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace PJW.Book
{

    public class DragonContent : MonoBehaviour
    {
        public bool isActive = true;
        private void OnEnable()
        {
            PlayAnim();
        }
        public void PlayAnim()
        {
            if (isActive)
            {
                transform.DOScale(1.1f, 0.6f).OnComplete(() =>
                {
                    transform.DOScale(1, 0.6f).OnComplete(() => PlayAnim());
                });
            }
        }
    }
}
