using UnityEngine;

public class RequestUserCommand : ClientPacket
{
    public RequestUserCommand(int idCommand) : base((byte)GameClientPacketType.BypassUserCmd)
    {
        WriteI(idCommand);
        BuildPacket();
    }
}
