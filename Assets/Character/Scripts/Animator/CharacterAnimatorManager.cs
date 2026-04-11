using UnityEngine;
using System.Collections.Generic;

namespace WEV.WhiteRoom
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        [HideInInspector] public CharacterManager character; // Reference to the Character Manager script

        int horizontal; // For horizontal value
        int vertical; // For vertical value

        [Header("FLAGS")]
        public bool applyRootMotion = false;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            horizontal = Animator.StringToHash("Horizontal");
            vertical = Animator.StringToHash("Vertical");
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
            bool canRotate = false,
            bool canMove = false)
        {
            this.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnim, 0.2f);
            // BELOW CODE: Can be used to stop character from attempting a new action
            // BELOW CODE: Example if you get damaged and perform damage animation
            // BELOW CODE: The below flag will turn true id player is stunned
            // BELOW CODE: We can then check for the flag before attempting a new action
            character.isPerformingAction = isPerformingAction;
            character.characterLocomotionManager.canRotate = canRotate;
            character.characterLocomotionManager.canMove = canMove;
        }

        public virtual void PlayTargetAttackActionAnimation(
            ItemWeapon weapon,
            AttackType attackType,
            string targetAnimation,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            // BELOW CODE: Keep track of the last attack performed
            // BELOW CODE: Keep track of current attack type (light, heavy, etc)
            // BELOW CODE: Update animation set to the current weapons animations
            // BELOW CODE: Decide if our attack can be parried or not
            // BELOW CODE: Tell the network our "isAttacking" flag is active (for counter damage etc)
            character.characterCombatManager.currentAttackType = attackType;
            character.characterCombatManager.lastAttackAnimationPerformed = targetAnimation;
            //UpdateAnimatorController(weapon.weaponAnimator);
            this.applyRootMotion = applyRootMotion;
            character.characterEquipmentManager.rightHandWeaponManager.weaponAnimator.CrossFade(targetAnimation, 0.2f);
            // BELOW CODE: Can be used to stop character from attempting new actions
            // BELOW CODE: Example if you get damage and start performing damage animations
            // BELOW CODE: Then the below flag will turn if player is stunned
            // BELOW CODE: We can then check for this flag before attempting new actions
            character.isPerformingAction = isPerformingAction;
            character.characterLocomotionManager.canRotate = canRotate;
            character.characterLocomotionManager.canMove = canMove;
        }
        
        public void UpdateAnimatorController(AnimatorOverrideController weaponController)
        {
            character.animator.runtimeAnimatorController = weaponController;
        }

        public void EnableCanMove()
        {
            character.characterLocomotionManager.canMove = true;
        }
        public void DisableCanMove()
        {
            character.characterLocomotionManager.canMove = false;
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