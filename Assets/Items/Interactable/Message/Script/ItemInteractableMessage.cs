using UnityEngine;

namespace WEV.WhiteRoom
{
    public class ItemInteractableMessage : ItemInteractable
    {
        [Header("Message Settings")]
        [SerializeField] string messagePopUp;

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            CoreSaveGameManager.instance.SaveGame();

            PlayerUIManager.instance.playerUIPopUpManager.SendPlayerMessagePopUp(messagePopUp);

            // TO-DO: Optionally play sfx here
        }
    }
}
