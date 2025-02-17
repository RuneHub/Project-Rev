using UnityEngine;

namespace KS
{
    public enum InputDirections { Zero, None, North, NorthEast, NorthWest, East, West, South, SouthEast, SouthWest }

    public class PlayerInputManager : MonoBehaviour
    {
        //this script is for player only.

        //refences
        PlayerControl controls;
        PlayerManager player;

        [Header("Movement Numericals")]
        public Vector2 movementInput;
        public float moveAmount;
        public float verticalInput;
        public float horizontalInput;
        public float lastVerticalInput;
        public float lastHorizontalInput;
        public string directionSTR = "none";

        [Header("movement Inputs")]
        public bool sprintInput;
        public bool jumpInput;
        public bool dodgeInput;

        [Header("Combat Inputs")]
        public bool basicAttackInput;
        public bool uniqueAttackInput;
        public bool SkillSetOpenInput;

        public bool SkillNorthInput;
        public bool SkillSouthInput;
        public bool SkillWestInput;
        public bool SkillEastInput;

        [Header("Healing Inputs")]
        public bool smallHealInput;
        public bool largeHealInput;

        [Header("Action Inputs")]
        public bool interactInput;

        [Header("Control inputs")]
        public bool lockOnInput;
        public bool lockOnLeftInput;
        public bool lockOnRightInput;

        public float lockonVerticalInput;
        public float lockonHorizontalInput;
        public bool lockonScrollInput;
        public Vector2 camDirectionalInput;

        private Coroutine lockOnCoroutine;

        [Header("Camera inputs")]
        public Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;

        [Header("Flags")]
        public bool lockOnFlag;
        public bool jumpFlag;
        public bool DashFlag;
        public bool dodgeFlag;
        public bool comboFlag;
        public bool perfectFlag;
        public bool UniqueFlag;

        [SerializeField]
        private InputDirections inputDir = InputDirections.None;

        [Header("Qued Inputs")]
        [SerializeField] private bool InputQueActive = false;
        [SerializeField] float queInputTimer = 0;
        [SerializeField] float defaultQueTime = 0.35f; 
        [SerializeField] bool quedAttackInput;
        [SerializeField] bool quedJumpInput;
        [SerializeField] bool quedUniqueInput;
        [SerializeField] bool quedSkillNInput;
        [SerializeField] bool quedSkillSInput;
        [SerializeField] bool quedSkillWInput;
        [SerializeField] bool quedSkillEInput;

        [Header("UI inputs")]
        [SerializeField] bool uiOpenMenu = false;
        [SerializeField] bool uiConfirm = false;
        [SerializeField] bool uiReturn = false;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        //enable the inputs so they can be read by the scripts.
        private void OnEnable()
        {
            if (controls == null)
            {
                controls = new PlayerControl();

                //movement
                controls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

                //a hold input
                controls.PlayerMovement.Sprint.performed += i => sprintInput = true;
                controls.PlayerMovement.Sprint.canceled += i => sprintInput = false;

                controls.PlayerAction.Unique.performed += i => uniqueAttackInput = true;
                controls.PlayerAction.Unique.canceled += i => uniqueAttackInput = false;

                //actions
                controls.PlayerAction.Shoot.performed += i => basicAttackInput = true;
                controls.PlayerAction.Interact.performed += i => interactInput = true;
                    
                //these actions get canceled by the animation controller, so no canceled statements
                controls.PlayerAction.Jump.performed += i => jumpInput = true;
                controls.PlayerAction.Dodge.performed += i => dodgeInput = true;

                controls.PlayerAction.HealSmall.performed += i => smallHealInput = true;
                controls.PlayerAction.HealSmall.canceled += i => smallHealInput = false;
                controls.PlayerAction.HealLarge.performed += i => largeHealInput = true;
                controls.PlayerAction.HealLarge.canceled += i => largeHealInput = false;

                //control
                controls.Control.LockOn.performed += i => lockOnInput = true;
                controls.Control.LockOnRight.performed += i => lockOnRightInput = true;
                controls.Control.LockOnLeft.performed += i => lockOnLeftInput = true;

                //hold input for the skills
                controls.Control.Skill.performed += i => SkillSetOpenInput = true;
                controls.Control.Skill.canceled += i => SkillSetOpenInput = false;


                //Camera
                controls.Camera.Rotation.performed += i => cameraInput = i.ReadValue<Vector2>();

                //UI
                controls.Control.OpenMainMenu.performed += i => uiOpenMenu = true;
                controls.UI.ResumeTime.performed += i => uiConfirm = true; //needs to be set to confirm when done with debugging menu's
                controls.UI.Return.performed += i => uiReturn = true;

            }

            controls.Enable(); 
            DisableGameplayInput();
        }

        //disable the controls.
        private void OnDisable()
        {
            controls.Disable();
        }

        //public, the function that gets called on Update(), so it checks the private input funcionts.
        public void HandleAllInputs()
        {
           
            HandleCameraInput();

            HandleSkillModifierInput();

            CheckQuedInputs();

            HandleMovementInput();
            HandleSprintInput();
            HandleJumpInput();

            HandleDodgeInput();

            HandleBasicAttackInput();
            HandleSkillInput();
            HandleUniqueAbilityInput();

            HandleSmallHealInput();
            HandleLargeHealInput();

            HandleInteractInput();

            HandleLockOnInput();

            HandleQuedInputs();

            HandleUIOpenMainMenuInput(); 
            HandleUIConfirmInput();
            HandleUiReturnInput();

        }

        #region Gameplay
        //seperates the horizontal and vertical inputs for the camera.
        private void HandleCameraInput()
        {
            if (UIManager.instance.menuWindowIsOpen)
                return; //might change this later?

            cameraHorizontalInput = cameraInput.x;
            cameraVerticalInput = cameraInput.y;

            camDirectionalInput = GetDirectionalValues(cameraHorizontalInput, cameraVerticalInput);
        }

        //movement inputs
        //seperates the horizontal & vertical inputs so that they can be modified individually,
        //moveAmount is using absolute positive numbers so that the animator has more clear animation blending.
        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            if (movementInput.y != 0)
            {
                lastVerticalInput = movementInput.y;
            }
            if (movementInput.x != 0)
            {
                lastHorizontalInput = movementInput.x;
            }

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        }

        //changes the boolean value based on if the sprint button is held down.
        private void HandleSprintInput()
        {
            if (sprintInput && moveAmount > 0.5f)
            {
                player.isSprinting = true;
                DashFlag = true;
                if (!ScreenManager.instance.speedLinesON)
                {
                    ScreenManager.instance.FullscreenSpeedlines(true);
                }
            }
            else
            {
                player.isSprinting = false;
                DashFlag = false;
                player.effectManager.SprintingEffect = false;
                if (ScreenManager.instance.speedLinesON)
                {
                    ScreenManager.instance.FullscreenSpeedlines(false);
                }
            }

            if (DashFlag && !player.effectManager.SprintingEffect)
            {
                player.effectManager.DashEffect();
            }
        }

        //changes the boolean value based on if the jump button is pressed, changes it too false immediately after it is reqqognized as true
        private void HandleJumpInput()
        {
            if (UIManager.instance.menuWindowIsOpen)
            {
                jumpInput = false;
                return;
            }

            if (SkillSetOpenInput)
            {
                jumpInput = false;
                return;
            }

            if (jumpInput && player.isHit)
            {
                jumpInput = false;
                player.playerStats.HandleRecovery();
                return;
            }

            if (jumpInput)
            {
                jumpInput = false;
                player.playerLocomotion.AttemptToPerformJump();
                
            }
        }

        //calls the handle dodge function directly for fast response.
        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;
                player.playerLocomotion.HandleDodge();
            }
        }

        //handles the basic attacks
        private void HandleBasicAttackInput()
        {

            if (SkillSetOpenInput)
            {
                basicAttackInput = false;
                return;
            }

            if (player.modeManager.currentMode == PlayMode.FreeMode ||
               player.modeManager.currentMode == PlayMode.LockOnMode)
            {
                if (basicAttackInput)
                {
                    basicAttackInput = false;

                    if (player.perfectTimedCombo && !perfectFlag)
                    {
                        player.perfectTimedCombo = false;
                        perfectFlag = true;
                        //Debug.Log("perfect timed combo");

                        player.combatManager.perfectCombo = true;
                        player.combatManager.perfectComboCounter++;

                        if (player.combatManager.perfectComboCounter >= player.combatManager.groundAttacks.Count)
                        {
                            player.combatManager.perfectFinish = true;
                            player.animator.SetBool("canDoCombo", true);
                            basicAttackInput = true;
                        }

                        player.effectManager.PerfectTimingEffect();
                        
                        player.animator.SetBool("perfectTimedCombo", false);

                        perfectFlag = false;
                    }

                    if (player.modeManager.currentMode == PlayMode.LockOnMode)
                    {
                        Vector3 rotationDirection = player.cameraHandler.currentLockOnTarget.position - transform.position;
                        rotationDirection.y = 0;
                        Quaternion tr = Quaternion.LookRotation(rotationDirection);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, 100);
                        transform.rotation = targetRotation;
                    }

                    if (player.canDoCombo)
                    {
                        comboFlag = true;
                        player.combatManager.HandleAttackCombo();
                        comboFlag = false;
                    }
                    else
                    {
                        if (player.isInteracting)
                            return;
                        if (player.canDoCombo)
                            return;

                        player.combatManager.HandleBasicAttack();
                    }
                }
            }

            /*I will keep this for now, even tho OTS mode is removed.
            *   because this is the way to rapid fire with the current system.
            
            else if (player.modeManager.currentMode == PlayMode.OTSMode)
            {
                if (basicAttackInput && Time.time >= timeToFire)
                {
                    basicAttackInput = false;
                    timeToFire = Time.time + (1 / player.playerStats.FireRate);
                    player.combatManager.HandleAimingAttack();
                }
            }
            */
        }

        //handles the unique attacks
        private void HandleUniqueAbilityInput()
        {
            if (SkillSetOpenInput)
            {
                uniqueAttackInput = false;
                return;
            }

            if (uniqueAttackInput)
            {
                player.combatManager.HandleUniqueAttack();
            }
            else
            {
                if (player.modeManager.currentMode == PlayMode.LockOnMode)
                {
                    Vector3 rotationDirection = player.cameraHandler.currentLockOnTarget.position - transform.position;
                    rotationDirection.y = 0;
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, 100);
                    transform.rotation = targetRotation;
                }
                player.combatManager.ChargeRelease = true;
                player.isCharging = false;
            }

        }

        //checks for the double input of the modifier + another button
        private void HandleSkillModifierInput()
        {
            if (SkillSetOpenInput && uniqueAttackInput)
            {//button north
                SkillNorthInput = true;
                //Debug.Log("combo North");
            }
            if (SkillSetOpenInput && jumpInput)
            {//button South
                SkillSouthInput = true;
                //Debug.Log("combo South");
            }
            if (SkillSetOpenInput && basicAttackInput)
            {//button west
                SkillWestInput = true;
                //Debug.Log("combo West");
            }
            if (SkillSetOpenInput && interactInput)
            {//button east
                SkillEastInput = true;
                //Debug.Log("combo East");
            }
        }

        //handles the skill attacks
        private void HandleSkillInput()
        {
            if (SkillNorthInput)
            {
                SkillNorthInput = false;
                //Debug.Log("input North");
                player.combatManager.HandleSkillAction('N');
            }
            else if (SkillSouthInput)
            {
                SkillSouthInput = false;
                //Debug.Log("input South");
                player.combatManager.HandleSkillAction('S');
            }
            else if (SkillWestInput)
            {
                SkillWestInput = false;
                //Debug.Log("input West");
                player.combatManager.HandleSkillAction('W');
            }
            else if (SkillEastInput)
            {
                SkillEastInput = false;
                //Debug.Log("input East");
                player.combatManager.HandleSkillAction('E');
            }
        }

        //handles small heal input
        private void HandleSmallHealInput()
        {
            if (largeHealInput)
                return;

            if (player.playerStats.smallhealingAmount == 0)
                return;

            if (smallHealInput)
            {
                player.playerStats.smallHealCharging = true;
                player.playerStats.HandleHealingCharge(); 
                if (player.playerStats.healingCharged)
                {
                    player.playerStats.HandleHealing(player.playerStats.smallHealingPercentage, false);
                    smallHealInput = false;
                }

            }
            else
            {
                player.playerStats.smallHealCharging = false;
                player.playerStats.healCharging = false;
                smallHealInput = false;
            }
        }

        //handles large heal input
        private void HandleLargeHealInput()
        {
            if (smallHealInput)
                return;

            if (player.playerStats.LargeHealingAmount == 0)
                return;

            if (largeHealInput)
            {
                player.playerStats.LargeHealCharging = true;
                player.playerStats.HandleHealingCharge();
                if (player.playerStats.healingCharged)
                {
                    player.playerStats.HandleHealing(0, true);
                    largeHealInput = false;
                }

            }
            else
            {
                player.playerStats.LargeHealCharging = false;
                player.playerStats.healCharging = false;
                largeHealInput = false;
            }
        }

        //sends out a signal that lockon has been pressed if it is available.
        private void HandleLockOnInput()
        {
            //checks if the current lockon target has died, then switches to new target.
            if (player.cameraHandler.LockedOn)
            {
                if (player.cameraHandler.currentLockOnTarget == null)
                    return;

                if (player.cameraHandler.currentLockOnTarget.GetComponentInParent<CharacterManager>().isDead)
                {
                    Debug.Log("lockon target died");
                    player.cameraHandler.LockedOn = false;
                
                    if (lockOnCoroutine != null)
                        StopCoroutine(lockOnCoroutine);

                    lockOnCoroutine = StartCoroutine(player.cameraHandler.WaitThenFindNewTarget());

                }
            }
            
            if (lockOnInput && !lockOnFlag) //searches for a lockon target, if found set that target as lockon Target.
            {
                lockOnInput = false;
                player.cameraHandler.HandleLockOn();

                if (player.cameraHandler.nearestLockOnTarget != null) //sets lockon on true
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.nearestLockOnTarget;
                    player.cameraHandler.LockedOn = true;
                    player.modeManager.OnLockedOn();
                }

            }
            else if (lockOnInput && lockOnFlag) //sets lockon on false
            {
                lockOnInput = false;
                player.modeManager.OnLockedOn();
            }

            //if your locked on scrolls to the right of the targets.
            if (lockOnFlag && lockOnRightInput) //swaps to right target
            {
                lockOnRightInput = false;
                player.cameraHandler.HandleLockOn();
                if (player.cameraHandler.RightLockTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.RightLockTarget;
                    lockonScrollInput = true;
                }
            }

            //if your locked on scrolls to the left of the targets.
            if (lockOnFlag && lockOnLeftInput)//swaps to left target
            {
                lockOnLeftInput = false;
                player.cameraHandler.HandleLockOn();
                if (player.cameraHandler.leftLockTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.leftLockTarget;
                    lockonScrollInput = true;
                }
            }

            //reset the lockonScroll
            if (lockOnFlag && camDirectionalInput == Vector2.zero && lockonScrollInput)
            {
                lockonScrollInput = false;
            }

        }

        //handle interact inputs
        private void HandleInteractInput()
        {

            if (SkillSetOpenInput)
            {
                interactInput = false;
                return;
            }

            if (interactInput)
            {
                //Debug.Log("interact");
                interactInput = false;

                //player.playerLocomotion.aeMovement = !player.playerLocomotion.aeMovement; //can't remember what this is for?
                player.interactionManager.Interact();
            }

        }
        #endregion

        #region Queying inputs
        private void CheckQuedInputs()
        {
            if (!SkillSetOpenInput)
            {
                if (basicAttackInput && !player.canDoCombo ||
                    basicAttackInput && player.isInteracting)
                {
                    //Debug.Log("que attack");
                    QueInput(ref quedAttackInput);
                }

                if (uniqueAttackInput && player.isInteracting && !player.isCharging)
                {
                    //Debug.Log("que unique");
                    QueInput(ref quedUniqueInput);
                }
            } 

            if (SkillNorthInput && player.isInteracting)
            {
                //Debug.Log("que Skill N");
                QueInput(ref quedSkillNInput);
            }
            if (SkillSouthInput && player.isInteracting)
            {
                //Debug.Log("que Skill S");
                QueInput(ref quedSkillSInput);
            }
            if (SkillWestInput && player.isInteracting)
            {
                //Debug.Log("que Skill W");
                QueInput(ref quedSkillWInput);
            }
            if (SkillEastInput && player.isInteracting)
            {
                // Debug.Log("que Skill E");
                QueInput(ref quedSkillEInput);
            }

            if (!SkillSetOpenInput)
            {    if (jumpInput && !player.isGrounded)
                {
                    //Debug.Log("que jump");
                    QueInput(ref quedJumpInput);
                }
            }
            
        }

        private void QueInput(ref bool quedInput)
        {

            ResetQuedInputs();

            if (player.isInteracting ||
                (player.isJumping && !player.isGrounded))
            {
                quedInput = true;
                queInputTimer = defaultQueTime;
                InputQueActive = true;
            }
           
        }

        private void ProcessQuedInputs()
        {
            if (player.isDead)
                return;

            if (quedAttackInput && player.canDoCombo ||
                quedAttackInput && !player.isInteracting)
            {

                //Debug.Log("qued attack!");
                basicAttackInput = true;
            }

            if (quedUniqueInput && !player.isInteracting)
            {
                //Debug.Log("qued unique!");
                uniqueAttackInput = true;
            }

           
            if (quedSkillNInput && !player.isInteracting)
            {
                //Debug.Log("qued north!");
                SkillNorthInput = true;
            }
            if (quedSkillSInput && !player.isInteracting)
            {
                //Debug.Log("qued south!");
                SkillSouthInput = true;
            }
            if (quedSkillWInput && !player.isInteracting)
            {
                //Debug.Log("qued west!");
                SkillWestInput = true;
            }
            if (quedSkillEInput && !player.isInteracting)
            {
                //Debug.Log("qued east!");
                SkillEastInput = true;
            }

            if (quedJumpInput && player.isGrounded)
            {
                //Debug.Log("qued jump!");
                jumpInput = true;
            }
        }

        private void HandleQuedInputs()
        {
            if (InputQueActive)
            {
                if (queInputTimer > 0)
                {
                    queInputTimer -= Time.deltaTime;
                    ProcessQuedInputs();
                }
                else
                {
                    ResetQuedInputs();
                    InputQueActive = false;
                    queInputTimer = 0;
                }
            }
        }

        private void ResetQuedInputs()
        {
            quedAttackInput = false;
            quedJumpInput = false;
            quedUniqueInput = false;
            quedSkillNInput = false;
            quedSkillSInput = false;
            quedSkillWInput = false;
            quedSkillEInput = false;

        }
        #endregion

        #region UI
        private void HandleUIOpenMainMenuInput()
        {
            if (!UIManager.instance.menuWindowIsOpen && uiOpenMenu)
            {
                uiOpenMenu = false;

                cameraVerticalInput = 0;
                cameraHorizontalInput = 0;

                UIManager.instance.CloseAllMenuWindows();
                UIManager.instance.menuManager.OpenMenu();
            }
        }

        private void HandleUiReturnInput()
        {
            if (uiReturn)
            {
                uiReturn = false;
                if (UIManager.instance.menuWindowIsOpen 
                    && UIManager.instance.gameplayMenuIsOpen)
                {
                    UIManager.instance.CloseAllMenuWindows();
                }
                else if (UIManager.instance.menuWindowIsOpen)
                {
                    Debug.Log("Return");
                    UIManager.instance.currentOpenMenu.CloseMenu();
                    Debug.Log("Returned!");
                }
            }
        }

        private void HandleUIConfirmInput()
        {
            if (uiConfirm)
            {
                uiConfirm = false;
                if (Time.timeScale == 0)
                    Time.timeScale = 1;
            }
        }

        public void DisableGameplayInput()
        {
            controls.PlayerAction.Disable();
            controls.PlayerMovement.Disable();
            controls.Control.Disable();
            controls.Camera.Disable();
            controls.UI.Enable();
        }

        public void EnableGameplayInput()
        {
            controls.PlayerAction.Enable();
            controls.PlayerMovement.Enable();
            controls.Control.Enable();
            controls.Camera.Enable();
            controls.UI.Disable();
        }
        #endregion

        #region Directional inputs
        //returns a vector2 of which direction is inputted.
        public Vector2 GetDirectionalValues(float horizontal, float vertical)
        {
            Vector2 snappedDirection;

            #region Horizontal
            if (horizontal > 0.55f)
            {
                snappedDirection.x = 1;
            }
            else if (horizontal < -0.55f)
            {
                snappedDirection.x = -1;
            }
            else
            {
                snappedDirection.x = 0;
            }
            #endregion
            #region vertical
            if (vertical > 0.55f)
            {
                snappedDirection.y = 1;
            }
            else if (vertical < -0.55f)
            {
                snappedDirection.y = -1;
            }
            else
            {
                snappedDirection.y = 0;
            }
            #endregion

            return snappedDirection;
        }

        //returns an inputdirection, for more easily reqognizion for which direction is inputted.
        public InputDirections GetInputDirections()
        {
            if (movementInput == Vector2.zero)
            {//none
                inputDir = InputDirections.None;
            }
            else if (movementInput == Vector2.up)
            {//north
                inputDir = InputDirections.North;
            }
            else if (movementInput == -Vector2.up)
            {//south
                inputDir = InputDirections.South;
            }
            else if (movementInput == -Vector2.right)
            {//west
                inputDir = InputDirections.West;
            }
            else if (movementInput == Vector2.right)
            {//east
                inputDir = InputDirections.East;
            }
            else if (movementInput.x > 0.55f && movementInput.x < 1f &&
               movementInput.y > 0.55f && movementInput.y < 1f)
            {//northwest
                inputDir = InputDirections.NorthWest;
            }
            else if (movementInput.x < -0.55f && movementInput.x > -1f &&
               movementInput.y > 0.55f && movementInput.y < 1f)
            {//northeast
                inputDir = InputDirections.NorthEast;
            }
            else if (movementInput.x > 0.55f && movementInput.x < 1f &&
               movementInput.y < -0.55f && movementInput.y > -1f)
            {//southwest
                inputDir = InputDirections.SouthWest;
            }
            else if (movementInput.x < -0.55f && movementInput.x > -1f &&
               movementInput.y < -0.55f && movementInput.y  > -1f)
            {//southwest
                inputDir = InputDirections.SouthEast;
            }

            return inputDir;
        }
        #endregion

    }

}