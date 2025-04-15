using UnityEngine;
using UnityEngine.UI;

public class SfxPlay : MonoBehaviour
{
    public int sfxIndex = 0;
    private Button button;
    private SfxManagerCtrl ctrl;

    private void Awake()
    {
        button = GetComponent<Button>();
        ctrl = SfxManagerCtrl.Instance;
    }

    private void OnEnable()
    {
        if (button!= null)
        {
            button.onClick.AddListener(OnClick);
        }    
    }

    private void OnDisable()
    {
        if (button!= null)
        {
            button.onClick.RemoveListener(OnClick);
        }
    }

    private void OnClick()
    {
        if (ctrl != null)
        { 
            ctrl.PlaySfx(sfxIndex);
        }
    }
}
