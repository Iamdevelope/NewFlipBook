using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PJW.Book
{
    public class Left : MonoBehaviour
    {
        private float startPosition;
        private void Start()
        {
            startPosition = transform.localPosition.x;
            StartCoroutine(ButtonAnim());
            GetComponent<Button>().onClick.AddListener(LeftButtonHandle);
        }

        private void LeftButtonHandle()
        {
            GetComponentInParent<Scene2Back>().CameraAnimation(-1);
        }

        private IEnumerator ButtonAnim()
        {
            yield return null;
            transform.DOLocalMoveX(startPosition + UnityEngine.Random.Range(3f, 7), 0.6f).OnComplete(() =>
            {
                transform.DOLocalMoveX(startPosition, 0.6f).OnComplete(() => StartCoroutine(ButtonAnim()));
            });
        }
    }
}