using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GSMessagePacketType : byte
{
    SystemMessage = 0x64,
    CreatureSay = 0x4A,
    NpcSay = 0x02
}
