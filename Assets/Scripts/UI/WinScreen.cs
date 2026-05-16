using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private int scene;
    [SerializeField] GameObject fadeObj;
    private ScreenFadeIn fade;

    private void Start()
    {
        fade = fadeObj.GetComponent<ScreenFadeIn>();
    }
    public void ShowWin()
    {
        Time.timeScale = 1f;
        fade.FadeStart(scene);
    }
}
