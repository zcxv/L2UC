
using System;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine.UIElements;

public class MemberInfoContent : AbstractClanContent
{
    private const string _templateNameElementChangeRank = "Data/UI/_Elements/Game/Clan/DetailedContent/MemberInfoElement/ChangeRank";
    private VisualTreeAsset _tamplateChangeRank;
    private DropdownField _dropdown;

    private const string _dropdownName = "comboBox";
    private const string _cancelButtonName = "CancelButtonBox2";
    private const string _centerBoxContentName = "Center";
    private const string _buttonRankName = "RankButton";
    private const string _buttonTitleName = "TitleButton";

    private const string _labelRankName = "RankLabel";
    private const string _labelTitleName = "TitleLabel";
    private const string _dismissTitleName = "DismissLabel";

    private bool _isRankButtonSubscribed = false;
    private bool _isTitleButtonSubscribed = false;

    private EventCallback<ClickEvent> _rankCallback;
    private EventCallback<ClickEvent> _titleCallBack;

    private UnityEngine.UIElements.Button cancelButton;
    private UnityEngine.UIElements.Button rankButton;
    private UnityEngine.UIElements.Button titleButton;


    private bool _isLeader = false;
    public MemberInfoContent(DataProviderClanInfo dataProvider)
    {
        _dataProvider = dataProvider;
    }

    public void LoadAsset(Func<string, VisualTreeAsset> loaderFunc)
    {
        _tamplateChangeRank = loaderFunc(_templateNameElementChangeRank);
    }

    public void PreShow(PledgeReceiveMemberInfo memberInfo, VisualElement detailedInfoElement, PledgeShowMemberListAll packetAll)
    {
        content = LoadContent(content, detailedInfoElement);
        ClearContent(content);
        SetLeader(memberInfo, packetAll);
        UnregisterCallBack(rankButton , titleButton);

        Show(memberInfo, detailedInfoElement, packetAll);
    }
    public void Show(PledgeReceiveMemberInfo memberInfo, VisualElement detailedInfoElement, PledgeShowMemberListAll packetAll)
    {
        VisualElement page = ToolTipsUtils.CloneOne(template);

        _dataProvider.SetMemberInfo(page, memberInfo, packetAll);

        cancelButton = page.Q<UnityEngine.UIElements.Button>(_cancelButtonName);
        rankButton = page.Q<UnityEngine.UIElements.Button>(_buttonRankName);
        titleButton = page.Q<UnityEngine.UIElements.Button>(_buttonTitleName);



        UnityEngine.UIElements.Label dismissLabel = page.Q<UnityEngine.UIElements.Label>(_dismissTitleName);
        UnityEngine.UIElements.Label rankLabel = page.Q<UnityEngine.UIElements.Label>(_labelRankName);
        UnityEngine.UIElements.Label titleLabel= page.Q<UnityEngine.UIElements.Label>(_labelTitleName);

        OnEnabledButton(memberInfo.Name , _isLeader, rankLabel, titleLabel , dismissLabel);

        VisualElement centerBox = page.Q<VisualElement>(_centerBoxContentName);



        SubscribeCloseButton(cancelButton, detailedInfoElement);
        SubscribeOnRank(memberInfo.Name , rankButton, centerBox , detailedInfoElement);
        SubscribeOnTitle(titleButton, centerBox, detailedInfoElement);




        if (content != null & page != null)
        {
            content.Add(page);
        }
    }


    private void SetLeader(PledgeReceiveMemberInfo memberInfo, PledgeShowMemberListAll packetAll)
    {
        _isLeader = StorageNpc.getInstance().GetFirstUser().PlayerInfoInterlude.Identity.Name == packetAll.SubPledgeLeaderName;
    }

    private void UnregisterCallBack(UnityEngine.UIElements.Button rankButton , UnityEngine.UIElements.Button titleButton)
    {
        if(_isRankButtonSubscribed | _isTitleButtonSubscribed)
        {
            if(_rankCallback != null) rankButton?.UnregisterCallback(_rankCallback);
            if (_titleCallBack != null) titleButton?.UnregisterCallback(_titleCallBack);
            _isTitleButtonSubscribed = false;
            _isRankButtonSubscribed = false;
        }
    }

    private void OnEnabledButton(string selectName, bool isLeader, UnityEngine.UIElements.Label rankLabel,
    UnityEngine.UIElements.Label titleLabel, UnityEngine.UIElements.Label dismisslabel)
    {
        ApplyLeaderStyles(isLeader, rankLabel, titleLabel, dismisslabel);
        ApplySelfRestrictions(selectName, rankLabel, dismisslabel);
    }

    private void ApplyLeaderStyles(bool isLeader, UnityEngine.UIElements.Label rankLabel,
    UnityEngine.UIElements.Label titleLabel, UnityEngine.UIElements.Label dismisslabel)
    {
        var labels = new[] { rankLabel, titleLabel, dismisslabel };
        string activeStyle = isLeader ? USS_STYLE_YELLOW : USS_STYLE_DISABLED;
        string inactiveStyle = isLeader ? USS_STYLE_DISABLED : USS_STYLE_YELLOW;

        foreach (var label in labels)
        {
            UpdateLabelStyle(label, activeStyle, inactiveStyle);
        }
    }

    private void ApplySelfRestrictions(string selectName, UnityEngine.UIElements.Label rankLabel,
    UnityEngine.UIElements.Label dismisslabel)
    {
        string selfName = StorageNpc.getInstance().GetFirstUser().PlayerInfoInterlude.Identity.Name;

        if (selectName == selfName)
        {
            UpdateLabelStyle(rankLabel, USS_STYLE_DISABLED, USS_STYLE_YELLOW);
            UpdateLabelStyle(dismisslabel, USS_STYLE_DISABLED, USS_STYLE_YELLOW);
        }
    }

    private void UpdateLabelStyle(UnityEngine.UIElements.Label label, string addStyle, string removeStyle)
    {
        label.RemoveFromClassList(removeStyle);
        label.AddToClassList(addStyle);
    }


    private void SubscribeOnRank(string selectName , UnityEngine.UIElements.Button rankButton, VisualElement centerBox, VisualElement detailedInfoElement)
    {
        string selfName = StorageNpc.getInstance().GetFirstUser().PlayerInfoInterlude.Identity.Name;

        if (_isLeader & selectName != selfName)
        {
            if (rankButton != null && !_isRankButtonSubscribed)
            {
                _rankCallback = (ClickEvent evt) => OnRankButtonClick(centerBox);
                rankButton.RegisterCallback(_rankCallback);
                _isRankButtonSubscribed = true;
            }
        }

    }

    private void SubscribeOnTitle(UnityEngine.UIElements.Button titleButton, VisualElement centerBox, VisualElement detailedInfoElement)
    {
        if (_isLeader)
        {
            if (titleButton != null && !_isTitleButtonSubscribed)
            {

                _titleCallBack = (ClickEvent evt) => OnTitleButtonClick(centerBox);
                titleButton.RegisterCallback(_titleCallBack);

                _isTitleButtonSubscribed = true;
            }
        }

    }

    private void OnRankButtonClick(VisualElement centerBox)
    {
        VisualElement elementChangeRank = ToolTipsUtils.CloneOne(_tamplateChangeRank);
        _dropdown = elementChangeRank.Q<DropdownField>(_dropdownName);
        _dropdown.value = "";
        SetDropdownList(_dropdown, new List<string> { "1st level Privilege", "2st level Privilege", "3st level Privilege", "4st level Privilege", "Joint Rank"});
        centerBox?.Add(elementChangeRank);
    }


    private void OnTitleButtonClick(VisualElement centerBox)
    {
        VisualElement elementChangeRank = ToolTipsUtils.CloneOne(_tamplateChangeRank);
        _dropdown = elementChangeRank.Q<DropdownField>(_dropdownName);
        SetDropdownList(_dropdown, new List<string> { "Title 1", "Title 2", "Title 3"});
        centerBox?.Add(elementChangeRank);
    }

    public void SetDropdownList(DropdownField dropdown, List<string> items)
    {
        if (items == null)
        {
            dropdown.value = null;
            dropdown.choices = null;
        }
        else
        {
            //var _listDropDown = new List<string> { "Main Clan - " + clanName };
            //dropdown.value = "Main Clan - " + clanName;
            dropdown.choices = items;

        }

    }
}
