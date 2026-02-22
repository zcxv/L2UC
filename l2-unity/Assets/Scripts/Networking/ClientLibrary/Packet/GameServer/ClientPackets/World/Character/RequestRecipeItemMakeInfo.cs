using UnityEngine;

public class RequestRecipeItemMakeInfo : ClientPacket
{
    public RequestRecipeItemMakeInfo(int recipeId) : base((byte)GameClientPacketType.RequestRecipeItemMakeInfo)
    {
        WriteI(recipeId);
        BuildPacket();
    }
}
