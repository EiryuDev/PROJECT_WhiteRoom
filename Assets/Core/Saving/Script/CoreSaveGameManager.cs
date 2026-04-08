using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CoreSaveGameManager : MonoBehaviour
    {
        public static CoreSaveGameManager instance;

        public PlayerManager player;

        [Header("FRAME RATE SETTINGS")]
        public bool lockTargetFrameRate = false;

        [Header("SAVING/LOADING")]
        [SerializeField] public bool saveGame;
        [SerializeField] bool loadGame;

        [Header("WORLD SCENE INDEX")]
        [SerializeField] int worldSceneIndex = 1;

        [Header("SAVE DATA WRITER")]
        public CharacterSaveFileDataWriter saveFileDataWriter;

        [Header("CURRENT CHARACTER DATA")]
        public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        private string saveFileName;

        [Header("CHARACTER SLOTS")]
        public CharacterSaveData characterSlot01;
        public CharacterSaveData characterSlot02;
        public CharacterSaveData characterSlot03;
        public CharacterSaveData characterSlot04;
        public CharacterSaveData characterSlot05;
        public CharacterSaveData characterSlot06;
        public CharacterSaveData characterSlot07;
        public CharacterSaveData characterSlot08;
        public CharacterSaveData characterSlot09;
        public CharacterSaveData characterSlot10;

        private void Awake()
        {
            if (instance == null) instance = this;
            else { Destroy(gameObject); return; }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            LockFrameRateTo60();
            LoadAllCharacterProfiles();

            // TO-DO: Remove this from here
            player = PlayerUIManager.instance.player;
        }

        private void Update()
        {
            if (saveGame) { saveGame = false; SaveGame(); }
            if (loadGame) { loadGame = false; LoadGame(); }
        }

        // ── UNCHANGED METHODS ─────────────────────────────────────────
        public bool HasFreeCharacterSlot()
        {
            saveFileDataWriter = new CharacterSaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            if (!saveFileDataWriter.CheckToSeeIfFileExists()) return true;
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            if (!saveFileDataWriter.CheckToSeeIfFileExists()) return true;
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            if (!saveFileDataWriter.CheckToSeeIfFileExists()) return true;
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
            if (!saveFileDataWriter.CheckToSeeIfFileExists()) return true;
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
            if (!saveFileDataWriter.CheckToSeeIfFileExists()) return true;
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
            if (!saveFileDataWriter.CheckToSeeIfFileExists()) return true;
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
            if (!saveFileDataWriter.CheckToSeeIfFileExists()) return true;
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
            if (!saveFileDataWriter.CheckToSeeIfFileExists()) return true;
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
            if (!saveFileDataWriter.CheckToSeeIfFileExists()) return true;
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
            if (!saveFileDataWriter.CheckToSeeIfFileExists()) return true;

            return false;
        }

        public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
        {
            string fileName = "";
            switch (characterSlot)
            {
                case CharacterSlot.CharacterSlot_01: fileName = "CharacterSlot_01"; break;
                case CharacterSlot.CharacterSlot_02: fileName = "CharacterSlot_02"; break;
                case CharacterSlot.CharacterSlot_03: fileName = "CharacterSlot_03"; break;
                case CharacterSlot.CharacterSlot_04: fileName = "CharacterSlot_04"; break;
                case CharacterSlot.CharacterSlot_05: fileName = "CharacterSlot_05"; break;
                case CharacterSlot.CharacterSlot_06: fileName = "CharacterSlot_06"; break;
                case CharacterSlot.CharacterSlot_07: fileName = "CharacterSlot_07"; break;
                case CharacterSlot.CharacterSlot_08: fileName = "CharacterSlot_08"; break;
                case CharacterSlot.CharacterSlot_09: fileName = "CharacterSlot_09"; break;
                case CharacterSlot.CharacterSlot_10: fileName = "CharacterSlot_10"; break;
            }
            return fileName;
        }

        public void AttemptToCreateNewGame()
        {
            saveFileDataWriter = new CharacterSaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            for (int i = 1; i <= 10; i++)
            {
                CharacterSlot slot = (CharacterSlot)(i - 1);
                saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(slot);
                if (!saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    currentCharacterSlotBeingUsed = slot;
                    currentCharacterData = new CharacterSaveData();
                    NewGame();
                    return;
                }
            }

            //UI_TitleScreenManager.Instance.DisplayNoFreeCharacterSlotsPopup();
        }

        private void NewGame()
        {
            player.playerStatsManager.vitality = 15;
            player.playerStatsManager.endurance = 10;
            SaveGame();
            CoreSceneManager.instance.LoadWorldScene(worldSceneIndex);
        }

        public void LoadGame()
        {
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
            saveFileDataWriter = new CharacterSaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;
            currentCharacterData = saveFileDataWriter.LoadSaveFile();
            CoreSceneManager.instance.LoadWorldScene(worldSceneIndex);
        }

        public void SaveGame()
        {
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
            saveFileDataWriter = new CharacterSaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;
            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);
            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
        }

        public void DeleteGame(CharacterSlot characterSlot)
        {
            saveFileDataWriter = new CharacterSaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
            saveFileDataWriter.DeleteSaveFile();
        }

        private void LoadAllCharacterProfiles()
        {
            saveFileDataWriter = new CharacterSaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            characterSlot02 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            characterSlot03 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
            characterSlot04 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
            characterSlot05 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
            characterSlot06 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
            characterSlot07 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
            characterSlot08 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
            characterSlot09 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
            characterSlot10 = saveFileDataWriter.LoadSaveFile();
        }

        public int GetWorldSceneIndex() => worldSceneIndex;

        public CharacterSerializableWeapon GetSerializableWeaponFromWeaponItem(ItemWeapon weapon)
        {
            CharacterSerializableWeapon serializedWeapon = new CharacterSerializableWeapon();
            serializedWeapon.itemID = weapon.itemID;
            return serializedWeapon;
        }

        private void LockFrameRateTo60()
        {
            if (lockTargetFrameRate)
                Application.targetFrameRate = 60;
        }
    }
}
