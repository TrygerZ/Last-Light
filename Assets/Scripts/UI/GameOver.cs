using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void setUp()
    {
        gameObject.SetActive(true);

        // Stop music & SFX kalah (lebih pelan dari SFX lain)
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.loseSFX, 0.5f);
        }
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
