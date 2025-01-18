using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    [CreateAssetMenu(menuName = "Player/Attacks/Basic Attack")]
    public class AttackStandardSO : ScriptableObject
    {
        public AnimationClip attackAnim;
        public GameObject FX_FakeProjectile; //this will later be change when projectile scripts are redone.
        public GameObject FX_Muzzleflash;
        public GameObject FX_Impact;

        public AudioClip ReleaseSFX;

        public float rawDamage = 1f;

        public bool useScreenShake;
        public float shakeDuration;
        public float shakeMagnitude;
    }
}