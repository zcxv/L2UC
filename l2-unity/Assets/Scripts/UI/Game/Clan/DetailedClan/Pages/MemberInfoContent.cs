
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

        Show(memberInfo, detailedInfoElement, packetAll);
    }
    public void Show(PledgeReceiveMemberInfo memberInfo, VisualElement detailedInfoElement, PledgeShowMemberListAll packetAll)
    {
        VisualElement page = ToolTipsUtils.CloneOne(template);
        VisualElement elementChangeRank = ToolTipsUtils.CloneOne(_tamplateChangeRank);
        _dataProvider.SetMemberInfo(page, memberInfo, packetAll);

        UnityEngine.UIElements.Button cancelButton = page.Q<UnityEngine.UIElements.Button>(_cancelButtonName);
        VisualElement centerBox = page.Q<VisualElement>(_centerBoxContentName);
        _dropdown = elementChangeRank.Q<DropdownField>(_dropdownName);

        SetDropdownList(_dropdown, new List<string> { "1-ое правило" , "2 правило" , "3-е правило", "4-е правило", "5-е правило", "6-е правило", "7-е правило" });
        SubscribeCloseButton(cancelButton, detailedInfoElement);

        centerBox?.Add(elementChangeRank);

        if (content != null & page != null)
        {
            content.Add(page);
        }
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
