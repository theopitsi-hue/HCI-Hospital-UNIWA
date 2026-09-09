
using UnityEngine;
using TMPro;
using System.Collections;

public class Toast : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Fade Settings")]
    [SerializeField] private float fadeInDuration = 0.15f;
    [SerializeField] private float fadeOutDuration = 0.4f;
    [SerializeField] private float lifetime = 3f;

    public void Setup(string message, Color color)
    {
        messageText.text = message;
        messageText.color = color;

        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        // Fade in
        canvasGroup.alpha = 0f;

        float timer = 0f;

        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(timer / fadeInDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // Stay visible
        yield return new WaitForSeconds(lifetime);

        // Fade out
        timer = 0f;

        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = 1f - Mathf.Clamp01(timer / fadeOutDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;

        Destroy(gameObject);
    }
}
