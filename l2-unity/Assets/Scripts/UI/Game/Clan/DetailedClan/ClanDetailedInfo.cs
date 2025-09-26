using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ClanDetailedInfo
{
    private const string _templateNamePrivilegesInfo = "Data/UI/_Elements/Game/Clan/DetailedContent/PrivilegesInfoContent";
    private const string _templateNameMemberInfo = "Data/UI/_Elements/Game/Clan/DetailedContent/MemberInfoContent";

    private DataProviderClanInfo _dataProvider;
    private MemberInfoContent _memberInfoContent;
    private PrivilegesInfoContent _privilegesInfoContent;
    private ICreatorPanelCheckBox _createPanelCheckBoxPrivilages;
    private ICreatorPanelCheckBox _createPanelCheckBoxClanHall;
    private ICreatorPanelCheckBox _createPanelCheckBoxCastle;

    private int _showPanel = -1;

    public ClanDetailedInfo(DataProviderClanInfo dataProvider)
    {
        _dataProvider = dataProvider;

        _createPanelCheckBoxPrivilages = new CreatePanelCheckBoxWindows();
        _createPanelCheckBoxClanHall = new CreatePanelCheckBoxWindows();
        _createPanelCheckBoxCastle = new CreatePanelCheckBoxWindows();

        _memberInfoContent = new MemberInfoContent(_dataProvider);
        _memberInfoContent.OnClickHide += OnClickHide;

        _privilegesInfoContent = new PrivilegesInfoContent(_dataProvider , new List<ICreatorPanelCheckBox> { _createPanelCheckBoxPrivilages , _createPanelCheckBoxClanHall , _createPanelCheckBoxCastle });
        _privilegesInfoContent.OnClickHide += OnClickHide;
    }

   public void LoadAssets(Func<string, VisualTreeAsset> loaderFunc)
   {
        _memberInfoContent.template = loaderFunc(_templateNameMemberInfo);
        _privilegesInfoContent.template = loaderFunc(_templateNamePrivilegesInfo);

        _createPanelCheckBoxPrivilages.LoadAsset(loaderFunc);
        _createPanelCheckBoxClanHall.LoadAsset(loaderFunc);
        _createPanelCheckBoxCastle.LoadAsset(loaderFunc);

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
