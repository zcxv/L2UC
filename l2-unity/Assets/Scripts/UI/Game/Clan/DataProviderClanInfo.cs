using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DataProviderClanInfo : AbstractDataFunction
{
    private const string IMG_ONLINE = "Data/UI/Clan/PConline_up";
    private const string IMG_OFFLINE= "Data/UI/Clan/PConline_down";
    public void SetClanInfo(VisualElement container, PledgeShowMemberListAll packet)
    {
        if (container != null)
        {
            Label leaderLabel = container.Q<Label>("labelLeaderName");
            AddElementIfNotEmpty(leaderLabel, leaderLabel, packet.SubPledgeLeaderName);

            Label titleLabel = container.Q<Label>("labelClanName");
            AddElementIfNotEmpty(titleLabel, titleLabel, packet.PledgeName);


            Label lvllabel = container.Q<Label>("lvlName");
            AddElementIfNotEmpty(lvllabel, lvllabel, packet.Level.ToString());

            int countOnline = 0;
            int allOnline = 0;

            if (packet.Members != null)
            {
                 countOnline = packet.Members.Count(x => x.Online > 0);
                 allOnline = packet.Members.Count;
            }

            Label onlineLabel = container.Q<Label>("labelCountOnline");
            AddElementIfNotEmpty(onlineLabel, onlineLabel, "(" + countOnline + "/"+ allOnline + ")");

            var icon = container.Q<VisualElement>("ImageOnline");

            if (countOnline == 0)
            {

                AddElementIfNotNull(icon, icon, IconManager.Instance.LoadTextureOtherSources(IMG_OFFLINE));
            }
            else
            {
                AddElementIfNotNull(icon, icon, IconManager.Instance.LoadTextureOtherSources(IMG_ONLINE));
            }


        }
    }
}
