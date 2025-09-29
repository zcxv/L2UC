using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;


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

    public void PreShow(PledgeReceivePowerInfo privilegesInfo, VisualElement detailedInfoElement, PledgeShowMemberListAll packetAll)
    {
        content = LoadContent(content, detailedInfoElement);
        ClearContent(content);

        Show(privilegesInfo, detailedInfoElement, packetAll);
    }
    public void Show(PledgeReceivePowerInfo privilegesInfo, VisualElement detailedInfoElement, PledgeShowMemberListAll packetAll)
    {


        VisualElement page = ToolTipsUtils.CloneOne(template);


        Button cancelButton = page.Q<Button>("CancelButtonBox2");
        SubscribeCloseButton(cancelButton, detailedInfoElement);


        var _panelPrivilages = page.Q<VisualElement>(_panelSystemPrivilegesName);
        var _panelClanHall = page.Q<VisualElement>(_panelSystemClanHallName);
        var _panelCastle = page.Q<VisualElement>(_panelSystemCastleName);


        // Clear and reuse lists
        _leftCheckBoxes.Clear();
        _rightCheckBoxes.Clear();


        switch (privilegesInfo.PowerGrade)
        {
            case 1:
                UsePowerGrade1(_panelPrivilages, _panelClanHall, _panelCastle);
                break;
            case 6:
                UsePowerGrade6(_panelPrivilages, _panelClanHall, _panelCastle);
                break;
            default:
                break;
        }


        if (content != null & page != null)
        {
            content.Add(page);
        }
    }

    private void UsePowerGrade1(VisualElement panelPrivilages , VisualElement panelClanHall , VisualElement panelCastle)
    {
        CheckBoxRootElements elements = new CheckBoxRootElements(
          new List<VisualElement> { panelPrivilages, panelClanHall, panelCastle },
             CreateAllLeft1(),
             CreateAllRight1()
         );
            _createPanelCheckBox.CreateTwoPanels(elements);
    }

    private void UsePowerGrade6(VisualElement panelPrivilages, VisualElement panelClanHall, VisualElement panelCastle)
    {


        CheckBoxRootElements elements = new CheckBoxRootElements(
         new List<VisualElement> { panelPrivilages, panelClanHall, panelCastle },
             CreateAllLeft6(),
             CreateAllRight6()
        );
        _createPanelCheckBox.CreateTwoPanels(elements);

    }

    private List<List<SettingCheckBox>> CreateAllLeft1()
    {
        _leftCheckBoxes.AddRange(new List<List<SettingCheckBox>>
        {
            CheckBoxInstaller.InitChecBoxPrivilegesLeft(true),
            CheckBoxInstaller.InitChecBoxClanHallLeft(true),
            CheckBoxInstaller.InitChecBoxCastleLeft(true)
        });

        return _leftCheckBoxes;

    }

    private List<List<SettingCheckBox>> CreateAllRight1()
    {
        _rightCheckBoxes.AddRange(new List<List<SettingCheckBox>>
        {
            CheckBoxInstaller.InitChecBoxPrivilegesRight(true),
            CheckBoxInstaller.InitChecBoxClanHallRight(true),
            CheckBoxInstaller.InitChecBoxCastleRight(true)
        });

        return _rightCheckBoxes;
    }

    private List<List<SettingCheckBox>> CreateAllLeft6()
    {
        _leftCheckBoxes.AddRange(new List<List<SettingCheckBox>>
        {
            CheckBoxInstaller.InitChecBoxPrivilegesLeft(false),
            CheckBoxInstaller.InitChecBoxClanHallLeft(false),
            CheckBoxInstaller.InitChecBoxCastleLeft(false)
        });

        return _leftCheckBoxes;

    }

    private List<List<SettingCheckBox>> CreateAllRight6()
    {
        _rightCheckBoxes.AddRange(new List<List<SettingCheckBox>>
        {
            CheckBoxInstaller.InitChecBoxPrivilegesRight(false),
            CheckBoxInstaller.InitChecBoxClanHallRight(false),
            CheckBoxInstaller.InitChecBoxCastleRight(false)
        });

        return _rightCheckBoxes;
    }


}
