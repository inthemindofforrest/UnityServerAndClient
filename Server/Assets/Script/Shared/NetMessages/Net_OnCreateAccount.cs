[System.Serializable]
public class Net_OnCreateAccount : NetMsg
{
    enum CODE
    {
        Success
    }
    public Net_OnCreateAccount()
    {
        OP = (int)NETOP.OnCreateAccount;
    }
    public byte Success { set; get; }
    public string GetCode()
    {
        return ((CODE)Success).ToString();
    }
}
