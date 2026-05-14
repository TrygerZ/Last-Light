using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject winPanel;

    private void Awake()
    {
        if (winPanel != null)
            winPanel.SetActive(false);
    }

    public void ShowWin()
    {
        if (winPanel != null)
            winPanel.SetActive(true);

        Time.timeScale = 0f;
        Debug.Log("🏆 PLAYER MENANG! Berhasil bertahan 5 menit!");
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
