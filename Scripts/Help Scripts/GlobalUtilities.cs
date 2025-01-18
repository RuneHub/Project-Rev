using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[System.Flags]
public enum TargetTypes
{
    //ID numbers in the power of two.
    //for mutliple selection
    Player = 1,
    Enemy = 2,
    Projectile = 4
}

[System.Serializable]
[System.Flags]
public enum DamageProperties
{
    //ID numbers in the power of two.
    Normal = 1,
    Knockback = 2,
    Stun = 4,
    Launcher = 8,
    PullIn = 16,
    PushOut = 32
}

[System.Serializable]
[System.Flags]
public enum StatusEffectType
{
    Offensive = 1, 
    Defensive = 2,
    Enfeeblement = 4
}

[System.Serializable]
[System.Flags]
public enum StatusEffectAffectedStat
{
    Attack = 1, 
    Defense = 2,
    CrititicalHit = 4,
    CriticalHitRate = 8, 
    Armor = 16,
    Invinsibility = 32,
    Other = 64
}

namespace KS
{
    public static class GlobalUtils
    {
        public static float RoundTwoDP(float num)
        {
            return num = Mathf.Round(num * 100f) / 100f;
        }
    }
}

[System.Serializable]
[System.Flags]
public enum BossTeleportLocations
{
    Mechanics = 1,
    NextLocation = 2,
}