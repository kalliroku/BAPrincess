using UnityEngine;

public class BackgroundCtrl : MonoBehaviour
{
    [SerializeField] Background[] backgrounds;
    [SerializeField] ClothManagerCtrl clothManagerCtrl;

    private void Awake()
    {
        if (backgrounds == null)
        {
            Debug.Log("backgrounds is null");
        }
        if (clothManagerCtrl == null)
        { 
            Debug.Log("clothManagerCtrl is null");
        }
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].Setup(clothManagerCtrl);
        }
    }

}
