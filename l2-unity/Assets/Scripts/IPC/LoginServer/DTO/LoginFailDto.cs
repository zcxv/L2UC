namespace IPC.LoginServer.DTO {
    
    [IngoingPacketDto]
    public record LoginFailDto(
        LoginFailureReason Reason
    ) : PacketDto;
}