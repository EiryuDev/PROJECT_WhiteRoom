using UnityEngine;

namespace WEV.WhiteRoom
{
    public class AICharacterAnimatorManager : CharacterAnimatorManager
    {
        AICharacterManager aiCharacter; // Reference to the AI Character Manager script
        
        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }

        private void OnAnimatorMove()
        {
            if (!aiCharacter.aiCharacterLocomotionManager.isGrounded)
                return;

            Vector3 velocity = aiCharacter.animator.deltaPosition;

            aiCharacter.controller.Move(velocity);
            aiCharacter.transform.rotation *= aiCharacter.animator.deltaRotation;
        }
    }
}
