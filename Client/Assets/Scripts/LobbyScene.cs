using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    Client MainClient;

    [SerializeField] CanvasGroup CG;
    [SerializeField] TMPro.TextMeshProUGUI WelcomeMessage;
    [SerializeField] TMPro.TextMeshProUGUI AuthenticationMessage;
    public static LobbyScene Instance { set; get; }

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        MainClient = Client.Instance;
    }

    public void OnClickCreateAccount()
    {
        try
        {
            DisableInputs();

            string Username = GameObject.Find("CreateUsername").GetComponent<TMPro.TMP_InputField>().text;
            string Password = GameObject.Find("CreatePassword").GetComponent<TMPro.TMP_InputField>().text;
            string Email = GameObject.Find("CreateEmail").GetComponent<TMPro.TMP_InputField>().text;

            Client.Instance.SendCreateAccount(Username, Password, Email);
        }
        catch
        {
            Debug.LogError("Cannot locate Fields");
        }

        
    }
    public void OnClickLoginRequest()
    {
        try
        {
            DisableInputs();

            string UsernameOrEmail = GameObject.Find("LoginUsernameEmail").GetComponent<TMPro.TMP_InputField>().text;
            string Password = GameObject.Find("LoginPassword").GetComponent<TMPro.TMP_InputField>().text;

            Client.Instance.SendLoginRequest(UsernameOrEmail, Password);
        }
        catch
        {
            Debug.LogError("Cannot locate Fields");
        }
    }

    public void ChangeWelcomeMessage(string _Msg)
    {
        try
        {
            WelcomeMessage.text = _Msg;
        }
        catch
        {
            Debug.LogError("Cannot locate Fields");
        }
    }
    public void ChangeAuthenticationMessage(string _Msg)
    {
        try
        {
            AuthenticationMessage.text = _Msg;
        }
        catch
        {
            Debug.LogError("Cannot locate Fields");
        }
    }

    public void EnableInputs()
    {
        try
        {
            CG.enabled = true;
        }
        catch
        {
            Debug.LogError("Cannot locate Fields");
        }
    }
    public void DisableInputs()
    {
        try
        {
            CG.enabled = false;
        }
        catch
        {
            Debug.LogError("Cannot locate Fields");
        }
    }

    public void Logout()
    {
        MainClient.SendLogoutRequest();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
