public class AnswerTradeRequest : ClientPacket
{
    private int _response;

    public AnswerTradeRequest(int response) 
        : base((byte)GameClientPacketType.AnswerTradeRequest)
    {
        _response = response;
        WriteI(_response);
        BuildPacket();
    }
}

