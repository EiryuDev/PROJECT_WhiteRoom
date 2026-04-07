using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace WEV.WhiteRoom
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        private PlayerManager player; // Reference to the Player Manager script

        [SerializeField] private List<ItemInteractable> currentInteractableActions; 

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            currentInteractableActions = new List<ItemInteractable>();
        }

        private void FixedUpdate()
        {
            // BELOW CODE: If player's ui menu is not open, and player don't have pop up (current interaction message) check for interactable
            if (!PlayerUIManager.instance.isMenuWindowOpen && !PlayerUIManager.instance.isPopUpWindowOpen)
                CheckForInteractable();
        }

        private void CheckForInteractable()
        {
            if (currentInteractableActions.Count == 0)
                return;

            if (currentInteractableActions[0] == null)
            {
                // BELOW CODE: If the current interactable item at position 0 becomes null (removed from the game), we remove position 0 from the list
                currentInteractableActions.RemoveAt(0);
                return;
            }
            
            // BELOW CODE: If we have an interactable action and we have not notified our player, so we do here
            if (currentInteractableActions[0] != null)
            {
                PlayerUIManager.instance.playerUIPopUpManager.SendPlayerMessagePopUp(currentInteractableActions[0]
                    .interactableText);

                if (currentInteractableActions[0].interactableType == InteractableType.NPC)
                {
                    // TO-DO: Call the NPC related interation events
                }
            }

        }
        
        public void Interact()
        {
            // BELOW CODE: Pressing the interact button with or without interactable, it will clear the pop up windows
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            
            if (player.playerInventoryManager.currentHeldObject != null)
            {
                player.playerInventoryManager.DropObject();
                return;
            }

            if (currentInteractableActions.Count == 0)
                return;
            
            if (currentInteractableActions[0] != null)
            {
                currentInteractableActions[0].Interact(player);
                RefreshInteractionList();
            }
        }
        public void RefreshInteractionList()
        {
            for (int i = currentInteractableActions.Count - 1; i > -1; i--)
            {
                if(currentInteractableActions[i] == null)
                    currentInteractableActions.RemoveAt(i);
            }
        }
        public void AddInteractionToList(ItemInteractable interactableObject)
        {
            RefreshInteractionList();

            if (!currentInteractableActions.Contains(interactableObject))
                currentInteractableActions.Add(interactableObject);
        }
        public void RemoveInteractionFromList(ItemInteractable interactableObject)
        {
            if (currentInteractableActions.Contains(interactableObject))
            {
            
                currentInteractableActions.Remove(interactableObject);
            }   
            
            RefreshInteractionList();
        }
    }
}
