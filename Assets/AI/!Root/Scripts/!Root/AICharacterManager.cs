using UnityEngine;
using UnityEngine.AI;

namespace WEV.WhiteRoom
{
    public class AICharacterManager : CharacterManager
    {
        [HideInInspector] public AICharacterAnimatorManager aiCharacterAnimatorManager;
        [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;
        [HideInInspector] public AICharacterStatsManager aiCharacterStatsManager;
        [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
        [HideInInspector] public AICharacterSoundFXManager aiCharacterSoundFXManager;
        [HideInInspector] public AICharacterInventoryManager aiCharacterInventoryManager;
        [HideInInspector] public AICharacterUIManager aiCharacterUIManager;

        [Header("NAVMESH AGENT")]
        public NavMeshAgent navMeshAgent;

        [Header("CURRENT STATE")]
        public CoreAIState currentState;

        [Header("STATES")]
        public CoreAIIdleState idle;
        public CoreAIPursueTargetState pursueTarget;
        public CoreAICombatStanceState combatStance;
        public CoreAIAttackState attack;

        [Header("Proximity Activator")]
        protected AIProximityActivator proximityActivator;

        protected override void Awake()
        {
            base.Awake();

            aiCharacterAnimatorManager = GetComponent<AICharacterAnimatorManager>();
            aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
            aiCharacterStatsManager = GetComponent<AICharacterStatsManager>();
            aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
            aiCharacterSoundFXManager = GetComponent<AICharacterSoundFXManager>();
            aiCharacterInventoryManager = GetComponent<AICharacterInventoryManager>();
            aiCharacterUIManager = GetComponent<AICharacterUIManager>();

            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        }

        protected override void Start()
        {
            base.Start();

            idle = Instantiate(idle);
            pursueTarget = Instantiate(pursueTarget);
            combatStance = Instantiate(combatStance);
            attack = Instantiate(attack);
            currentState = idle;

            CreateProximityActivator();
        }

        protected override void OnEnable()
        {
            //if (aiCharacterUIManager.hasFloatingHPBar)
            //    aiCharacterUIManager.OnHPChanged(0, characterStatsManager.currentHealth);
        }

        private void OnDestroy()
        {
            if (proximityActivator != null)
                Destroy(proximityActivator.gameObject);
        }

        protected override void Update()
        {
            base.Update();

            aiCharacterCombatManager.UseActionRecovery(this);

            if (navMeshAgent == null)
                return;

            ProcessStateMachine();

            if (!navMeshAgent.enabled)
                return;

            Vector3 positionDifference = navMeshAgent.transform.position - transform.position;

            if (positionDifference.magnitude > 0.2f)
                navMeshAgent.transform.localPosition = Vector3.zero;
        }

        private void ProcessStateMachine()
        {
            CoreAIState nextState = null;

            if (currentState != null)
            {
                nextState = currentState.Tick(this);
            }

            if (nextState != null)
            {
                currentState = nextState;
            }

            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;

            if (aiCharacterCombatManager.currentTarget != null)
            {
                aiCharacterCombatManager.targetsDirection =
                    aiCharacterCombatManager.currentTarget.transform.position - transform.position;

                aiCharacterCombatManager.viewableAngle =
                    CoreUtilityManager.instance.GetAngleOfTarget(
                        transform,
                        aiCharacterCombatManager.targetsDirection);

                aiCharacterCombatManager.distanceFromTarget =
                    Vector3.Distance(
                        transform.position,
                        aiCharacterCombatManager.currentTarget.transform.position);
            }

            if (navMeshAgent.enabled)
            {
                Vector3 agentDestination = navMeshAgent.destination;
                float remainingDistance = Vector3.Distance(agentDestination, transform.position);

                if (remainingDistance > navMeshAgent.stoppingDistance)
                {
                    aiCharacterLocomotionManager.isMoving = true;
                }
                else
                {
                    aiCharacterLocomotionManager.isMoving = false;
                }
            }
            else
            {
                aiCharacterLocomotionManager.isMoving = false;
            }
        }

        public void ActivateCharacter(PlayerManager player)
        {
            aiCharacterCombatManager.AddPlayerToPlayersWithinActivationRange(player);

            if (aiCharacterCombatManager.playersWithinActivationRange.Count > 0)
            {
                aiCharacterLocomotionManager.enabled = true;
            }
            else
            {
                aiCharacterLocomotionManager.enabled = false;
            }
        }

        public void DeactivateCharacter(PlayerManager player)
        {
            aiCharacterCombatManager.RemovePlayerFromPlayersWithinActivationRange(player);

            if (proximityActivator != null)
            {
                proximityActivator.transform.position = transform.position;
                proximityActivator.gameObject.SetActive(true);
            }

            if (aiCharacterCombatManager.playersWithinActivationRange.Count > 0)
            {
                aiCharacterLocomotionManager.enabled = true;
            }
            else
            {
                aiCharacterCombatManager.SetTarget(null);
                aiCharacterLocomotionManager.enabled = false;
            }
        }

        public void CreateProximityActivator()
        {
            if (proximityActivator == null)
            {
                GameObject proximityActivatorGameObject =
                    Instantiate(CoreAIManager.instance.proximityActivatorGameObject);

                proximityActivatorGameObject.transform.position = transform.position;

                proximityActivator =
                    proximityActivatorGameObject.GetComponent<AIProximityActivator>();

                proximityActivator.SetOwnerOfProximityActivator(this);
            }
            else
            {
                proximityActivator.transform.position = transform.position;
                proximityActivator.gameObject.SetActive(true);
            }
        }
    }
}