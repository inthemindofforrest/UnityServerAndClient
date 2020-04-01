public enum NETOP
{
    None = 0,
    CreateAccount = 1,
    LoginRequest = 2,
    OnCreateAccount = 3,
    OnLoginRequest = 4,
    OnLogoutRequest = 5,
    OnGoldRequest = 6,
    OnRetrieveCivsRequest = 7,
    OnCivCheck = 8,
    OnCivTaskRequest = 9

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
