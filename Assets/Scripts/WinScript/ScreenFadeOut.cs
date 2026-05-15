using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFadeOut : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeDuration;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
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
        gameObject.SetActive(false);
    }
}
