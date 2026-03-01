using System;

namespace IPC.LoginServer.DTO {

    [IngoingPacketDto]
    public record ServerListDto(
        int LastServerId,
        ServerListEntry[] ServerList
    ) : PacketDto;

    public record ServerListEntry(
        int Id,
        string Host,
        int Port,
        ServerAgeLimit AgeLimit,
        bool IsPvpAllowed,
        int OnlinePlayers,
        int MaxPlayers,
        bool IsOnline,
        ServerConfiguration ServerConfiguration,
        int Region // 0x01 - server name in brackets
    );

    public enum ServerAgeLimit {
        Limit15,
        Limit18,
        LimitFree
    }

    [Flags]
    public enum ServerConfiguration {
        General = 0,
        NonPvp = 0x01,
        /// <remarks>Clock around server name</remarks>
        Relax = 0x02,
        Test =  0x04,
        /// <remarks>Hide server name</remarks>
        Broad = 0x08, 
        DisableAvatarCreation = 0x10,
        Event = 0x20,
        Free = 0x40
    }

    public static class ServerConfigurationExtensions {
        public static bool HasFlag(this ServerConfiguration flags, ServerConfiguration flag) {
            return (flags & flag) == flag;
        }
    }

}