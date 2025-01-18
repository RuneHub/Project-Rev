using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class BMech_Base : MonoBehaviour
    {
        protected AIBossManager boss;

        public enum MechanicType { Sequencer, Instant, Random }

        public string ID;
        public bool MechanicPlaying;

        //starts the mechanic coroutine.
        public virtual void PlayMechanic()
        {
            Debug.Log("play mech");
        }

        //stops the mechanic coroutine.
        public virtual void StopMechanic()
        {
        }

    }

}