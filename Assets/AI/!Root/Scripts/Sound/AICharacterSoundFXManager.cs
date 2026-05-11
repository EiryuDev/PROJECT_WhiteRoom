using UnityEngine;

namespace WEV.WhiteRoom
{
    public class AICharacterSoundFXManager : CharacterSoundFXManager
    {
        private AICharacterManager aiCharacter;

        [Header("DIALOGUE DATA")]
        [Tooltip("Character Dialogue ID (We will load possible dialogues from a database per dialogue character ID)")]
        public CharacterDialogueID characterDialogueID;
        public CharacterDialogueType characterDialogueType;
        public GameObject interactableDialogueCollider;
        public CharacterDialogue currentDialogue;
        public GameObject interactableDialogueObject;
        public bool dialogueIsPlaying = false;
        public ItemInteractableDialogue dialogueInteractable;

        [Header("EVENT AUDIO SOURCE")]
        public AudioSource eventAudioSource;

        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }

        protected override void Start()
        {
            base.Start();

            if (characterDialogueID != CharacterDialogueID.NoDialogueID)
            {
                currentDialogue = CoreSaveGameManager.instance.GetCharacterDialogueByEnum(characterDialogueID);
                interactableDialogueObject = Instantiate(CoreAIManager.instance.dialogueInteractable, transform);
                dialogueInteractable = interactableDialogueObject.GetComponent<ItemInteractableDialogue>();
            }
        }

        public virtual void PlayCurrentDialogueEvent()
        {
            if (currentDialogue == null)
                return;

            if (!dialogueIsPlaying)
            {
                currentDialogue.PlayDialogueEvent(aiCharacter);
            }
            else
            {
                if (characterDialogueType == CharacterDialogueType.Standalone)
                    PlayerUIManager.instance.playerUIPopUpManager.SendNextDialoguePopUpIndex(currentDialogue, aiCharacter);
                else
                    aiCharacter.aiCharacterUIManager.uiDialogue.SendNextDialoguePopUpIndex(currentDialogue, aiCharacter);
            }
        }

        public virtual void CancelCurrentDialogueEvent()
        {
            if (dialogueIsPlaying)
            {
                dialogueIsPlaying = false;

                if (characterDialogueType == CharacterDialogueType.Standalone)
                    PlayerUIManager.instance.playerUIPopUpManager.CancelDialoguePopUp(aiCharacter);
                else
                    aiCharacter.aiCharacterUIManager.uiDialogue.CancelDialoguePopUp(aiCharacter);
            }
        }

        // BELOW CODE: Used for specific calls when a dialogue is over (npc dies, shop opens, etc.)
        public virtual void OnCurrentDialogueEnded()
        {
            currentDialogue = CoreSaveGameManager.instance.GetCharacterDialogueByEnum(characterDialogueID);

            // BELOW CODE: Re-activate the collider for hover dialogue, so you can talk again
            if (aiCharacter.aiCharacterSoundFXManager.characterDialogueType == CharacterDialogueType.Hover)
            {
                dialogueIsPlaying = false;

                if (dialogueInteractable != null &&
                    dialogueInteractable.interactableCollider != null)
                {
                    dialogueInteractable.interactableCollider.enabled = true;
                }
            }
        }
    }
}
