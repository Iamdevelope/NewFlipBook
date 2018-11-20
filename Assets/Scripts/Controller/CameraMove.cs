using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace PJW.Book
{
    /// <summary>
    /// 摄像机控制
    /// </summary>
    public class CameraMove : MonoBehaviour
    {
        public void MoveToTarget(Vector3 targetPoint)
        {
            transform.DOLocalMove(targetPoint, 1f).OnComplete(() => StartCoroutine(ChangeScene()));
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
}