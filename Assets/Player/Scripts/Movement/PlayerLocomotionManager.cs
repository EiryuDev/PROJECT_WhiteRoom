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
        [HideInInspector] public Vector3 velocity;
        [HideInInspector] public bool isCrouching;
        [HideInInspector] public float originalHeight;
        private Vector3 originalCenter;
        private float targetHeight;

        private Vector3 moveDirectionThisFrame = Vector3.zero;
        private float moveSpeedThisFrame = 0f;

        [Header("Sliding Data")]
        [HideInInspector] public float slideTimer;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            originalHeight = player.controller.height;
            originalCenter = player.controller.center;
            targetHeight = originalHeight;
        }

        public void UseAllMovement()
        {
            UseSprint();
            UseCrouch();
            UseGroundedMovement();  // Calculates direction only — no Move() call
            UseJumpingMovement();   // Applies gravity + single combined Move() call
            UseSlidingMovement();
        }

        private void GetMovementValues()
        {
            verticalMovement = player.playerInputManager.verticalInput;
            horizontalMovement = player.playerInputManager.horizontalInput;
        }

        private void UseGroundedMovement()
        {
            if (!canMove || !isGrounded || isSliding)
            {
                moveDirectionThisFrame = Vector3.zero;
                moveSpeedThisFrame = 0f;
                return;
            }

            GetMovementValues();

            moveSpeedThisFrame =
                isSprinting ? player.playerInventoryManager.currentPlayerDataBeingUsed.sprintingSpeed :
                player.playerInputManager.moveAmount > 0.5f ? player.playerInventoryManager.currentPlayerDataBeingUsed.movementSpeed :
                player.playerInventoryManager.currentPlayerDataBeingUsed.walkingSpeed;

            if (isCrouching)
                moveSpeedThisFrame = player.playerInventoryManager.currentPlayerDataBeingUsed.crouchMovementSpeed;

            moveDirectionThisFrame = CalculateMoveDirection();
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
                Vector3 moveDirection = CalculateMoveDirection();

                // BELOW CODE: If sprinting, carry full sprint momentum into the jump arc
                if (isSprinting)
                {
                    velocity.x = moveDirection.x * player.playerInventoryManager.currentPlayerDataBeingUsed.sprintingSpeed;
                    velocity.z = moveDirection.z * player.playerInventoryManager.currentPlayerDataBeingUsed.sprintingSpeed;
                }
                else
                {
                    velocity.x = moveDirection.x * player.playerInventoryManager.currentPlayerDataBeingUsed.movementSpeed;
                    velocity.z = moveDirection.z * player.playerInventoryManager.currentPlayerDataBeingUsed.movementSpeed;
                }

                velocity.y = Mathf.Sqrt(player.playerInventoryManager.currentPlayerDataBeingUsed.jumpForce * -2f * player.playerInventoryManager.currentPlayerDataBeingUsed.gravity);
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
            isGrounded = character.controller.isGrounded;

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            velocity.y += player.playerInventoryManager.currentPlayerDataBeingUsed.gravity * Time.deltaTime;

            Vector3 finalMove;

            if (!isGrounded && (velocity.x != 0 || velocity.z != 0))
            {
                // BELOW CODE: Airborne with stored momentum — use velocity.x/z for horizontal
                finalMove = new Vector3(velocity.x, velocity.y, velocity.z) * Time.deltaTime;
            }
            else
            {
                // BELOW CODE: Grounded — use normal directional movement
                finalMove = moveDirectionThisFrame * moveSpeedThisFrame * Time.deltaTime;
                finalMove.y = velocity.y * Time.deltaTime;
            }

            character.controller.Move(finalMove);

            if (character.controller.isGrounded)
            {
                velocity.y = 0f;
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

            float heightDifference = newHeight - oldHeight;
            player.controller.center += new Vector3(0, heightDifference / 2f, 0);
        }

        private void UseSprint()
        {
            bool wantsToSprint = player.playerInputManager.sprint_Input;
            bool isMovingForward = player.playerInputManager.verticalInput > 0.5f;
            isSprinting = wantsToSprint && isMovingForward && isGrounded && !isCrouching && !isSliding;
        }

        public void AttemptToUseSliding()
        {
            if (!canSlide || isSliding)
                return;

            isSliding = true;
            isCrouching = false;
            player.playerInputManager.crouch_Input = false;

            float slideHeight = player.playerInventoryManager.currentPlayerDataBeingUsed.slideHeight;
            player.controller.height = slideHeight;
            player.controller.center = new Vector3(originalCenter.x, originalCenter.y - (originalHeight - slideHeight) / 2f, originalCenter.z);

            slideTimer = player.playerInventoryManager.currentPlayerDataBeingUsed.slideDuration;
        }

        public void UseSlidingMovement()
        {
            if (!canSlide)
                return;

            if (isSliding)
            {
                slideTimer -= Time.deltaTime;
                if (slideTimer <= 0)
                {
                    ResetSliding();
                }
                else
                {
                    Vector3 moveDirection = CalculateMoveDirection();
                    player.controller.Move(moveDirection * player.playerInventoryManager.currentPlayerDataBeingUsed.slideSpeed * Time.deltaTime);
                }
            }
        }

        public void ResetSliding()
        {
            isSliding = false;
            player.controller.height = originalHeight;
            player.controller.center = originalCenter;
        }
    }
}