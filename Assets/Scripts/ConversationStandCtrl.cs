using UnityEngine;
using UnityEngine.UI;

public class ConversationStandCtrl : MonoBehaviour
{
    public int Position;
    [SerializeField] Animator animator;
    [SerializeField] Image[] images;
    [SerializeField] RawImage[] rawImages;
    public bool isRawImage = false;

    public void SetStand(bool show, int position, int imageIndex)
    {
        gameObject.SetActive(Position == position);
        if (!isRawImage)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].gameObject.SetActive(show && i == imageIndex);
            }
        }
        else
        {
            for (int i = 0; i < rawImages.Length; i++)
            {
                rawImages[i].gameObject.SetActive(show && i == imageIndex);
            }
        }
    }
}
