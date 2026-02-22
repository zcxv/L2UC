using System;
using UnityEngine;

public enum PlayerModel {
    FElf = 0,
    MElf = 1,
    MDarkElf = 2,
    FDarkElf = 3,
    FDwarf = 4,
    MDwarf = 5,
    MShaman = 6,
    FShaman = 7,
    MOrc = 8,
    FOrc = 9,
    FMagic = 10,
    MMagic = 11,
    FFighter = 12,
    MFighter = 13
}

public static class CharacterRaceAnimationParser {
    public static PlayerModel ParseRace(CharacterRace race, byte sex, bool isMage) {
        switch (race) {
            case CharacterRace.Elf:
                return sex == 0 ? PlayerModel.MElf : PlayerModel.FElf;
            case CharacterRace.DarkElf:
                return sex == 0 ? PlayerModel.MDarkElf : PlayerModel.FDarkElf;
            case CharacterRace.Dwarf:
                return sex == 0 ? PlayerModel.MDwarf : PlayerModel.FDwarf;
            case CharacterRace.Orc:
                if (isMage) {
                    return sex == 0 ? PlayerModel.MShaman : PlayerModel.FShaman;
                } else {
                    return sex == 0 ? PlayerModel.MOrc : PlayerModel.FOrc;
                }
            case CharacterRace.Human:
                if (isMage) {
                    return sex == 0 ? PlayerModel.MMagic : PlayerModel.FMagic;
                } else {
                    return sex == 0 ? PlayerModel.MFighter : PlayerModel.FFighter;
                }
            default:
                return PlayerModel.FDwarf;
        }

    }


    public static PlayerModel SafeConvertToRace(int value)
    {
        try
        {
            // First convert int to byte
            byte byteValue = Convert.ToByte(value);

            // Then check if it's a valid enum value
            if (Enum.IsDefined(typeof(PlayerModel), byteValue))
            {
                return (PlayerModel)byteValue;
            }
        }
        catch (OverflowException)
        {
            Debug.LogWarning($"Value {value} is too large for PlayerModel enum");
        }

        Debug.LogWarning($"Invalid PlayerModel value: {value}");
        return PlayerModel.FElf; // or some default value
    }


    public static PlayerModel ParseRaceInterlude(CharacterRace race, int sex, int baseClass)
    {
        switch (race)
        {
            case CharacterRace.Elf:
                return sex == 0 ? PlayerModel.MElf : PlayerModel.FElf;
            case CharacterRace.DarkElf:
                return sex == 0 ? PlayerModel.MDarkElf : PlayerModel.FDarkElf;
            case CharacterRace.Dwarf:
                return sex == 0 ? PlayerModel.MDwarf : PlayerModel.FDwarf;
            case CharacterRace.Orc:
                if (baseClass == (int)BaseClass.MMagic)
                {
                    return sex == 0 ? PlayerModel.MShaman : PlayerModel.FShaman;
                }
                else
                {
                    return sex == 0 ? PlayerModel.MOrc : PlayerModel.FOrc;
                }
            case CharacterRace.Human:
                if (baseClass == (int)BaseClass.MMagic)
                {
                    return sex == 0 ? PlayerModel.MMagic : PlayerModel.FMagic;
                }
                else
                {
                    return sex == 0 ? PlayerModel.MFighter : PlayerModel.FFighter;
                }
            default:
                return PlayerModel.FDwarf;
        }
    }


}



public enum BaseClass : int
{
    Fighter = 0,
    MMagic = 10,

    //MFighter = 13
}