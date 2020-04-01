using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    public string Name;
    public string Description;

    public bool UsableItem;

    public int Amount;

    public TMP_Text MyName;
    public TMP_Text MyAmount;

    private void Start()
    { 
        MyName.text = Name;
        MyAmount.text = Amount.ToString();
    }
}
