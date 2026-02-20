using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterAccessType
{
    private static readonly Dictionary<GameServerPacketType, bool> GamePacketTypes = new Dictionary<GameServerPacketType, bool>
    {
        { GameServerPacketType.InterludeKeyPacket, true },
        { GameServerPacketType.CharSelectionInfo, true },
        { GameServerPacketType.CharTemplate, true },
        { GameServerPacketType.CharCreateOk, true },
        { GameServerPacketType.SocialAction, true },
        { GameServerPacketType.CharSelected, true },
        { GameServerPacketType.CharCreateFail, true },
        { GameServerPacketType.SkillList, true },
        { GameServerPacketType.UserInfo, true },
        { GameServerPacketType.SkillCoolTime, true },
        { GameServerPacketType.MacroList, true },
        { GameServerPacketType.NpcHtmlMessage, true },
        { GameServerPacketType.PackageToList, true },
        { GameServerPacketType.TutorialShowHtml, true },
        { GameServerPacketType.BuyList, true },
        { GameServerPacketType.ShopPreviewList, true },
        { GameServerPacketType.ShopPreviewInfo, true },
        { GameServerPacketType.MultiSellList, true },
        { GameServerPacketType.SellList, true },
        { GameServerPacketType.WhDepositList, true },
        { GameServerPacketType.PackageSendableList, true },
        { GameServerPacketType.WhWithdrawList, true },
        { GameServerPacketType.BuyListSeed, true },
        { GameServerPacketType.ShortCutInit, true },
        { GameServerPacketType.ShortCutRegister, true },
        { GameServerPacketType.ShortCutDel, true },
        { GameServerPacketType.HennaInfo, true },
        { GameServerPacketType.QuestList, true },
        { GameServerPacketType.PledgeShowMemberListUpdate, true },
        { GameServerPacketType.PledgeShowMemberListDelete, true },
        { GameServerPacketType.PledgeShowMemberListAll, true },
        { GameServerPacketType.PledgeShowMemberListDeleteAll, true },
        { GameServerPacketType.PledgeShowMemberListAdd, true },
        { GameServerPacketType.PledgeInfo, true },
        { GameServerPacketType.ManagePledgePower, true },
        { GameServerPacketType.PledgeStatusChanged, true },
        { GameServerPacketType.NpcInfo, true },
        { GameServerPacketType.DeleteObject, true },
        { GameServerPacketType.CharMoveToLocation, true },
        { GameServerPacketType.ValidateLocation, true },
        { GameServerPacketType.FriendList, true },
        { GameServerPacketType.EtcStatusUpdate, true },
        { GameServerPacketType.ExTypePacket, true },
        { GameServerPacketType.StatusUpdate, true },
        { GameServerPacketType.TargetUnselected, true },
        { GameServerPacketType.TeleportToLocation, true },
        { GameServerPacketType.Revive, true },
        { GameServerPacketType.AbnormalStatusUpdate, true },
        { GameServerPacketType.ShortBuffStatusUpdate, true },
        { GameServerPacketType.AcquireSkillList, true },
        { GameServerPacketType.AcquireSkillInfo, true },
        { GameServerPacketType.RecipeBookItemList, true },
        { GameServerPacketType.RecipeItemMakeInfo, true },
        { GameServerPacketType.SendTradeRequest, true },
        { GameServerPacketType.AskJoinParty, true },
        { GameServerPacketType.TradeStart, true },
        { GameServerPacketType.TradeUpdate, true },
        { GameServerPacketType.TradeDone, true },
        { GameServerPacketType.TradeOwnAdd, true },
        { GameServerPacketType.TradeOtherAdd, true },
        { GameServerPacketType.TradePressOtherOk, true },
        { GameServerPacketType.TradePressOwnOk, true },
        { GameServerPacketType.CharInfo, true },
        { GameServerPacketType.DropItem, true },
        { GameServerPacketType.GetItem, true },

    };

    private static readonly Dictionary<GSCombatPacketType, bool> CombatPacketTypes = new Dictionary<GSCombatPacketType, bool>
    {
        { GSCombatPacketType.MyTargetSelected, true },
        { GSCombatPacketType.MoveToPawn, true },
        { GSCombatPacketType.STOP_MOVE, true },
        { GSCombatPacketType.ATTACK, true },
        { GSCombatPacketType.ActionFailed, true },
        { GSCombatPacketType.AUTO_ATTACK_START, true },
        { GSCombatPacketType.AUTO_ATTACK_STOP, true },
        { GSCombatPacketType.DIE, true },
        { GSCombatPacketType.MagicSkillUse, true },
        { GSCombatPacketType.SetupGauge, true },
        { GSCombatPacketType.ItemList, true },
        { GSCombatPacketType.InventoryUpdate, true },
        { GSCombatPacketType.ChooseInventoryItem, true },
        { GSCombatPacketType.EnchantResult, true },
        { GSCombatPacketType.MagicSkillLaunched, true }

    };

    private static readonly Dictionary<GSMessagePacketType, bool> MessagePacketTypes = new Dictionary<GSMessagePacketType, bool>
    {
        { GSMessagePacketType.SystemMessage, true },
        { GSMessagePacketType.CreatureSay, true },
        { GSMessagePacketType.NpcSay, true }
    };

    public static bool IsAccessTypeGame(ItemServer item)
    {
        return GamePacketTypes.ContainsKey(item.PaketType());
    }

    public static bool IsAccessTypeCombat(ItemServer item)
    {
        GSCombatPacketType type;
        if (Enum.TryParse(item.ByteType().ToString(), out type))
        {
            return CombatPacketTypes.ContainsKey(type);
        }
        return false;
    }

    public static bool IsAccessTypeMessage(ItemServer item)
    {
        GSMessagePacketType type;
        if (Enum.TryParse(item.ByteType().ToString(), out type))
        {
            return MessagePacketTypes.ContainsKey(type);
        }
        return false;
    }
}
