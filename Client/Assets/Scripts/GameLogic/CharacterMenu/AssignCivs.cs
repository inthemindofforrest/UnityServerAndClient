using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCivs : MonoBehaviour
{
    public static AssignCivs Instance;
    GameLogic.SerializedCiv[] AllCivs;

    GameObject DisplayObject;
    public GameObject ParentForDisplay;

    private void Start()
    {
        if(Instance != this)
        {
            Instance = this;
        }
        //Destroy this gameobject on load :)

        DisplayObject = Resources.Load<GameObject>("Civ");
    }

    public void AssignAndPlaceAllCivs(GameLogic.SerializedCiv[] _Civs)
    {
        AllCivs = _Civs;
        DisplayAllCivs();
    }

    void DisplayAllCivs()
    {
        for(int i = 0; i < AllCivs.Length; i++)
        {
            if (GameObject.Instantiate(DisplayObject, ParentForDisplay.transform).TryGetComponent(out AssignCivDisplay _CivDisplay))
            {
                _CivDisplay.AssignDisplays(AllCivs[i]);
            }
        }
    }
}
