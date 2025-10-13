using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RankingPrivelege : AbstractClanContent
{
    private ICreatorTables _creatorTableWindows;
    private Func<string, VisualTreeAsset> _loaderFunc;
    private const string master_table_content = "Content";
    public RankingPrivelege(DataProviderClanInfo dataProvider)
    {
        _dataProvider = dataProvider;
        _creatorTableWindows = new CreatorTableWindows();


    }

    public void LoadAsset(Func<string, VisualTreeAsset> loaderFunc)
    {
        _loaderFunc = loaderFunc;
    }
    public void PreShow(PledgePowerGradeList rankingList, VisualElement detailedInfoElement, PledgeShowMemberListAll packetAll)
    {
        content = LoadContent(content, detailedInfoElement);
        ClearContent(content);
        if (_creatorTableWindows != null) _creatorTableWindows.DestroyTable();

        Show(rankingList, detailedInfoElement, packetAll);
    }
    public void Show(PledgePowerGradeList rankingList, VisualElement detailedInfoElement, PledgeShowMemberListAll packetAll)
    {


        VisualElement page = ToolTipsUtils.CloneOne(template);
        VisualElement master_table_content_result = page.Q<VisualElement>(master_table_content);

        CreateTable(rankingList , master_table_content_result, _loaderFunc);

        if (content != null & page != null)
        {
            content.Add(page);
        }
    }

    private void CreateTable(PledgePowerGradeList rankingList , VisualElement master_table_content , Func<string, VisualTreeAsset> loaderFunc)
    {
        if(master_table_content != null)
        {
            _creatorTableWindows.InitTable(master_table_content);
            _creatorTableWindows.LoadAsset(loaderFunc);
            RankedClanTableColumn.ForEachClan(rankingList.GradeList, _creatorTableWindows);
        }

    }
}
