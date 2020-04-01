using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class Client : MonoBehaviour
{
    public static Client Instance;
    public string Username;

    public UnityEvent OnConnected;

    private const int MAX_USER = 100;
    private const int PORT = 26000;
    private const int WEB_PORT = 26001;
    private const int BYTE_SIZE = 1024;//Max Byte Size

    private const string SERVER_IP = "127.0.0.1";//"144.202.88.54";


    private byte ReliableChannel;
    private int ConnectionID;
    private int HostID;

    private static bool IsClientStarted;
    private byte error;

    public bool WaitingForResponse;

    #region MonoBehaivor
    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        
        Init();
    }
    private void Update()
    {
        UpdateMessagePump();
    }
    #endregion

    public void Init()
    {
        if (!IsClientStarted)
        {
            NetworkTransport.Init();

            ConnectionConfig cc = new ConnectionConfig();
            ReliableChannel = cc.AddChannel(QosType.Reliable);

            HostTopology Topo = new HostTopology(cc, MAX_USER);

            //Client only code
            HostID = NetworkTransport.AddHost(Topo, 0);

#if UNITY_WEBGL && !UNITY_EDITOR
        //Web Client
        ConnectionID = NetworkTransport.Connect(HostID, SERVER_IP, WEB_PORT, 0, out error);
        Debug.Log("Connecting from WebGL");
#else
            //Standalone Client
            ConnectionID = NetworkTransport.Connect(HostID, SERVER_IP, PORT, 0, out error);
            Debug.Log("Connecting from Standalone");
#endif

            Debug.Log(string.Format("Attempting to connect on {0}...", SERVER_IP));
            IsClientStarted = true;
        }
    }
    public void Shutdown()
    {
        IsClientStarted = false;
        NetworkTransport.Shutdown();
    }

    public void UpdateMessagePump()
    {
        if (!IsClientStarted) return;

        int RecHostID;//Web or Standalone
        int ConnectionID;//Which user is sending the message
        int ChannelID;//Which lane is message coming from

        byte[] RecBuffer = new byte[BYTE_SIZE];
        int DataSize;

        NetworkEventType type = NetworkTransport.Receive(out RecHostID, out ConnectionID, out ChannelID, RecBuffer, BYTE_SIZE, out DataSize, out error);

        switch (type)
        {
            case NetworkEventType.DataEvent:
                BinaryFormatter Formatter = new BinaryFormatter();
                MemoryStream MS = new MemoryStream(RecBuffer);
                NetMsg msg = (NetMsg)Formatter.Deserialize(MS);

                OnData(ConnectionID, ChannelID, RecHostID, msg);
                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log(string.Format("We have connected to the server"));
                OnConnected.Invoke();
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log(string.Format("We have been disconnected"));
                break;
            case NetworkEventType.Nothing:
                //Do nothing, gets called every frame nothing happens
                break;
            case NetworkEventType.BroadcastEvent:
            default:
                Debug.Log("Unexpected network event type");
                break;
        }
    }

    #region OnData
    private void OnData(int _CnnID, int _ChannelID, int _RecHostID, NetMsg _Msg)
    {
        print("MSG back: " + _Msg.OP.ToString());
        WaitingForResponse = false;
        switch ((NETOP)_Msg.OP)
        {
            case NETOP.None:
                Debug.Log("Unexpected NETOP");
                break;
            case NETOP.OnCreateAccount:
                OnCreateAccount((Net_OnCreateAccount)_Msg);
                break;
            case NETOP.OnLoginRequest:
                OnLoginRequest((Net_OnLoginRequest)_Msg);
                break;
            case NETOP.OnGoldRequest:
                OnGoldRequest((Net_RequestGold)_Msg);
                break;
            case NETOP.OnRetrieveCivsRequest:
                OnRetrieveCivsRequest((Net_RetrieveCivs)_Msg);
                break;
        }
    }

    private void OnCreateAccount(Net_OnCreateAccount _OCA)
    {
        LobbyScene.Instance.EnableInputs();
        LobbyScene.Instance.ChangeAuthenticationMessage(_OCA.GetCode());
    }
    private void OnLoginRequest(Net_OnLoginRequest _OLR)
    {
        LobbyScene.Instance.ChangeAuthenticationMessage(_OLR.GetCode());
        if (_OLR.Success == 0)
        {
            LobbyScene.Instance.EnableInputs();
        }
        else
        {
            //Successful Login
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            Username = _OLR.Username;
        }
    }
    private void OnGoldRequest(Net_RequestGold _RG)
    {
        RequestGold.GoldAmount = _RG.RecievingAmount;
        RequestGold.UpdateGoldText();
    }
    private void OnRetrieveCivsRequest(Net_RetrieveCivs _RC)
    {
        Debug.Log(_RC.Civs[0].Focus.ToString());
        AssignCivs.Instance.AssignAndPlaceAllCivs(_RC.Civs);
        
    }
    #endregion

    #region Send
    public void SendServer(NetMsg _msg)
    {
        //this is where we hold our data
        byte[] buffer = new byte[BYTE_SIZE];

        //This is where we would crush data into byte array
        BinaryFormatter Formatter = new BinaryFormatter();
        MemoryStream MS = new MemoryStream(buffer);
        Formatter.Serialize(MS, _msg);

        NetworkTransport.Send(HostID, ConnectionID, ReliableChannel, buffer, BYTE_SIZE, out error);
        WaitingForResponse = true;
    }

    public void SendCreateAccount(string _Username, string _Password, string _Email)
    {
        Net_CreateAccount CA = new Net_CreateAccount();

        if(!Utility.IsUsername(_Username))
        {
            //Invalid Username
            LobbyScene.Instance.ChangeAuthenticationMessage("Username is Invalid");
            LobbyScene.Instance.EnableInputs();
            return;
        }
        if (!Utility.IsEmail(_Email))
        {
            //Invalid Email
            LobbyScene.Instance.ChangeAuthenticationMessage("Email is Invalid");
            LobbyScene.Instance.EnableInputs();
            return;
        }
        if (_Password == null || _Password == "")
        {
            //Invalid Password
            LobbyScene.Instance.ChangeAuthenticationMessage("Password is Empty");
            LobbyScene.Instance.EnableInputs();
            return;
        }

        CA.Username = _Username;
        CA.Password = Utility.Sha256FromString(_Password);
        CA.Email = _Email;
        LobbyScene.Instance.ChangeAuthenticationMessage("Sending Request...");

        SendServer(CA);
    }
    public void SendLoginRequest(string _UsernameOrEmail, string _Password)
    {
        if (!Utility.IsUsernameAndDiscriminator(_UsernameOrEmail) && !Utility.IsEmail(_UsernameOrEmail))
        {
            //Invalid Username
            LobbyScene.Instance.ChangeAuthenticationMessage("Username or email is Invalid");
            LobbyScene.Instance.EnableInputs();
            return;
        }
        if (_Password == null || _Password == "")
        {
            //Invalid Password
            LobbyScene.Instance.ChangeAuthenticationMessage("Password is Empty");
            LobbyScene.Instance.EnableInputs();
            return;
        }
        Net_LoginRequest LR = new Net_LoginRequest();

        LR.UsernameOrEmail = _UsernameOrEmail;
        LR.Password = Utility.Sha256FromString(_Password);

        SendServer(LR);
    }
    public void SendLogoutRequest()
    {
        Net_LogoutRequest NLG = new Net_LogoutRequest();
        SaveData.RemoveSavedData();
        SendServer(NLG);
    }
    #endregion
}
