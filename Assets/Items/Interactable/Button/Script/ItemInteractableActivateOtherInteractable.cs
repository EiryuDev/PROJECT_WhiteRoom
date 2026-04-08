using System;
using UnityEngine;

namespace WEV.WhiteRoom
{
    public class ItemInteractableActivateOtherInteractable : ItemInteractable
    {
        public bool buttonHasBeenPushed = false;

        [Header("Interactable")]
        [SerializeField] private ItemInteractable interactableObject;

        [Header("Use Once")]
        [SerializeField] private bool useOnce = true;

        [Header("Animator Settings")]
        [SerializeField] private Animator animator;
        [SerializeField] private string pushButtonAnimation;
        [SerializeField] private string releaseButtonAnimation;
        [SerializeField] private string pushedButtonAnimation;

        public override void Interact(PlayerManager player)
        {
            UseButton();

            // BELOW CODE: Save game after interacting
            CoreSaveGameManager.instance.SaveGame();

            if (interactableObject == null)
                return;

            interactableObject.Interact(player);
        }

        protected override void Start()
        {
            base.Start();

            if (buttonHasBeenPushed)
                animator.Play(pushedButtonAnimation);
        }

        private void UseButton()
        {
            AttemptToPullButton();
        }

        private void AttemptToPullButton()
        {
            // BELOW CODE: Remove the interaction from the player
            if (useOnce)
            {
                interactableCollider.enabled = false;
                PlayerUIManager.instance.player.playerInteractionManager.RemoveInteractionFromList(this);
            }

            animator.Play(pushButtonAnimation);

            buttonHasBeenPushed = true;
        }
    }
}