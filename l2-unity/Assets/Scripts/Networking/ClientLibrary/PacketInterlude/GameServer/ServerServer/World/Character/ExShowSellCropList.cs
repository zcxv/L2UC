using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
using static UnityEditor.Progress;

public class ExShowSellCropList : ServerPacket
{

    private int _manorId;
    private List<CastleCrop> _list = new List<CastleCrop>();
    public List<CastleCrop> List { get => _list; }
    public int ManorId { get => _manorId; }

    public ExShowSellCropList(byte[] d) : base(d)
    {
        Parse();
    }

    public override void Parse()
    {
        _manorId = ReadI();
        int size = ReadI();

        for(int i=0; i < size; i++)
        {
            int objectId = ReadI();
            int item_id = ReadI();
            int level = ReadI();
            int unk1 = ReadB();
            int reward1 = ReadI();
            int unk2 = ReadB();
            int reward2 = ReadI();


            int manorId = ReadI();
            int amount = ReadI();
            int price = ReadI();
            int reward = ReadB();
            int count = ReadI();

            CastleCrop crop = new CastleCrop(objectId, item_id, ItemLocation.Trade, 0, count, ItemCategory.Item, false, ItemSlot.none, 0, 9999);
            crop.SetCrop(level, reward, reward1, reward2, price, amount);
        }
    }
}

public class CastleCrop : ItemInstance
{
    private int _level;
    private int _reward;
    private int _reward1;
    private int _reward2;
    private int _price;
    private int _amount;


    public CastleCrop(int objectId, int itemId, ItemLocation location, int slot, int count, ItemCategory category, bool equipped, ItemSlot bodyPart, int enchantLevel, long remainingTime) 
        : base(objectId, itemId, location, slot, count, category, equipped, bodyPart, enchantLevel, remainingTime)
    {

    }

    public void SetCrop(int level , int reward, int reward1 ,  int reward2, int price , int amount)
    {
        _level = level;
        _reward = reward;
        _reward1 = reward1;
        _reward2 = reward2;
        _price = price;
        _amount = amount;
    }

    public int Level => _level;
    public int Reward => _reward;
    public int Reward1 => _reward1;
    public int Reward2 => _reward2;
    public int Price => _price;
    public int Amount => _amount;


}
