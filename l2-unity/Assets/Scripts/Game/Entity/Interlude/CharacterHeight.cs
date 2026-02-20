using UnityEngine;

public class CharacterHeight
{
    public static float GetHeight(PlayerModel race)
    {
        if (PlayerModel.FDwarf.Equals(race) | PlayerModel.MDwarf.Equals(race))
        {
            return 0.7f;
        }
        else if (PlayerModel.FDarkElf.Equals(race))
        {
            return 0.92f;
        }
        //default
        return 0.92f;
    }
}
