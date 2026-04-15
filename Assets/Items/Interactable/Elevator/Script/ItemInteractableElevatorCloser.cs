using UnityEngine;

namespace WEV.WhiteRoom
{
    public class ItemInteractableElevatorCloser : ItemInteractable
    {
        ItemInteractableElevator mainElevator;

        protected override void Awake()
        {
            mainElevator = GetComponentInParent<ItemInteractableElevator>();
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (mainElevator != null)
            {
                CharacterManager character = other.GetComponent<CharacterManager>();
                mainElevator.CloseElevatorDoor(character);
                CoreSaveGameManager.instance.SaveGame();

                // BELOW CODE: Remove the interaction to the player
                interactableCollider.enabled = false;
                PlayerUIManager.instance.player.playerInteractionManager.RemoveInteractionFromList(this);
            }
        }
    }
}
