using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class AICombatManager : MonoBehaviour
    {
        private AIManager manager;

        public BaseDamageCollider hitbox;
        public float bulletSpeed = 20f;
        public float bulletAtkPow = 1f;
        [Space]
        public float ChargePwr = 20f;

        private Vector3 Barrel = Vector3.zero;

        private void Awake()
        {
            manager = GetComponent<AIManager>();
        }

        //temp function 
        public void ShootHitBox()
        {
            SetUp();

            BaseDamageCollider bullet = Instantiate(hitbox, Barrel, Quaternion.identity, transform);
            bullet.transform.position = Barrel;
            bullet.GetComponent<Rigidbody>().velocity = transform.forward.normalized * bulletSpeed;
            bullet.GetComponent<BaseDamageCollider>().Init(DestroyHitbox, manager, bulletAtkPow);

        }

        public void ChargeHitBox()
        {
            SetUp();

            transform.LookAt(manager.GetTarget().transform.position);

            //move forward for x seconds.
            StartCoroutine(Charge());
            //instantiate hitbox
            BaseDamageCollider bullet = Instantiate(hitbox, Barrel, Quaternion.identity, transform);
            bullet.transform.position = Barrel;
            bullet.GetComponent<Rigidbody>().velocity = transform.forward.normalized * ChargePwr;
            bullet.GetComponent<BaseDamageCollider>().Init(DestroyHitbox, manager, bulletAtkPow);


        }

        public IEnumerator Charge()
        {
            manager.rb.velocity = transform.forward * ChargePwr;
            yield return new WaitForSeconds(.5f);

            manager.rb.velocity = Vector3.zero;
        }

        //temp function
        private void SetUp()
        {
            Barrel = new Vector3(transform.position.x,
                                             transform.position.y + 1f,
                                             transform.position.z - .75f);
        }

        //temp function
        private void DestroyHitbox(BaseDamageCollider obj)
        {
            Destroy(obj.gameObject);
        }


    }
}