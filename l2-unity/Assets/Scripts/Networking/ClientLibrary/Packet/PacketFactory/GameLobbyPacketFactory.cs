using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLobbyPacketFactory 
{
    public static ProtocolVersion CreateProtocolVersion(int version)
    {
        return new ProtocolVersion(version);
    }

    public static NewCharacter CreateNewCharacter()
    {
        return new NewCharacter();
    }

    public static CharacterCreate CreateCharacter(
        List<PlayerTemplates> playerTemplates,
        string className,
        int sex,
        int hairColor,
        int hairStyle,
        int face,
        string raceName,
        string name
    ) {
        ClassIdTemplate temp = MapClassId.GetClassIdByName(className, raceName);
        PlayerTemplates template = GetByClassId(temp.GetClassId(), playerTemplates);

        return new CharacterCreate(name, sex, hairStyle, hairColor, face, template);
    }

    public static AuthLogin CreateAuthLogin(string account, int playKey1, int playKey2, int loginKey1, int loginKey2)
    {
        return new AuthLogin(account, playKey1, playKey2, loginKey1, loginKey2);
    }

    public static CharacterSelect CharacterSelect(int slot)
    {
        return new CharacterSelect(slot);
    }

    public static CharacterDelete RequestCharacterDelete(int slot)
    {
        return new CharacterDelete(slot);
    }

    private static PlayerTemplates GetByClassId(int classId , List<PlayerTemplates> playerTemplates)
    {
       return  playerTemplates.First(x => x._classId == classId);
    }

    public static EnterWorld CreateEnterWorld()
    {
        return new EnterWorld();
    }




}
