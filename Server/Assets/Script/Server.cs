using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    private const int MAX_USER = 100;
    private const int PORT = 26000;
    private const int WEB_PORT = 26001;
    private const int BYTE_SIZE = 1024;//Max Byte Size


    private byte ReliableChannel;
    private int HostID;
    private int WebHostID;

    private bool IsServerStarted;
    private byte error;

    private Mongo DB;

    #region MonoBehaivor
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        UpdateMessagePump();

        if(Input.GetKeyDown(KeyCode.P))
        {
            DB.InsertCiv("FakeEmailmadeForMe@Me.com");
        }
    }
    #endregion

    public void Init()
    {
        DB = new Mongo();
        DB.Init();

        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        ReliableChannel = cc.AddChannel(QosType.Reliable);

        HostTopology Topo = new HostTopology(cc, MAX_USER);

        //Server only code
        HostID = NetworkTransport.AddHost(Topo, PORT, null);
        WebHostID = NetworkTransport.AddWebsocketHost(Topo, WEB_PORT, null);

        Console.DeveloperConsole.sLog(string.Format("Opening connection on port {0} and webport {1}", PORT, WEB_PORT));
        IsServerStarted = true;
    }
    public void Shutdown()
    {
        IsServerStarted = false;
        NetworkTransport.Shutdown();
    }

    public void UpdateMessagePump()
    {
        if (!IsServerStarted) return;

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
                Console.DeveloperConsole.sLog(string.Format("User {0} has connected through host {1}", ConnectionID, RecHostID));
                break;
            case NetworkEventType.DisconnectEvent:
                DisconnectEvent(RecHostID, ConnectionID);
                Console.DeveloperConsole.sLog(string.Format("User {0} has disconnected", ConnectionID));
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

    private GameLogic.Civ CreateNewCiv()
    {
        GameLogic.Civ NewCiv = new GameLogic.Civ();

        NewCiv.AssignAtts();

        return NewCiv;
    }

    private GameLogic.Civ CreateStarterCiv()
    {
        GameLogic.Civ NewCiv = new GameLogic.Civ();

        NewCiv.AssignStarterAtts();

        return NewCiv;
    }

    #region OnData
    private void OnData(int _CnnID, int _ChannelID, int _RecHostID, NetMsg _Msg)
    {
        switch ((NETOP)_Msg.OP)
        {
            case NETOP.None:
                Debug.Log("Unexpected NETOP");
                break;
            case NETOP.CreateAccount:
                CreateAccount(_CnnID, _ChannelID, _RecHostID, (Net_CreateAccount)_Msg);
                break;
            case NETOP.LoginRequest:
                LoginRequest(_CnnID, _ChannelID, _RecHostID, (Net_LoginRequest)_Msg);
                break;
            case NETOP.OnLogoutRequest:
                DisconnectEvent(_RecHostID, _CnnID);
                break;
            case NETOP.OnGoldRequest:
                OnGoldRequest(_CnnID, _ChannelID, _RecHostID, (Net_RequestGold)_Msg);
                break;
            case NETOP.OnRetrieveCivsRequest:
                OnRetrieveCivs(_CnnID, _ChannelID, _RecHostID, (Net_RetrieveCivs)_Msg);
                break;

            default:
                break;
        }
    }

    private void DisconnectEvent(int _recHostID, int _cnnID)
    {
        Console.DeveloperConsole.sLog(string.Format("User {0} has Logged out", _cnnID));

        Model_Account account = DB.FindAccountByConnectionID(_cnnID);
        if(account == null)
        {
            return;
        }

        DB.UpdateAccountAfterDisconnection(account.Email);
    }

    private void CreateAccount(int _CnnID, int _ChannelID, int _RecHostID, Net_CreateAccount _CA)
    {
        Net_OnCreateAccount OCA = new Net_OnCreateAccount();

        if(DB.InsertAccount(_CA.Username, _CA.Password, _CA.Email) && DB.InsertStats(_CA.Email) && DB.InsertCiv(_CA.Email))
        {
            OCA.Success = 1;
        }
        else
        {
            OCA.Success = 0;
        }

        OCA.Success = 0;

        SendClient(_RecHostID, _CnnID, OCA);
    }
    private void LoginRequest(int _CnnID, int _ChannelID, int _RecHostID, Net_LoginRequest _LR)
    {
        string randomToken = Utility.GenerateRandom(64);
        Model_Account account = DB.LoginAccount(_LR.UsernameOrEmail, _LR.Password, _CnnID, randomToken);
        Net_OnLoginRequest OLR = new Net_OnLoginRequest();

        if(account != null)
        {
            OLR.Success = 1;
            OLR.Username = account.Username;
            OLR.Discriminator = account.Discriminator;
            OLR.Token = randomToken;
            OLR.ConnectionID = _CnnID;
        }
        else
        {
            OLR.Success = 0;
        }
        

        SendClient(_RecHostID, _CnnID, OLR);
    }

    private void OnGoldRequest(int _CnnID, int _ChannelID, int _RecHostID, Net_RequestGold _RG)
    {
        Net_RequestGold NewRG = new Net_RequestGold();
        NewRG.RecievingAmount = DB.UpdateStats(DB.FindAccountEmailByConnectionID(_CnnID)).Gold;
        SendClient(_RecHostID, _CnnID, NewRG);
    }

    private void OnRetrieveCivs(int _CnnID, int _ChannelID, int _RecHostID, Net_RetrieveCivs _RC)
    {
        Net_RetrieveCivs NewRG = new Net_RetrieveCivs();
        GameLogic.SerializedCiv[] T = DB.FindCivStatsByConnectionID(_CnnID).Civs;
        NewRG.Civs = T;
        Console.DeveloperConsole.sLog("Num: " + T[0].Focus.ToString());
        SendClient(_RecHostID, _CnnID, NewRG);
    }
    #endregion

    #region Send
    public void SendClient(int _RecHost, int _CnnID, NetMsg _msg)
    {
        //this is where we hold our data
        byte[] buffer = new byte[BYTE_SIZE];

        //This is where we would crush data into byte array
        BinaryFormatter Formatter = new BinaryFormatter();
        MemoryStream MS = new MemoryStream(buffer);
        Formatter.Serialize(MS, _msg);

        if(_RecHost == 0)
            NetworkTransport.Send(HostID, _CnnID, ReliableChannel, buffer, BYTE_SIZE, out error);
        else
            NetworkTransport.Send(WebHostID, _CnnID, ReliableChannel, buffer, BYTE_SIZE, out error);
    }
    #endregion
}
