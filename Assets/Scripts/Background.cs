using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] GameObject[] Components;
    [SerializeField] ClothManagerCtrl.ItemType type;
    private ClothManagerCtrl clothManager;

    private void OnDestroy()
    {
        if (clothManager != null)
        {
            clothManager.OnItemTypeChangedEvent.RemoveListener(SetBackground);
            clothManager.OnSelectedItemChangedEvent.RemoveListener(UpdateComponents);
        }
    }

    public void Setup(ClothManagerCtrl clothManager)
    {
        this.clothManager = clothManager;
        if (clothManager != null)
        {
            SetBackground(this.clothManager.SelectedItemType);
            UpdateComponents(type, clothManager.GetSeletedItemIndex(type));
            clothManager.OnItemTypeChangedEvent.AddListener(SetBackground);
            clothManager.OnSelectedItemChangedEvent.AddListener(UpdateComponents);
        }
    }

    private void SetBackground(ClothManagerCtrl.ItemType type)
    {
        gameObject.SetActive(this.type == type);
    }

    private void UpdateComponents(ClothManagerCtrl.ItemType type, int index)
    {
        return;
        if (clothManager == null) return;
        for (int i = 0; i < Components.Length; i++)
        {
            Components[i].SetActive(i != index);
        }
    }
}
