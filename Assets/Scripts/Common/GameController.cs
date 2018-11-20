using UnityEngine;

namespace PJW
{
    /// <summary>
    /// 游戏控制器
    /// </summary>
    public class GameController : MonoBehaviour
    {
        private float t;
        private void FixedUpdate()
        {
            GameQuit();
        }
        /// <summary>
        /// 退出游戏
        /// </summary>
        private void GameQuit()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                t = 0;
            t += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Escape) && t < 0.2f)
                Application.Quit();
        }
    }
}