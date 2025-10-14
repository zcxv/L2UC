using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.FilterWindow;


public class PrivilegesInfoContent : AbstractClanContent
{
    private ICreatorPanelCheckBox _createPanelCheckBox;
    private const string _panelSystemPrivilegesName = "PanelCheckBoxSystemPrivileges";
    private const string _panelSystemClanHallName = "PanelCheckBoxClanHall";
    private const string _panelSystemCastleName = "PanelCheckBoxCastle";


    // Reusable lists
    private List<List<SettingCheckBox>> _leftCheckBoxes;
    private List<List<SettingCheckBox>> _rightCheckBoxes;

    public PrivilegesInfoContent(DataProviderClanInfo dataProvider , ICreatorPanelCheckBox createPanelCheckBox)
    {
        _dataProvider = dataProvider;
        _createPanelCheckBox = createPanelCheckBox;

        // Initialize lists once
        _leftCheckBoxes = new List<List<SettingCheckBox>>();
        _rightCheckBoxes = new List<List<SettingCheckBox>>();

    }

    public void PreShow(ServerPacket serverPacket, VisualElement detailedInfoElement)
    {
        content = LoadContent(content, detailedInfoElement);
        ClearContent(content);

        Show(serverPacket, detailedInfoElement);
    }

    private void Show(ServerPacket serverPacket, VisualElement detailedInfoElement)
    {


        VisualElement page = ToolTipsUtils.CloneOne(template);


        Button cancelButton = page.Q<Button>("CancelButtonBox2");
        SubscribeCloseButton(cancelButton, detailedInfoElement);


        var _panelPrivilages = page.Q<VisualElement>(_panelSystemPrivilegesName);
        var _panelClanHall = page.Q<VisualElement>(_panelSystemClanHallName);
        var _panelCastle = page.Q<VisualElement>(_panelSystemCastleName);

        if(serverPacket.GetType() == typeof(PledgeReceivePowerInfo))
        {
            PledgeReceivePowerInfo privilegesInfo = serverPacket as PledgeReceivePowerInfo;
            UsePowerGrade(privilegesInfo.PowerGrade, _panelPrivilages, _panelClanHall, _panelCastle);
        }

        if (serverPacket.GetType() == typeof(ManagePledgePower))
        {
            ManagePledgePower managePledgePower = serverPacket as ManagePledgePower;
            UseRank(managePledgePower.PrivilegesByRank, _panelPrivilages, _panelClanHall, _panelCastle);
        }


        // Clear and reuse lists
        _leftCheckBoxes.Clear();
        _rightCheckBoxes.Clear();

        if (content != null & page != null)
        {
            content.Add(page);
        }
    }





    private void UsePowerGrade(int powerGrade , VisualElement _panelPrivilages , VisualElement _panelClanHall, VisualElement _panelCastle)
    {
        var elements = new VisualElement[] { _panelPrivilages, _panelClanHall, _panelCastle };

        switch (powerGrade)
        {
            case -1:
                CheckBoxInstaller.UsePowerGradeMinus1(_createPanelCheckBox , _leftCheckBoxes , _rightCheckBoxes, elements);
                break;
            case 1:
                CheckBoxInstaller.UsePowerGrade1(_createPanelCheckBox, _leftCheckBoxes, _rightCheckBoxes, elements);
                break;
            case 6:
                CheckBoxInstaller.UsePowerGrade6(_createPanelCheckBox, _leftCheckBoxes, _rightCheckBoxes, elements);
                break;
            default:
                break;
        }
    }

    private void UseRank(int rank, VisualElement _panelPrivilages, VisualElement _panelClanHall, VisualElement _panelCastle)
    {

        PledgePowerCheckBoxInstaller.CreateCheckboxByPowerRanked(rank , new ModelPowerCheckBox(_createPanelCheckBox , _leftCheckBoxes , _rightCheckBoxes , new VisualElement[] { _panelPrivilages, _panelClanHall, _panelCastle }));
    }



}
