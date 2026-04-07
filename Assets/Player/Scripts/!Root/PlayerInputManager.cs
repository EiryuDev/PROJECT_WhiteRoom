using UnityEngine;

namespace WEV.WhiteRoom
{
    public class PlayerInputManager : MonoBehaviour
    {
        PlayerControls playerControls;
        PlayerManager player;

        [Header("PLAYER MOVEMENT INPUT")]
        [SerializeField] Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        [Header("PLAYER CAMERA INPUT")]
        public float cameraVerticalInput;
        public float cameraHorizontalInput;

        [Header("PLAYER ACTION INPUT")]
        public bool crouch_Input = false;
        public bool sprint_Input = false;
        public bool jump_Input = false; 
        public bool interactInput = false;
        public bool rotateInput = false;
        public bool throwInput = false;
        public bool jumpInputHandled;
        [SerializeField] bool rbInput = false; 
        [SerializeField] bool rtInput = false; 

        private bool crouchPressedThisFrame = false;

        void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                // MOVEMENT
                playerControls.PlayerMovement.Movement.performed +=
                    i => movementInput = i.ReadValue<Vector2>();

                // LOOK (MOUSE / CAMERA)
                playerControls.PlayerCamera.Camera.performed += i =>
                {
                    Vector2 look = i.ReadValue<Vector2>();
                    cameraHorizontalInput = look.x;
                    cameraVerticalInput = look.y;
                };

                playerControls.PlayerCamera.Camera.canceled += i =>
                {
                    cameraHorizontalInput = 0;
                    cameraVerticalInput = 0;
                };

                // CROUCH
                playerControls.PlayerMovement.Crouch.performed += 
                    i => crouchPressedThisFrame = true;

                // SPRINT
                playerControls.PlayerMovement.Sprint.performed += 
                    i => sprint_Input = true;
                playerControls.PlayerMovement.Sprint.canceled += 
                    i => sprint_Input = false;

                // JUMP
                playerControls.PlayerMovement.Jump.performed += 
                    i => jump_Input = true;

                // INTERACT (PRESS)
                playerControls.PlayerActions.Interact.performed += 
                    i => interactInput = true;

                // ROTATE (HOLD)
                playerControls.PlayerActions.Rotate.performed += 
                    i => rotateInput = true;
                playerControls.PlayerActions.Rotate.canceled += 
                    i => rotateInput = false;

                // THROW (TAP)
                playerControls.PlayerActions.Throw.performed += 
                    i => throwInput = true;

                playerControls.PlayerActions.RB.performed += i => rbInput = true;
                playerControls.PlayerActions.RT.performed += i => rtInput = true;
            }

            playerControls.Enable();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus) playerControls.Enable();
                else playerControls.Disable();
            }
        }

        public void UseAllInputs()
        {
            // Movement
            UseMovementInput();
            UseJumpInput();
            UseCrouchInput();

            // Interact
            UseInteractInput();

            // Weapon
            UseRBInput();
            UseRTInput();
        }

        public void UseMovementInput()
        {
            horizontalInput = movementInput.x;
            verticalInput = movementInput.y;

            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            if (moveAmount <= 0.5 && moveAmount > 0)
                moveAmount = 0.5f;
            else if (moveAmount > 0.5)
                moveAmount = 1;

            if (player == null) return;
        }

        public void UseCrouchInput()
        {
            if (crouchPressedThisFrame)
            {
                crouch_Input = !crouch_Input;
                crouchPressedThisFrame = false;
            }
        }

        public void UseJumpInput()
        {
            if (jump_Input && !jumpInputHandled)
            {
                jump_Input = false;
                jumpInputHandled = true;
                player.playerLocomotionManager.AttemptToPerformJump();
            }
            else if (!jump_Input)
            {
                jumpInputHandled = false;
            }
        }
        public void UseInteractInput()
        {
            if(interactInput)
            {
                interactInput = false;

                player.playerInteractionManager.Interact();
            }
        }

        private void UseRBInput()
        {
            if(rbInput)
            {
                rbInput = false;

                // TO-DO: If we have a UI window open, return and do nothing

                player.playerCombatManager.SetPlayerActionHand(true);

                if(player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action != null)
                    player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action, player.playerInventoryManager.currentRightHandWeapon);
            }
        }

        private void UseRTInput()
        {
            if (rtInput)
            {
                rtInput = false;

                // TO-DO: If we have a UI window open, return and do nothing

                player.playerCombatManager.SetPlayerActionHand(true);

                if(player.playerInventoryManager.currentRightHandWeapon.oh_RT_Action != null)
                    player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RT_Action, player.playerInventoryManager.currentRightHandWeapon);
            }
        }
    }
}