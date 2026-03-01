namespace IPC.LoginServer.DTO {
    
    [IngoingPacketDto]
    public record BlockedAccountDto(
        BlockedAccountReason Reason
    ) : PacketDto;
    
    public enum BlockedAccountReason {
        DataStealer = 0x01,
        GenericViolation = 0x08,
        SevenDaysSuspended = 0x10,
        PermanentlyBanned = 0x20,
    }
}