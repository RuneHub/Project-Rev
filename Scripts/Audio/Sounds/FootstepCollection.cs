using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    [CreateAssetMenu(fileName = "New Footstep Collection", menuName = "Sounds/Create new FootStep Collection")]
    public class FootstepCollection : ScriptableObject
    {
        public List<AudioClip> footstepSounds = new List<AudioClip>();
        public AudioClip jumpSound;
        public AudioClip landSound;

    }
}