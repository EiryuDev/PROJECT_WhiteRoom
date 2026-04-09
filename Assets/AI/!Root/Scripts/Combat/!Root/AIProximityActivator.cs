using UnityEngine;

namespace WEV.WhiteRoom
{
    public class AIProximityActivator : MonoBehaviour
    {
        [SerializeField] AICharacterManager proximityActivatorOwner;

        public void SetOwnerOfProximityActivator(AICharacterManager aiCharacter)
        {
            proximityActivatorOwner = aiCharacter;
        }

        public void ReactivateAICharacter(PlayerManager player)
        {
            if (proximityActivatorOwner == null)
                return;

            proximityActivatorOwner.ActivateCharacter(player);
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerProximityActivatorDetector detector = other.GetComponent<PlayerProximityActivatorDetector>();
            
            if(detector == null)
                return;
            
            ReactivateAICharacter(detector.player);
        }
    }
}
