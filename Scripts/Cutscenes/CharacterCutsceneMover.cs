using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class CharacterCutsceneMover : MonoBehaviour
    {
        public AIBossManager boss;
        public bool moveBoss = false;
        public PlayerManager player;
        public bool movePlayer = false;
        public CameraManager gameplayCam;
        public bool StopFollowingTarget;

        public Transform targetLocationBoss;
        public Transform targetLocationPlayer;

        private void Awake()
        {
            
        }

        private void Start()
        {
            
        }

        public void MoveToTarget()
        {
            if (boss != null && targetLocationBoss != null)
            {
                if (moveBoss)
                {
                    boss.transform.position = targetLocationBoss.position;
                }
            }
            if (player != null && targetLocationPlayer != null && gameplayCam != null)
            {
                if (movePlayer)
                {
                    player.transform.position = targetLocationPlayer.position;
                    player.transform.rotation = targetLocationPlayer.rotation;
                    gameplayCam.StopFollowingTarget = StopFollowingTarget;
                    gameplayCam.transform.rotation = targetLocationPlayer.rotation;
                }
            }
        }

    }
}