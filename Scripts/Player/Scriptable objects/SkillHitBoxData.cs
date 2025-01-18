using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{[CreateAssetMenu(menuName = "Player/Skills/Attack Skill Hitbox data")]
    public class SkillHitBoxData : ScriptableObject
    {
        public BaseDamageCollider hitBox;
        public float hitboxKillTime;

        public Vector3 placement;

        public AudioClip ReleaseSFX;

        public bool useScreenShake;
        public float shakeDuration;
        public float shakeMagnitude;
    }
}