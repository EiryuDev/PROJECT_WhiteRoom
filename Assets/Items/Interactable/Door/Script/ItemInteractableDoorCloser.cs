using UnityEngine;

namespace WEV.WhiteRoom
{
    public class ItemInteractableDoorCloser : ItemInteractable
    {
        ItemInteractableDoor mainDoor;

        protected override void Awake()
        {
            mainDoor = GetComponentInParent<ItemInteractableDoor>();
        }

        public override void OnTriggerEnter(Collider other)
        {
            if(mainDoor != null)
            {
                mainDoor.CloseDoor();
                CoreSaveGameManager.instance.SaveGame();

                // BELOW CODE: Remove the interaction to the player
                interactableCollider.enabled = false;
                PlayerUIManager.instance.player.playerInteractionManager.RemoveInteractionFromList(this);
            }
        }
    }
}
