using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class BossObjectSpawner : MonoBehaviour
    {
        public GameObject ObjectSpawn(GameObject obj, Vector3 pos, Quaternion rot, Transform parent = null)
        {
            GameObject SpawnObject = Instantiate(obj, pos, rot, parent);
            return SpawnObject;
        }

        public GameObject ObjectSpawn(ProjectileCollisionDetection obj, Vector3 pos, Quaternion rot, Transform parent = null)
        {
            GameObject SpawnObject = Instantiate(obj.gameObject, pos, rot, parent);
            SpawnObject.GetComponent<ProjectileCollisionDetection>().Init(DestroyProjectile);
            return SpawnObject;
        }

        private void DestroyProjectile(ProjectileCollisionDetection proj)
        {
            Destroy(proj.gameObject);
        }

    }
}