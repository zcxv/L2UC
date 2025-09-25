using UnityEngine;

public class PledgeReceivePowerInfo : ServerPacket
{

    private string _name;
    private int _powerGrade;
    private int _powerGradeByRank;

    public string Name => _name;
    public int PowerGrade => _powerGrade;
    public int PowerGradeByRank => _powerGradeByRank;

    public PledgeReceivePowerInfo(byte[] d) : base(d)
    {
        Parse();
    }
    public override void Parse()
    {
        _powerGrade = ReadI();
        _name = ReadOtherS();
        _powerGradeByRank = ReadI();
    }
}