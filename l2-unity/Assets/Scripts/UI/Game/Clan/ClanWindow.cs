using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Utilities.Encoders;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ClanWindow : L2TwoPanels
{
   
    private static ClanWindow _instance;
    private MasterClan _masterClan;
    private ClanDetailedInfo _detailedClan;
    private ICreatorTables _creatorTableWindows;

    private PledgeClanInfo _pladgeClanInfo = new PledgeClanInfo(new byte[1]);
    private DropdownField _dropdown;
    private string _selectDropDown = "";
    private List<string> _listDropDown;
    private DataProviderClanInfo _dataProviderClanInfo;
    //data
    private PledgeShowMemberListAll _packet;

    public static ClanWindow Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _creatorTableWindows = new CreatorTableWindows();
            _masterClan = new MasterClan();

            _dataProviderClanInfo = new DataProviderClanInfo();
            _detailedClan = new ClanDetailedInfo(_dataProviderClanInfo);
        }
        else
        {
            Destroy(this);
        }
    }

    protected override void LoadAssets()
    {
        _windowTemplate = LoadAsset("Data/UI/_Elements/Game/Clan/ClanWindow");
    }

    protected override IEnumerator BuildWindow(VisualElement root)
    {


        InitWindow(root);


        yield return new WaitForEndOfFrame();

        var dragArea = GetElementByClass("drag-area");
        DragManipulator drag = new DragManipulator(dragArea, _windowEle);
        dragArea.AddManipulator(drag);


        _dropdown = _windowEle.Q<DropdownField>("comboBox");
        DisableEventOnOver(_dropdown);
        _dropdown.RegisterValueChangedCallback(OnDropdownValueChanged);
        _dropdown.RegisterCallback<PointerDownEvent>(OnDropdownPointer, TrickleDown.TrickleDown);


        var master_table_content = GetElementByClass("master-table-list");

        _creatorTableWindows.InitTable(master_table_content);
        _creatorTableWindows.LoadAsset(LoadAsset);
        _masterClan.ForEachClan(_creatorTableWindows);
        _creatorTableWindows.OnRowClicked += _masterClan.SelectMember;
        _creatorTableWindows.OnRowClicked += SelectMember;

        _detailedClan.LoadAssets(LoadAsset);

        var memberButton = (UnityEngine.UIElements.Button)GetElementById("MemberButton");
        memberButton?.RegisterCallback<ClickEvent>(evt => OnClickShowMember(evt));

        var privilegesButton = (UnityEngine.UIElements.Button)GetElementById("PrivilegesButton");
        privilegesButton?.RegisterCallback<ClickEvent>(evt => OnClickPrivileges(evt));

        var clanInfo = (UnityEngine.UIElements.Button)GetElementById("ClanInoButton");
        clanInfo?.RegisterCallback<ClickEvent>(evt => OnClickClanInfo(evt));


        var penaltyButton = (UnityEngine.UIElements.Button)GetElementById("PenaltyButton");
        penaltyButton?.RegisterCallback<ClickEvent>(evt => OnClickPenalty(evt));

        _detailedInfoElement = (VisualElement)GetElementById("detailedInfo");
        var masterClan = (VisualElement)GetElementById("masterClan");

        SetMouseOverDetectionSubElement(_detailedInfoElement);
        SetMouseOverDetectionRefreshTargetElement(masterClan);


        RegisterCloseWindowEvent("btn-close-frame");
        RegisterClickWindowEvent(_windowEle, dragArea);
        HideDetailedInfo();
        OnCenterScreen(_root);
    }

    //Detailed Clan
    public void UpdateDetailedInfo(ServerPacket packet)
    {
        _detailedClan.UpdateDetailedInfo(packet , _detailedInfoElement , _packet);
        ShowDetailedInfo();
    }




    //Master Clan 
    public void AddClanData(PledgeShowMemberListAll packet )
    {
        _dataProviderClanInfo.SetMasterClanInfo(_windowEle , packet);
        _listDropDown = _masterClan.SetDropdownList(_dropdown , packet.PledgeName);
        _masterClan.CreateMembersTable(packet.Members, _creatorTableWindows);
        _packet = packet;
    }

    public void UpdateMemberData(PledgeShowMemberListUpdate packetUpdate)
    {
        _masterClan.UpdateMemberData(packetUpdate, _packet, _creatorTableWindows);
    }

    public void UpdatePledge(PledgeInfo pledge)
    {
        _dataProviderClanInfo.UpdateClanInfo(_windowEle, pledge);
    }

    public void UpdateClanIdInfo(PledgeStatusChanged changed)
    {
        if(_packet != null & changed != null)
        {
            _packet.ClanId = changed.ClanId;
            _packet.CrestId = changed.CrestId;
            _packet.LeaderId = changed.LeaderId;
            _packet.AllyId = changed.AllyId;
            _packet.AllyCrestId = changed.AllyCrestId;

        }
    }


    private void OnClickShowMember(ClickEvent evt)
    {
        if (!string.IsNullOrEmpty(_masterClan.GetSelectMemberName()))
        {
            SendGameDataQueue.Instance().AddItem(
                CreatorPacketsUser.CreateRequestPladgeMemberInfo(_masterClan.GetSelectMemberName()), 
                GameClient.Instance.IsCryptEnabled(), 
                GameClient.Instance.IsCryptEnabled());
        }

    }

    private void OnClickPrivileges(ClickEvent evt)
    {
        if (!string.IsNullOrEmpty(_masterClan.GetSelectMemberName()))
        {
            SendGameDataQueue.Instance().AddItem(
                CreatorPacketsUser.CreateRequestPledgeMemberPowerInfo(_masterClan.GetSelectMemberName()),
                GameClient.Instance.IsCryptEnabled(),
                GameClient.Instance.IsCryptEnabled());
        }

    }

    private void OnClickPenalty(ClickEvent evt)
    {

            SendGameDataQueue.Instance().AddItem(
                CreatorPacketsUser.CreateRequestUserCommand(100),
                GameClient.Instance.IsCryptEnabled(),
                GameClient.Instance.IsCryptEnabled());
        
    }

    private void OnClickClanInfo(ClickEvent evt)
    {
        _detailedClan.UpdateDetailedInfo(_pladgeClanInfo, _detailedInfoElement, _packet);
        ShowDetailedInfo();
    }


    public void SelectMember(int selectIndex, string select_text)
    {
        if (_detailedInfoElement.style.display == DisplayStyle.Flex)
        {


            switch (_detailedClan.GetShowPanel())
            {
                case 0:
                    OnClickShowMember(null);
                    break;

                case 1:
                    OnClickPrivileges(null);
                    break;
                default:
                    break;
            }
        }
    }

    private void OnDropdownPointer(PointerDownEvent evt)
    {
        if (_listDropDown == null || _listDropDown.Count == 0)
        {
            evt.PreventDefault();
            evt.StopImmediatePropagation();
        }
    }

    private void OnDropdownValueChanged(ChangeEvent<string> evt)
    {
        //string playeName = evt.newValue;
        //_selectDropDown = playeName;
    }

    private void OnDestroy()
    {
        _instance = null; 
        _creatorTableWindows = null;
        _masterClan = null;
        _detailedClan = null;
        _dataProviderClanInfo = null;
    }
}
