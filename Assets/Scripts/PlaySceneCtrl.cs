using UnityEngine;

public class PlaySceneCtrl : MonoBehaviour
{
    [SerializeField] GameObject controlPanel;
    [SerializeField] GameObject hero;
    [SerializeField] FadeEffectCtrl fadeEffectCtrl;

    private UserInfo userInfo;

    private void Awake()
    {
        if (fadeEffectCtrl != null)
        {
            fadeEffectCtrl.gameObject.SetActive(true);
        }
        userInfo = UserInfo.GetInstance();
        if (userInfo != null)
        {
            userInfo.OnItemsSelected.AddListener(LoadNextScene);
        }
        else
        {
            Debug.LogWarning("userInfo is null");
        }
    }

    void Start()
    {
        MusicManagerCtrl musicManager = MusicManagerCtrl.Instance;
        if (musicManager != null)
        {
            musicManager.PlayBGM(1, 1f);
        }
        if (hero != null)
        {
            hero.SetActive(true);
        }
        if (controlPanel != null)
        {
            controlPanel.SetActive(true);
        }
    }

    void LoadNextScene()
    {
        fadeEffectCtrl.LoadSceneWithFade("EndingScene");
    }

    private void OnDestroy()
    {
        if (userInfo != null)
        {
            userInfo.OnItemsSelected.RemoveListener(LoadNextScene);
        }
    }
}
