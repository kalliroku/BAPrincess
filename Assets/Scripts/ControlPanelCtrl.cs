using UnityEngine;
using UnityEngine.UI;

public class ControlPanelCtrl : MonoBehaviour
{
    [SerializeField] private ClothManagerCtrl clothManager;
    [SerializeField] private TransitionCtrl transitionCtrl; 
    [SerializeField] private ItemMenuCtrl[] itemMenus;
    [SerializeField] private SelectItemTypeButton[] selectTypeButtons;
    [SerializeField] private Button confirmSelectItemButton; 

    private void Awake()
    {
        for (int i = 0; i < itemMenus.Length; i++)
        {
            itemMenus[i].SetMenu(clothManager);
        }
        for (int i=0; i < selectTypeButtons.Length; i++)
        {
            selectTypeButtons[i].SetButton(clothManager, transitionCtrl);
        }
    }

    private void OnEnable()
    {
        if (confirmSelectItemButton != null)
        {
            confirmSelectItemButton.onClick.AddListener(OnItemSelectionConfirm);
        }
    }

    private void OnDisable()
    {
        if (confirmSelectItemButton != null)
        {
            confirmSelectItemButton.onClick.RemoveListener(OnItemSelectionConfirm);
        }
    }

    private void OnItemSelectionConfirm()
    {
        Debug.Log("confirm button clicked");
        if (clothManager != null)
        {
            clothManager.ConfirmItemSelection();
        }
    }

    private void OnItemSelectionCanceled()
    {
        if (clothManager != null)
        {
            clothManager.ConfirmItemSelection();
        }
    }
}
