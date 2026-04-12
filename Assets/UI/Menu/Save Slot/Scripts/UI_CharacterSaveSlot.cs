using UnityEngine;
using TMPro;

namespace WEV.WhiteRoom
{
    public class UI_CharacterSaveSlot : MonoBehaviour
    {
        CharacterSaveFileDataWriter saveFileWriter; // Reference to the Character Save File Data Writer script

        [Header("GAME SLOTS")]
        public CharacterSlot characterSlot; // Reference to the character slots enums for accessing character slots

        [Header("CHARACTER INFO")]
        public TextMeshProUGUI characterName; // Text mesh pro text for character name
        public TextMeshProUGUI timePlayed; // Text mesh pro text for time played

        private void OnEnable()
        {
            LoadSaveSlots();
        }
        private void LoadSaveSlots()
        {
            saveFileWriter = new CharacterSaveFileDataWriter();
            saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;
        
            if(characterSlot == CharacterSlot.CharacterSlot_01)
            {
                saveFileWriter.saveFileName = CoreSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = CoreSaveGameManager.instance.characterSlot01.characterName;   
                }
                // BELOW CODE: If it doesn't exist, disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_02)
            {
                saveFileWriter.saveFileName = CoreSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = CoreSaveGameManager.instance.characterSlot02.characterName;
                }
                // BELOW CODE: If it doesn't exist, disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_03)
            {
                saveFileWriter.saveFileName = CoreSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = CoreSaveGameManager.instance.characterSlot03.characterName;
                }
                // BELOW CODE: If it doesn't exist, disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_04)
            {
                saveFileWriter.saveFileName = CoreSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = CoreSaveGameManager.instance.characterSlot04.characterName;
                }
                // BELOW CODE: If it doesn't exist, disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_05)
            {
                saveFileWriter.saveFileName = CoreSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = CoreSaveGameManager.instance.characterSlot05.characterName;
                }
                // BELOW CODE: If it doesn't exist, disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_06)
            {
                saveFileWriter.saveFileName = CoreSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = CoreSaveGameManager.instance.characterSlot06.characterName;
                }
                // BELOW CODE: If it doesn't exist, disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_07)
            {
                saveFileWriter.saveFileName = CoreSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = CoreSaveGameManager.instance.characterSlot07.characterName;
                }
                // BELOW CODE: If it doesn't exist, disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_08)
            {
                saveFileWriter.saveFileName = CoreSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = CoreSaveGameManager.instance.characterSlot08.characterName;
                }
                // BELOW CODE: If it doesn't exist, disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_09)
            {
                saveFileWriter.saveFileName = CoreSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = CoreSaveGameManager.instance.characterSlot09.characterName;
                }
                // BELOW CODE: If it doesn't exist, disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_10)
            {
                saveFileWriter.saveFileName = CoreSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // BELOW CODE: If file exists, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = CoreSaveGameManager.instance.characterSlot10.characterName;
                }
                // BELOW CODE: If it doesn't exist, disable the game object
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
        public void LoadGameFromCharacterSlot()
        {
            CoreSaveGameManager.instance.currentCharacterSlotBeingUsed = characterSlot;
            CoreSaveGameManager.instance.LoadGame();
        }
        public void SelectCurrentSlot()
        {
            UI_TitleScreenManager.Instance.SelectCharacterSlot(characterSlot);
        }
    }
}