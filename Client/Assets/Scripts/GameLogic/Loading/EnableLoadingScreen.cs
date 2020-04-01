using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableLoadingScreen : MonoBehaviour
{
    [SerializeField] GameObject LoadingScreen;
    private void Update()
    {
        if(Client.Instance)
            LoadingScreen.SetActive(Client.Instance.WaitingForResponse);
    }
}
