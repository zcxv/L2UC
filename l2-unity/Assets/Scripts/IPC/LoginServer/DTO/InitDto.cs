namespace IPC.LoginServer.DTO {

    [IngoingPacketDto]
    public record InitDto(
        int SessionId,
        int ProtocolVersion,
        byte[] ScrambledPublicKey,
        GameGuardKey GameGuardKey,
        byte[] BlowfishKey
    ) : PacketDto;

}