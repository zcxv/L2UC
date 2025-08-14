using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class SellCropListWindow : L2PopupWindow
{
    private static SellCropListWindow _instance;
    private ICreatorTables _creatorTableWindows;
    private VisualElement _content;
    public static SellCropListWindow Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance == null)
        {
            _creatorTableWindows = new CreatorTableWindows();
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    protected override void LoadAssets()
    {
        _windowTemplate = LoadAsset("Data/UI/_Elements/Game/Manor/SellCropListWindow");

    }

    protected override IEnumerator BuildWindow(VisualElement root)
    {


        InitWindow(root);


        yield return new WaitForEndOfFrame();

        var dragArea = GetElementByClass("drag-area");
        _content = GetElementByClass("quest-content");
        DragManipulator drag = new DragManipulator(dragArea, _windowEle);
        dragArea.AddManipulator(drag);

        _creatorTableWindows.InitTable(_content, _windowEle);
        _creatorTableWindows.LoadAsset(LoadAsset);

        


        RegisterCloseWindowEvent("btn-close-frame");
        RegisterClickWindowEvent(_windowEle, dragArea);
        OnCenterScreen(root);

    }


    public void SetDataTable(ExShowSellCropList showSellCropList)
    {

        List<CastleCrop> list = new List<CastleCrop>();

        var harvestsList = new List<string>();
        var authList = new List<string>();
        var buyList = new List<string>();
        var priceList = new List<string>();
        var rewardList = new List<string>();
        var propList = new List<string>();
        var salesList = new List<string>();

        if(showSellCropList.List.Count > 0)
        {
            foreach (CastleCrop crop in showSellCropList.List)
            {
                harvestsList.Add(crop.GetName());
                authList.Add(crop.Level.ToString());
                buyList.Add(crop.Amount.ToString());
                priceList.Add(crop.Price.ToString());
                rewardList.Add(crop.Reward.ToString());
                propList.Add(crop.Reward1.ToString());
                salesList.Add(crop.Reward2.ToString());
            }

        }
        else
        {
            //Add Test Data
            harvestsList.Add("Cold code");
            authList.Add("3");
            buyList.Add("5");
            priceList.Add("7");
            rewardList.Add("Varnish");
            propList.Add("Suede");
            salesList.Add("coke");

            //Add Test Data
            harvestsList.Add("Blue code");
            authList.Add("2");
            buyList.Add("1");
            priceList.Add("4");
            rewardList.Add("Varnish");
            propList.Add("Suede");
            salesList.Add("coke");


            //Add Test Data
            harvestsList.Add("Golden code");
            authList.Add("2");
            buyList.Add("2");
            priceList.Add("3");
            rewardList.Add("Varnish");
            propList.Add("Suede");
            salesList.Add("coke");
        }




        var harvest = new TableColumn(false, "Harvest Name", 0, harvestsList, 13);
        var auth = new TableColumn(true, "Lvl", 0, authList, 0);
        var buy = new TableColumn(true, "Buy Remaining", 0, buyList, 0);
        var price = new TableColumn(true, "Purchase Price", 0, priceList, 0);
        var reward = new TableColumn(false, "Reward", 60, rewardList, 13);
        var prop = new TableColumn(false, "My Prop.", 60, propList, 13);
        var sales = new TableColumn(false, "Sales", 60, salesList, 13);

        List<TableColumn> listTableColumn = new List<TableColumn> { harvest, auth, buy, price, reward, prop, sales };
        _creatorTableWindows.CreateTable(listTableColumn);


        //_creatorTableWindows.CreateTable(new List<TableColumn> { new TableColumn(false, "Mission Name", 13 ,  new List<string> { "Letters of Love" , "What Women Want", "Will the Seal Be Broken" } , 13) ,
        //  new TableColumn(false, "Conditions", 0, new List<string> { "No Requirements" , "Elf,Human", "Dark Elf" } ,13),
        //  new TableColumn(true, "Level", 0, new List<string> { "2-5" , "2-5" , "16-26" } , 0),
        //  new TableColumn(true, "Repeatable", 0 , new List<string> { "1"  , "1"  , "1" } , 0),
        //  new TableColumn(false, "Source", 0 , new List<string> { "Darin" , "Arujien", "Talloth" }, 18)});
    }

 
}
