namespace IPC.LoginServer.DTO {

    [IngoingPacketDto]
    public record PlayFailDto(
        LoginFailureReason Reason
    ) : PacketDto;
    
}