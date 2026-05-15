using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public void setUp()
    {
        gameObject.SetActive(true);
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
