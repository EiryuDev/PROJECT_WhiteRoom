using System;
using TMPro;
using UnityEngine;
using WEV.WhiteRoom;

namespace WEV.WhiteRoom
{
    // BELOW CODE: This is attached to ai itself as child. So based on dialogueType the dialoguestyle will play
    public class UI_CharacterDialogue : MonoBehaviour
    {
        [Header("DIALOGUE POP UP")]
        [SerializeField] GameObject popUpDialogueGameObject; 
        public TextMeshProUGUI popUpDialogueText;

        [Header("CURRENT DIALOGUE")]
        [SerializeField] CharacterDialogue currentDialogue;
        private Coroutine dialogueCoroutine;

        private void Update()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }

        public void SendDialoguePopUp(CharacterDialogue dialogue, AICharacterManager aiCharacter)
        {
            //PlayerUIManager.instance.playerHUDManager.ToggleHUDWithOutPopUps(false);
            currentDialogue = dialogue;

            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);

            //PlayerUIManager.instance.popUpManager.CloseAllPopUpWindows();
            //PlayerUIManager.instance.isPopUpWindowOpen = true;

            dialogueCoroutine = StartCoroutine(dialogue.PlayDialogueCoroutine(aiCharacter));
        }

        public void SendNextDialoguePopUpIndex(CharacterDialogue dialogue, AICharacterManager aiCharacter)
        {
            currentDialogue = dialogue;
            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);

            if (aiCharacter.aiCharacterSoundFXManager.dialogueIsPlaying)
                aiCharacter.aiCharacterSoundFXManager.audioSource.Stop();

            //PlayerUIManager.instance.popUpManager.CloseAllPopUpWindows();
            //PlayerUIManager.instance.isPopUpWindowOpen = true;

            currentDialogue.dialogueIndex++;
            dialogueCoroutine = StartCoroutine(dialogue.PlayDialogueCoroutine(aiCharacter));
        }

        public void SetDialoguePopUpSubtitles(String dialogueText)
        {
            popUpDialogueGameObject.SetActive(true);
            popUpDialogueText.text = dialogueText;
        }

        public void EndDialoguePopUp()
        {
            popUpDialogueText.text = "";
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
