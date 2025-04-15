using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectItemTypeButton : MonoBehaviour
{
    [SerializeField] private ClothManagerCtrl.ItemType type;
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    
    private ClothManagerCtrl clothCtrl;

    private void OnDestroy()
    {
        if (clothCtrl != null)
        { 
            clothCtrl.OnItemTypeChangedEvent.RemoveListener(UpdateUI);
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
            }
        }
    }

    public void SetButton(ClothManagerCtrl clothCtrl, TransitionCtrl transitionCtrl)
    {
        if (clothCtrl == null || transitionCtrl == null) return;
        this.clothCtrl = clothCtrl;
        UpdateUI(clothCtrl.SelectedItemType);
        clothCtrl.OnItemTypeChangedEvent.AddListener(UpdateUI);
        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                StartCoroutine(SelectItemTypeDelay(1f));
                transitionCtrl.OnShopTransition(type);
            });
        }
    }

    private System.Collections.IEnumerator SelectItemTypeDelay(float duration = 1f)
    {
        yield return new WaitForSeconds(duration);
        clothCtrl.SelectedItemType = type;
    }

    private void UpdateUI(ClothManagerCtrl.ItemType type)
    {
        image.color = type == this.type ? new Color(0.7215686f, 0.3529412f, 0.3607843f) : new Color(0.9607844f, 0.6627451f, 0.682353f);
    }
}
