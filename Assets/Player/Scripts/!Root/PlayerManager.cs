using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WEV.WhiteRoom
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerCameraManager playerCameraManager;
        [HideInInspector] public PlayerInputManager playerInputManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        [HideInInspector] public PlayerCombatManager playerCombatManager;
        [HideInInspector] public PlayerInteractionManager playerInteractionManager;
        [HideInInspector] public PlayerSoundFXManager playerSoundFXManager;
        [HideInInspector] public PlayerUIManager playerUIManager;

        [Header("Player Settings")]
        public string characterName;

        [Header("AREA")] 
        public CoreLocationSceneSet areaCurrentlyIn;

        protected override void Awake()
        {
            base.Awake();
            playerCameraManager = GetComponentInChildren<PlayerCameraManager>();
            playerInputManager = GetComponent<PlayerInputManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInteractionManager = GetComponent<PlayerInteractionManager>();
            playerSoundFXManager = GetComponent<PlayerSoundFXManager>();
            playerUIManager = FindAnyObjectByType<PlayerUIManager>();
        }

        protected override void Start()
        {
            base.Start();
        }

        public void Update()
        {
            playerInputManager.UseAllInputs();
            playerLocomotionManager.UseAllMovement();
            playerInventoryManager.HandleHeldObject();
        }
        private void LateUpdate()
        {
            playerCameraManager.UseAllCameraMovement();
        }

        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentIndex != 0)
                currentCharacterData.sceneIndex = currentIndex;
            currentCharacterData.characterName = characterName;
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.zPosition = transform.position.z;

            currentCharacterData.currentHealth = playerStatsManager.currentHealth;
            currentCharacterData.currentStamina = playerStatsManager.currentStamina;

            currentCharacterData.vitality = playerStatsManager.vitality;
            currentCharacterData.endurance = playerStatsManager.endurance;
            currentCharacterData.strength = playerStatsManager.strength;
            currentCharacterData.intelligence = playerStatsManager.intelligence;
            currentCharacterData.willpower = playerStatsManager.willpower;
            currentCharacterData.agility = playerStatsManager.agility;
            currentCharacterData.speed = playerStatsManager.speed;

            // BELOW CODE: Clear list before save
            currentCharacterData.weaponsInInventory = new List<CharacterSerializableWeapon>();

            for (int i = 0; i < playerInventoryManager.itemsInInventory.Count; i++)
            {
                if (playerInventoryManager.itemsInInventory[i] == null)
                    continue;

                ItemWeapon weaponInInventory = playerInventoryManager.itemsInInventory[i] as ItemWeapon;

                if (weaponInInventory != null)
                    currentCharacterData.weaponsInInventory.Add(
                        CoreSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(weaponInInventory));
            }
        }

        public void LoadGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            characterName = currentCharacterData.characterName;
            Vector3 myPosition = new Vector3
            (currentCharacterData.xPosition,
                currentCharacterData.yPosition,
                currentCharacterData.zPosition);
            transform.position = myPosition;

            playerStatsManager.vitality = currentCharacterData.vitality;
            playerStatsManager.endurance = currentCharacterData.endurance;
            playerStatsManager.strength = currentCharacterData.strength;
            playerStatsManager.intelligence = currentCharacterData.intelligence;
            playerStatsManager.willpower = currentCharacterData.willpower;
            playerStatsManager.agility = currentCharacterData.agility;
            playerStatsManager.speed = currentCharacterData.speed;;

            playerStatsManager.maxHealth =
                playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerStatsManager.vitality);
            playerStatsManager.maxStamina =
                playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerStatsManager.endurance);

            playerStatsManager.currentHealth = currentCharacterData.currentHealth;
            playerStatsManager.currentStamina = currentCharacterData.currentStamina;

            // TO-DO: Stats HUD UI
            //PlayerUIManager.instance.playerHUDManager.SetMaxStaminaValue(playerStatsManager.maxStamina);

            for (int i = 0; i < currentCharacterData.weaponsInInventory.Count; i++)
            {
                ItemWeapon weapon = currentCharacterData.weaponsInInventory[i].GetWeapon();
                playerInventoryManager.AddItemToInventory(weapon);
            }
        }
    }
}
