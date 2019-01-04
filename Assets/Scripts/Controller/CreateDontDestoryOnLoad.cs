using PJW.Book;
using UnityEngine;

public class CreateDontDestoryOnLoad : MonoBehaviour {
    private static bool isDontDestroyOnLoad;
    private static GameObject temp;
    public GameObject messageObject;
    private void Awake()
    {
        if (!isDontDestroyOnLoad)
        {
            isDontDestroyOnLoad = true;
            temp = new GameObject("GameCore");
            GameObject message = Instantiate(messageObject);
            temp.AddComponent<GameController>();
            temp.AddComponent<SoundManager>();
            temp.AddComponent<GameCore>();
            temp.AddComponent<AudioSource>();
            temp.AddComponent<Console>().toggleKey = KeyCode.Escape;
            DontDestroyOnLoad(temp);
            DontDestroyOnLoad(message);
        }
        temp.GetComponent<GameCore>().Init();
    }
}
