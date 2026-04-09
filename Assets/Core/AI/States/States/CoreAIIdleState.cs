using UnityEngine;
using UnityEngine.AI;

namespace WEV.WhiteRoom
{
    [CreateAssetMenu(menuName = "White Room/A.I/States/Idle")]
    public class CoreAIIdleState : CoreAIState
    {
        [Header("IDLE OPTIONS DATA")]
        [Tooltip("Which type of idle state our character has? (\n Idle,\n Patrol,\n Sleep,\n Follow,\n Wander)")]
        [SerializeField] private IdleStateMode idleStateMode;

        [Header("PATROL OPTIONS DATA")] 
        [Tooltip("Reference to the AI Character Patrol Path")]
        public AICharacterPatrolPath aiCharacterPatrolPath;
        [Tooltip("If the character spawns closer to the second point, start at second point")]
        [SerializeField] private bool hasFoundClosestPointNearCharacterSpawn = false;
        [Tooltip("Check if we have finished the patrol yet?")]
        [SerializeField] bool patrolComplete = false;
        [Tooltip("Does the character upon finishing the patrol, repeat the path again?")]
        [SerializeField]  bool patrolRepeat = false;
        [Tooltip("Which point of the patrol the character is working towards?")]
        [SerializeField] int patrolDestinationIndex;
        [Tooltip("Do the character have a point, which they currently walking towards?")]
        [SerializeField] bool hasPatrolDestination = false;
        [Tooltip("The specific destination coordinates the character is walking towards?")]
        [SerializeField] Vector3 currentPatrolDestination;
        [Tooltip("The distance from the character to the destination?")]
        [SerializeField] float distanceFromCurrentDestination;
        [Tooltip("Minimum time before starting a new patrol")]
        [SerializeField] float timeBetweenPatrols = 15;
        [Tooltip("Active timer counting the time rested?")] [SerializeField]
        private float restTimer = 0;
        public override CoreAIState Tick(AICharacterManager aiCharacter)
        {
            aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);

            switch (idleStateMode)
            {
                case IdleStateMode.Idle:
                    return Idle(aiCharacter);
                case IdleStateMode.Patrol:
                    return Patrol(aiCharacter);
                default:
                    return this;
            }
        }
        protected virtual CoreAIState Idle(AICharacterManager aiCharacter)
        {
            if (aiCharacter.characterCombatManager.currentTarget != null)
            {
                // BELOW CODE: Return the pursue target state
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);     
            }
            else
            {
                // BELOW CODE: Return this state, to continually search for a target
                return this;
            }
        }
        protected virtual CoreAIState Patrol(AICharacterManager aiCharacter)
        {
            if (!aiCharacter.aiCharacterLocomotionManager.isGrounded)
                return this;

            if (aiCharacter.isPerformingAction)
            {
                aiCharacter.navMeshAgent.enabled = false;
                aiCharacter.characterLocomotionManager.isMoving = false;
                return this;
            }

            if (!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;

            if (aiCharacter.aiCharacterCombatManager.currentTarget != null)
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);

            // BELOW CODE: If character's patrol is complete, repeat it check for rest time
            if (patrolComplete && patrolRepeat)
            {
                // BELOW CODE: If the time has not exceeded it's set limit, stop and wait
                if (timeBetweenPatrols > restTimer)
                {
                    aiCharacter.navMeshAgent.enabled = false;
                    aiCharacter.characterLocomotionManager.isMoving = false;
                    restTimer += Time.deltaTime;
                }
                else
                {
                    patrolDestinationIndex = -1;
                    hasPatrolDestination = false;
                    currentPatrolDestination = aiCharacter.transform.position;
                    patrolComplete = false;
                    restTimer = 0;
                }
            }
            else if(patrolComplete && !patrolRepeat)
            {
                aiCharacter.navMeshAgent.enabled = false;
                aiCharacter.characterLocomotionManager.isMoving = false;
            }

            // BELOW CODE: If a character has a destination, move towards it
            if (hasPatrolDestination)
            {
                distanceFromCurrentDestination = Vector3.Distance(aiCharacter.transform.position, currentPatrolDestination);

                if (distanceFromCurrentDestination > 2)
                {
                    aiCharacter.navMeshAgent.enabled = true;
                    aiCharacter.aiCharacterLocomotionManager.RotateTowardsAgent(aiCharacter);
                }
                else
                {
                    currentPatrolDestination = aiCharacter.transform.position;
                    hasPatrolDestination = false;
                }
            }
            // BELOW CODE: Otherwise, get a new destination
            else
            {
                patrolDestinationIndex += 1;
                
                if (patrolDestinationIndex > aiCharacterPatrolPath.patrolPoints.Count - 1)
                {
                    patrolComplete = true;
                    return this;
                }

                if (!hasFoundClosestPointNearCharacterSpawn)
                {
                    hasFoundClosestPointNearCharacterSpawn = true;
                    float closestDistance = Mathf.Infinity;

                    for (int i = 0; i < aiCharacterPatrolPath.patrolPoints.Count; i++)
                    {
                        float distanceFromThisPoint = Vector3.Distance(
                            aiCharacter.transform.position,
                            aiCharacterPatrolPath.patrolPoints[i]);

                        if (distanceFromThisPoint < closestDistance)
                        {
                            closestDistance = distanceFromThisPoint;
                            patrolDestinationIndex = i;
                            currentPatrolDestination = aiCharacterPatrolPath.patrolPoints[i];
                        }
                    }
                }
                else
                {
                    currentPatrolDestination = aiCharacterPatrolPath.patrolPoints[patrolDestinationIndex];
                }
                
                hasPatrolDestination = true;
            }
            
            NavMeshPath path  = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(currentPatrolDestination, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }
    }
}
