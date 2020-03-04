﻿using System.IO;
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

        Debug.Log(string.Format("Opening connection on port {0} and webport {1}", PORT, WEB_PORT));
        IsServerStarted = true;

        //Test
        DB.InsertAccount("Bambooze", "Magnificent", "Email@Ema.com");
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
                Debug.Log(string.Format("User {0} has connected through host {1}", ConnectionID, RecHostID));
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log(string.Format("User {0} has disconnected", ConnectionID));
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

            default:
                break;
        }
    }

    private void CreateAccount(int _CnnID, int _ChannelID, int _RecHostID, Net_CreateAccount _CA)
    {
        Debug.Log(string.Format("{0},{1},{2}", _CA.Username, _CA.Password, _CA.Email));

        Net_OnCreateAccount OCA = new Net_OnCreateAccount();

        OCA.Success = 0;

        SendClient(_RecHostID, _CnnID, OCA);
    }
    private void LoginRequest(int _CnnID, int _ChannelID, int _RecHostID, Net_LoginRequest _LR)
    {
        Debug.Log(string.Format("{0},{1}", _LR.UsernameOrEmail, _LR.Password));
        Net_OnLoginRequest OLR = new Net_OnLoginRequest();

        OLR.Success = 0;
        OLR.Username = "User";
        OLR.Discriminator = "0000";
        OLR.Token = "TOKEN";

        SendClient(_RecHostID, _CnnID, OLR);
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
