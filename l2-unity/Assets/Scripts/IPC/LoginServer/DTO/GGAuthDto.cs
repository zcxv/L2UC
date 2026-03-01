namespace IPC.LoginServer.DTO {

    [IngoingPacketDto]
    public record GGAuthDto(
        int SessionId,
        GameGuardKey GameGuardKey
    ) : PacketDto;

}