using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{

    [CreateAssetMenu(menuName = "Boss/Melee/Dash")]
    public class BossDashAttackSO : BossBaseSO
    {
        public string ID;
        public GameObject CastingVFX;
    }
}