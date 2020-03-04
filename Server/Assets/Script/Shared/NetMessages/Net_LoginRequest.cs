[System.Serializable]
public class Net_LoginRequest : NetMsg
{
    public Net_LoginRequest()
    {
        OP = (int)NETOP.LoginRequest;
    }
    public string UsernameOrEmail { set; get; }
    public string Password { set; get; }
}
