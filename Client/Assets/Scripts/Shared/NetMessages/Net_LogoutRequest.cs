[System.Serializable]
public class Net_LogoutRequest : NetMsg
{
    public Net_LogoutRequest()
    {
        OP = (int)NETOP.OnLogoutRequest;
    }
    public int CNNID { get; set; }
}
