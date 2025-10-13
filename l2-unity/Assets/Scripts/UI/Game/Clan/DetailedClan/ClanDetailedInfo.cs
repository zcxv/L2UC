using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ClanDetailedInfo
{
    private const string _templateNameClanInfo = "Data/UI/_Elements/Game/Clan/DetailedContent/ClanInfoContent";
    private const string _templateNamePrivilegesInfo = "Data/UI/_Elements/Game/Clan/DetailedContent/PrivilegesInfoContent";
    private const string _templateNameRankingPrivelege = "Data/UI/_Elements/Game/Clan/DetailedContent/RankingPrivilege";
    private const string _templateNameMemberInfo = "Data/UI/_Elements/Game/Clan/DetailedContent/MemberInfoContent";

    private DataProviderClanInfo _dataProvider;
    private MemberInfoContent _memberInfoContent;

    private PrivilegesInfoContent _privilegesInfoContent;
    private RankingPrivelege _rankingPrivilege;
    private ClanInfoContent _clanInfoContent;
    private ICreatorPanelCheckBox _createPanelCheckBox;
    private ICreatorSkillsPanel _creatorSkillsPanel;


    private int _showPanel = -1;

    public ClanDetailedInfo(DataProviderClanInfo dataProvider)
    {
        _dataProvider = dataProvider;

        _createPanelCheckBox = new CreatePanelCheckBoxWindows();
        _creatorSkillsPanel = new CreatorSkillsPanel();

        _memberInfoContent = new MemberInfoContent(_dataProvider);
        _memberInfoContent.OnClickHide += OnClickHide;

        _privilegesInfoContent = new PrivilegesInfoContent(_dataProvider , _createPanelCheckBox);
        _privilegesInfoContent.OnClickHide += OnClickHide;

        _clanInfoContent = new ClanInfoContent(_dataProvider , _creatorSkillsPanel);
        _clanInfoContent.OnClickHide += OnClickHide;

        _rankingPrivilege = new RankingPrivelege(_dataProvider);
        _rankingPrivilege.OnClickHide += OnClickHide;
    }

   public void LoadAssets(Func<string, VisualTreeAsset> loaderFunc)
   {
        _memberInfoContent.template = loaderFunc(_templateNameMemberInfo);
        _privilegesInfoContent.template = loaderFunc(_templateNamePrivilegesInfo);
        _clanInfoContent.template = loaderFunc(_templateNameClanInfo);
        _rankingPrivilege.template = loaderFunc(_templateNameRankingPrivelege);

        _createPanelCheckBox.LoadAsset(loaderFunc);
        _creatorSkillsPanel.LoadAsset(loaderFunc);
        _rankingPrivilege.LoadAsset(loaderFunc);

    }

    public void UpdateDetailedInfo(ServerPacket packet, VisualElement detailedInfoElement, PledgeShowMemberListAll packetAll)
    {
        switch (packet)
        {
            case PledgeReceiveMemberInfo memberInfo:
                _memberInfoContent.PreShow(memberInfo, detailedInfoElement, packetAll);
                _showPanel = 0;
                break;

            case PledgeReceivePowerInfo powerInfo:
                _privilegesInfoContent.PreShow(powerInfo, detailedInfoElement, packetAll);
                _showPanel = 1;
                break;
            case PledgeClanInfo clanInfo:
                _clanInfoContent.PreShow(clanInfo, detailedInfoElement, packetAll);
                _showPanel = 2;
                break;
            case PledgePowerGradeList authInfo:
                _rankingPrivilege.PreShow(authInfo, detailedInfoElement, packetAll);
                _showPanel = 3;
                break;
            default:
                // Handle unknown packet types if necessary
                break;
        }
    }
    public int GetShowPanel()
    {
        return _showPanel;
    }
    private void OnClickHide(int id)
    {
        _showPanel = -1;
    }
}
