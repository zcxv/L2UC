using UnityEngine;

public class RequestUserCommand : ClientPacket
{
    public RequestUserCommand(int idCommand) : base((byte)GameInterludeClientPacketType.RequestUserCommand)
    {
        WriteI(idCommand);
        BuildPacket();
    }
}
