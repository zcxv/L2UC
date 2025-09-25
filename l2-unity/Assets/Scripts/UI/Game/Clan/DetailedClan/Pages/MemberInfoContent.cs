using Org.BouncyCastle.Bcpg;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.LookDev;
using UnityEngine.UIElements;

public class MemberInfoContent : AbstractClanContent
{

    public MemberInfoContent(DataProviderClanInfo dataProvider)
    {
        _dataProvider = dataProvider;
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
        _dataProvider.SetMemberInfo(page, memberInfo, packetAll);

        Button cancelButton = page.Q<Button>("CancelButtonBox2");
        SubscribeCloseButton(cancelButton, detailedInfoElement);


        if (content != null & page != null)
        {
            content.Add(page);
        }
    }
}
