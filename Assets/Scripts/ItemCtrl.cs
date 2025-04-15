using UnityEngine;
using UnityEngine.UI;

public class ItemCtrl : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    private int index;
    private ClothManagerCtrl clothManager;
    private ClothManagerCtrl.ItemType itemType;

    private void OnEnable()
    {
        if (button != null)
            button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    private void OnDestroy()
    {
        if (clothManager != null)
        {
            clothManager.OnSelectedItemChangedEvent.RemoveListener(UpdateUI);
        }
    }

    public void SetItem(int index, ClothManagerCtrl.ItemType itemType, ClothManagerCtrl clothManager)
    {
        this.index = index;
        this.itemType = itemType;
        if (clothManager != null)
        {
            this.clothManager = clothManager;
            UpdateUI(clothManager.SelectedItemType, clothManager.GetSeletedItemIndex(clothManager.SelectedItemType));
            clothManager.OnSelectedItemChangedEvent.AddListener(UpdateUI);
        } else
        {
            throw new System.Exception("cloth manager is requried to item ctrl");
        }
    }

    private void OnClick()
    {
        if (clothManager != null)
        {
            clothManager.SetSelectedItem(itemType, index);
        }
    }

    private Color NormalColor => new Color(0.9607844f, 0.7176471f, 0.7372549f);
    private Color SelectedColor => new Color(0.8584906f, 0.1835765f, 0.238005f);

    private void UpdateUI(ClothManagerCtrl.ItemType itemType, int index)
    {
        if (this.itemType != itemType) return;
        if (image != null)
        {
            image.color = this.index == index ? SelectedColor : NormalColor;
        }
    }
}
