using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class CameraManager : MonoBehaviour
    {
        private PlayerManager player;
        private CameraEffectManager effectManager;

        [Header("Camera references")]
        public Transform targetTransform;
        public Transform cameraPivot;
        public Transform cameraTransform;
        public LayerMask collisionLayers;
        public Transform CrossHairTarget;
        public bool StopFollowingTarget = false;
        public bool resettingCam = false;
        public bool stopRotatingCamera = false;

        [SerializeField] private Vector3 cameraFollowVelocity = Vector3.zero;
        private float defaultPosition;
        private Vector3 cameraVectorPosition = Vector3.zero;

        [Header("Camera Collisions")]
        public float cameraCollisionOffset = 0.2f;
        public float minCollisionOffset = 0.2f;
        public float cameraCollisionRadius = 0.2f;

        [Header("Camera variables")]
        public float cameraLookSpeed = 2;
        public float cameraPivotSpeed = 2;
        [SerializeField] private float groundFollowSpeed = 2f; //the smooth timer of the following
        [SerializeField] private float aerialFollowSpeed = 1f;
        public float cameraSmoothTime = 0.2f; //the smooth rotation.
        public float cameraResetTime = 0.7f;

        public float LookAngle; //camera look up and right
        public float pivotAngle; //camera look left and right

        public float minPivotAngle = -35;
        public float maxPivotAngle = 35;

        public float minCombatPivotAngle = 8;
        public float maxCombatPivotAngle = 35;

        public float lockedPivotPosition = 2.25f;
        public float unlockedPivotPosition = 1.65f;

        public float lockedPivotOffset = 2f;

        private Vector3 rotation;
        private Quaternion targetRotation;
        public static CameraManager singleton;
        private CinemachineVirtualCamera mainCam;

        [Header("Lock on")]
        public bool LockedOn = false;
        [SerializeField] List<CharacterManager> availableTargets = new List<CharacterManager>();
        public Transform currentLockOnTarget;
        public Transform nearestLockOnTarget;

        public Transform leftLockTarget;
        public Transform RightLockTarget;

        public float maxLockOnDistance;
        [SerializeField] float setCameraOffsetSpeed = 1f;
        [SerializeField] float lockOnRadius = 26;
        [SerializeField] float minViewAngle = -50;
        [SerializeField] float maxViewAngle = 50;

        private Coroutine cameraLockOnOffsetCoroutine;

        [Header("Camera Effects")]
        IEnumerator ShakeEffect = null;

        private void Awake()
        {
            singleton = this;

            mainCam = GetComponentInChildren<CinemachineVirtualCamera>();

            player = FindObjectOfType<PlayerManager>();

            effectManager = GetComponent<CameraEffectManager>();

            defaultPosition = cameraTransform.localPosition.z;

            maxLockOnDistance = player.CombatRange;

        }

        //the function that gets called every update, it calls other functions.
        public void HandleCamera()
        {
            if (!StopFollowingTarget)
            {
                FollowTarget();
            }

            if (!stopRotatingCamera)
            {
                HandleCameraRotation();
            }

            HandleCameraCollision();
        }

        //Follow's the player, changes follow speed depending on if the player is grounded or not,
        //and follows in different manners depending on the playmode.
        private void FollowTarget()
        {
            Vector3 targetPosition = Vector3.zero;
            float followSpeed = 0;

            if (player.isGrounded)
            {
                followSpeed = groundFollowSpeed;
            }
            else 
            {
                followSpeed = aerialFollowSpeed;
            }

            if (player.modeManager.currentMode == PlayMode.FreeMode ||
                player.modeManager.currentMode == PlayMode.LockOnMode)
            {
                targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, followSpeed);
            }

            transform.position = targetPosition;
        }

        //Handles the rotation of the camera, changes behaviour depending on the Playmode.
        //OTS mode rotation is for some tighter control of the camera.
        //whilst the others are a bit more loose.
        private void HandleCameraRotation()
        {
            if (player.modeManager.currentMode == PlayMode.FreeMode ||
                player.modeManager.currentMode == PlayMode.LockOnMode)
            {
                if (player.inputs.lockOnFlag == false && currentLockOnTarget == null)
                {
                    //free mode.
                    //reset camera pos & rot
                    cameraTransform.transform.localRotation = Quaternion.Euler(0, 0, 0);

                    LookAngle = LookAngle + (player.inputs.cameraHorizontalInput * cameraLookSpeed);
                    pivotAngle = pivotAngle - (player.inputs.cameraVerticalInput * cameraPivotSpeed);

                    pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

                    rotation = Vector3.zero;
                    rotation.y = LookAngle;
                    targetRotation = Quaternion.Euler(rotation);
                    transform.rotation = targetRotation;

                    rotation = Vector3.zero;
                    rotation.x = pivotAngle;
                    targetRotation = Quaternion.Euler(rotation);

                    cameraPivot.localRotation = targetRotation;
                }
                else
                {
                    //locked on mode.
                    Vector3 dir = currentLockOnTarget.position - transform.position;
                    dir.Normalize();
                    dir.y = 0;

                    Quaternion targetRot = Quaternion.LookRotation(dir);
                    //transform.rotation = targetRot;
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, cameraLookSpeed);

                    dir = currentLockOnTarget.position - cameraPivot.position;
                    dir.Normalize();

                    targetRot = Quaternion.LookRotation(dir);
                    //Vector3 eulerAngle = targetRot.eulerAngles;
                    //eulerAngle.y = 0;
                    //cameraPivot.localEulerAngles = eulerAngle;
                    cameraPivot.transform.rotation = Quaternion.Slerp(cameraPivot.rotation, targetRot, cameraLookSpeed);

                }
            }
        }

        //checks for collision of the camera so that it can move for better visuals.
        private void HandleCameraCollision()
        {
            float targetPos = defaultPosition;

            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivot.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPos), collisionLayers))
            {
                float distance = Vector3.Distance(cameraPivot.position, hit.point);
                targetPos = -(distance - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPos) < minCollisionOffset)
            {
                targetPos = targetPos - minCollisionOffset;
            }

            cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPos, 0.2f);
            cameraTransform.localPosition = cameraVectorPosition;

        }

        //checks of there are any entities that are targeteable for the lockon system.
        //puts them into an list and grabs the most close one.
        //can change lockon target by checking if there are targets right & lefts of the current one.
        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;


            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, lockOnRadius);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag.Contains("Hurtbox"))
                {
                    CharacterManager character = colliders[i].transform.root.gameObject.GetComponent<CharacterManager>();

                    if (character != null)
                    {
                        Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                        float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                        float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

                        if (character.isDead)
                            continue;

                        if (character.transform.root == targetTransform.transform.root)
                            continue;

                        if (distanceFromTarget > maxLockOnDistance)
                            continue;

                        if (viewableAngle > minViewAngle && viewableAngle < maxViewAngle &&
                            distanceFromTarget <= maxLockOnDistance)
                        {
                            if (!availableTargets.Contains(character))
                            {
                                availableTargets.Add(character);
                            }
                        }

                    }
                    else
                    {
                        Debug.Log("no chracter nearby that can be locked on");
                    }
                }
            }

            for (int k = 0; k < availableTargets.Count; k++)
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[k].transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[k].lockOnTransform;
                }

                if (player.inputs.lockOnFlag)
                {
                    Vector3 relativeEnemyPos = targetTransform.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    var distanceFromLeftTarget = targetTransform.transform.transform.position.x - availableTargets[k].transform.position.x;
                    var distanceFromRightTarget = targetTransform.transform.transform.transform.position.x + availableTargets[k].transform.position.x;

                    if (availableTargets[k] != currentLockOnTarget)
                    {
                        if (relativeEnemyPos.x > 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                        {
                            shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                            leftLockTarget = availableTargets[k].lockOnTransform;
                        }

                        if (relativeEnemyPos.x < 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                        {
                            shortestDistanceOfRightTarget = distanceFromRightTarget;
                            RightLockTarget = availableTargets[k].lockOnTransform;
                        }
                    }

                }

            }

        }

        //clears the lockon targets.
        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
            LockedOn = false;
        }

        //starts courtine for camera offset
        public void SetLockCameraOffset()
        {
            if (cameraLockOnOffsetCoroutine != null)
            {
                StopCoroutine(cameraLockOnOffsetCoroutine);
            }

            cameraLockOnOffsetCoroutine = StartCoroutine(SetCameraLockOffset());

        }

        //activates the camera shake effect
        public void EffectShake(float duration, float magnitude)
        {
            if (player.ScreenShake)
            {
                ShakeEffect = effectManager.Shake(duration, magnitude);
                StartCoroutine(ShakeEffect);
            }
        }

        //stops the camera shake effect
        public void StopEffectShake()
        {
            if (ShakeEffect != null)
            {
                StopCoroutine(ShakeEffect);
            }
        }

        //finds a new target to lock on to
        public IEnumerator WaitThenFindNewTarget()
        {
            while (player.isInteracting)
            {
                yield return null;
            }

            ClearLockOnTargets();
            HandleLockOn();

            if (nearestLockOnTarget != null)
            {
                currentLockOnTarget = nearestLockOnTarget;
                Debug.Log("current target: " + currentLockOnTarget);
                LockedOn = true;
            }

            yield return null;
        }

        //returns a boolean depending on if the target is to the camera's right or left.
        //left return 0, right returns 1.
        public bool GetCameraCross(Transform target)
        {

            Vector3 delta = (target.transform.position - cameraPivot.transform.position).normalized;
            Vector3 cross = Vector3.Cross(delta, cameraPivot.transform.forward).normalized;

            if (cross.y > 0)
            {
                //Debug.Log("target is to the left");
                return false;
            }
            else
            {
                //Debug.Log("target is to the right");
                return true;
            }
        }

        //Changes the position of the camera pivot depending on the game mode
        private IEnumerator SetCameraLockOffset()
        {
            float duration = 1;
            float timer = 0;

            if (currentLockOnTarget != null)
            {
                if (!GetCameraCross(currentLockOnTarget))
                {
                    //Debug.Log("target is to the left");
                    lockedPivotOffset = -Mathf.Abs(lockedPivotOffset);
                }
                else
                {
                    //Debug.Log("target is to the right");
                    lockedPivotOffset = Mathf.Abs(lockedPivotOffset);
                }
            }

            Vector3 velocity = Vector3.zero;
            Vector3 newLockedCameraOffset = new Vector3(lockedPivotOffset, lockedPivotPosition);
            Vector3 newUnlockedCameraOffset = new Vector3(0, unlockedPivotPosition);

            while (timer < duration)
            {
                timer += Time.deltaTime;

                if (player != null)
                {
                    if (currentLockOnTarget != null)
                    {
                        cameraPivot.transform.localPosition = Vector3.SmoothDamp(cameraPivot.transform.localPosition, newLockedCameraOffset, ref velocity, setCameraOffsetSpeed);
                    }
                    else
                    {
                        cameraPivot.transform.localPosition = Vector3.SmoothDamp(cameraPivot.transform.localPosition, newUnlockedCameraOffset, ref velocity, setCameraOffsetSpeed);
                    }

                }

                yield return null;
            }

            //failsafe, set's the position witha a snap
            if (player != null)
            {
                if (currentLockOnTarget != null)
                {
                    cameraPivot.transform.localPosition = newLockedCameraOffset;
                }
                else
                {
                    cameraPivot.transform.localPosition = newUnlockedCameraOffset;
                }
            }

            yield return null;

        }

        //returns the camera.
        public CinemachineVirtualCamera GetCamera()
        {
            return mainCam;
        }

        //calls the coroutine to reset camera rotation
        public void ResetCamera()
        {
            StartCoroutine(ResettingCamera());

            if(!resettingCam)
                StopCoroutine(ResettingCamera());
        }

        //resets the camera rotation back to zero
        private IEnumerator ResettingCamera()
        {
            Quaternion targetRot = Quaternion.Euler(0, 0, 0);
            float elapsedTime = 0;
            while (elapsedTime < cameraResetTime )
            {
                resettingCam = true;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, cameraLookSpeed);
                cameraPivot.transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, cameraLookSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            LookAngle = 0;
            pivotAngle = 0;
            transform.rotation = targetRot;
            cameraPivot.transform.rotation = targetRot;
            resettingCam = false;
            stopRotatingCamera = false;
        }

    }

}