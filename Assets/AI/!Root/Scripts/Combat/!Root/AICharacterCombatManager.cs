using System.Collections.Generic;
using UnityEngine;

namespace WEV.WhiteRoom
{
    public class AICharacterCombatManager : CharacterCombatManager
    {
        protected AICharacterManager aiCharacter; // Reference to the AI Character Manager script

        [Header("ACTION RECOVERY")]
        public float actionRecoveryTimer = 0; // Value to get the action recovery timer
        [SerializeField] float loseTargetDelay = 1.5f;
float loseTargetTimer;

        [Header("PIVOT")] 
        public bool enablePivot = true; // Check if the AI character can pivot or not

        [Header("TARGET INFORMATION")]
        public float distanceFromTarget; // Value to get the AI distance from target
        public float viewableAngle; // Value to get the viewable angle of the AI
        public Vector3 targetsDirection; // Value for the target's direction position

        [Header("DETECTION")]
        [SerializeField] float detectionRadius = 15; // Value for the radius to detect the target
        public float minimumFOV = -35; // Value for the minimum detection angle to detect the target
        public float maximumFOV = 35; // Value for the maximum detection angle to detect the target

        [Header("ATTACK ROTATION SPEED")]
        public float attackRotationSpeed = 25; // Value for the rotation speed of AI while attacking
        
        [Header("Activation Range")]
        public List<PlayerManager> playersWithinActivationRange =  new List<PlayerManager>();
        protected override void Awake()

        {
            base.Awake();

            aiCharacter = GetComponent<AICharacterManager>();
            lockOnTransform = GetComponentInChildren<CoreUtilityLockOnTransform>().transform;
        }
        public void AddPlayerToPlayersWithinActivationRange(PlayerManager player)
        {
            if(playersWithinActivationRange.Contains(player))
                return;
            
            playersWithinActivationRange.Add(player);

            for (int i = 0; i < playersWithinActivationRange.Count; i++)
            {
                if(playersWithinActivationRange[i] == null)
                    playersWithinActivationRange.RemoveAt(i);
            }
        }
        public void RemovePlayerFromPlayersWithinActivationRange(PlayerManager player)
        {
            if(!playersWithinActivationRange.Contains(player))
                return;
            
            playersWithinActivationRange.Remove(player);

            for (int i = 0; i < playersWithinActivationRange.Count; i++)
            {
                if(playersWithinActivationRange[i] == null)
                    playersWithinActivationRange.RemoveAt(i);
            }
        }
        public void FindATargetViaLineOfSight(AICharacterManager aiCharacter)
        {
            /* BELOW CODE: This doesn't work for some reason
            if (currentTarget != null)
                return;
            */

            if (currentTarget != null)
            {
                float distanceToTarget =
                    Vector3.Distance(aiCharacter.transform.position, currentTarget.transform.position);

                if (distanceToTarget <= detectionRadius)
                {
                    loseTargetTimer = 0f;
                    return;
                }

                loseTargetTimer += Time.deltaTime;

                if (loseTargetTimer >= loseTargetDelay)
                {
                    SetTarget(null);
                    loseTargetTimer = 0f;
                }

                return;
            }

            Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, CoreUtilityManager.instance.GetCharacterLayers());

            for(int i = 0; i < colliders.Length; i++) 
            { 
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                if (targetCharacter == null)
                    continue;

                if (targetCharacter == aiCharacter)
                    continue;

                if(targetCharacter.isDead)
                    continue;

                // BELOW CODE: Can AI attack the target, If so, make them the target
                if(CoreUtilityManager.instance.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup))
                {
                    // BELOW CODE: If a potential target is found, it has to be infront of the AI
                    Vector3 targetsDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                    float angleOfPotentialTarget = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                    if(angleOfPotentialTarget > minimumFOV && angleOfPotentialTarget < maximumFOV)
                    {
                        // BELOW CODE: Lastly, we check for the environment blockage
                        if(Physics.Linecast(aiCharacter.characterCombatManager.lockOnTransform.position, 
                            targetCharacter.characterCombatManager.lockOnTransform.position, 
                            CoreUtilityManager.instance.GetEnvironmentLayers()))
                        {
                            Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                            Debug.LogWarning("BLOCKED");
                        }
                        else
                        {
                            targetsDirection = targetCharacter.transform.position - transform.position;
                            viewableAngle = CoreUtilityManager.instance.GetAngleOfTarget(transform, targetsDirection);
                            aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                            Debug.Log("Target Found");

                            if (enablePivot)
                                PivotTowardsTarget(aiCharacter);
                        }
                    }
                }
            }
        }
        public virtual void PivotTowardsTarget(AICharacterManager aiCharacter)
        {
            // BELOW CODE: Play a pivot animation depending on viewable angle of target
            if (aiCharacter.isPerformingAction)
                return;

            //if (viewableAngle >= 20 && viewableAngle <= 60)
            //{
            //    aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation("Turn_Right_45_01", true);
            //}
            //else if(viewableAngle <= -20 && viewableAngle >= -60)
            //{
            //    aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation("Turn_Left_45_01", true);
            //}
            if (viewableAngle >= 61 && viewableAngle <= 110)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation("Turn_Right_90_01", true);
            }
            else if (viewableAngle <= -61 && viewableAngle >= -110)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation("Turn_Left_90_01", true);
            }
            //else if (viewableAngle >= 110 && viewableAngle <= 145)
            //{
            //    aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation("Turn_Right_135_01", true);
            //}
            //else if (viewableAngle <= -110 && viewableAngle >= -145)
            //{
            //    aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation("Turn_Left_135_01", true);
            //}
            else if (viewableAngle >= 146 && viewableAngle <= 180)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation("Turn_Right_180_01", true);
            }
            else if (viewableAngle <= -146 && viewableAngle >= -180)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetActionAnimation("Turn_Left_180_01", true);
            }
        }
        public void RotateTowardsAgent(AICharacterManager aiCharacter)
        {
            if(aiCharacter.aiCharacterLocomotionManager.isMoving)
            {
                aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
            }    
        }
        public void RotateTowardsTargetWhileAttacking(AICharacterManager aiCharacter)
        {
            if(currentTarget == null)
                return;

            // BELOW CODE: Check if the character can rotate
            if (!aiCharacter.aiCharacterLocomotionManager.canRotate)
                return;

            if (!aiCharacter.isPerformingAction)
                return;

            // BELOW CODE: If yes, then rotate towards target / A specified rotation speed during specified frames
            Vector3 targetDirection = currentTarget.transform.position - aiCharacter.transform.position;
            targetDirection.y = 0;
            targetDirection.Normalize();

            if(targetDirection == Vector3.zero)
                targetDirection = aiCharacter.transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);
        }
        public void UseActionRecovery(AICharacterManager aiCharacter)
        {
            if(actionRecoveryTimer > 0)
            {
                if(!aiCharacter.isPerformingAction)
                {
                    actionRecoveryTimer -= Time.deltaTime;
                }
            }
        }
        public void AwardSomethingOnDeath(PlayerManager player)
        {
            // BELOW CODE: Check if the player is friendly
            if (player.characterGroup == CharacterGroup.Enemy)
                return;
            
            // TO-DO: Award Something 
        }
    }
}
