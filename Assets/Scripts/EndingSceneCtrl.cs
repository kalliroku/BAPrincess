using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class EndingSceneCtrl : MonoBehaviour
{
    [SerializeField] private ReplayQueryModalCtrl replayQueryModal;
    [SerializeField] private HeroCtrl heroCtrl;
    [SerializeField] private ConversationCtrl conversationCtrl;
    [SerializeField] private GameObject[] backgrounds;
    [SerializeField] private GameObject videoScreen;
    [SerializeField] private VideoPlayer videoPlayer;

    private void Awake()
    {
        var userInfo = UserInfo.GetInstance();
        if (userInfo == null)
        {
            throw new MissingComponentException("user info is missing");
        }
        if (heroCtrl == null)
        {
            throw new MissingComponentException("hero is missig");
        }
        if (conversationCtrl == null)
        {
            throw new MissingComponentException("conversation is missing");
        }
        heroCtrl.SetSelectedItems(userInfo.SelectedItems);
        SetBackground(0);
    }

    void Start()
    {
        if (replayQueryModal != null)
        {
            replayQueryModal.gameObject.SetActive(false);
        }
        if (videoScreen != null)
        {
            videoScreen.SetActive(false);
        }
        conversationCtrl.StartConversation(ShowReplayQueryModalDelay);
    }

    private void OnEnable()
    {
        if (conversationCtrl!= null)
        {
            conversationCtrl.checkPointEvent.AddListener(SetBackgroundTeaParty);
        }
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
        }
    }

    private void PlayGardenVideo()
    { 
        conversationCtrl.gameObject.SetActive(false);
        videoScreen.SetActive(true);
        videoPlayer.Play();
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        videoScreen.SetActive(false);
        conversationCtrl.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (conversationCtrl != null)
        {
            conversationCtrl.checkPointEvent.AddListener(SetBackgroundTeaParty);
        }
    }

    private void ShowReplayQueryModalDelay()
    {
        conversationCtrl.gameObject.SetActive(false);
        if (replayQueryModal != null)
        {
            replayQueryModal.ShowModal(SetBlackBackground);
        }
    }

    private void SetBackgroundTeaParty(int eventIndex)
    {
        if (videoPlayer != null)
        {
            PlayGardenVideo();
        }
        if (eventIndex == 0)
        {
            SetBackground(1);
        }
    }

    private void SetBlackBackground()
    {
        SetBackground(2);
    }

    private void SetBackground(int index)
    { 
        for (int i =0; i< backgrounds.Length; i++)
        {
            backgrounds[i].SetActive(i == index);
        }
    }

}
