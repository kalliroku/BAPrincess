using System.Collections;
using UnityEngine;

public class MusicManagerCtrl : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] bgms;

    private static MusicManagerCtrl instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static MusicManagerCtrl Instance => instance;

    int currentPlayBgmIndex = -1;

    public void PlayBGM(int index, float fadeInDuration = 1f)
    {
        if (audioSource == null || bgms.Length == 0) return;
        int bgmIndex = index % bgms.Length;
        if (bgmIndex != currentPlayBgmIndex)
        {
            currentPlayBgmIndex = bgmIndex;
            PlayBgmWithFadeIn(bgms[bgmIndex], fadeInDuration);
        }
    }

    private void PlayBgmWithFadeIn(AudioClip clip, float fadeInDuration = 1f)
    {
        StartCoroutine(FadeInBgm(clip, fadeInDuration));
    }

    IEnumerator FadeInBgm(AudioClip clip, float duration)
    {
        audioSource.clip = clip;
        audioSource.volume = 0;
        audioSource.Play();

        float time = 0;
        while (time < duration)
        {
            audioSource.volume = Mathf.Lerp(0, 1, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 1;
    }

    public void StopBGM(float fadeOutDuration = 0.2f)
    {
        if (currentPlayBgmIndex == -1) return;
        StartCoroutine(FadeOutBgm(fadeOutDuration));
    }

    IEnumerator FadeOutBgm(float duration)
    {
        currentPlayBgmIndex = -1;
        float startVolume = audioSource.volume;
        float time = 0;

        while (time < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = null;
        audioSource.volume = 1;
    }

    public void ChangeBGM(int index, float fadeInDuration = 1f)
    {
        StartCoroutine(ChangeBgmCoroutine(index, fadeInDuration));
    }

    IEnumerator ChangeBgmCoroutine(int index, float fadeInDuration = 1f)
    {
        yield return FadeOutBgm(0.2f);
        PlayBgmWithFadeIn(bgms[index % bgms.Length], fadeInDuration);
    }
}