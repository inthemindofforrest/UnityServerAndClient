using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLogin : MonoBehaviour
{
    [SerializeField] string SaveName;
    [SerializeField] Toggle toggle;
    [SerializeField] TMPro.TMP_InputField Username;
    [SerializeField] TMPro.TMP_InputField ShaPassword;

    public void OnConnectionToServer()
    {
        AssignReferences();
        SaveData.User User = SaveData.Load();

        if (User != null)
        {
            Username.text = User.Username;
            ShaPassword.text = User.ShaPassword;
            toggle.isOn = true;
            Client.Instance.SendLoginRequest(User.Username, User.ShaPassword);
            //SignIn
        }
    }

    public void CheckToggleChecked(Toggle _Toggle)
    {
        AssignReferences();
        if(_Toggle.isOn)
        {
            //Encrypt data
            SaveData.Save(Username.text, ShaPassword.text);
        }
        else
        {
            SaveData.RemoveSavedData();
        }
    }

    public void AssignReferences()
    {
        toggle = GameObject.Find("StaySignedIn").GetComponent<Toggle>();
        Username = GameObject.Find("LoginUsernameEmail").GetComponent<TMPro.TMP_InputField>();
        ShaPassword = GameObject.Find("LoginPassword").GetComponent<TMPro.TMP_InputField>();
    }
}
