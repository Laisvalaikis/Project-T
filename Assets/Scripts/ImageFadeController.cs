using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageFadeController : MonoBehaviour
{
    public float fadeInDuration = 0.2f;
    public float fadeOutDuration = 0.2f;

    private Image targetImage;
    private Coroutine fadeCoroutine;
    private float currentAlpha;

    private void Start()
    {
        targetImage = GetComponent<Image>();
        currentAlpha = targetImage.color.a;
    }

    public void FadeIn()
    {
        if (targetImage == null) return;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeImage(currentAlpha, 0.5f, fadeInDuration * (1f - currentAlpha)));
    }

    public void FadeOut()
    {
        if (targetImage == null) return;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeImage(currentAlpha, 0f, fadeOutDuration * currentAlpha));
    }

    private IEnumerator FadeImage(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color currentColor = targetImage.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            currentColor.a = currentAlpha;
            targetImage.color = currentColor;
            yield return null;
        }

        currentColor.a = endAlpha;
        targetImage.color = currentColor;
        currentAlpha = endAlpha;
    }
}
