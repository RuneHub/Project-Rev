using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    [CreateAssetMenu(menuName = "Boss/Magic/Cast")]
    public class BossMagicCastsMechanicSO : BossBaseSO
    {
        public string ID;
        public float CastTime;

        public bool fastCast = false;

        [DrawIf("fastCast", true)] public GameObject buildUp;
        [DrawIf("fastCast", true)] public GameObject magicCast;


    }
}