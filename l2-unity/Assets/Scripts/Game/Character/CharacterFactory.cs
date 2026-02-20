using UnityEngine;

public static class CharacterFactory {
    
    public static GameObject Create(PlayerModel model, PlayerAppearance appearance, EntityType entityType) {
        GameObject prototype = ModelTable.Instance.GetContainer(model, entityType);
        if (prototype == null) {
            // m0nster: временная заглушка, пока не реализованы все персонажи
            return null;
        }
        
        GameObject entity = Object.Instantiate(prototype);
        Transform container = entityType != EntityType.Player ? 
            entity.transform.GetChild(0) : 
            entity.transform;
        GameObject face = Object.Instantiate(ModelTable.Instance.GetFace(model, appearance.Face), container, false);
        GameObject hair1 = Object.Instantiate(ModelTable.Instance.GetHair(model, appearance.HairStyle, appearance.HairColor, false), container, false);
        GameObject hair2 = Object.Instantiate(ModelTable.Instance.GetHair(model, appearance.HairStyle, appearance.HairColor, true), container, false);

        UserGear gear = entity.GetComponent<UserGear>();
        gear.AddUserGearLink(face, hair1, hair2);

        return entity;
    }
 
}
