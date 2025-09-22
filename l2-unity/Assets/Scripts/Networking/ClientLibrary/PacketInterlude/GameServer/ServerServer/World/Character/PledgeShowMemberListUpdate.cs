using UnityEngine;

public class PledgeShowMemberListUpdate : ServerPacket
{
    private string _name;
    private int _level;
    private int _classId;
    private int _sex;
    private int _race;
    private int _isOnline;
    private int _pledgeType;
    private int _hasSponsor;
    public PledgeShowMemberListUpdate(byte[] d) : base(d)
    {
        Parse();
    }
    public override void Parse()
    {
        _name = ReadOtherS();
        _level = ReadI();
        _classId = ReadI();
        _sex = ReadI();
        _race = ReadI();
        _isOnline = ReadI();
        _pledgeType = ReadI();
        _hasSponsor = ReadI();

    }
}
