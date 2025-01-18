using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    [CreateAssetMenu(menuName = "Boss/Locomotion/Movement")]
    public class BossMovementSO : ScriptableObject
    {
        public AnimationClip startupAnim;
        public AnimationClip LoopingAnim;
        public AnimationClip endAnim;
        public AudioClip SFX;
        public GameObject StartUpVFX;
        public GameObject EndVFX;
        public float timer;
        public float VFXDuration;

    }
}