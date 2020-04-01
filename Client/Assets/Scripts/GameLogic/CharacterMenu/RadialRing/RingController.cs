using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour
{
    public RingMenuMB MainMenuPrefab;
    public static GameObject Instance;
    protected RingMenuMB RingMenuInstance;

    private void Start()
    {
        if(Instance != gameObject)
        {
            Instance = gameObject;
        }

        RingMenuInstance = Instantiate(MainMenuPrefab, transform);
        RingMenuInstance.callback = MenuClick;
        //gameObject.SetActive(false);
    }

    private void MenuClick(string _Path)
    {
        //Make Dictionary with Vector2(Task,SubTask) and string of task
        //Dictionary(Vector2(Task,SubTask), String)
        //Example Vector2(0,0) -> "Building->BuildFarm"
        string[] SubStrings = _Path.Split('/');
        for(int i = 0; i < SubStrings.Length; i++)
        {
            Debug.Log(SubStrings[i]);
        }
        RingController.Instance.SetActive(false);
    }
}
