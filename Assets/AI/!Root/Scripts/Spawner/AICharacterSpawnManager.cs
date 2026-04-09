using UnityEngine;

namespace WEV.WhiteRoom
{
    public class AICharacterSpawnManager : MonoBehaviour
    {
        [Header("CHARACTERS")]
        [SerializeField] GameObject characterGameObject; // Reference to the character game object
        [SerializeField] GameObject instantiatedGameObject; // Reference to the instantiated game object
        private AICharacterManager aiCharacter;

        [Header("PATROL")] 
        [SerializeField] private bool hasPatrolPath = false;
        [SerializeField] private int patrolPathID = 0;
        
        private void Start()
        {
            CoreAIManager.instance.SpawnCharacter(this);
            gameObject.SetActive(false);
        }
        public void AttemptToSpawnCharacter()
        {
            if(characterGameObject != null)
            {
                instantiatedGameObject = Instantiate(characterGameObject);  
                instantiatedGameObject.transform.position = transform.position;
                instantiatedGameObject.transform.rotation = transform.rotation;
                aiCharacter = instantiatedGameObject.GetComponent<AICharacterManager>();

                if (aiCharacter == null)
                    return;
                
                CoreAIManager.instance.AddCharacterToSpawnedCharacterList(aiCharacter);
                
                if(hasPatrolPath)
                    aiCharacter.idle.aiCharacterPatrolPath = CoreAIManager.instance.GetAICharacterPatrolPathByID(patrolPathID);

                //aiCharacter.aiCharacterNetworkManager.isActive.Value = false;
            }
        }
        public void ResetCharacter()
        {
            if(instantiatedGameObject == null)
                return;
            
            if(aiCharacter == null)
                return;
            
            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;
            aiCharacter.characterStatsManager.currentHealth =
                aiCharacter.characterStatsManager.maxHealth;
            aiCharacter.aiCharacterCombatManager.SetTarget(null);

            if (aiCharacter.isDead)
            {
                aiCharacter.isDead = false;
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation
                    ("Empty", false, false, true, true);
                aiCharacter.currentState.SwitchState(aiCharacter, aiCharacter.idle);
            }

            aiCharacter.aiCharacterUIManager.ResetCharacterHPBar();
        }
    }
}
