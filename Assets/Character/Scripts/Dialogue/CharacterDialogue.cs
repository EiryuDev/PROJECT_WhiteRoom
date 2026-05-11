using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEV.WhiteRoom
{
    [CreateAssetMenu(menuName = "White Room/Dialogue")]
    public class CharacterDialogue : ScriptableObject
    {
        [Header("Dialogue Requirements Settings")]
        public int requiredStageID = 0;
        [Tooltip("How many seconds to wait to go from one dialogue to another. (For dialogues with no audio)")]
        public float dialogueWaitTime = 2f;

        [Header("Greeting Dialogue Settings")]
        [Tooltip("List of all the greeting dialogue text said by the character")]
        [TextArea] public List<string> greetingDialogueString = new List<string>();
        [Tooltip("List of all the greeting dialogue audio said by the character")]
        public List<AudioClip> greetingDialogueAudio = new List<AudioClip>();
        [Tooltip("Was the greeting already played?")]
        private bool greetingAlreadyPlayed = false;

        [Header("Core Dialogue Settings")]
        [Tooltip("List of all the dialogue text said by the character")]
        [TextArea] public List<string> dialogueString = new List<string>();
        [Tooltip("List of all the dialogue audio said by the character")]
        public List<AudioClip> dialogueAudio = new List<AudioClip>();
        [Tooltip("What is the current dialogue index?")]
        public int dialogueIndex = 0;

        [Header("Farewell Dialogue Settings")]
        [Tooltip("List of all the farewell dialogue text said by the character")]
        [TextArea] public List<string> farewellDialogueString = new List<string>();
        [Tooltip("List of all the farewell dialogue audio said by the character")]
        public List<AudioClip> farewellDialogueAudio = new List<AudioClip>();
        [Tooltip("Was the farewell already played?")]
        private bool farewellAlreadyPlayed = false;

        // Optional Settings
        // BACK BURNER: Face Character
        // BACK BURNER: Kill Character On Cancel
        // BACK BURNER: Open Menu On Cancel 

        private List<int> greetingShuffleBag = new List<int>();

        [Header("End Trigger Settings")]
        [Tooltip("Can we set the stage to the new dialogue id? New Dialogue will be selected depending on ID")]
        [SerializeField] bool setStageIndex = false;
        [Tooltip("What is the dialogue stage id?")]
        [SerializeField] int stageID = 0;
        [SerializeField] CharacterDialogueEndEvents endEvent;

        public void PlayDialogueEvent(AICharacterManager aiCharacter)
        {
            if (dialogueString.Count != dialogueAudio.Count)
            {
                Debug.Log("Audio clips don't match subtitle count, missing files");
                return;
            }

            aiCharacter.aiCharacterSoundFXManager.dialogueIsPlaying = true;

            if (aiCharacter.aiCharacterSoundFXManager.characterDialogueType == CharacterDialogueType.Standalone)
                PlayerUIManager.instance.playerUIPopUpManager.SendDialoguePopUp(this, aiCharacter);
            else
                aiCharacter.aiCharacterUIManager.uiDialogue.SendDialoguePopUp(this, aiCharacter);
        }

        private int GetNextGreetingIndex()
        {
            if (greetingShuffleBag.Count == 0)
            {
                // refill bag
                for (int i = 0; i < greetingDialogueAudio.Count; i++)
                    greetingShuffleBag.Add(i);

                // shuffle bag
                for (int i = 0; i < greetingShuffleBag.Count; i++)
                {
                    int randomIndex = Random.Range(i, greetingShuffleBag.Count);
                    int temp = greetingShuffleBag[i];
                    greetingShuffleBag[i] = greetingShuffleBag[randomIndex];
                    greetingShuffleBag[randomIndex] = temp;
                }
            }

            int index = greetingShuffleBag[0];
            greetingShuffleBag.RemoveAt(0);

            return index;
        }

        public IEnumerator PlayDialogueCoroutine(AICharacterManager aiCharacter)
        {
            // Play a random greeting dialogue, then wait the length of that audio clip + a second
            if (greetingDialogueAudio.Count != 0 && !greetingAlreadyPlayed)
            {
                greetingAlreadyPlayed = true;
                int randomGreetingDialogueIndex = Random.Range(0, greetingDialogueAudio.Count);
                if (aiCharacter.aiCharacterSoundFXManager.characterDialogueType == CharacterDialogueType.Standalone)
                {
                    PlayerUIManager.instance.playerUIPopUpManager
                        .SetDialoguePopUpSubtitles(greetingDialogueString[randomGreetingDialogueIndex]);
                }
                else
                {
                    aiCharacter.aiCharacterUIManager.uiDialogue.SetDialoguePopUpSubtitles(
                        greetingDialogueString[randomGreetingDialogueIndex]);
                }

                aiCharacter.aiCharacterSoundFXManager
                    .PlaySoundFX(greetingDialogueAudio[randomGreetingDialogueIndex], 1f, false);
                yield return new WaitForSeconds(
                    greetingDialogueAudio[randomGreetingDialogueIndex].length + 1);
            }

            while (dialogueIndex < dialogueString.Count)
            {
                if (aiCharacter.aiCharacterSoundFXManager.characterDialogueType == CharacterDialogueType.Standalone)
                {
                    PlayerUIManager.instance.playerUIPopUpManager
                        .SetDialoguePopUpSubtitles(dialogueString[dialogueIndex]);
                }
                else
                {
                    aiCharacter.aiCharacterUIManager.uiDialogue
                        .SetDialoguePopUpSubtitles(dialogueString[dialogueIndex]);
                }

                aiCharacter.aiCharacterSoundFXManager
                    .PlaySoundFX(dialogueAudio[dialogueIndex], 1f, false);
                yield return new WaitForSeconds(dialogueAudio[dialogueIndex].length + 1);
                dialogueIndex++;
            }

            // Play a random farewell dialogue, then wait the length of that audio clip + a second
            if (farewellDialogueAudio.Count != 0 && !farewellAlreadyPlayed)
            {
                farewellAlreadyPlayed = true;
                int randomFarewellDialogueIndex = Random.Range(0, farewellDialogueAudio.Count);

                if (aiCharacter.aiCharacterSoundFXManager.characterDialogueType == CharacterDialogueType.Standalone)
                {
                    PlayerUIManager.instance.playerUIPopUpManager
                        .SetDialoguePopUpSubtitles(farewellDialogueString[randomFarewellDialogueIndex]);
                }
                else
                {
                    aiCharacter.aiCharacterUIManager.uiDialogue.SetDialoguePopUpSubtitles(
                        farewellDialogueString[randomFarewellDialogueIndex]);
                }

                aiCharacter.aiCharacterSoundFXManager
                    .PlaySoundFX(farewellDialogueAudio[randomFarewellDialogueIndex], 1f, false);
                yield return new WaitForSeconds(
                    farewellDialogueAudio[randomFarewellDialogueIndex].length + 1);
            }

            OnDialogueEnded(aiCharacter);
            if (aiCharacter.aiCharacterSoundFXManager.characterDialogueType == CharacterDialogueType.Standalone)
                PlayerUIManager.instance.playerUIPopUpManager.EndDialoguePopUp();
            else
                aiCharacter.aiCharacterUIManager.uiDialogue.EndDialoguePopUp();

            yield return null;
        }

        /*
        public IEnumerator PlayDialogueCoroutine(AICharacterManager aiCharacter)
        {
            // GREETING
            if (!greetingAlreadyPlayed)
            {
                greetingAlreadyPlayed = true;

                if (greetingDialogueAudio != null && greetingDialogueAudio.Count > 0)
                {
                    int randomGreetingDialogueIndex = GetNextGreetingIndex();

                    if (aiCharacter.aiCharacterSoundFXManager.characterDialogueType == CharacterDialogueType.Standalone)
                        PlayerUIManager.instance.popUpManager.SetDialoguePopUpSubtitles(
                            greetingDialogueString[randomGreetingDialogueIndex]);
                    else
                        aiCharacter.aiCharacterUIManager.uiDialogue.SetDialoguePopUpSubtitles(
                            greetingDialogueString[randomGreetingDialogueIndex]);

                    AudioClip clip = null;

                    if (greetingDialogueAudio != null && greetingDialogueAudio.Count > randomGreetingDialogueIndex)
                        clip = greetingDialogueAudio[randomGreetingDialogueIndex];

                    if (clip != null)
                        aiCharacter.aiCharacterSoundFXManager.PlaySoundFX(clip, 1, false);

                    yield return new WaitForSeconds((clip != null ? clip.length : dialogueWaitTime) + 1f);
                }
                else
                {
                    yield return new WaitForSeconds(dialogueWaitTime);
                }
            }

            // MAIN DIALOGUE
            while (dialogueIndex < dialogueString.Count)
            {
                if (aiCharacter.aiCharacterSoundFXManager.characterDialogueType == CharacterDialogueType.Standalone)
                    PlayerUIManager.instance.popUpManager.SetDialoguePopUpSubtitles(dialogueString[dialogueIndex]);
                else
                    aiCharacter.aiCharacterUIManager.uiDialogue
                        .SetDialoguePopUpSubtitles(dialogueString[dialogueIndex]);

                AudioClip clip = null;

                if (dialogueAudio != null && dialogueAudio.Count > dialogueIndex)
                    clip = dialogueAudio[dialogueIndex];

                if (clip != null)
                    aiCharacter.aiCharacterSoundFXManager.PlaySoundFX(clip, 1, false);

                yield return new WaitForSeconds((clip != null ? clip.length : dialogueWaitTime) + 1f);

                dialogueIndex++;
            }

            // FAREWELL
            if (!farewellAlreadyPlayed)
            {
                farewellAlreadyPlayed = true;

                if (farewellDialogueAudio != null && farewellDialogueAudio.Count > 0)
                {
                    int randomFarewellDialogueIndex = Random.Range(0, farewellDialogueAudio.Count);

                    if (aiCharacter.aiCharacterSoundFXManager.characterDialogueType == CharacterDialogueType.Standalone)
                        PlayerUIManager.instance.popUpManager.SetDialoguePopUpSubtitles(
                            farewellDialogueString[randomFarewellDialogueIndex]);
                    else
                        aiCharacter.aiCharacterUIManager.uiDialogue.SetDialoguePopUpSubtitles(
                            farewellDialogueString[randomFarewellDialogueIndex]);

                    AudioClip clip = null;

                    if (farewellDialogueAudio != null && farewellDialogueAudio.Count > randomFarewellDialogueIndex)
                        clip = farewellDialogueAudio[randomFarewellDialogueIndex];

                    if (clip != null)
                        aiCharacter.aiCharacterSoundFXManager.PlaySoundFX(clip, 1, false);

                    yield return new WaitForSeconds((clip != null ? clip.length : dialogueWaitTime) + 1f);
                }
                else
                {
                    yield return new WaitForSeconds(dialogueWaitTime);
                }
            }

            OnDialogueEnded(aiCharacter);

            if (aiCharacter.aiCharacterSoundFXManager.characterDialogueType == CharacterDialogueType.Standalone)
                PlayerUIManager.instance.popUpManager.EndDialoguePopUp();
            else
                aiCharacter.aiCharacterUIManager.uiDialogue.EndDialoguePopUp();

            yield return null;
        }
        */

        public void OnDialogueEnded(AICharacterManager aiCharacter)
        {
            // TO-DO: Do stuff with character dialogue scriptable if desired
            greetingAlreadyPlayed = false;
            dialogueIndex = 0;

            // BELOW CODE: Resetting the state so you can talk again
            //aiCharacter.aiCharacterSoundFXManager.dialogueIsPlaying = false;

            if (setStageIndex)
                CoreSaveGameManager.instance.SetStageOfDialogue(
                    aiCharacter.aiCharacterSoundFXManager.characterDialogueID, stageID);

            // BELOW CODE: Do stuff with the AI character if desired
            aiCharacter.aiCharacterSoundFXManager.OnCurrentDialogueEnded();

            if (endEvent != CharacterDialogueEndEvents.None)
            {
                switch (endEvent)
                {
                    case CharacterDialogueEndEvents.None:
                        break;
                    case CharacterDialogueEndEvents.Robi:
                        Debug.Log("");
                        break;
                    default:
                        break;
                }
            }
        }

        // BELOW CODE: Called when we leave the dialogue interaction radius
        public void OnDialogueCancelled(AICharacterManager aiCharacter)
        {
            // BELOW CODE: Get new dialogue based on stage id
            switch (endEvent)
            {
                case CharacterDialogueEndEvents.None:
                    break;
                case CharacterDialogueEndEvents.Robi:
                    Debug.Log("");
                    break;
                default:
                    break;
            }
        }
    }
}