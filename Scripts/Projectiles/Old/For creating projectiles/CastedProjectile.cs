using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{ 
    public class CastedProjectile : MonoBehaviour
    {
        public float playTime = 2f;
        public VFXBase indicator;
        public float indicatorTimer;
        public VFXBase vfxAttack; //Change this later to prefab which contains the real tornado + hitbox.
        public float attackTimer;
        
        // Start is called before the first frame update
        void Start()
        {
            if (indicator != null && indicatorTimer != 0)
            {
                indicator.SetDuration(indicatorTimer);
            }

            if (vfxAttack != null && attackTimer != 0)
            {
                vfxAttack.SetDuration(attackTimer);
            }
            StartCoroutine(Projectile());
        }

        IEnumerator Projectile()
        {
            VFXBase aoe = Instantiate(indicator, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(indicatorTimer);
            Destroy(aoe.gameObject, playTime);

            VFXBase attack = Instantiate(vfxAttack, transform.position, Quaternion.identity, transform);
            if (attack.GetComponent<ProjectileCollisionDetection>() != null || (attack.GetComponentInChildren<ProjectileCollisionDetection>() != null))
            {
                attack.GetComponentInChildren<ProjectileCollisionDetection>().lastingTime = (attackTimer + playTime);
                attack.GetComponentInChildren<ProjectileCollisionDetection>().Init(DestroyProjectile);
            }
            else if (attack.GetComponent<ProjectileCollisionDetection>() != null)
            {
                attack.GetComponent<ProjectileCollisionDetection>().lastingTime = (attackTimer + playTime);
                attack.GetComponent<ProjectileCollisionDetection>().Init(DestroyProjectile);
            }
        }

        private void DestroyProjectile(ProjectileCollisionDetection proj)
        {
            Destroy(proj.transform.root.gameObject);
        }

    }
}
