using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScreenFadeIn : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    public float fadeDuration;

    public void FadeStart(int scene)
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeIn(scene));
    }

    IEnumerator FadeIn(int scene)
    {
        float timer = 0f;

        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            color.a = timer / fadeDuration;

            fadeImage.color = color;

            yield return null;
        }

        color.a = 1;
        fadeImage.color = color;
        SceneManager.LoadScene(scene);
    }
}
