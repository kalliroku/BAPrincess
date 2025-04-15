using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_ANDROID && !UNITY_EDITOR
    using UnityEngine.Android;
#endif

public class ReplayQueryModalCtrl : MonoBehaviour
{
    [SerializeField] private Button quitButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private FadeEffectCtrl fadeEffectCtrl;
    [SerializeField] private GameObject modalBody;

    private Action callback = null;

    private void OnEnable()
    {
        if (replayButton != null)
        {
            replayButton.onClick.AddListener(OnReplayButtonClicked);
        }
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }
    }

    private void OnDisable()
    {
        if (replayButton != null)
        {
            replayButton.onClick.RemoveListener(OnReplayButtonClicked);
        }
        if (quitButton != null)
        {
            quitButton.onClick.RemoveListener(OnQuitButtonClicked);
        }
    }

    public void ShowModal(Action callback)
    {
        gameObject.SetActive(true);
        this.callback = callback;
    }

    private void OnQuitButtonClicked()
    {
        if (callback != null)
        {
            callback();
        }
        gameObject.SetActive(false);
        Application.Quit();

#if UNITY_ANDROID && !UNITY_EDITOR
    System.Diagnostics.Process.GetCurrentProcess().Kill(); // 강제 종료
#endif
    }

    private void OnReplayButtonClicked()
    {
        if(callback != null)
        {
            callback();
        }
        var userInfo = UserInfo.GetInstance();
        if (userInfo != null) userInfo.ClearUserInfo();
        if (modalBody != null)
        {
            modalBody.SetActive(false);
        }
        if (fadeEffectCtrl != null)
        {
            fadeEffectCtrl.gameObject.SetActive(true);
            fadeEffectCtrl.LoadSceneWithFade("StartScene");
        }
    }
}
