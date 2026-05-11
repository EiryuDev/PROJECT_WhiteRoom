using UnityEngine;
using WEV.WhiteRoom;

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
            if (characterGameObject != null)
            {
                instantiatedGameObject = Instantiate(characterGameObject);
                instantiatedGameObject.transform.position = transform.position;
                instantiatedGameObject.transform.rotation = transform.rotation;
                aiCharacter = instantiatedGameObject.GetComponent<AICharacterManager>();

                if (aiCharacter == null)
                    return;

                CoreAIManager.instance.AddCharacterToSpawnedCharacterList(aiCharacter);

                if (hasPatrolPath)
                    aiCharacter.idle.aiCharacterPatrolPath = CoreAIManager.instance.GetAICharacterPatrolPathByID(patrolPathID);
            }
        }

        public void ResetCharacter()
        {
            if (instantiatedGameObject == null)
                return;

            if (aiCharacter == null)
                return;

            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;
            aiCharacter.aiCharacterStatsManager.currentHealth = aiCharacter.aiCharacterStatsManager.maxHealth;

            if (aiCharacter.isDead)
            {
                aiCharacter.isDead = false;
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation
                    ("Empty", false, false, true, true);
            }

            aiCharacter.aiCharacterUIManager.ResetCharacterHPBar();
        }
    }
}
