using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class BookAutoAnimation : MonoBehaviour {
    private float startPosition;
    private float time;
    private void Start()
    {
        time = UnityEngine.Random.Range(0.2f, 1f);
        startPosition = transform.localPosition.y;

        Invoke("StartBookAnimation", time);
    }

    private void StartBookAnimation()
    {
        transform.DOLocalMoveY(startPosition + UnityEngine.Random.Range(0.1f, 0.4f), 0.6f).OnComplete(() =>
        {
            transform.DOLocalMoveY(startPosition, 0.6f).OnComplete(() => StartBookAnimation());
        });
    }
}