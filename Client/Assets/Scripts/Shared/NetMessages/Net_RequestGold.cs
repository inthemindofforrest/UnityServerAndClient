[System.Serializable]
public class Net_RequestGold : NetMsg
{
    public Net_RequestGold()
    {
        OP = (int)NETOP.OnGoldRequest;
    }
    public int RecievingAmount { get; set; }
}
