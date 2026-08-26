using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class PlayerColliderProperties : MonoBehaviour
    {
        private PlayerManager player;

        private BaseDamageCollider hitbox;

        public void Init(PlayerManager player)
        {
            this.player = player;
            hitbox = GetComponent<BaseDamageCollider>();
        }

        private void Update()
        {
            if (hitbox.collided)
            {
                player.UltManager.ChangeBarAmount(PlayerUltimateAttackManager.BarSource.SkillAttack);
            }
        }

    }
}