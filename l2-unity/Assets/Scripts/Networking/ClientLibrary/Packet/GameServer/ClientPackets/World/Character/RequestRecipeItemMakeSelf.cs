using UnityEngine;

public class RequestRecipeItemMakeSelf : ClientPacket
{
    public RequestRecipeItemMakeSelf(int recipeId) : base((byte)GameClientPacketType.RequestRecipeItemMakeSelf)
    {
        WriteI(recipeId);
        BuildPacket();
    }
}
