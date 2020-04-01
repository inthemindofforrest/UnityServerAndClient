using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestGold : MonoBehaviour
{
    public static int GoldAmount = 0;

    public static TMPro.TMP_Text StaticText;
    public TMPro.TMP_Text GoldText;

    private void Start()
    {
        if(StaticText == null)
        {
            StaticText = GoldText;
        }
    }


    public static void UpdateGoldText()
    {
        if(StaticText != null)
            StaticText.text = "Gold: " + GoldAmount.ToString();
    }

    public void GoldButtonPressed()
    {
        Net_RequestGold newRequest = new Net_RequestGold();
        Client.Instance.SendServer(newRequest);
    }
}
