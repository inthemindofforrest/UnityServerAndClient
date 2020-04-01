[System.Serializable]
public class Net_RetrieveCivs : NetMsg
{
    public Net_RetrieveCivs()
    {
        OP = (int)NETOP.OnRetrieveCivsRequest;
    }
    public GameLogic.SerializedCiv[] Civs;
}