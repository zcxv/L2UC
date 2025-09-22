using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PledgeShowMemberListAll : ServerPacket
{
    private List<ClanMember> _members = new List<ClanMember>();
    private int _pledgeTypeEnabled;
    private int _pledgeType;
    private int _clanId;
    private string _pledgeName;
    private string _subPledgeLeaderName;

    private int _crestId;
    private int _level;
    private int _castleId;
    private int _clanHallId;
    private int _rank;
    private int _reputationScore;
    private int _dissolvingExpiryTime;
    private int _allyId;
    private string _allyName;
    private int _allyCrestId;
    private bool _isWar;
    public PledgeShowMemberListAll(byte[] d) : base(d)
    {
   
        Parse();
    }
    public override void Parse()
    {
        _pledgeTypeEnabled = ReadI();
        _clanId = ReadI();
        _pledgeType = ReadI();
        _pledgeName = ReadOtherS();
        _subPledgeLeaderName = ReadOtherS();

        _crestId = ReadI();
        _level = ReadI();
        _castleId = ReadI();
        _clanHallId = ReadI();
        _rank = ReadI();
        _reputationScore = ReadI();
        _dissolvingExpiryTime = ReadI();
        var unk1 = ReadI();
        _allyId = ReadI();
        _allyName = ReadOtherS();
        _allyCrestId = ReadI();
        _isWar = ReadI() == 1;

        int membersCount = ReadI();
        
        for(int i=0; i < membersCount; i++)
        {
            string memberName = ReadOtherS();
            int level = ReadI();
            int classId = ReadI();
            int sex = ReadI();
            int race = ReadI();
            int online = ReadI();
            int sponsor = ReadI();

            _members.Add(new ClanMember(memberName, level, classId, sex, race, online, sponsor));
        }
        PrintClanInfo();
    }

    public void PrintClanInfo()
    {
        Debug.Log($"Пришел пакет : PledgeShowMemberListAll {_level}");
        Debug.Log($"PledgeShowMemberListAll > Clan ID: {_clanId}");
        Debug.Log($"PledgeShowMemberListAll > Clan Name: {_pledgeName}");
        Debug.Log("Members:");

        foreach (var member in _members)
        {
            Debug.Log($"PledgeShowMemberListAll Member >Name: {member.MemberName}, Level: {member.Level}, Class ID: {member.ClassId}");
        }
    }

}

public class ClanMember
{
    private string _memeberName;
    private int _level;
    private int _classId;
    private int _sex;
    private int _race;
    private int _online;
    private int _sponsor;

    // Конструктор класса
    public ClanMember(string memberName, int level, int classId, int sex, int race, int online, int sponsor)
    {
        _memeberName = memberName;
        _level = level;
        _classId = classId;
        _sex = sex;
        _race = race;
        _online = online;
        _sponsor = sponsor;
    }

    // Геттеры и сеттеры для каждого поля
    public string MemberName
    {
        get { return _memeberName; }
        set { _memeberName = value; }
    }

    public int Level
    {
        get { return _level; }
        set { _level = value; }
    }

    public int ClassId
    {
        get { return _classId; }
        set { _classId = value; }
    }

    public int Sex
    {
        get { return _sex; }
        set { _sex = value; }
    }

    public int Race
    {
        get { return _race; }
        set { _race = value; }
    }

    public int Online
    {
        get { return _online; }
        set { _online = value; }
    }

    public int Sponsor
    {
        get { return _sponsor; }
        set { _sponsor = value; }
    }
}
