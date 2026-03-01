namespace IPC.LoginServer.DTO {

    [IngoingPacketDto]
    public record BlockedAccountWithMsgDto(
        BlockedAccountMessage[] Messages
    ) : PacketDto;

    public record BlockedAccountMessage(
        int Message,
        string Token
    );

}