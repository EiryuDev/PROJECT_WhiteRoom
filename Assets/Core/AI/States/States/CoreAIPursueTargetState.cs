using UnityEngine;
using UnityEngine.AI;

namespace WEV.WhiteRoom
{
    [CreateAssetMenu(menuName = "White Room/A.I/States/Pursue Target")]
    public class CoreAIPursueTargetState : CoreAIState
    {
        public override CoreAIState Tick(AICharacterManager aiCharacter)
        {
            // BELOW CODE: Check if AI is performing an action (If so do nothing until the action is completed)
            if (aiCharacter.isPerformingAction)
                return this;

            // BELOW CODE: Check if AI's target is null, if AI do not have a target, return to idle state
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);

            // BELOW CODE: Make sure AI's navmesh agent is active, if it's not then enable it
            if (!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;

            // BELOW CODE: If AI's target goes outside of the characters F.O.V, pivot to face them
            if (aiCharacter.aiCharacterCombatManager.enablePivot)
            {
                if (aiCharacter.aiCharacterCombatManager.viewableAngle < aiCharacter.aiCharacterCombatManager.minimumFOV ||
                    aiCharacter.aiCharacterCombatManager.viewableAngle > aiCharacter.aiCharacterCombatManager.maximumFOV)
                    aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
            }
            
            aiCharacter.aiCharacterLocomotionManager.RotateTowardsAgent(aiCharacter);

            /* BELOW CODE: Old System
            if (!aiCharacter.navMeshAgent.pathPending &&
                aiCharacter.navMeshAgent.remainingDistance <= aiCharacter.navMeshAgent.stoppingDistance)
            {
                return SwitchState(aiCharacter, aiCharacter.combatStance);
            }
            */

            // BELOW CODE: If AI's within combat range of a target, switch to combat stance state
            if (aiCharacter.aiCharacterCombatManager.distanceFromTarget <= aiCharacter.combatStance.maximumEngagementDistance)
                return SwitchState(aiCharacter, aiCharacter.combatStance);

            // BELOW CODE: If the target is not reachable, and target is far away, return back

            // BELOW CODE: Lastly Pursue the target
            // BELOW CODE: Performant code
            //aiCharacter.navMeshAgent.SetDestination(aiCharacter.aICharacterCombatManager.currentTarget.transform.position);
            // BELOW CODE: Quicker runtime code
            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }
    }
}
