using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeEffectCtrl : MonoBehaviour
{
    public Image fadeImage; // UI의 검은색 이미지
    public float fadeDuration = 1f; // 페이드 시간

    private void OnEnable()
    {
        if (fadeImage != null)
            fadeImage.enabled = true;
    }

    void Start()
    {
        StartCoroutine(FadeIn()); // 씬 시작 시 페이드 인 효과 실행
    }

    public void LoadSceneWithFade(string sceneName)
    {
        if (gameObject.activeSelf != false)
        {
            gameObject.SetActive(true);
        }
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0)
        {
            t -= Time.deltaTime / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }
    }

    IEnumerator FadeOut(string sceneName)
    {
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
}
