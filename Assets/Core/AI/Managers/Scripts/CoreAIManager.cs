using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CoreAIManager : MonoBehaviour
    {
        public static CoreAIManager instance; // Static Instance for the WRLD AI MANAGER script

        [Header("LOADING")]
        public bool isPerformingLoadingOperation = false;

        [Header("CHARACTERS")]
        [SerializeField] List<AICharacterSpawnManager> aiCharacterSpawners; // List of all the AI character spawners
        [SerializeField] List<AICharacterManager> spawnedInCharacters; // List of all ai characters who are spawned in the scene
        private Coroutine spawnAllCharactersCoroutine;
        private Coroutine despawnAllCharactersCoroutine;
        private Coroutine resetAllCharactersCoroutine;

        [Header("BEACON PREFAB")]
        public GameObject proximityActivatorGameObject;

        /* TO-DO: Bosses
        [Header("BOSSES")]
        [SerializeField] List<AICharacterBossManager> spawnedInBosses;
        */

        [Header("PATROL PATHS")]
        [SerializeField] List<AICharacterPatrolPath> aiCharacterPatrolPaths = new List<AICharacterPatrolPath>();

        [Header("Dialogue Interactable Prefab Settings")]
        public GameObject dialogueInteractable;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SpawnCharacter(AICharacterSpawnManager aiCharacterSpawner)
        {
            aiCharacterSpawners.Add(aiCharacterSpawner);
            aiCharacterSpawner.AttemptToSpawnCharacter();
        }

        public void AddCharacterToSpawnedCharacterList(AICharacterManager character)
        {
            if (spawnedInCharacters.Contains(character))
                return;

            spawnedInCharacters.Add(character);

            /* TO-DO: Bosses
            AICharacterBossManager boss = character as AICharacterBossManager;

            if (boss != null)
            {
                if (spawnedInBosses.Contains(boss))
                    return;

                spawnedInBosses.Add(boss);
            }

            */

        }

        /* TO-DO: Bosses
        public AICharacterBossManager GetBossCharacterByID(int ID)
        {
            return spawnedInBosses.FirstOrDefault(boss => boss.bossID == ID);
        }
        */

        public void SpawnAllCharacters()
        {
            isPerformingLoadingOperation = true;

            if (spawnAllCharactersCoroutine != null)
                StopCoroutine(spawnAllCharactersCoroutine);

            spawnAllCharactersCoroutine = StartCoroutine(SpawnAllCharactersCoroutine());
        }
        private IEnumerator SpawnAllCharactersCoroutine()
        {
            for (int i = 0; i < aiCharacterSpawners.Count; i++)
            {
                yield return new WaitForFixedUpdate();
                aiCharacterSpawners[i].AttemptToSpawnCharacter();
                yield return null;
            }

            isPerformingLoadingOperation = false;

            yield return null;
        }
        public void ResetAllCharacters()
        {
            isPerformingLoadingOperation = true;

            if (resetAllCharactersCoroutine != null)
                StopCoroutine(resetAllCharactersCoroutine);

            resetAllCharactersCoroutine = StartCoroutine(ResetAllCharactersCoroutine());
        }
        private IEnumerator ResetAllCharactersCoroutine()
        {
            for (int i = 0; i < aiCharacterSpawners.Count; i++)
            {
                yield return new WaitForFixedUpdate();
                aiCharacterSpawners[i].ResetCharacter();
                yield return null;
            }

            isPerformingLoadingOperation = false;

            yield return null;
        }

        private void DespawnAllCharacters()
        {
            isPerformingLoadingOperation = true;

            if (despawnAllCharactersCoroutine != null)
                StopCoroutine(despawnAllCharactersCoroutine);

            despawnAllCharactersCoroutine = StartCoroutine(DespawnAllCharactersCoroutine());
        }

        private IEnumerator DespawnAllCharactersCoroutine()
        {
            for (int i = 0; i < spawnedInCharacters.Count; i++)
            {
                yield return new WaitForFixedUpdate();
                //spawnedInCharacters[i].GetComponent<NetworkObject>().Despawn();
                Destroy(spawnedInCharacters[i].gameObject);
                yield return null;
            }

            spawnedInCharacters.Clear();
            isPerformingLoadingOperation = false;
            yield return null;
        }
        private void DisableAllCharacters()
        {
            // TO-DO: Disable character gameObjects, sync disabled status on the network
            // TO-DO: Disable gameObjects for clients upon connecting, if disabled status is true
            // TO-DO: Can be used to disable characters that are far from players to save memory
            // TO-DO: Characters can be split into areas (Area_00_, Area_01, Area_02), etc
        }

        // PATROL PATHS
        public void AddPatrolPathToList(AICharacterPatrolPath patrolPath)
        {
            if (aiCharacterPatrolPaths.Contains(patrolPath))
                return;

            aiCharacterPatrolPaths.Add(patrolPath);
        }
        public AICharacterPatrolPath GetAICharacterPatrolPathByID(int patrolPathID)
        {
            AICharacterPatrolPath patrolPath = null;

            for (int i = 0; i < aiCharacterPatrolPaths.Count; i++)
            {
                if (aiCharacterPatrolPaths[i].patrolPathID == patrolPathID)
                    patrolPath = aiCharacterPatrolPaths[i];
            }

            return patrolPath;
        }
    }
}
