using UnityEngine;

namespace WEV.WhiteRoom
{
    public class ItemInteractablePickUp : ItemInteractable
    {
        [Header("Pick Up Settings")]
        public ItemPickUpType pickUpType;
        public bool willPickedUpItemBeAddedToHand = true;

        [Header("ITEM")] 
        public Item item;
        public int itemAmount = 1;
        public bool canDrop = true;
        
        [Header("WORLD SPAWN PICK UP")]
        [Tooltip("This is a unique id given to each world spawn item, so player may not loot them more than once")]
        public int worldSpawnInteractableID;
        public bool hasBeenLooted = false;

        private AudioSource audioSource;

        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
        }

        protected override void Start()
        {
            base.Start();

            if (pickUpType == ItemPickUpType.World)
                CheckIfWorldItemWasAlreadyLooted();
        }

        private void CheckIfWorldItemWasAlreadyLooted()
        {
            // BELOW CODE: Compare the data of the looted items I.D's with this item's I.D
            if (!CoreSaveGameManager.instance.currentCharacterData.coreItemsLooted.ContainsKey(worldSpawnInteractableID))
            {
                CoreSaveGameManager.instance.currentCharacterData.coreItemsLooted.Add(worldSpawnInteractableID, false);
            }

            hasBeenLooted = CoreSaveGameManager.instance.currentCharacterData.coreItemsLooted[worldSpawnInteractableID];

            // BELOW CODE: If it has been looted, hide the game object 
            if (hasBeenLooted)
                gameObject.SetActive(false);
        }

        public override void Interact(PlayerManager player)
        {
            if (player.isPerformingAction)
                return;

            // BELOW CODE: Play a SFX
            player.characterSoundFXManager.PlaySoundFX(CoreSoundFXManager.instance.pickUpItemSFX);

            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();

            // Add item to inventory
            player.playerInventoryManager.AddItemToInventory(item, itemAmount);
            item.currentItemAmount += itemAmount;

            // BELOW CODE: New Display Pop Up
            ShowPickUpPopUp();

            // BELOW CODE: Save Loot status if it's a world spawn
            if (pickUpType == ItemPickUpType.World)
            {
                if (CoreSaveGameManager.instance.currentCharacterData.coreItemsLooted.ContainsKey((int)worldSpawnInteractableID))
                {
                    CoreSaveGameManager.instance.currentCharacterData.coreItemsLooted.Remove(worldSpawnInteractableID);
                }

                CoreSaveGameManager.instance.currentCharacterData.coreItemsLooted.Add(worldSpawnInteractableID, true);
            }

            // BELOW CODE: Add item to hand and pick up world object
            if (willPickedUpItemBeAddedToHand)
            {
                player.playerInventoryManager.currentItemInHand = item;
                player.playerInventoryManager.PickUpWorldObject(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ShowPickUpPopUp()
        {
            GameObject animatedPopUp = Instantiate(
                PlayerUIManager.instance.playerUIPopUpManager.animatedPopUp,
                PlayerUIManager.instance.playerUIPopUpManager.popUpOrganiser.GetComponent<Transform>());

            animatedPopUp.transform.SetSiblingIndex(0);

            UIAnimatedPopUp animatedPopUpUI = animatedPopUp.GetComponent<UIAnimatedPopUp>();

            animatedPopUpUI.StartCoroutine(
                animatedPopUpUI.ShowAnimatedPopUp(item.itemName, itemAmount));
        }
    }
}