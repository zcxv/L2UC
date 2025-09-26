using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;


public class PrivilegesInfoContent : AbstractClanContent
{
    private List<ICreatorPanelCheckBox> _listCreatePanelCheckBox;
    public PrivilegesInfoContent(DataProviderClanInfo dataProvider , List<ICreatorPanelCheckBox> listCreatePanelCheckBox)
    {
        _dataProvider = dataProvider;
        _listCreatePanelCheckBox = listCreatePanelCheckBox;

    }

    public void PreShow(PledgeReceivePowerInfo privilegesInfo, VisualElement detailedInfoElement, PledgeShowMemberListAll packetAll)
    {
        content = LoadContent(content, detailedInfoElement);
        ClearContent(content);

        Show(privilegesInfo, detailedInfoElement, packetAll);
    }
    public void Show(PledgeReceivePowerInfo privilegesInfo, VisualElement detailedInfoElement, PledgeShowMemberListAll packetAll)
    {


        VisualElement page = ToolTipsUtils.CloneOne(template);
        //_dataProvider.SetMemberInfo(page, memberInfo, packetAll);

        Button cancelButton = page.Q<Button>("CancelButtonBox2");
        SubscribeCloseButton(cancelButton, detailedInfoElement);

        VisualElement panelPrivilages = page.Q<VisualElement>("PanelCheckBoxSystemPrivileges");
        _listCreatePanelCheckBox[0].CreateTwoPanels(panelPrivilages, CheckBoxInstaller.InitChecBoxPrivilegesLeft() , CheckBoxInstaller.InitChecBoxPrivilegesRight());

        VisualElement panelClanHall = page.Q<VisualElement>("PanelCheckBoxClanHall");
        _listCreatePanelCheckBox[1].CreateTwoPanels(panelClanHall, CheckBoxInstaller.InitChecBoxClanHallLeft(), CheckBoxInstaller.InitChecBoxClanHallRight());

        VisualElement panelCastle = page.Q<VisualElement>("PanelCheckBoxCastle");
        _listCreatePanelCheckBox[2].CreateTwoPanels(panelCastle, CheckBoxInstaller.InitChecBoxCastleLeft(), CheckBoxInstaller.InitChecBoxCastleRight());

        if (content != null & page != null)
        {
            content.Add(page);
        }
    }






}
