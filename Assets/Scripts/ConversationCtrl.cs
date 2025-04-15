using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ConversationCtrl : MonoBehaviour
{
    [SerializeField] private UserInfo userInfo;
    [SerializeField] private TMP_Text speachText;
    [SerializeField] private TMP_Text[] textNeedReplaceNames;
    [SerializeField] private GameObject speachBox;
    [SerializeField] private GameObject[] backgrounds;
    [SerializeField] private ConversationStandCtrl[] conversationStandCtrls;
    [SerializeField] private Button progressButton;
    [SerializeField] private Dialogue[] dialogues;

    public UnityEvent<int> checkPointEvent = new();
    private Action callback;
    private int progress = 0;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (progressButton!= null)
        {
            progressButton.onClick.AddListener(OnClickProgress);
        }
    }

    private void OnDisable()
    {
        if (progressButton != null)
        {
            progressButton.onClick.RemoveListener(OnClickProgress);
        }
    }

    public void StartConversation(Action callback) 
    { 
        this.callback = callback;
        ShowNextDialogue(progress);
        gameObject.SetActive(true);
    }

    private void OnClickProgress() 
    {
        progress++;
        ShowNextDialogue(progress);
    }

    private string GetConvertedText(string origin)
    {
        if (userInfo == null)
        {
            userInfo = UserInfo.GetInstance();
        }
        if (userInfo == null)
        {
            Debug.LogError("UserInfo is missing");
            return "";
        }
        var convertedText = origin;
        if (origin.Contains("{{name}}"))
        {
            convertedText = origin.Replace("{{name}}", userInfo.UserName);
        }
        return convertedText;
    }

    private void ShowNextDialogue(int current)
    {
        if (current < dialogues.Length)
        {
            Dialogue nextDialogue = dialogues[current];

            for (int i =0; i < conversationStandCtrls.Length; i++)
            {
                conversationStandCtrls[i].SetStand(nextDialogue.showStandImage, nextDialogue.standPosition, nextDialogue.imageIndex);
            }
            for (int j = 0; j < textNeedReplaceNames.Length; j++)
            {
                textNeedReplaceNames[j].text = GetConvertedText(textNeedReplaceNames[j].text);
            }
            for (int k =0; k < backgrounds.Length; k++)
            {
                backgrounds[k].SetActive(nextDialogue.showBackground && k == nextDialogue.backgroundIndx);
            }
            if (nextDialogue.triggerEvent)
            {
                checkPointEvent.Invoke(nextDialogue.eventIndex);
            }
            speachText.text = GetConvertedText(nextDialogue.text);
        }
        else
        {
            OnConversationEnd();
        }
    }

    private void OnConversationEnd()
    {
        if (this.callback != null)
        {
            callback.Invoke();
            this.callback = null;
        }
    }
}

[Serializable]
public struct Dialogue
{
    public string name;
    public string text;
    public bool showStandImage;
    public int standPosition;
    public int imageIndex;
    public bool showBackground;
    public int backgroundIndx;
    public bool triggerEvent;
    public int eventIndex;
}