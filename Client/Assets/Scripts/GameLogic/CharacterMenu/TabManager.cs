using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TabDictionary
{
    public TabManager.TABS Tab;
    public GameObject gameObject;
}
[System.Serializable]
public class CustomTabDictionary
{
    public List<TabDictionary> Dictionary = new List<TabDictionary>();

    public GameObject SearchByKey(TabManager.TABS _Search)
    {
        for(int i = 0; i < Dictionary.Count; i++)
        {
            if(Dictionary[i].Tab == _Search)
            {
                return Dictionary[i].gameObject;
            }
        }
        return null;
    }
}

public class TabManager : MonoBehaviour
{
    public enum TABS { Inventory, CivSelection, Settings}

    public TABS CurrentTabOpen;
    [SerializeField]
    CustomTabDictionary TabList = new CustomTabDictionary();

    private void Start()
    {
        SwitchTab((int)CurrentTabOpen);
    }

    public void SwitchTab(int _NewTab)
    {
        GameObject TempObject = TabList.SearchByKey((TABS)_NewTab);
        if (TempObject != null)
        {
            CloseAllTabs();
            TempObject.SetActive(true);
        }
    }
    private void CloseAllTabs()
    {
        for(int i = 0; i < TabList.Dictionary.Count; i++)
        {
            TabList.Dictionary[i].gameObject.SetActive(false);
        }
    }

}
