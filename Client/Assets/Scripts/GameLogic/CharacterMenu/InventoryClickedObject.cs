using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryClickedObject : MonoBehaviour
{
    public RectTransform ScrollView;
    public GameObject Description;
    public TMP_Text ItemName;
    public TMP_Text ItemDescription;


    public void ItemClicked()
    {
        InventoryItem SelectedItem = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<InventoryItem>();

        if (!(ItemName.text == SelectedItem.Name) || !Description.activeInHierarchy)
        {
            Description.SetActive(true);
            ScrollView.offsetMin = new Vector2(ScrollView.offsetMin.x, 148);

            ItemName.text = SelectedItem.Name;
            ItemDescription.text = SelectedItem.Description;
        }
        else
        {
            Description.SetActive(false);
            ScrollView.offsetMin = new Vector2(ScrollView.offsetMin.x, 0);
        }
    }
}
