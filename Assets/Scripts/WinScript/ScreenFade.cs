using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeDuration;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;

        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            color.a = 1 - (timer / fadeDuration);

            fadeImage.color = color;

            yield return null;
        }

        color.a = 0;
        fadeImage.color = color;
    }
}