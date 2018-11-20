using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace PJW.Book
{
    /// <summary>
    /// 灯光颜色变化
    /// </summary>
    public class LightRangeColor : MonoBehaviour
    {
        private Light[] lights;
        private Color newColor;
        private void Start()
        {
            lights = GetComponentsInChildren<Light>();
            StartCoroutine(MeshColorChanger());
        }
        private IEnumerator MeshColorChanger()
        {
            yield return new WaitForSeconds(Random.Range(3, 8f));
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].DOColor(newColor, 0.6f);
            }
            StartCoroutine(MeshColorChanger());
        }

        // Update is called once per frame
        void Update()
        {

            float offset = transform.position.magnitude / 3;

            float r = Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad + offset));
            float g = Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 0.45f + offset));
            float b = Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 1.2f + offset));
            newColor = new Color(r, g, b);

        }
    }
}