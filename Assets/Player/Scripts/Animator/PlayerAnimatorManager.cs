using UnityEngine;

namespace WEV.WhiteRoom
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        private PlayerManager player; // Reference to the Player Manager script

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        private void OnAnimatorMove()
        {
            if(applyRootMotion)
            {
                // BELOW CODE: Take the rotation from particular animation and apply to the player rotation
                Vector3 velocity = player.animator.deltaPosition;
                player.controller.Move(velocity);
                player.transform.rotation *= player.animator.deltaRotation;
            }
        }
    }
}
