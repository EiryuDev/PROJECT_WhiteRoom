using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        [HideInInspector] public CharacterManager character; // Reference to the Character Manager script

        [Header("MOVEMENT FLAGS")]
        public bool canMove = true;
        public bool canRotate = true;
        public bool canJump = true;
        public bool canCrouch = true;
        public bool canSlide = true;
        public bool isMoving = false;
        public bool isSprinting = false;
        public bool isSliding = false;
        public bool isGrounded = true;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
    }
}
