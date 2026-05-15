using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject fadeObj;
    private ScreenFadeIn fadeIn;
    [SerializeField] int scene;

    private void Start()
    {
        fadeIn = fadeObj.GetComponent<ScreenFadeIn>();
    }
    public void PlayGame()
    {
        fadeIn.FadeStart(scene);
    }
}