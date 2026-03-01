namespace IPC.LoginServer.DTO {

    [OutgoingPacketDto]
    public record RequestServerLoginDto(
        int UID,
        int SID,
        int ServerId
    ) : PacketDto;

}