using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenuButtons : MonoBehaviour
{
    GameObject CharacterMenu;


    private void Start()
    {
        CharacterMenu = GameObject.Find("CharacterMenu");
        CharacterMenu.SetActive(false);
    }

    public void OpenCharacterMenu()
    {
        CharacterMenu.SetActive(true);
        if(Client.Instance)Client.Instance.SendServer(new Net_RetrieveCivs());
        if(Client.Instance)
            GameObject.Find("UserNameText").GetComponent<TMPro.TMP_Text>().text = Client.Instance.Username;
    }
    public void CloseCharacterMenu()
    {
        CharacterMenu.SetActive(false);
    }
    public void OpenInventoryFromMainGame(TabManager _SetToInventory)
    {
        OpenCharacterMenu();
        _SetToInventory.SwitchTab((int)TabManager.TABS.Inventory);
    }
}
