using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEV.WhiteRoom
{
    public class WorldUtilityManager : MonoBehaviour
    {
        public static WorldUtilityManager instance; // Static instance for the WRLD UTILITY MANAGER script

        [Header("LAYERS")]
        [SerializeField] LayerMask characterLayers; // Reference to the Layer Mask for the character layer
        [SerializeField] LayerMask environmentLayers; // Reference to the layer mask for the environment layer
        [SerializeField] LayerMask slipperyEnvironmentLayers;

        [Header("FORCES")] 
        public float slopeSlideForce = -15;
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
            
            DontDestroyOnLoad(gameObject);
        }
        public LayerMask GetCharacterLayers() 
        {
            return characterLayers;
        }
        public LayerMask GetEnvironmentLayers()
        {
            return environmentLayers;
        }
        public LayerMask GetSlipperyEnvironmentLayers()
        {
            return slipperyEnvironmentLayers;
        }
        public bool CanIDamageThisTarget(CharacterGroup attackingCharacter, CharacterGroup targetCharacter)
        {
            if(attackingCharacter == CharacterGroup.Player)
            {
                switch (targetCharacter)
                {
                    case CharacterGroup.Player:
                        return false;
                    case CharacterGroup.Enemy:
                        return true;
                    default:
                        break;
                }
            }
            else if(attackingCharacter == CharacterGroup.Enemy)
            {
                switch (targetCharacter)
                {
                    case CharacterGroup.Player:
                        return true;
                    case CharacterGroup.Enemy:
                        return false;
                    default:
                        break;
                }
            }

            return false;
        }
        public float GetAngleOfTarget(Transform characterTransform, Vector3 targetsDirection)
        {
            targetsDirection.y = 0;
            float viewableAngle = Vector3.Angle(characterTransform.forward, targetsDirection);
            Vector3 cross = Vector3.Cross(characterTransform.forward, targetsDirection);

            if (cross.y < 0) 
                viewableAngle = -viewableAngle;

            return viewableAngle;
        }
        
    }
}
