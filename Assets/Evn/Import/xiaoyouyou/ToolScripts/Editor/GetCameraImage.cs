using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class GetCameraImage : MonoBehaviour {

    public TextureFormat m_textureFormat = TextureFormat.ARGB32;
    public Transform m_targetTran;

    private Texture2D m_texture;
    private bool m_photograph = false;

    void Start()
    {
        if (GetComponent<Camera>() == null || m_targetTran == null)
        {
            this.enabled = false;
            return;
        }
        GetComponent<Camera>().orthographic = true;
        GetComponent<Camera>().aspect = 1;

        transform.position = m_targetTran.position - m_targetTran.forward * 5;
        transform.LookAt(m_targetTran);
        GetComponent<Camera>().orthographicSize = m_targetTran.lossyScale.x / 2;
    }

    void OnGUI()
    {
        if(m_photograph)
            return;

        GUI.Label(new Rect(100, 100, 200, 50), "W = " + Screen.width + "\t H = " + Screen.height);

        if(GUI.Button(new Rect(100,250,100,50),"Take a texture"))
        {
            m_photograph = true;
            StartCoroutine(Photograph());
        }
    }

    private Texture2D GetScreenTexture(TextureFormat format)
    {
        int width, height;
        width = Screen.width;
        height = Screen.height;

        Texture2D screenTex = new Texture2D(width, height,format,false);
        screenTex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenTex.Apply();
        return screenTex;
    }

    IEnumerator Photograph()
    {
        yield return new WaitForSeconds(0.1f);

        yield return new WaitForEndOfFrame();
        m_texture = GetScreenTexture(m_textureFormat);
        m_photograph = false;
        SaveTexture();
    }

    private void SaveTexture()
    {
        string path = EditorUtility.SaveFilePanel("Save", Application.dataPath, "", "png");

        if (path != "")
        {
            byte[] bytes = m_texture.EncodeToPNG();
            if (File.Exists(path))
                File.Delete(path);

            File.WriteAllBytes(path,bytes);
        }
    }
}
