using UnityEngine;
using UnityEngine.Events;

public class UserInfo : MonoBehaviour
{
    static private UserInfo instance;
    public Items SelectedItems { get; private set; }
    [HideInInspector] public UnityEvent OnItemsSelected = new();

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    static public UserInfo GetInstance()
    { 
        if (instance == null)
        {
            instance = FindObjectOfType<UserInfo>();
        }
        return instance;
    }

    public void SetSelectedItems(Items items)
    {
        SelectedItems = items;
        OnItemsSelected.Invoke();
    }

    public string UserName { get; private set; }
    public void SetName(string name)
    {
        UserName = name;
    }

    public void ClearUserInfo()
    {
        UserName = "";
        SelectedItems = new Items();
    }
}

public struct Items
{
    public int hairIndex;
    public int shoesIndex;
    public int dressIndex;
}
