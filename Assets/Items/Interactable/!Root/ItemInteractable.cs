using System;
using UnityEngine;

namespace WEV.WhiteRoom
{
    public class ItemInteractable : MonoBehaviour
    {
        [Header("INTERACTABLE DATA")] 
        public string interactableText; // What will be written when interacting to this item
        public Collider interactableCollider; // Reference to the collider for the interaction
        public InteractableType interactableType;

        protected virtual void Awake()
        {
            if(interactableCollider == null)
                interactableCollider = GetComponent<Collider>();    
        }
        protected virtual void Start()
        {
            
        }
        public virtual void Interact(PlayerManager player)
        {   
            Debug.Log("You have interacted successfully");
            
            // BELOW CODE: Remove the interaction to the player
            interactableCollider.enabled = false;
            player.playerInteractionManager.RemoveInteractionFromList(this);
            
            // BELOW CODE: Close all the other UI popup
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
        }
        public virtual void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
            {
                // BELOW CODE: Pass the interaction to the player
                player.playerInteractionManager.AddInteractionToList(this);
            }
        }
        public virtual void OnTriggerExit(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
            {
                // BELOW CODE: Remove the interaction to the player
                player.playerInteractionManager.RemoveInteractionFromList(this);

                // BELOW CODE: Close all the other UI popup
                PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            }
        }
        
        public void EnableInteraction()
        {
            if (interactableCollider != null)
                interactableCollider.enabled = true;
        }
    }
}
