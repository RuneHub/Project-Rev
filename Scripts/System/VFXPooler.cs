using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

namespace KS
{
    public class VFXPooler : MonoBehaviour
    {

        public VFXBase VFXPrefab;
        public int defaultCapacity;
        public int maxCapacity;

        public ObjectPool<VFXBase> _pool;

        private GameObject poolParent;

        public void Init()
        {
            _pool = new ObjectPool<VFXBase>(
                () =>
                {
                    return Instantiate(VFXPrefab);    
                },
                prefab =>
                {
                    prefab.gameObject.SetActive(true);
                },
                prefab =>
                {
                    prefab.gameObject.SetActive(false);
                },
                prefab =>
                {
                    Destroy(prefab.gameObject);
                },
                false,
                defaultCapacity,
                maxCapacity
                );

            if (poolParent == null)
            {
                poolParent = new GameObject();
                poolParent.name = VFXPrefab.name + "_poolParent";
                poolParent.gameObject.tag = transform.root.gameObject.tag;
            }

        }

        public VFXBase Get()
        {
            return _pool.Get();
        }

        public void Release(VFXBase obj)
        {
            _pool.Release(obj);
        }
        
    }
}