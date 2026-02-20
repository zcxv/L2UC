public class RequestAnswerJoinParty : ClientPacket
{
    private int _response;
    public RequestAnswerJoinParty(int response) : base((byte)GameClientPacketType.RequestAnswerJoinParty)
    {
        _response = response;
        WriteI(_response);
        BuildPacket();
    }
}

