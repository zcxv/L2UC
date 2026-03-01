namespace IPC.LoginServer.DTO {

    [IngoingPacketDto]
    public record AccountKickedDto(
        LoginFailureReason Reason
    ) : PacketDto;
    
}