using UnityEngine;

public class ItemMenuCtrl : MonoBehaviour
{
    [SerializeField] ClothManagerCtrl.ItemType type;
    [SerializeField] ItemCtrl[] items;
    private ClothManagerCtrl clothManager;

    private void OnDestroy()
    {
        if (clothManager != null)
        {
            clothManager.OnItemTypeChangedEvent.RemoveListener(SetActive);
        }
    }

    public void SetMenu(ClothManagerCtrl clothManager)
    {
        this.clothManager = clothManager;
        SetActive(clothManager.SelectedItemType);
        for (int i = 0; i < items.Length; i++)
        {
            items[i].SetItem(i, type, clothManager);
        }
        clothManager.OnItemTypeChangedEvent.AddListener(SetActive);
    }

    private void SetActive(ClothManagerCtrl.ItemType selectedType) 
    {
        gameObject.SetActive(type == selectedType);
    }
}
