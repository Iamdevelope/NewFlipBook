using UnityEngine;
using System.Collections;

public class WorldRotationTween : MonoBehaviour
{

    private string m_path;
    private float moveTime = 1.2f;
    private float moveDelay = 0.2f;
    private iTween.EaseType moveEaseType = iTween.EaseType.easeOutCubic;
    public Transform m_lookTarget;

    // Use this for initialization
    void Start()
    {
        if (m_lookTarget == null)
        {
            return;
        }
        if (string.IsNullOrEmpty(m_path))
        {
            m_path = m_lookTarget.name;
        }
        Vector3[] path = iTweenPath.GetPath(m_path);
        if (m_lookTarget)
            iTween.MoveTo(gameObject, iTween.Hash("path", path, "time", moveTime, "delay", moveDelay, "looktarget", m_lookTarget, "axis", "x", "easetype", moveEaseType));
        else
            iTween.MoveTo(gameObject, iTween.Hash("path", path, "time", moveTime, "delay", moveDelay, "orienttopath", true, "axis", "x", "easetype", moveEaseType));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
