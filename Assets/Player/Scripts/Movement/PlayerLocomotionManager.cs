
using System;
using UnityEngine;
using System.Collections;

namespace WEV.WhiteRoom
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        [HideInInspector] public PlayerManager player;
        [HideInInspector] public float verticalMovement; 
        [HideInInspector] public float horizontalMovement; 

        [Header("MOVEMENT DATA")]
        public Transform groundCheck;
        [HideInInspector] public Vector3 velocity;
        [HideInInspector] public bool isCrouching;
        [HideInInspector] public float originalHeight;
        private float targetHeight;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            originalHeight = player.controller.height;
            targetHeight = originalHeight;
        }

        public void UseAllMovement()
        {
            // BELOW CODE: Grounded movement
            UseSprint();
            UseCrouch();
            UseGroundedMovement();
            // BELOW CODE: Jumping movement
            UseJumpingMovement();
        }
        private void GetMovementValues()
        {
            verticalMovement = player.playerInputManager.verticalInput;
            horizontalMovement = player.playerInputManager.horizontalInput;
        }
        private void UseGroundedMovement()
        {
            if (!canMove || player.isPerformingAction || !isGrounded)
                return; // To stop the player from moving while interacting in the falling

            GetMovementValues();
            MovePlayer();
        }
        private void MovePlayer()
        {
            float baseSpeed =
                isSprinting ? player.playerInventoryManager.currentPlayerDataBeingUsed.sprintingSpeed :
                player.playerInputManager.moveAmount > 0.5f ? player.playerInventoryManager.currentPlayerDataBeingUsed.movementSpeed :
                player.playerInventoryManager.currentPlayerDataBeingUsed.walkingSpeed;

            // If crouching, override speed
            float speed = isCrouching ? player.playerInventoryManager.currentPlayerDataBeingUsed.crouchMovementSpeed : baseSpeed;

            Vector3 moveDirection = CalculateMoveDirection();

            player.controller.Move(moveDirection * speed * Time.deltaTime);
        }
        public Vector3 CalculateMoveDirection()
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            Vector3 moveDirection = (cameraForward * verticalMovement) + (cameraRight * horizontalMovement);
            moveDirection.y = 0;
            moveDirection.Normalize();

            return moveDirection;
        }
        public void AttemptToPerformJump()
        {
            if (canJump)
            {
                
                // Get the player's forward movement direction
                Vector3 moveDirection = CalculateMoveDirection();

                // Add the forward movement direction to the velocity
                velocity = moveDirection * player.playerInventoryManager.currentPlayerDataBeingUsed.movementSpeed;

                // Add the jump force to the Y velocity
                velocity.y = Mathf.Sqrt(player.playerInventoryManager.currentPlayerDataBeingUsed.jumpForce * -2f * player.playerInventoryManager.currentPlayerDataBeingUsed.gravity);

                // Start the jump cooldown
                StartCoroutine(JumpCooldown());
            }
        }
        private IEnumerator JumpCooldown()
        {
            canJump = false;
            yield return new WaitForSeconds(player.playerInventoryManager.currentPlayerDataBeingUsed.jumpCooldown);
            canJump = true;
        }

        public void UseJumpingMovement()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, player.playerInventoryManager.currentPlayerDataBeingUsed.groundDistance, player.playerInventoryManager.currentPlayerDataBeingUsed.groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            velocity.y += player.playerInventoryManager.currentPlayerDataBeingUsed.gravity * Time.deltaTime;
            character.controller.Move(velocity * Time.deltaTime);

            // Reset the Y velocity and forward velocity after the movement is applied
            if (character.controller.isGrounded)
            {
                velocity.y = 0f;
                // Reset the forward velocity
                velocity.x = 0f;
                velocity.z = 0f;
            }
        }

        private void UseCrouch()
        {
            if (!canCrouch)
                return;
                
            bool crouchHeld = player.playerInputManager.crouch_Input;

            isCrouching = crouchHeld;

            float crouchHeight = player.playerInventoryManager.currentPlayerDataBeingUsed.crouchHeight;

            targetHeight = isCrouching ? crouchHeight : originalHeight;

            float oldHeight = player.controller.height;

            float newHeight = Mathf.Lerp(oldHeight, targetHeight, Time.deltaTime);
            player.controller.height = newHeight;

            // Keep feet grounded while resizing
            float heightDifference = newHeight - oldHeight;
            player.controller.center += new Vector3(0, heightDifference / 2f, 0);
        }

        private void UseSprint()
        {
            bool wantsToSprint = player.playerInputManager.sprint_Input;

            // Must be moving forward and grounded
            bool isMovingForward = player.playerInputManager.verticalInput > 0.5f;

            isSprinting = wantsToSprint && isMovingForward && isGrounded && !isCrouching;
        }
    }
}
