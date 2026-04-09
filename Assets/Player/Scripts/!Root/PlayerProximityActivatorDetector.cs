using UnityEngine;

namespace WEV.WhiteRoom
{ 
    public class PlayerProximityActivatorDetector : MonoBehaviour
    {
        // Q: What is this?
        // A: A trigger that enables or disables AI characters
        // based on player proximity (optimization system)

        public PlayerManager player;

        private void OnTriggerEnter(Collider other)
        {
            
        }

        private void OnTriggerExit(Collider other)
        {
            AICharacterManager aiCharacter = other.GetComponent<AICharacterManager>();
            
            if(aiCharacter != null)
                aiCharacter.DeactivateCharacter(player);
        }
    }
}
