using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public void ShowWin()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Win Scene");
        Debug.Log($"🏆 PLAYER MENANG! Loading scene: Win Scene");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Scene");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
