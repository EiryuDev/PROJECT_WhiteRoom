using UnityEngine;

namespace WEV.WhiteRoom
{
    public class ItemInteractableDialogue : ItemInteractable
    {
        private AICharacterManager aiCharacter;

        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponentInParent<AICharacterManager>();
        }
        public override void Interact(PlayerManager player)
        {
            if (PlayerUIManager.instance.isMenuWindowOpen)
                return;

            if (aiCharacter.aiCharacterSoundFXManager.characterDialogueType == CharacterDialogueType.Hover)
                base.Interact(player);

            if (aiCharacter.isDead)
            {
                interactableCollider.enabled = false;
                return;
            }

            CoreSaveGameManager.instance.SaveGame();

            // BELOW CODE: Play current dialogue
            aiCharacter.aiCharacterSoundFXManager.PlayCurrentDialogueEvent();

            // TO-DO: Use Face IK tracking to look at the player
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (aiCharacter.isDead)
            {
                interactableCollider.enabled = false;

                // BELOW CODE: If there is an active dialogue with this character and the player end it
                PlayerManager player = other.GetComponent<PlayerManager>();

                if (player != null)
                    aiCharacter.aiCharacterSoundFXManager.CancelCurrentDialogueEvent();
            }

            base.OnTriggerEnter(other);
        }
        public override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);

            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null)
                return;

            // BELOW CODE: Cancel current dialogue (if any) with this character when player leaves interaction radius
            aiCharacter.aiCharacterSoundFXManager.CancelCurrentDialogueEvent();
            // BELOW CODE: Close all menu related to this character (blacksmith, shop, etc)
            // BELOW CODE: Reset head ik tracking if enabled
        }

        public void ShowNPCName()
        {
            // BELOW CODE: Show the AI Character Name
            if (aiCharacter.aiCharacterUIManager.uiDialogue != null)
                aiCharacter.aiCharacterUIManager.uiDialogue.popUpDialogueText.text = aiCharacter.characterName;
        }
        public void HideNPCName()
        {
            // BELOW CODE: Show the AI Character Name
            if (aiCharacter.aiCharacterUIManager.uiDialogue != null)
                aiCharacter.aiCharacterUIManager.uiDialogue.popUpDialogueText.text = "";
        }
    }
}