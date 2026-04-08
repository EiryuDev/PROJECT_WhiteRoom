using UnityEngine;

namespace WEV.WhiteRoom
{
    public class ItemInteractableActivateOtherInteractable : ItemInteractable
    {
        [Header("Interactable")]
        [SerializeField] ItemInteractable interactableObject;

        [Header("Use Once")]
        [SerializeField] bool useOnce = true;

        [Header("Animator Settings")]
        [SerializeField] Animator animator;
        [SerializeField] string pullLeverAnimation;
        [SerializeField] string releaseLeverAnimation;

        public override void Interact(PlayerManager player)
        {
            // BELOW CODE: Remove the interaction from the player
            interactableCollider.enabled = false;
            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            animator.Play(pullLeverAnimation);


            if (!useOnce)
            {

 
            }

            // BELOW CODE: Save game after interacting
            CoreSaveGameManager.instance.SaveGame();

            if (interactableObject == null)
                return;

            interactableObject.Interact(player);
        }
    }
}
