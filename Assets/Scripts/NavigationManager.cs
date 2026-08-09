using System.Collections;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public Material[] panoramas;
    private int currentIndex = 0;

    [Header("Transição (fade + som)")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 0.25f;
    public AudioSource audioSource;
    public AudioClip transitionSound;

    void Start()
    {
        UpdateSkybox();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            GoForward();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            GoBack();
        }
    }

    public void GoForward()
    {
        if (currentIndex < panoramas.Length - 1)
        {
            currentIndex++;
            UpdateSkybox();
        }
    }

    public void GoBack()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateSkybox();
        }
    }

    void UpdateSkybox()
    {
        StartCoroutine(FadeAndSwitch());
    }

    IEnumerator FadeAndSwitch()
    {
        if (audioSource != null && transitionSound != null)
            audioSource.PlayOneShot(transitionSound);

        yield return StartCoroutine(Fade(1f));
        RenderSettings.skybox = panoramas[currentIndex];
        yield return StartCoroutine(Fade(0f));
    }

    IEnumerator Fade(float targetAlpha)
    {
        if (fadeCanvasGroup == null) yield break;

        float startAlpha = fadeCanvasGroup.alpha;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = targetAlpha;
    }
}