using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterRace : byte { 
    Human = 0,
    Elf = 1,
    DarkElf = 2,
    Orc = 3,
    Dwarf = 4,
    Monster = 5
}

public static class CharacterRaceParser {
    public static CharacterRace ParseRace(PlayerModel race) {
        switch (race) {
            case PlayerModel.MDwarf:
            case PlayerModel.FDwarf:
                return CharacterRace.Dwarf;
            case PlayerModel.FDarkElf:
            case PlayerModel.MDarkElf:
                return CharacterRace.DarkElf;
            case PlayerModel.MElf:
            case PlayerModel.FElf:
                return CharacterRace.Elf;
            case PlayerModel.MShaman:
            case PlayerModel.FShaman:
            case PlayerModel.MOrc:
            case PlayerModel.FOrc:
                return CharacterRace.Orc;
            case PlayerModel.MFighter:
            case PlayerModel.FFighter:
            case PlayerModel.MMagic:
            case PlayerModel.FMagic:
                return CharacterRace.Human;
            default:
                return CharacterRace.Dwarf;
        }
    }
}
