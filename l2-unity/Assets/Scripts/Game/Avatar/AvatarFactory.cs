using UnityEngine;

public static class AvatarFactory {

    private static readonly PlayerModel FALLBACK_MODEL = PlayerModel.FFighter;
    private static readonly PlayerAppearance EMPTY_APPEARANCE = new();

    public static GameObject Create(PlayerModel raceId, PlayerAppearance appearance) {
        GameObject pawnObject = CharacterFactory.Create(raceId, appearance, EntityType.Pawn) ?? 
                                CharacterFactory.Create(FALLBACK_MODEL, EMPTY_APPEARANCE, EntityType.Pawn);
        
        UserGear gear = pawnObject.GetComponent<UserGear>();
        gear.Initialize(-1, raceId);
        CharacterDefaultEquipment.EquipStarterGear(gear, appearance);
        
        return pawnObject;
    }
    
}