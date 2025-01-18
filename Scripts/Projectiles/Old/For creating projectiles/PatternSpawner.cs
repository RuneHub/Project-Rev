using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace KS
{
    public enum Pattern {  Circle, Square }
    public class PatternSpawner : MonoBehaviour
    {
        public ProjectileCollisionDetection spawnPrefab;
        public Pattern spawnPattern = Pattern.Circle;

        public int spawnCount;
        public float radius;

        public float moveSpeed;

        public bool usePool = true;

        private List<Transform> markers = new List<Transform>();

        private GameObject poolParent;

        private ObjectPool<ProjectileCollisionDetection> _pool;

        private void Start()
        {
            _pool = new ObjectPool<ProjectileCollisionDetection>(
                () =>
                {
                    return Instantiate(spawnPrefab);
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
                (spawnCount * 3),
                (spawnCount * 10)
                );
        }

        public void CreatePattern(Pattern p)
        {
            if (poolParent == null)
            {
                poolParent = new GameObject();
                poolParent.name = spawnPrefab.name + "_poolParent";
                poolParent.gameObject.tag = transform.root.gameObject.tag;
            }

            switch (p)
            {
                case Pattern.Circle:
                    CreateCirclePattern();
                    break;
                case Pattern.Square:
                    CreateSquarePattern();
                    break;
                default:
                    CreateCirclePattern();
                    break;
            }
        }

        public void CreateRandomPattern()
        {
            if (poolParent == null)
            {
                poolParent = new GameObject();
                poolParent.name = spawnPrefab.name + "_poolParent";
                poolParent.gameObject.tag = transform.root.gameObject.tag;
            }

            float randomNum = Random.Range(1, 3);

            switch (randomNum)
            {
                case 1:
                    CreateCirclePattern();
                    break;
                case 2:
                    CreateSquarePattern();
                    break;
                default:
                    CreateCirclePattern();
                    break;
            }

           
        }

        private void CreateCirclePattern()
        {
            Quaternion quat = Quaternion.AngleAxis(360f / spawnCount, transform.forward);
            Vector3 vec = transform.up * radius;

            for (int i = 0; i < spawnCount; i++)
            {
                if (usePool)
                {
                    markers.Add(_pool.Get().transform);
                }
                else
                {
                    markers.Add(Instantiate(spawnPrefab.transform, Vector3.zero, Quaternion.identity) as Transform);
                }
            }

            for (int i = 0; i < spawnCount; ++i)
            {
                markers[i].transform.parent = poolParent.transform;

                markers[i].GetComponent<ProjectileCollisionDetection>().Init(DestroyProjectile);
                markers[i].position = transform.position + vec;
                markers[i].rotation = transform.rotation;
                vec = quat * vec;

                if (markers[i].GetComponent<Rigidbody>() == null)
                {
                    markers[i].gameObject.AddComponent<Rigidbody>();
                }

                Rigidbody rb = markers[i].gameObject.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.velocity += transform.forward * moveSpeed * Time.deltaTime;

            }

            markers.Clear();
        }

        private void CreateSquarePattern()
        {
            Quaternion facingRot = Quaternion.Euler(transform.eulerAngles);
            float sqrt = Mathf.Sqrt(spawnCount);

            for (int i = 0; i < spawnCount; i++)
            {
                if (usePool)
                {
                    markers.Add(_pool.Get().transform);
                }
                else
                {
                    markers.Add(Instantiate(spawnPrefab.transform, Vector3.zero, Quaternion.identity) as Transform);
                }
            }

            for (int i = 0; i < spawnCount; i++)
            {
                float posY = (i % (int)sqrt) * radius;
                float posX = (i / (int)sqrt) * radius;
                Vector3 pos = new Vector3(posX, posY, 0);

                Vector3 middlePos = GetGridMiddlePos(sqrt);
                pos += middlePos;

                Vector3 facingPos = facingRot * pos;

                markers[i].transform.parent = poolParent.transform;
                markers[i].GetComponent<ProjectileCollisionDetection>().Init(DestroyProjectile);
                
                if (markers[i].GetComponent<Rigidbody>() == null)
                {
                    markers[i].gameObject.AddComponent<Rigidbody>();
                }

                markers[i].position = transform.position + facingPos;
                markers[i].rotation = transform.rotation;

                Rigidbody rb = markers[i].gameObject.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.velocity += transform.forward * moveSpeed * Time.deltaTime;
            }

            markers.Clear();
        }

        private Vector3 GetGridMiddlePos(float size)
        {
            float middle = -(size - 1) / 2f;
            Vector3 middlePos = new Vector3(middle, middle);
            return middlePos;
        }

        private void DestroyProjectile(ProjectileCollisionDetection proj)
        {
            if (usePool)
            {
                _pool.Release(proj);
            }
            else
            {
                Destroy(proj.gameObject);
            }
        }

    }
}