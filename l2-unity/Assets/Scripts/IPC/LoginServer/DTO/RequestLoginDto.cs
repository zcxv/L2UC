namespace IPC.LoginServer.DTO {

    [OutgoingPacketDto]
    public record RequestLoginDto(
        byte[] EncryptedData,
        int SessionId
    ) : PacketDto;

}