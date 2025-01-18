using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    [CreateAssetMenu(menuName = "Effects/Effect")]
    public class BaseEffectSO : ScriptableObject
    {

        public bool useVFX = true;
        [DrawIf("useVFX", true)] public GameObject VFX;
        
        public bool useSFX = true;
        [DrawIf("useSFX", true)] public AudioClip SFX;

        [Tooltip("Transform will be the default if both 'Transform' and 'Vector' are checked")]
        public bool useEffectTransform = true;
        public bool useVector = false;
        [DrawIf("useVector", true)] public Vector3 Location;


        public float DestroyTimer = 2f;

    }
}