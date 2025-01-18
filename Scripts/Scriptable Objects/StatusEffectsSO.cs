using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    [CreateAssetMenu(menuName = "Status Effects/Status Effect")]
    public class StatusEffectsSO : ScriptableObject
    {
        CharacterManager AffectedChar;

        public string StatusEffectName;
        public StatusEffectUI_Icon StatusEffectIcon;
        public string Description;

        public StatusEffectType statusEffectType;
        public StatusEffectAffectedStat affectedStat;
        [Tooltip("Value is in percentages %")]
        public float value;
        [Tooltip("Active Time is in seconds")]
        public float ActiveTime;
        public bool useTime;
        public bool Active;
        [NonSerialized] public bool IconBlink;
        public float blinkSpeed = 3f;

    }
}