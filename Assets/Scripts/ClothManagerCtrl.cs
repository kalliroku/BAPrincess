using UnityEngine;
using UnityEngine.Events;

public class ClothManagerCtrl : MonoBehaviour
{
    [System.Serializable] public enum ItemType { HAIR, DRESS, SHOES };
    private int selectedHairIndex = -1;
    private int selectedDressIndex = -1;
    private int selectedShoseIndex = -1;

    private ItemType selectedItemType = ItemType.HAIR;
    public ItemType SelectedItemType
    {
        get => selectedItemType;
        set
        {
            if (selectedItemType != value)
            {
                selectedItemType = value;
                OnItemTypeChangedEvent.Invoke(value);
            }
        }
    }

    [HideInInspector] public UnityEvent<ItemType> OnItemTypeChangedEvent = new();
    [HideInInspector] public UnityEvent<ItemType, int> OnSelectedItemChangedEvent = new();

    public void ConfirmItemSelection()
    {
        if (selectedDressIndex == -1 || selectedShoseIndex == -1) return;
        Items selctedItems = new()
        {
            dressIndex = selectedDressIndex,
            shoesIndex = selectedShoseIndex,
            hairIndex = selectedHairIndex
        };
        UserInfo userInfo = UserInfo.GetInstance();
        Debug.Log(userInfo);
        if (userInfo != null)
        {
            userInfo.SetSelectedItems(selctedItems);
        }
        else
        {
            Debug.LogWarning("user Info is missing");
        }
    }

    public void SetSelectedItem(ItemType type, int index)
    {
        int oldValue = GetSeletedItemIndex(type);
        if (oldValue == index) return;

        switch (type)
        {
            case ItemType.HAIR: selectedHairIndex = index; break;
            case ItemType.DRESS: selectedDressIndex = index; break;
            case ItemType.SHOES: selectedShoseIndex = index; break;
            default:
                break;
        }
        OnSelectedItemChangedEvent.Invoke(type, index);
    }

    public int GetSeletedItemIndex(ItemType type)
    {
        return type switch
        {
            ItemType.HAIR => selectedHairIndex == -1 ? 0 : selectedHairIndex,
            ItemType.DRESS => selectedDressIndex,
            ItemType.SHOES => selectedShoseIndex,
            _ => -1,
        };
    }
}
