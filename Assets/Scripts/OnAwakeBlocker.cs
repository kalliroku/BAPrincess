using UnityEngine;
using UnityEngine.UI;

public class OnAwakeBlocker : MonoBehaviour
{
    public Image[] fadeImages;
    [SerializeField] GameObject loadingText;
    public float fadeDuration = 1f; // 페이드 시간

    private void Awake()
    {
        StartCoroutine(DelayedFadeOut());
    }

    System.Collections.IEnumerator DelayedFadeOut()
    {
        yield return new WaitForSeconds(0.5f); // 0.5초 기다림
        if (loadingText!= null )
        {
            loadingText.SetActive(false);
        }
        yield return StartCoroutine(FadeOut());
    }

    System.Collections.IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / fadeDuration;
            for (int i = 0; i < fadeImages.Length; i++)
            {
                fadeImages[i].color = new Color(0, 0, 0, t);
            }
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
