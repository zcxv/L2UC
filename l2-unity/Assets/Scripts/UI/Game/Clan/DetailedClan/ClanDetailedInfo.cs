using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ClanDetailedInfo
{
    private const string _templateNamePrivilegesInfo = "Data/UI/_Elements/Game/Clan/DetailedContent/PrivilegesInfoContent";
    private const string _templateNameMemberInfo = "Data/UI/_Elements/Game/Clan/DetailedContent/MemberInfoContent";

    private DataProviderClanInfo _dataProvider;
    private MemberInfoContent _memberInfoContent;
    private PrivilegesInfoContent _privilegesInfoContent;
    private int _showPanel = -1;

    public ClanDetailedInfo(DataProviderClanInfo dataProvider)
    {
        _dataProvider = dataProvider;
        _memberInfoContent = new MemberInfoContent(_dataProvider);
        _memberInfoContent.OnClickHide += OnClickHide;
        _privilegesInfoContent = new PrivilegesInfoContent(_dataProvider);
        _privilegesInfoContent.OnClickHide += OnClickHide;
    }

   public void LoadAssets(Func<string, VisualTreeAsset> loaderFunc)
   {
        _memberInfoContent.template = loaderFunc(_templateNameMemberInfo);
        _privilegesInfoContent.template = loaderFunc(_templateNamePrivilegesInfo);
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
