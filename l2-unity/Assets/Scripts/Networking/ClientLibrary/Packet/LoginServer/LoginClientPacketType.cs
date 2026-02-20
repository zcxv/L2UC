using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoginClientPacketType : byte {
    Ping = 0x00,
    ProtocolVersion = 0x00,
    AuthGameGuard = 0x07,
    RequestAuthLogin = 0x00,
    RequestServerList = 0x05,
    RequestServerLogin = 0x02,
}
