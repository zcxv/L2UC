using UnityEngine;
using UnityEngine.UIElements;

public class PrivilegesInfoContent : AbstractClanContent
{
    public PrivilegesInfoContent(DataProviderClanInfo dataProvider)
    {
        _dataProvider = dataProvider;
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


        if (content != null & page != null)
        {
            content.Add(page);
        }
    }
}
