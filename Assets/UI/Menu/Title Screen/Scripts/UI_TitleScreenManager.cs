using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace WEV.WhiteRoom
{
    public class UI_TitleScreenManager : MonoBehaviour
    {
        public static UI_TitleScreenManager Instance; // Static instance for Title Screen Manager 

        [Header("Player Prefab Settings")]
        public GameObject playerPrefab;

        [Header("MAIN MENU MENUES")]
        [SerializeField] GameObject titleScreenMainMenu; // Reference to title screen main menu game object
        [SerializeField] GameObject titleScreenLoadMenu; // Reference to title screen load menu game object
        
        [Header("MAIN MENU BUTTONS")]
        [SerializeField] Button mainMenuNewGameButton; // New game button to start new game
        [SerializeField] Button mainMenuLoadGameButton; // Load game button to open loading screen
        [SerializeField] Button loadMenuReturnButton; // Return button to return to title screen
        [SerializeField] Button deleteCharacterPopUpConfirmButton; // Delete character button to confirm the popup
        
        [Header("MAIN MENU POP UPS")]
        [SerializeField] GameObject noCharacterSlotsPopUp; // Reference to no character slots pop ups game object
        [SerializeField] Button noCharacterSlotsOkayButton; // Reference to no character slots okay button game object
        [SerializeField] GameObject deleteCharacterSlotPopUp; // Reference to delete character slot pop up game object
        
        [Header("CHARACTER SLOTS")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT; // Reference to current selected slot

        [Header("VFX")] 
        [SerializeField]  private ParticleSystem mainMenuParticleSystem; // Reference to main menu particle system

        [Header("CLASSES")] 
        public CharacterClass[] startingClasses;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AttemptToCreateNewCharacter()
        {
            if (CoreSaveGameManager.instance.HasFreeCharacterSlot())
            {

            }
            else
            {
                // BELOW CODE: If there are no free slots, notify the player
                DisplayNoFreeCharacterSlotsPopup();
            }
        }

        public void InstantitatePlayer()
        {
            GameObject playerObject = Instantiate(playerPrefab, playerPrefab.transform.position, playerPrefab.transform.rotation);
            PlayerManager player = playerObject.GetComponent<PlayerManager>();
            PlayerUIManager.instance.player = player;
            CoreSaveGameManager.instance.player = player;
        }

        public void StartNewGame()
        {
            CoreSaveGameManager.instance.AttemptToCreateNewGame();
        }

        public void OpenLoadGameMenu()
        {
            // BELOW CODE: Close main menu
            titleScreenMainMenu.SetActive(false);
            // BELOW CODE: Open load menu
            titleScreenLoadMenu.SetActive(true);
            // BELOW CODE: Select the return button
            loadMenuReturnButton.Select();
        }

        public void CloseLoadGameMenu()
        {
            // BELOW CODE: Close load menu
            titleScreenLoadMenu.SetActive(false);
            // BELOW CODE: Open main menu
            titleScreenMainMenu.SetActive(true);

            // BELOW CODE: Play VFX again
            mainMenuParticleSystem.Play();

            // BELOW CODE: Select the load button
            mainMenuLoadGameButton.Select();
        }

        public void DisplayNoFreeCharacterSlotsPopup()
        {
            noCharacterSlotsPopUp.SetActive(true);
            noCharacterSlotsOkayButton.Select();
        }

        public void CloseNoFreeCharacterSlotsPopup()
        {
            noCharacterSlotsPopUp.SetActive(false);
            mainMenuNewGameButton.Select();
        }

        public void SelectCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedSlot = characterSlot;
        }

        public void SelectNoSlot()
        {
            currentSelectedSlot = CharacterSlot.NO_SLOT;
        }

        public void AttemptToDeleteCharacterSlot()
        {
            if(currentSelectedSlot != CharacterSlot.NO_SLOT)
            {
                deleteCharacterSlotPopUp.SetActive(true);
                deleteCharacterPopUpConfirmButton.Select();
            }
        }

        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            CoreSaveGameManager.instance.DeleteGame(currentSelectedSlot);

            // BELOW CODE: We disable and enable the load menu, to refresh all the character slots
            titleScreenLoadMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();
        }
        public void CloseDeleteCharacterPopUp()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            loadMenuReturnButton.Select();
        }

        public void SelectClass(int classID)
        {
            PlayerManager player = PlayerUIManager.instance.player.GetComponent<PlayerManager>();

            if (startingClasses.Length <= 0)
                return;
            
            startingClasses[classID].SetClass(player);
        }

        public void PreviewClass(int classID)
        {
            PlayerManager player = PlayerUIManager.instance.player.GetComponent<PlayerManager>();

            if (startingClasses.Length <= 0)
                return;
            
            startingClasses[classID].SetClass(player);
        }

        public void SetCharacterClass(PlayerManager player, int vitality, int endurance,
            int strength, int intelligence, int willpower, int agility, int speed,
            ItemWeapon[] mainHandWeapons, ItemWeapon[] offHandWeapons)
        {
            // BELOW CODE: Set the stats
            player.playerStatsManager.vitality = vitality;
            player.playerStatsManager.endurance = endurance;
            player.playerStatsManager.strength = strength;
            player.playerStatsManager.intelligence = intelligence;
            player.playerStatsManager.willpower = willpower;
            player.playerStatsManager.agility = agility;
            player.playerStatsManager.speed = speed;

            // BELOW CODE: Set the weapons
            player.playerInventoryManager.weaponsInRightHandSlots[0] = Instantiate(mainHandWeapons[0]);
            player.playerInventoryManager.weaponsInRightHandSlots[1] = Instantiate(mainHandWeapons[1]);
            player.playerInventoryManager.weaponsInRightHandSlots[2] = Instantiate(mainHandWeapons[2]);
            player.playerInventoryManager.currentRightHandWeapon =
                player.playerInventoryManager.weaponsInRightHandSlots[0];
            
            player.playerInventoryManager.weaponsInLeftHandSlots[0] = Instantiate(offHandWeapons[0]);
            player.playerInventoryManager.weaponsInLeftHandSlots[1] = Instantiate(offHandWeapons[1]);
            player.playerInventoryManager.weaponsInLeftHandSlots[2] = Instantiate(offHandWeapons[2]);
            player.playerInventoryManager.currentLeftHandWeapon =
                player.playerInventoryManager.weaponsInLeftHandSlots[0];
        }     
    }
}
