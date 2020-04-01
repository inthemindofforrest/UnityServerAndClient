using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameLogic.SerializedCiv[] TTT = new GameLogic.SerializedCiv[1];
            GameLogic.SerializedCiv VVV = new GameLogic.SerializedCiv();
            VVV.Name = "Bob";
            VVV.CurrentTask = 1;
            VVV.HoursToCompleteTask = 15;

            VVV.Focus = 15;
            VVV.Stamina = 2;
            VVV.Agility = 5;
            VVV.Charisma = 11;
            VVV.Intelegence = 15;
            VVV.Strength = 1;

            TTT[0] = VVV;

            AssignCivs.Instance.AssignAndPlaceAllCivs(TTT);
        }
    }
}
