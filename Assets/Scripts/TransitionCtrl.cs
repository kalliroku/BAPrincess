using UnityEngine;

public class TransitionCtrl : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] shops;

    public void OnShopTransition(ClothManagerCtrl.ItemType type)
    {
        for (int i = 0; i < shops.Length; i++)
        {
            shops[i].SetActive(i == (int)type);
        }
        if (animator != null)
        {
            animator.SetTrigger("temp");
        }
    }
}
