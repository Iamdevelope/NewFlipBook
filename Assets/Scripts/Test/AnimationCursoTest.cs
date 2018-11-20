using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCursoTest : MonoBehaviour {
    public AnimationCurve curso;

    private void Update()
    {
        float t = curso.Evaluate(Time.time);
        transform.Translate(new Vector3(t, 0, 0));
    }
}
