namespace IPC.LoginServer.DTO {
    public enum LoginFailureReason {
        SystemError = 0x01,
        PassWrong = 0x02,
        UserOrPassWrong = 0x03,
        AccessFailed = 0x04,
        AccountInfoIncorrect = 0x05,
        AccountInUse = 0x07,
        ServerOverloaded = 0x0f,
        ServerMaintenance =  0x10,
        TempPassExpired = 0x11,
        GameTimeExpired = 0x12,
        NoTimeLeft = 0x13,
        RestrictedIP = 0x16,
        UsageTimeHasFinished = 0x1e,
        InvalidSecurityCardNo = 0x1f,
        NotVerifiedAge = 0x20,
        CannotAccessedByCoupon = 0x21,
        LoginWithTwoAccounts = 0x23,
        GuardianConsentRequired = 0x26,
        DeclinedUserAgreement = 0x27,
        AccountSuspended = 0x28,
        LoggedInto10Accounts = 0x2a,
        MasterAccountRestricted =  0x2b
    }
}