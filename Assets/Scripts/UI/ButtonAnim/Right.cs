using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Right : MonoBehaviour {
    private float startPosition;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(RightButtonHandle);
        startPosition = transform.localPosition.x;
        StartCoroutine(ButtonAnim());
    }

    public void RightButtonHandle()
    {
        GetComponentInParent<Scene2Back>().CameraAnimation(1);
    }

    private IEnumerator ButtonAnim()
    {
        yield return null;
        transform.DOLocalMoveX(startPosition - UnityEngine.Random.Range(3f,7), 0.6f).OnComplete(() =>
        {
            transform.DOLocalMoveX(startPosition, 0.6f).OnComplete(() => StartCoroutine(ButtonAnim()));
        });
    }
}
