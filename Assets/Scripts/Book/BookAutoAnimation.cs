using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
using PJW.Book;

public class BookAutoAnimation : MonoBehaviour {
    private float startPosition;
    private float time;
    private bool autoMove = true;
    private int index;
    private Vector3 dic;
    private void Start()
    {
        time = UnityEngine.Random.Range(0.2f, 1f);
        startPosition = transform.localPosition.y;

        Invoke("StartBookAnimation", time);
    }
    /// <summary>
    /// 当书被点击到了，书飞向相机位置
    /// </summary>
    /// <param name="point"></param>
    public void Clicked(Vector3 point)
    {

        dic = new Vector3(point.x , point.y - 1, point.z);
        ClickedAnimation((dic)=> {
            //dic = dic.normalized;
            autoMove = false;
            transform.DOLocalRotate(new Vector3(-90, 0, 0), 2f);
            transform.DOScale(0.2f, 2f);
            transform.DOMove(dic, 2f).OnComplete(() => StartCoroutine(ChangeScene()));
        });
        
    }
    private void ClickedAnimation(Action<Vector3> action)
    {
        if (index > 1)
        {
            action(dic);
            return;
        }
        index++;
        transform.DOLocalRotate(new Vector3(0, 0, 10), 0.3f).OnComplete(() =>
        {
            transform.DOLocalRotate(new Vector3(0, 0, -10), 0.3f).OnComplete(() => ClickedAnimation(action));
        });
    }
    private void StartBookAnimation()
    {
        if (!autoMove) return;
        transform.DOLocalMoveY(startPosition + UnityEngine.Random.Range(0.1f, 0.4f), 0.3f).OnComplete(() =>
        {
            transform.DOLocalMoveY(startPosition, 0.3f).OnComplete(() => StartBookAnimation());
        });
    }
    private IEnumerator ChangeScene()
    {

        SceneManager.LoadSceneAsync("Book");
        //PlayerPrefs.SetString("AssetBundle", "dinosaurchangefoam.dinosaurchangefoam");
        yield return StartCoroutine(WaitLoadingNextScene("Book"));
    }
    private IEnumerator WaitLoadingNextScene(string v)
    {
        if (SceneManager.GetActiveScene().name != v)
        {
            GameCore.Instance.OpenLoadingPanel(Vector3.one);
            yield return null;
        }
    }
}