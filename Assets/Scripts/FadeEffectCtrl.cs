using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeEffectCtrl : MonoBehaviour
{
    public Image fadeImage; // UI�� ������ �̹���
    public float fadeDuration = 1f; // ���̵� �ð�

    private void OnEnable()
    {
        if (fadeImage != null)
            fadeImage.enabled = true;
    }

    void Start()
    {
        StartCoroutine(FadeIn()); // �� ���� �� ���̵� �� ȿ�� ����
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
