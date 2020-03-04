[System.Serializable]
public class Net_OnLoginRequest : NetMsg
{
    enum CODE
    {
        Success
    }

    public Net_OnLoginRequest()
    {
        OP = (int)NETOP.OnLoginRequest;
    }
    public byte Success { set; get; }

    public int ConnectionID { set; get; }
    public string Username { set; get; }
    public string Discriminator { set; get; }
    public string Token { set; get; }

    public string GetCode()
    {
        return ((CODE)Success).ToString();
    }
}
