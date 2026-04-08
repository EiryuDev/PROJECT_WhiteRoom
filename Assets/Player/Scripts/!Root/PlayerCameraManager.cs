using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

namespace WEV.WhiteRoom
{
    public class PlayerCameraManager : MonoBehaviour
    {
        [HideInInspector] public PlayerManager player;

        [Header("PLAYER DATA")]
        public Transform playerBody;
        public Camera playerCamera;

        [Header("PICKUP & HOLD DATA")]
        public Transform holdPos;

        private float xRotation = 0f;
        private float timer = 0.0f;

        private Vector3 baseLocalPosition;
        private float originalMouseSensitivityValue;

        // Crouch offset variables
        private float currentCrouchOffset;
        private float crouchVelocity;

        // Slide offset variables
        private float currentSlideOffset;
        private float slideVelocity;

        void Awake()
        {
            player = GetComponentInParent<PlayerManager>();
        }
        void Start()
        {
            if (playerCamera == null)
            {
                Debug.LogError("PlayerCamera is not assigned in the inspector.");
                return;
            }
            
            baseLocalPosition = transform.localPosition;
            originalMouseSensitivityValue = player.playerInventoryManager.currentPlayerDataBeingUsed.mouseSensitivity;

            Cursor.lockState = CursorLockMode.Locked;
            playerCamera.fieldOfView = player.playerInventoryManager.currentPlayerDataBeingUsed.baseFOV;
        }

        public void UseAllCameraMovement()
        {
            HandleMouseLook();
            HandleHeadBobbing();
            HandleFieldOfView();
            HandleCameraSway();
            HandleCrouchOffset();
            HandleSlideOffset();   
        }

        // Mouse Look

        void HandleMouseLook()
        {
            float mouseX = player.playerInputManager.cameraHorizontalInput * player.playerInventoryManager.currentPlayerDataBeingUsed.mouseSensitivity * Time.deltaTime;
            float mouseY = player.playerInputManager.cameraVerticalInput * player.playerInventoryManager.currentPlayerDataBeingUsed.mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Vertical rotation (camera)
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Horizontal rotation (player body)
            playerBody.Rotate(Vector3.up * mouseX);
        }

        // Headbob
        void HandleHeadBobbing()
        {
            float waveslice = 0.0f;

            float horizontal = player.playerInputManager.horizontalInput;
            float vertical = player.playerInputManager.verticalInput;

            if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
            {
                timer = 0.0f;
            }
            else
            {
                waveslice = Mathf.Sin(timer);
                timer += player.playerInventoryManager.currentPlayerDataBeingUsed.bobbingSpeed;

                if (timer > Mathf.PI * 2)
                    timer -= Mathf.PI * 2;
            }

            Vector3 localPos = transform.localPosition;

            if (waveslice != 0)
            {
                float translateChange = waveslice * player.playerInventoryManager.currentPlayerDataBeingUsed.bobbingAmount;
                float totalAxes = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

                translateChange *= totalAxes;
                localPos.y = player.playerInventoryManager.currentPlayerDataBeingUsed.midpoint + translateChange;
            }
            else
            {
                localPos.y = player.playerInventoryManager.currentPlayerDataBeingUsed.midpoint;
            }

            transform.localPosition = localPos;
        }

        // FOV (SPRINT)
        void HandleFieldOfView()
        {
            if (player.playerLocomotionManager.isSprinting)
            {
                playerCamera.fieldOfView = Mathf.Lerp(
                    playerCamera.fieldOfView,
                    player.playerInventoryManager.currentPlayerDataBeingUsed.sprintFOV,
                    player.playerInventoryManager.currentPlayerDataBeingUsed.fovChangeSpeed * Time.deltaTime
                );
            }
            else
            {
                playerCamera.fieldOfView = Mathf.Lerp(
                    playerCamera.fieldOfView,
                    player.playerInventoryManager.currentPlayerDataBeingUsed.baseFOV,
                    player.playerInventoryManager.currentPlayerDataBeingUsed.fovChangeSpeed * Time.deltaTime
                );
            }
        }

        // Camera Sway 
        void HandleCameraSway()
        {
            float movementX = Mathf.Clamp(
                player.playerInputManager.cameraHorizontalInput * player.playerInventoryManager.currentPlayerDataBeingUsed.swayAmount,
                -player.playerInventoryManager.currentPlayerDataBeingUsed.swayAmount,
                player.playerInventoryManager.currentPlayerDataBeingUsed.swayAmount
            );

            float movementY = Mathf.Clamp(
                player.playerInputManager.cameraVerticalInput * player.playerInventoryManager.currentPlayerDataBeingUsed.swayAmount,
                -player.playerInventoryManager.currentPlayerDataBeingUsed.swayAmount,
                player.playerInventoryManager.currentPlayerDataBeingUsed.swayAmount
            );

            Vector3 finalPosition = new Vector3(movementX, movementY, 0);

            Vector3 targetPosition = baseLocalPosition + finalPosition;

            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                targetPosition,
                player.playerInventoryManager.currentPlayerDataBeingUsed.swaySpeed * Time.deltaTime
            );
        }

        // Crouch Offset Camera
        void HandleCrouchOffset()
        {
            bool isCrouching = player.playerLocomotionManager.isCrouching;

            float targetOffset = isCrouching
                ? player.playerInventoryManager.currentPlayerDataBeingUsed.crouchOffsetY
                : 0f;

            currentCrouchOffset = Mathf.SmoothDamp(
                currentCrouchOffset,
                targetOffset,
                ref crouchVelocity,
                player.playerInventoryManager.currentPlayerDataBeingUsed.crouchSmoothTime
            );

            // Apply offset cleanly (no stacking)
            Vector3 pos = transform.localPosition;
            pos.y += currentCrouchOffset;
            transform.localPosition = pos;
        }

        // Slide Offset Camera
        void HandleSlideOffset()
        {
            bool isSliding = player.playerLocomotionManager.isSliding;

            float targetOffset = isSliding
                ? player.playerInventoryManager.currentPlayerDataBeingUsed.slideOffsetY
                : 0f;

            currentSlideOffset = Mathf.SmoothDamp(
                currentSlideOffset,
                targetOffset,
                ref slideVelocity,
                player.playerInventoryManager.currentPlayerDataBeingUsed.slideSmoothTime
            );

            // Apply offset cleanly (no stacking)
            Vector3 pos = transform.localPosition;
            pos.y += currentSlideOffset;
            transform.localPosition = pos;
        }

        public IEnumerator ShakeCamera()
        {
            float elapsedTime = 0f;
            Vector3 originalCameraPosition = playerCamera.transform.localPosition;

            while (elapsedTime < player.playerInventoryManager.currentPlayerDataBeingUsed.shakeDuration)
            {
                float x = Random.Range(-1f, 1f) * player.playerInventoryManager.currentPlayerDataBeingUsed.shakeMagnitude;
                float y = Random.Range(-1f, 1f) * player.playerInventoryManager.currentPlayerDataBeingUsed.shakeMagnitude;

                playerCamera.transform.localPosition = new Vector3(x, y, originalCameraPosition.z);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            playerCamera.transform.localPosition = originalCameraPosition;
        }

        public void RotateObject(GameObject heldObj, bool canDrop)
        {
            if (player.playerInputManager.rotateInput)
            {
                canDrop = false;
                player.playerInventoryManager.currentPlayerDataBeingUsed.mouseSensitivity = 0f;

                float xRot = player.playerInputManager.cameraHorizontalInput *  player.playerInventoryManager.currentPlayerDataBeingUsed.rotationSensitivity;
                float yRot = player.playerInputManager.cameraVerticalInput *  player.playerInventoryManager.currentPlayerDataBeingUsed.rotationSensitivity;

                heldObj.transform.Rotate(Vector3.down, xRot);
                heldObj.transform.Rotate(Vector3.right, yRot);
            }
            else
            {
                player.playerInventoryManager.currentPlayerDataBeingUsed.mouseSensitivity = originalMouseSensitivityValue;
                canDrop = true;
            }
        }
    }
}
