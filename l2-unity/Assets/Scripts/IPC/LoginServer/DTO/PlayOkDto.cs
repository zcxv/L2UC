namespace IPC.LoginServer.DTO {

    [IngoingPacketDto]
    public record PlayOkDto(
        int SID,
        int UID
    ) : PacketDto;

}