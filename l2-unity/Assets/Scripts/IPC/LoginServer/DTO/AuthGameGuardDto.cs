namespace IPC.LoginServer.DTO {
    
    [OutgoingPacketDto]
    public record AuthGameGuardDto(
        int SessionId,
        GameGuardKey GameGuardKey
    ) : PacketDto;
}