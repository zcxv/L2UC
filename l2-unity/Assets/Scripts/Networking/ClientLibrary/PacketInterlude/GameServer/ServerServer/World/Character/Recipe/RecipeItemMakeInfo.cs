using System;
using UnityEngine;

public class RecipeItemMakeInfo : ServerPacket
{
    private int _recipeId;
    private int _isDwarven; // 0 = dwarven, 1 = common
    private int _currentMp;
    private int _maxMp;
    private int _status;

    // Публичные геттеры
    public int RecipeId => _recipeId;
    public bool IsDwarvenRecipe => _isDwarven == 0;
    public int CurrentMp => _currentMp;
    public int MaxMp => _maxMp;
    public int CraftStatus => _status;

    public RecipeItemMakeInfo(byte[] data) : base(data)
    {
        Parse();
    }

    public override void Parse()
    {
        try
        {
            // OpCode (0xD7) уже должен быть прочитан базовым классом
            _recipeId = ReadI();
            _isDwarven = ReadI();
            _currentMp = ReadI();
            _maxMp = ReadI();
            _status = ReadI();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[RecipeItemMakeInfo] Parse error: {ex.Message}");
            _recipeId = -1;
            _isDwarven = -1;
            _currentMp = 0;
            _maxMp = 0;
            _status = -1;
        }
    }

    public override string ToString()
    {
        return $"[RecipeItemMakeInfo] RecipeId: {_recipeId}, Type: {(IsDwarvenRecipe ? "Dwarven" : "Common")}, " +
               $"MP: {_currentMp}/{_maxMp}, Status: {_status}";
    }
}