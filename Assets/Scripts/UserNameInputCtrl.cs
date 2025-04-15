using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserNameInputCtrl : MonoBehaviour
{
    [SerializeField] private TMP_InputField userNameInputField;
    [SerializeField] private Button confirmButton;

    private Action<string> onConfirm;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (confirmButton!= null)
        {
            confirmButton.onClick.AddListener(OnClickConfirmButton);
        }
    }

    private void OnDisable()
    {
        if (confirmButton!= null)
        {
            confirmButton.onClick.RemoveListener(OnClickConfirmButton);
        }
    }

    public void ShowUserNameInput(Action<string> callback)
    {
        gameObject.SetActive(true);
        onConfirm = callback;
    }

    private void OnClickConfirmButton()
    {
        if (userNameInputField == null || userNameInputField.text == string.Empty) return;
        if (onConfirm != null)
        {
            onConfirm(userNameInputField.text);
            userNameInputField.text = string.Empty;
            gameObject.SetActive(false);
        }
    }

}
