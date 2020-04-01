using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenRing : MonoBehaviour
{
    GameObject RingControllerObject;

    public void Click()
    {
        if(RingControllerObject == null)
        {
            RingControllerObject = RingController.Instance;
        }
        if(RingControllerObject)
        {
            RingControllerObject.SetActive(!RingControllerObject.activeInHierarchy);
        }
    }
}
