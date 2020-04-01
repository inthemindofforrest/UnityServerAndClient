using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AssignCivDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text CurrentTask;
    [SerializeField] TMP_Text HoursToComplete;

    [SerializeField] TMP_Text Focus;
    [SerializeField] TMP_Text Strength;
    [SerializeField] TMP_Text Stamina;
    [SerializeField] TMP_Text Agility;
    [SerializeField] TMP_Text Charisma;
    [SerializeField] TMP_Text Intellegence;

    public void AssignDisplays(GameLogic.SerializedCiv _Civ)
    {
        Name.text = _Civ.Name;
        CurrentTask.text = _Civ.CurrentTask.ToString();
        HoursToComplete.text = _Civ.HoursToCompleteTask.ToString();

        Focus.text = "FC" + ": " + _Civ.Focus.ToString();
        Strength.text = "ST" + ": " + _Civ.Strength.ToString();
        Stamina.text = "SM" + ": " + _Civ.Stamina.ToString();
        Agility.text = "AG" + ": " + _Civ.Agility.ToString();
        Charisma.text = "CH" + ": " + _Civ.Charisma.ToString();
        Intellegence.text = "IN" + ": " + _Civ.Intelegence.ToString();
    }
}
