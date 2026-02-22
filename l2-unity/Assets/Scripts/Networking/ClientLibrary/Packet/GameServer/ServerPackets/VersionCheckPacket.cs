using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// VersionCheckPacket (original name) / KeyPacket (l2j name)
/// </summary>
public class VersionCheckPacket : ServerPacket {
    public byte[] BlowFishKey { get; private set; }
    public bool AuthAllowed { get; private set; }

    public int ServerId { get; private set; }
    public bool UseBlowfish { get; private set; }

    public VersionCheckPacket(byte[] d) : base(d) {
        Parse();
    }

    public override void Parse() {
        // FIXME m0nster: check with debugger, right struct:
        //  c - 0x00
        //  c - protocol compatible (bool)
        //  b[16] - key
        //  d - enable blowfish (bool)
        //  d - server id
        AuthAllowed = ReadB() != 0x00;
        BlowFishKey = ReadB(8); //Setting server Interlude
        UseBlowfish = ReadI() != 0x00;
        ServerId = ReadI();
        //Debug.Log("VersionCheckPacket success  !!!!!!!!!!!!!! " + AuthAllowed);
        //Debug.Log("VersionCheckPacket success  !!!!!!!!!!!!!! auth " + auth);
        //Debug.Log("VersionCheckPacket success  !!!!!!!!!!!!!! BlowFishKey " + BlowFishKey.ToString());
        //Debug.Log("VersionCheckPacket success  !!!!!!!!!!!!!! ServerId "  + ServerId);
        //Debug.Log($"BlowFishKey Print  : {StringUtils.ByteArrayToString(BlowFishKey)}");
    }

}
