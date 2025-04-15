using UnityEngine;

public class OpeningSceneCtrl : MonoBehaviour
{
    [SerializeField] private UserNameInputCtrl nameInputCtrl;
    [SerializeField] private ConversationCtrl conversationCtrl;
    [SerializeField] private UserInfo userInfo;
    [SerializeField] private FadeEffectCtrl fadeEffectCtrl;

    private void OnUserNameSubmit(string userName)
    {
        if (userInfo == null)
        {
            userInfo = UserInfo.GetInstance();
        }
        if (userInfo != null)
        {
            userInfo.SetName(userName ?? "¿Ãª€ ¡ˆ¿Ø");
        }

        if (conversationCtrl != null)
        {
            conversationCtrl.StartConversation(LoadPlayScene);
        }
        else
        {
            LoadPlayScene();
        }
    }

    private void LoadPlayScene()
    {
        string sceneName = "PlayScene";
        fadeEffectCtrl.LoadSceneWithFade(sceneName);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (nameInputCtrl != null)
        {
            nameInputCtrl.ShowUserNameInput(OnUserNameSubmit);
        }
        else
        {
            LoadPlayScene();
        }
    }
}
