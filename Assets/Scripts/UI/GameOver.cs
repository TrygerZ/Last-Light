using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void setUp()
    {
        gameObject.SetActive(true);

        // SFX kalah
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.loseSFX);
    }

    public void restart()
    {
        SceneManager.LoadScene("Scene Utama");
    }

    public void menu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
