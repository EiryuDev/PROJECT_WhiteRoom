using UnityEngine;
using TMPro;

namespace WEV.WhiteRoom
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {
        [Header("Message Pop Up Settings")] 
        [SerializeField] GameObject popUpMessageGameObject; 
        [SerializeField] TextMeshProUGUI popUpMessageText; 

        [Header("Animated Pop Up Settings")] 
        public GameObject popUpOrganiser;
        public GameObject animatedPopUp;

        [Header("Dialogue Pop Up Settings")]
        [SerializeField] GameObject popUpDialogueGameObject;
        [SerializeField] TextMeshProUGUI popUpDialogueText;

        [Header("Current Dialogue Settings")]
        [SerializeField] CharacterDialogue currentDialogue;

        private Coroutine dialogueCoroutine;

        public void SendPlayerMessagePopUp(string messageText)
        {
            PlayerUIManager.instance.isPopUpWindowOpen = true;
            popUpMessageText.text = messageText;
            popUpMessageGameObject.SetActive(true);
        }

        public void CloseAllPopUpWindows()
        {
            popUpMessageGameObject.SetActive(false);
            PlayerUIManager.instance.isPopUpWindowOpen = false;
        }

        public void SendDialoguePopUp(CharacterDialogue dialogue, AICharacterManager aiCharacter)
        {
            PlayerUIManager.instance.playerHUDManager.ToggleHUDWithOutPopUps(false);
            currentDialogue = dialogue;

            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);

            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            PlayerUIManager.instance.isPopUpWindowOpen = true;

            dialogueCoroutine = StartCoroutine(dialogue.PlayDialogueCoroutine(aiCharacter));
        }

        public void SendNextDialoguePopUpIndex(CharacterDialogue dialogue, AICharacterManager aiCharacter)
        {
            currentDialogue = dialogue;
            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);

            if (aiCharacter.aiCharacterSoundFXManager.dialogueIsPlaying)
                aiCharacter.aiCharacterSoundFXManager.audioSource.Stop();

            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            PlayerUIManager.instance.isPopUpWindowOpen = true;

            currentDialogue.dialogueIndex++;
            dialogueCoroutine = StartCoroutine(dialogue.PlayDialogueCoroutine(aiCharacter));
        }

        public void SetDialoguePopUpSubtitles(string dialogueText)
        {
            popUpDialogueGameObject.SetActive(true);
            popUpDialogueText.text = dialogueText;
        }

        public void EndDialoguePopUp()
        {
            popUpDialogueGameObject.SetActive(false);
            PlayerUIManager.instance.playerHUDManager.ToggleHUDWithOutPopUps(true);
        }

        public void CancelDialoguePopUp(AICharacterManager aiCharacter)
        {
            PlayerUIManager.instance.playerHUDManager.ToggleHUDWithOutPopUps(true);

            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);

            if (aiCharacter.aiCharacterSoundFXManager.audioSource.isPlaying)
                aiCharacter.aiCharacterSoundFXManager.audioSource.Stop();

            popUpDialogueGameObject.SetActive(false);
            currentDialogue.OnDialogueCancelled(aiCharacter);
        }
    }
}
