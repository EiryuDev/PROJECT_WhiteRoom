using UnityEngine;

namespace WEV.WhiteRoom
{
    public class ItemInteractablePickUp : ItemInteractable
    {
        public ItemPickUpType pickUpType;

        [Header("ITEM")] 
        public Item item;
        public int itemAmount = 1;
        public bool canDrop = true;
        
        [Header("WORLD SPAWN PICK UP")] 
        public bool hasBeenLooted = false;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            if (pickUpType == ItemPickUpType.World)
                CheckIfWorldItemWasAlreadyLooted();
        }

        private void CheckIfWorldItemWasAlreadyLooted()
        {
            if (hasBeenLooted)
            {
                gameObject.SetActive(false);
            }
        }

        public override void Interact(PlayerManager player)
        {
            if (player.isPerformingAction)
                return;

            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();

            // Add item to inventory
            player.playerInventoryManager.AddItemToInventory(item, itemAmount);
            item.currentItemAmount += itemAmount;

            player.playerInventoryManager.currentItemInHand = item;

            // Mark as looted (for world items)
            if (pickUpType == ItemPickUpType.World)
                hasBeenLooted = true;

            player.playerInventoryManager.PickUpWorldObject(gameObject);

            // Destroy object locally
            //Destroy(gameObject);
        }
    }
}