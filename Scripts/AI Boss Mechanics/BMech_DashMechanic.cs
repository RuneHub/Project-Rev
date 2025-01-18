using kS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KS
{
    public class BMech_DashMechanic : BMech_Base
    {

        [Space(10)]
        public float DashAmount;
        public float DashAngle = 35f;
        public float DashAngle2ndF = 5f; //dash angle from 2nd forward

        [Space(10)]
        public float shootHeight = 1f;
        public float spawnTimer;
        public float stopDistance = 2f;

        public AIBoss_IndicatorCast dashCast;
        [SerializeField] private Vector3 hitboxScale;

        [SerializeField] private Transform startPosition;
        [SerializeField] private LayerMask hitLayer;

        [Space(10)]
        public Vector3 LastPosition;

        [Space(10)]
        public List<Vector3> DashRoutes = new List<Vector3>();
        public List<RouteData> Routes = new List<RouteData>();

        private PlayerManager player;

        public struct RouteData
        {
            public Vector3 startPos;
            public Vector3 endPos;
            public float distance; //processed distance
            public float uDistance; //unprocessed distance
            public Quaternion direction;
        }

        private void Awake()
        {
            boss = FindAnyObjectByType<AIBossManager>();
            player = FindAnyObjectByType<PlayerManager>();
        }

        [ContextMenu("PlayMechanic")]
        public override void PlayMechanic()
        {
            base.PlayMechanic();

            Debug.DrawLine(startPosition.position, transform.forward * Mathf.Infinity, Color.cyan, 10);

            EstablishRoute();

            StartCoroutine(ExecuteMechanicSequencer());
        }

        public override void StopMechanic()
        {
            StopCoroutine(ExecuteMechanicSequencer());
            FinishUpMechanic();
            Debug.Log("Stopped!");
        }

        IEnumerator ExecuteMechanicSequencer()
        {
            MechanicPlaying = true;

            for (int i = 0; i < Routes.Count; i++)
            {
                Vector3 pos = Routes[i].startPos;
                pos.y -= shootHeight;
                AIBoss_IndicatorCast dash = Instantiate(dashCast, pos, Routes[i].direction);

                hitboxScale.z = Routes[i].uDistance * 10;
                dash.Cast.transform.GetChild(0).transform.localScale = hitboxScale;
                /**/
                dash.SetVFXFloat(dash.Indicator, "Length", Routes[i].distance);
                dash.SetVFXFloat(dash.Cast, "Length", Routes[i].distance);

                yield return new WaitForSeconds(spawnTimer);
            }

            yield return new WaitForSeconds(spawnTimer * (DashAmount+1));
            FinishUpMechanic();
            Debug.Log("Completed!");
        }

        private void EstablishRoute()
        {
            bool firstRoute = true;
            Vector3 startPos = startPosition.position + new Vector3(0, shootHeight,0);
            DashRoutes.Add(startPos);
            Vector3 RouteDir = startPosition.forward;
            for (int i = 0; i < DashAmount; i++)
            {

                if (!firstRoute) 
                {
                    int prev = DashRoutes.Count - 1;
                    startPos = DashRoutes[prev];
                    DashAngle = DashAngle2ndF;
                }

                float angle = Random.Range(-DashAngle, DashAngle);
                Vector3 direction = Quaternion.Euler(0, angle, 0) * RouteDir;

                RaycastHit hit;
                if (Physics.Raycast(startPos, direction, out hit, Mathf.Infinity, hitLayer))
                {
                    Debug.DrawLine(startPos, hit.point, Color.cyan, 10);

                    Vector3 end = hit.point;
                    //Debug.Log("Hit: " + hit.transform.name);

                    //Debug.Log("Hit: " + hit.transform.name);
                    RouteDir = player.transform.position - hit.point;
                    RouteDir.y = 0;

                    DashRoutes.Add(end);
                    RouteData data = new RouteData();
                    data.startPos = startPos;
                    data.endPos = hit.point;

                    float distance = Vector3.Distance(startPos, hit.point);

                    if (i == DashAmount)
                    {
                        distance -= stopDistance;
                    }

                    data.uDistance = distance;
                    //Debug.Log("distance: " + distance);
                    distance = distance / dashCast.GetVFXFloat(dashCast.Indicator, "Size");
                    //Debug.Log("prosessed distance: " + distance);
                    data.distance = distance;

                    data.direction = Quaternion.LookRotation(hit.point - startPos);

                    Routes.Add(data);

                    firstRoute = false;
                }
                else
                {
                    return;
                }
            }
        }

        private void FinishUpMechanic()
        {
            if (Routes.Count > 0)
            {
                int lastIndex = Routes.Count - 1;
                //LastPosition = Routes[lastIndex].endPos;

                Vector3 middle = Routes[lastIndex].endPos + (Routes[lastIndex].startPos - Routes[lastIndex].endPos) / 2;
                LastPosition = Routes[lastIndex].endPos + (middle - Routes[lastIndex].endPos) / 2;

            }
            else
            {
                LastPosition = boss.transform.position;
            }
            MechanicPlaying = false;
            boss.ActiveMechanic = false;
            DashRoutes.Clear();
            Routes.Clear();

            boss.combatManager.HandleDashAttackAftermath(LastPosition);
        }

    }

}