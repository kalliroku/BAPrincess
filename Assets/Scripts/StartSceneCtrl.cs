using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartSceneCtrl : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private FadeEffectCtrl fadeEffectCtrl;
    [SerializeField] private GameObject titleText;
    [SerializeField] private GameObject startText;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject videoScreen;
    [SerializeField] private Image fairy;

    private void OnEnable()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnClickStartButton);
        }
        if (videoPlayer!= null)
        {
            videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
        }
        MusicManagerCtrl.Instance.PlayBGM(0, 1.8f);
    }

    private void OnDisable()
    {
        if (startButton != null)
        {
            startButton.onClick.RemoveListener(OnClickStartButton);
        }
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= VideoPlayer_loopPointReached;
        }
    }

    private void PlayVideo()
    { 
        if (videoScreen != null && videoPlayer != null)
        {
            videoScreen.SetActive(true);
            videoPlayer.Play();
        }
        else
        {
            videoPlayer.loopPointReached -= VideoPlayer_loopPointReached;
        }
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        // videoScreen.SetActive(false);
        LoadNextScean();
    }

    private void LoadNextScean()
    {
        SfxManagerCtrl.Instance.PlaySfx(5);
        MusicManagerCtrl.Instance.StopBGM(1f);
        string sceneName = "OpeningScene";
        fadeEffectCtrl.LoadSceneWithFade(sceneName);
    }

    private void OnClickStartButton()
    {
        if (startText != null)
        {
            startText.SetActive(false);
        }
        if (titleText!= null)
        {
            titleText.SetActive(false);
        }
        PlayVideo();
        StartCoroutine(ShowFairyDelay());
    }

    private System.Collections.IEnumerator ShowFairyDelay()
    {
        yield return new WaitForSeconds(3f);

        float duration = 3f;
        float elapsed = 0f;

        // 초기 상태 저장
        Color startColor = fairy.color;
        startColor.a = 0f;
        fairy.color = startColor;

        Vector3 startScale = new(0.2f, 0.2f, 1f);
        Vector3 endScale = new(1f, 1f, 1f);
        fairy.transform.localScale = startScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // 알파값 점점 증가
            Color newColor = fairy.color;
            newColor.a = Mathf.Lerp(0f, 1f, t);
            fairy.color = newColor;

            // 스케일 점점 증가
            fairy.transform.localScale = Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }

        Color finalColor = fairy.color;
        finalColor.a = 1f;
        fairy.color = finalColor;
        fairy.transform.localScale = endScale;
    }
}
