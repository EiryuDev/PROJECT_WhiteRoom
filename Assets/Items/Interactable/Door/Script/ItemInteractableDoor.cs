using UnityEngine;

namespace WEV.WhiteRoom
{
    public class ItemInteractableDoor : ItemInteractable
    {
        [Header("Status Settings")]
        public bool isOpen = false;
        [SerializeField] private string doorID;

        [Header("Key Settings")]
        [SerializeField] private bool requiresItem = false;
        [SerializeField] private Item itemRequiredToOpen;

        [Header("Animation Settings")]
        [SerializeField] private Animator animator;
        [SerializeField] private string openDoorAnimation;
        [SerializeField] private string openedDoorAnimation;
        [SerializeField] private string closeDoorAnimation;

        [Header("SFX Settings")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip doorOpeningSFX;
        [SerializeField] private AudioClip doorCloseSFX;

        [Header("Levers & Button Settings")]
        [SerializeField] private ItemInteractableActivateOtherInteractable[] leversAndButtons;

        [Header("Cannot Open From A Side Settings")]
        [SerializeField] private ItemInteractableMessage cannotOpenFromThisSideInteractable;

        protected override void Start()
        {
            doorID = gameObject.scene.buildIndex + " " + gameObject.name;

            // Check if door already opened in save data
            for (int i = 0; i < CoreSaveGameManager.instance.currentCharacterData.doorsOpened.Count; i++)
            {
                if (CoreSaveGameManager.instance.currentCharacterData.doorsOpened[i] == null)
                    continue;

                if (CoreSaveGameManager.instance.currentCharacterData.doorsOpened[i] == doorID)
                    isOpen = true;
            }

            OnIsOpenChanged();

            CheckIfDoorIsAlreadyOpened();
        }

        private void DisableDoorInteractable()
        {
            interactableCollider.enabled = false;

            for (int i = 0; i < leversAndButtons.Length; i++)
            {
                if (leversAndButtons[i] == null)
                    continue;

                leversAndButtons[i].interactableCollider.enabled = false;
                PlayerUIManager.instance.player.playerInteractionManager.RemoveInteractionFromList(leversAndButtons[i]);
            }

            if (cannotOpenFromThisSideInteractable != null)
            {
                cannotOpenFromThisSideInteractable.interactableCollider.enabled = false;
                PlayerUIManager.instance.player.playerInteractionManager.RemoveInteractionFromList(cannotOpenFromThisSideInteractable);
            }
        }

        private void OnIsOpenChanged()
        {
            if (isOpen)
            {
                DisableDoorInteractable();
            }
        }

        private void CheckIfDoorIsAlreadyOpened()
        {
            if (isOpen)
            {
                animator.Play(openedDoorAnimation);
                interactableCollider.enabled = false;
            }
        }

        private bool PlayerHasKey(PlayerManager player)
        {
            bool hasKey = false;

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                if (player.playerInventoryManager.itemsInInventory[i] == null)
                    continue;

                if (player.playerInventoryManager.itemsInInventory[i].itemID ==
                    itemRequiredToOpen.itemID)
                {
                    return true;
                }
            }

            return hasKey;
        }

        public override void Interact(PlayerManager player)
        {
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();

            CoreSaveGameManager.instance.SaveGame();

            if (requiresItem && PlayerHasKey(player))
            {
                UseDoor();
                player.playerInteractionManager.RemoveInteractionFromList(this);
                PlayerUIManager.instance.playerUIPopUpManager.SendPlayerMessagePopUp("Used " + itemRequiredToOpen.itemName + ".");
                player.playerInventoryManager.RemoveItemFromInventory(itemRequiredToOpen);
                return;
            }
            else if (requiresItem && !PlayerHasKey(player))
            {
                PlayerUIManager.instance.playerUIPopUpManager.SendPlayerMessagePopUp("It's locked");
                return;
            }

            UseDoor();
            player.playerInteractionManager.RemoveInteractionFromList(this);
        }

        private void UseDoor()
        {
            isOpen = true;

            if (!CoreSaveGameManager.instance.currentCharacterData.doorsOpened.Contains(doorID))
            {
                CoreSaveGameManager.instance.currentCharacterData.doorsOpened.Add(doorID);
            }

            OnIsOpenChanged();

            AttemptToOpenDoor();
        }

        private void AttemptToOpenDoor()
        {
            animator.Play(openDoorAnimation);
            audioSource.PlayOneShot(doorOpeningSFX);
            interactableCollider.enabled = false;
        }

        public void CloseDoor()
        {
            if (!isOpen)
                return;

            isOpen = false;

            if (CoreSaveGameManager.instance.currentCharacterData.doorsOpened.Contains(doorID))
            {
                CoreSaveGameManager.instance.currentCharacterData.doorsOpened.Remove(doorID);
            }

            animator.Play(closeDoorAnimation);
            audioSource.PlayOneShot(doorCloseSFX);
        }
    }
}