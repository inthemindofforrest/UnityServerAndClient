public enum NETOP
{
    None = 0,
    CreateAccount = 1,
    LoginRequest = 2,
    OnCreateAccount = 3,
    OnLoginRequest = 4

}

[System.Serializable]
public abstract class NetMsg
{
    public byte OP { set; get; }
    public NetMsg()
    {
        OP = (int)NETOP.None;
    }
}
