using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace KS
{
    [CreateAssetMenu(menuName = "Player/Passive/Passive Text")]
    public class PassiveAbilityText : ScriptableObject
    {
        public string passiveName;
        [TextArea] public string passiveDescription;
    }
}