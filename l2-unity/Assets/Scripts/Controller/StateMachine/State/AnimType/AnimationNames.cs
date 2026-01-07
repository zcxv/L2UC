using System;
using System.Collections.Generic;

public class Animation
{
    public string Value { get; set; }
    public TypesAnimation Type { get; set; }
    public Animation(string value , TypesAnimation type)
    {
        Value = value;
        Type = type;
    }

    public string Concat(string name)
    {
        return Value + name;
    }

    public void MergeStrings(string name)
    {
        Value = Value + name;
    }

    public override string ToString()
    {
        return Value;
    }

    public bool AreAnimationsEqual(Animation first, Animation second)
    {
        if (ReferenceEquals(first, second))
            return true;

        if (first is null || second is null)
            return false;

        return first.Value == second.Value;
    }

}

public static class AnimationNames
{
    public static readonly Animation ATK_WAIT = new Animation("atkwait_" , TypesAnimation.OtherType);
    public static readonly Animation WAIT = new Animation("wait_", TypesAnimation.OtherType);
    public static readonly Animation WALK = new Animation("walk_", TypesAnimation.OtherType);
    public static readonly Animation RUN = new Animation("run_", TypesAnimation.OtherType);
    public static readonly Animation DEAD = new Animation("death", TypesAnimation.OtherType);
    public static readonly Animation REBIRTH = new Animation("rebirth", TypesAnimation.OtherType);
    public static readonly Animation ATK01 = new Animation("jatk01_", TypesAnimation.MeleeAttack);
    public static readonly Animation ATK02 = new Animation("jatk02_", TypesAnimation.MeleeAttack);
    public static readonly Animation ATK03 = new Animation("jatk03_", TypesAnimation.MeleeAttack);

    public static readonly Animation MONSTER_WAIT = new Animation("wait", TypesAnimation.OtherType);
    public static readonly Animation MONSTER_WALK = new Animation("walk", TypesAnimation.OtherType);
    public static readonly Animation MONSTER_RUN = new Animation("run", TypesAnimation.OtherType);
    public static readonly Animation MONSTER_ATK01 = new Animation("atk01", TypesAnimation.OtherType);
    public static readonly Animation MONSTER_DamageAction = new Animation("damageaction", TypesAnimation.OtherType);
    public static readonly Animation MONSTER_DamageAction1 = new Animation("damageaction1", TypesAnimation.OtherType);
}

public enum TypesAnimation
{
    MeleeAttack,
    BowAttack,
    OtherType
}

public static class SpecialAnimationNames
{
    public static readonly Animation ATK_BOW_ATK1 = new Animation("jatk01_bow", TypesAnimation.BowAttack);
    public static readonly Animation ATK_BOW_ATK2 = new Animation("jatk02_bow", TypesAnimation.BowAttack);
    public static readonly Animation ATK_BOW_ATK3 = new Animation("jatk03_bow", TypesAnimation.BowAttack);
    public static readonly Animation ATK01_1HS = new Animation("jatk01_1HS", TypesAnimation.MeleeAttack);

    public static readonly Animation[]  arrayAtkSpecials = new Animation[]{ ATK_BOW_ATK1 , ATK_BOW_ATK2 , ATK_BOW_ATK3 , ATK01_1HS };


    public static Animation[] GetSpecialsAttackAnimations()
    {
            return arrayAtkSpecials;
    }

}



