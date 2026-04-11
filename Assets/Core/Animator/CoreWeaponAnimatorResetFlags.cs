using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CoreWeaponAnimatorResetFlags : StateMachineBehaviour
    {
        CharacterManager character; // Reference to the Character Manager Script

        // BELOW CODE: OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null)
            {
                character = animator.GetComponentInParent<CharacterManager>();
            }

            // BELOW CODE: This is called when an action ends, and the states return to "empty"
            character.isPerformingAction = false;
            character.characterAnimatorManager.applyRootMotion = false;
            character.characterLocomotionManager.canRotate = true;
            character.characterLocomotionManager.canMove = true;
            character.characterCombatManager.DisableCanDoCombo();

            character.characterLocomotionManager.isJumping = false;
            character.characterLocomotionManager.isSliding = false;
            character.isInvulnerable = false;
            character.characterCombatManager.isAttacking = false;
        }
    }
}
