using UnityEngine;
using UnityEngine.UI;

public class StartSceneCtrl : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] FadeEffectCtrl fadeEffectCtrl;

    private void OnEnable()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnClickStartButton);
        }
        MusicManagerCtrl.Instance.PlayBGM(0, 1.8f);
    }

    private void OnDisable()
    {
        if (startButton != null) {
            startButton.onClick.RemoveListener(OnClickStartButton);
        }
    }

    private void OnClickStartButton()
    {
        SfxManagerCtrl.Instance.PlaySfx(5);
        MusicManagerCtrl.Instance.StopBGM(1f);
        string sceneName = "OpeningScene";
        fadeEffectCtrl.LoadSceneWithFade(sceneName);
    }
}
