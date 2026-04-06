using UnityEngine;
using System.Collections.Generic;

namespace WEV.WhiteRoom
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        [HideInInspector] public CharacterManager character; // Reference to the Character Manager script

        int horizontal; // For horizontal value
        int vertical; // For vertical value

        
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            horizontal = Animator.StringToHash("Horizontal");
            vertical = Animator.StringToHash("Vertical");
        }

        public virtual void OnAnimatorMove()
        {
            if (character.isPerformingAction == false)
                return;

            if (character.applyRootMotion)
            {
                // BELOW CODE: Take the rotation from particular animation and apply to the character rotation
                Vector3 velocity = character.animator.deltaPosition;
                character.controller.Move(velocity);
                character.transform.rotation *= character.animator.deltaRotation;
            }
        }

        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            // BELOW CODE: Adding the values
            float horizontalAmount = horizontalMovement;
            float verticalAmount = verticalMovement;  

            if(isSprinting)
            {
                verticalAmount = 2;
            }

            character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
        }

        public void PlayTargetActionAnimation(
            string targetAnim,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canMove = false)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnim, 0.2f);
            // BELOW CODE: Can be used to stop character from attempting a new action
            // BELOW CODE: Example if you get damaged and perform damage animation
            // BELOW CODE: The below flag will turn true id player is stunned
            // BELOW CODE: We can then check for the flag before attempting a new action
            character.isPerformingAction = isPerformingAction;
            character.canMove = canMove;
        }
        public void EnableCanMove()
        {
            character.canMove = true;
        }
        public void EnableIsInvulnerable()
        {
            character.animator.SetBool("isInvulnerable", true);
        }
        public void DisableIsInvulnerable()
        {
            character.animator.SetBool("isInvulnerable", false);
        }
        public void DisableCollision()
        {
            character.controller.enabled = false;
        }
        public void EnableCollision()
        {
            character.controller.enabled = true;
        }
    }
}