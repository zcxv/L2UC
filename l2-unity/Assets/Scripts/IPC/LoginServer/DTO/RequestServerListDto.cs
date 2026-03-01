namespace IPC.LoginServer.DTO {

    [OutgoingPacketDto]
    public record RequestServerListDto(
        int UID,
        int SID
    ) : PacketDto;

}