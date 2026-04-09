using UnityEngine;

namespace WEV.WhiteRoom
{
    [CreateAssetMenu(menuName = "White Room/A.I/States/Attack")]
    public class CoreAIAttackState : CoreAIState
    {
        [Header("CURRENT ATTACK")]
        [HideInInspector] public AICharacterAttackAction currentAttack; // The current attack which is done by the AI 
        [HideInInspector] public bool willPerformCombo = false; // Check if the AI character will perform combo or not

        [Header("STATE FLAGS")]
        protected bool hasPerformedAttack = false; // Check if AI performed attack or not   
        protected bool hasPerformedCombo = false; // Check if the AI character had performed combo or not

        [Header("PIVOT AFTER ATTACK")]
        [Tooltip("Check if AI can pivot after attack or not.")]
        [SerializeField] protected bool pivotAfterAttack = false; // Check if AI can pivot after attack or not
        public override CoreAIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);

            if(aiCharacter.aiCharacterCombatManager.currentTarget.isDead)
                return SwitchState(aiCharacter, aiCharacter.idle);

            // BELOW CODE: Rotate towards the target while attacking
            aiCharacter.aiCharacterCombatManager.RotateTowardsTargetWhileAttacking(aiCharacter);

            // BELOW CODE: Set movement values to 0
            aiCharacter.aiCharacterAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false);

            // BELOW CODE: Perform a combo
            if(willPerformCombo && !hasPerformedCombo)
            {
                if(currentAttack.comboAction != null)
                {
                    // BELOW CODE: If AI can do combo
                    //hasPerformedCombo = true;
                    //currentAttack.comboAction.AttemptToPerformAction(aiCharacter);
                }
            }

            if (aiCharacter.isPerformingAction)
                return this;

            if (!hasPerformedAttack)
            {
                // BELOW CODE: If the AI still recovering from an action, wait before performing an another
                if (aiCharacter.aiCharacterCombatManager.actionRecoveryTimer > 0)
                    return this;

                PerformAttack(aiCharacter);

                // BELOW CODE: Return to the start, so if we have a combo we process that when AI able to do so
                return this;
            }

            if (pivotAfterAttack)
                aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);

            return SwitchState(aiCharacter, aiCharacter.combatStance);
        }
        protected void PerformAttack(AICharacterManager aiCharacter)
        {
            hasPerformedAttack = true;
            currentAttack.AttemptToPerformAction(aiCharacter);
            aiCharacter.aiCharacterCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTimer;
        }
        protected override void ResetStateFlags(AICharacterManager aiCharacter)
        {
            base.ResetStateFlags(aiCharacter);
            hasPerformedAttack = false;
            hasPerformedCombo = false;
        }
    }
}
