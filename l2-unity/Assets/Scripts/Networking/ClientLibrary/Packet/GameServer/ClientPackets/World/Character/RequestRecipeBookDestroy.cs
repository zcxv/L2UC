using UnityEngine;

public class RequestRecipeBookDestroy : ClientPacket
{
    public RequestRecipeBookDestroy(int recipeId) : base((byte)GameClientPacketType.RequestRecipeBookDestroy)
    {
        WriteI(recipeId);
        BuildPacket();
    }
}
