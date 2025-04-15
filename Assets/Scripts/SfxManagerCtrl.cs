using UnityEngine;

public class SfxManagerCtrl : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] sfxs;

    private static SfxManagerCtrl instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static SfxManagerCtrl Instance => instance;

    public void PlaySfx(int index)
    {
        if (audioSource == null || sfxs.Length == 0) return;
        audioSource.clip = sfxs[index % sfxs.Length];
        audioSource.volume = 1;
        audioSource.Play();
    }
}
