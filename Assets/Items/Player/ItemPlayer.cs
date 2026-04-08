using UnityEngine;

namespace WEV.WhiteRoom
{
    [CreateAssetMenu(menuName = "White Room/Player/Player Data")]
    public class ItemPlayer : Item
    {
        [Header("Movement Settings")]
        [Tooltip("How much is the movement speed")]
        public float movementSpeed = 3.0f; 
        [Tooltip("How much is the walking speed")]
        public float walkingSpeed = 3.0f; 
        [Tooltip("How much is the sprint speed")]
        public float sprintingSpeed = 3.0f;

        [Header("Camera Settings")] 
        [Tooltip("How much is the mouse Sensitivity?")]
        public float mouseSensitivity = 100f;
        [Header("Head bob Settings")]
        [Tooltip("How much is the camera bobbing speed?")]
        public float bobbingSpeed = 0.18f;
        [Tooltip("How much is the camera bobbing amount?")]
        public float bobbingAmount = 0f;
        [Tooltip("How much is the camera midPoint?")]
        public float midpoint = 2f;
        [Header("Field of View Settings")]
        [Tooltip("How much is the base camera fov?")]
        public float baseFOV = 60f;
        [Header("Camera Sway Settings")]
        [Tooltip("How much is the camera sway amount?")]
        public float swayAmount = 0.05f;
        [Tooltip("How much is the camera sway speed?")]
        public float swaySpeed = 4f;
        [Header("Camera Shake Settings")]
        [Tooltip("How much is the camera shake duration?")]
        public float shakeDuration = 0.1f;
        [Tooltip("How much is the camera shake magnitude?")]        
        public float shakeMagnitude = 0.1f;

        [Header("Sprint Data")]
        [Tooltip("How much is the camera fov while sprinting?")]
        public float sprintFOV = 70f;
        [Tooltip("How much is the camera fov speed?")]
        public float fovChangeSpeed = 5f;

        [Header("Jump Settings")]
        [Tooltip("How much is the jump force")]
        public float jumpForce = 10f; 
        [Tooltip("How long the player will jump")]
        public float jumpCooldown = 0.2f;
        public float gravity = -9.81f;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;

        [Header("Crouch Settings")]
        [Tooltip("How much is the crouch movement speed")]
        public float crouchMovementSpeed = 2f;
        [Tooltip("How much is the height of player when crouched?")]
        public float crouchHeight = 1f;   
        [Header("Crouch Camera Settings")]
        [Tooltip("How much is the y offset of the crouch camera?")]
        public float crouchOffsetY = -0.55f;
        [Tooltip("How much is the smooth rate of crouch?")]
        public float crouchSmoothTime = 0.20f;

        [Header("Sliding Settings")]
        [Tooltip("How much is the sliding speed")]
        public float slideSpeed = 10f;
        [Tooltip("How much is the sliding duration")]
        public float slideDuration = 1f;
        [Tooltip("How much is the sliding Height")]
        public float slideHeight = 0.5f;
        [Header("Slide Camera Settings")]
        [Tooltip("How much is the y offset of the slide camera?")]
        public float slideOffsetY = -0.55f;
        [Tooltip("How much is the smooth rate of slide?")]
        public float slideSmoothTime = 0.20f;

        [Header("Pickup Settings")]
        [Tooltip("How much is the throw force?")] 
        public float throwForce = 500f;
        [Tooltip("How much is the rotation sensitivity of picked Up object?")] 
        public float rotationSensitivity = 1f;
        [Tooltip("How much is the pickup range?")] 
        public float pickUpRange = 5f;
    }
}
