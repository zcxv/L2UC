namespace IPC.LoginServer.DTO {

    [IngoingPacketDto]
    public record LoginOkDto(
        int UID,
        int SID,
        BillingMode BillingMode,
        int PaidTime,
        int Warnings // bitmask?
    ) : PacketDto;

    public enum BillingMode {
        Free = 0x03,
        Time = 0x04,
        Prepaid = 0x05,
        Subscription = 0x06
    }

}