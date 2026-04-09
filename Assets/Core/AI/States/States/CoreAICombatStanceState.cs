using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace WEV.WhiteRoom
{
    [CreateAssetMenu(menuName = "White Room/A.I/States/Combat Stance")]
    public class CoreAICombatStanceState : CoreAIState
    {
        // LOGIC: 
        // 1. Select an attack for the attack state, depending on distance and angle of target in relation to character
        // 2. Process any combat logic here whilist waiting to attack (blocking, strafing, dodging etc)
        // 3. If target moves out of combat range, switch to pursue target state
        // 4. If target is no longer present, switch to idle state

        [Header("ATTACKS")]
        [Tooltip("List of all the AI character attacks.")]
        public List<AICharacterAttackAction> aiCharacterAttacks; // List of all the AI character attacks
        [SerializeField] protected List<AICharacterAttackAction> potentialAttacks; // List of all the potential AI character attacks created during this state
        [SerializeField] private AICharacterAttackAction choosenAttack; // The attack which is choosen by the AI to attack
        [SerializeField] private AICharacterAttackAction previousAttack; // The previous attack which is done by the AI 
        protected bool hasAttack = false; // Check if the AI has attacks or not

        [Header("COMBO")]
        [Tooltip("Check if the AI character can perform combo or not.")]
        [SerializeField] protected bool canPerformCombo = false; // Check if the AI character can perform combo or not
        [Tooltip("Value for the chance to perform combo attack.")]
        [SerializeField] protected int chanceToPerformCombo = 25; // Value for the chance to perform combo attack
        [Tooltip("Check if the AI character rolled for combo chance or not.")]
        protected bool hasRolledForComboChance = false; // Check if the AI character rolled for combo chance or not
        
        [Header("ENGAGEMENT DISTANCE")]
        [Tooltip("The distance which AI have to be away from target before AI enter pursue target.")]
        public float maximumEngagementDistance = 5; // The distance which AI have to be away from target before AI enter pursue target
        public override CoreAIState Tick(AICharacterManager aiCharacter)
        {
            if(aiCharacter.isPerformingAction)
                return this;

            if(!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;

            if (aiCharacter.aiCharacterCombatManager.enablePivot)
            {
                // BELOW CODE: AI character face and turn towards the target when it's outside it's FOV 
                if(!aiCharacter.aiCharacterLocomotionManager.isMoving)
                {
                    if (aiCharacter.aiCharacterCombatManager.viewableAngle < -30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                        aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
                }
            }

            // BELOW CODE: Rotate to face the target
            aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);

            // BELOW CODE: If the AI's target is no longer present, switch back to idle
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);

            // BELOW CODE: If AI don't have the attack, get the attack
            if (!hasAttack)
            {
                GetNewAttack(aiCharacter);
            }
            else
            {
                // BELOW CODE: Check for recovery timer
                // BELOW CODE: Pass the attack to the attack state
                aiCharacter.attack.currentAttack = choosenAttack;
                // BELOW CODE: Roll for a combo chance
                // BELOW CODE: Switch State
                return SwitchState(aiCharacter, aiCharacter.attack);
            }

            // BELOW CODE: If AI is outside of the combat engagement distance, switch to pursue target state
            if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);

            // BELOW CODE: Lastly Pursue the target
            // BELOW CODE: Performant code
            //aiCharacter.navMeshAgent.SetDestination(aiCharacter.aICharacterCombatManager.currentTarget.transform.position);
            // BELOW CODE: Quicker runtime code
            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }

        protected virtual void GetNewAttack(AICharacterManager aiCharacter)
        {
            // BELOW CODE: Sort through all the possible attacks
            potentialAttacks = new List<AICharacterAttackAction>();

            foreach (var potentialAttack in aiCharacterAttacks)
            {
                if (potentialAttack.minimumDistanceNeededToAttack > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                    continue;

                if (potentialAttack.maximumDistanceNeededToAttack < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                    continue;

                if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle)
                    continue;

                if (potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.viewableAngle)
                    continue;

                potentialAttacks.Add(potentialAttack);
            }

            //foreach (var potentialAttack in aiCharacterAttacks)
            //{
            //    // BELOW CODE: Remove attacks that can't be used in the situation (Based on angle and distance)
            //    // BELOW CODE: If AI is too close for the attack, check the next
            //    if (potentialAttack.minimumDistanceNeededToAttack > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
            //        continue;

            //    // BELOW CODE: If AI is too far for the attack, check the next
            //    if (potentialAttack.maximumDistanceNeededToAttack < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
            //        continue;

            //    // BELOW CODE: If the target is outside the minimum field of view for the attack, check the next
            //    if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle)
            //        continue;

            //    // BELOW CODE: If the target is outside the maximum field of view for the attack, check the next
            //    if (potentialAttack.maximumDistanceNeededToAttack < aiCharacter.aiCharacterCombatManager.viewableAngle)
            //        continue;

            //    potentialAttacks.Add(potentialAttack);
            //}

            // BELOW CODE: Place remaining attacks into a list
            if (potentialAttacks.Count <= 0)
                return;

            var totalWeight = 0;

            foreach(var attack in potentialAttacks)
            {
                totalWeight += attack.attackWeight;
            }

            // BELOW CODE: Pick one of the remaining attacks randomly, based on weight
            var randomWeightValue = Random.Range(1, totalWeight + 1);
            var processedWeight = 0;

            foreach(var attack in potentialAttacks)
            {
                processedWeight += attack.attackWeight;

                if(randomWeightValue <= processedWeight)
                {
                    // BELOW CODE: Select this attack and pass it to the attack state
                    // BELOW CODE: It is the AI character
                    choosenAttack = attack;
                    previousAttack = choosenAttack;
                    hasAttack = true;
                    return;
                }
            }
        }
        protected virtual bool RoolforOutcomeChance(int outcomeChance)
        {
            bool outcomeWillBePerformed = false;

            int randomPercentage = Random.Range(0, 100);

            if(randomPercentage < outcomeChance)
                outcomeWillBePerformed = true;  

            return outcomeWillBePerformed;
        }
        protected override void ResetStateFlags(AICharacterManager aiCharacter)
        {
            base.ResetStateFlags(aiCharacter);

            hasAttack = false;
            hasRolledForComboChance = false;
        }
    }
}
