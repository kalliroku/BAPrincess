using UnityEngine;

public class HeroCtrl : MonoBehaviour
{
    [SerializeField] private GameObject[] hairs;
    [SerializeField] private GameObject[] dressess;
    [SerializeField] private GameObject[] shoses;
    [SerializeField] private GameObject defaultDress;

    [SerializeField] private ClothManagerCtrl clothManager;

    private void OnEnable()
    {
        if (clothManager != null)
        {
            clothManager.OnSelectedItemChangedEvent.AddListener(OnSelectedItemChanged);
        }
    }

    private void OnDisable()
    {
        if (clothManager != null)
        {
            clothManager.OnSelectedItemChangedEvent.RemoveListener(OnSelectedItemChanged);
        }
    }

    private void OnSelectedItemChanged(ClothManagerCtrl.ItemType type, int index)
    {
        GameObject[] items = type == ClothManagerCtrl.ItemType.HAIR ? hairs : type == ClothManagerCtrl.ItemType.DRESS ? dressess : shoses;
        if (type == ClothManagerCtrl.ItemType.DRESS && defaultDress != null)
        {
            defaultDress.SetActive(false);
        }
        for (int i = 0; i < items.Length; i++)
        {
            items[i].SetActive(i == index);
        }
    }

    public void SetSelectedItems(Items items)
    {
        int hairIndex = Mathf.Max(items.hairIndex, 0); 
        OnSelectedItemChanged(ClothManagerCtrl.ItemType.HAIR, hairIndex);
        OnSelectedItemChanged(ClothManagerCtrl.ItemType.DRESS, items.dressIndex);
        OnSelectedItemChanged(ClothManagerCtrl.ItemType.SHOES, items.shoesIndex);
    }
}
