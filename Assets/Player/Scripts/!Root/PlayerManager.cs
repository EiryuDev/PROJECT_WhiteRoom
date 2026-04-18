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

        [Header("AREA")] 
        public CoreSceneLocationSet areaCurrentlyIn;

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

        protected override void Update()
        {
            base.Update();

            playerInputManager.UseAllInputs();
            playerLocomotionManager.UseAllMovement();
            playerInventoryManager.HandleHeldObject();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

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
            currentCharacterData.regularItemsInInventory = new List<CharacterSerializableRegularItem>();
            currentCharacterData.vitalItemsInInventory = new List<CharacterSerializableVitalItem>();
            currentCharacterData.weaponsInInventory = new List<CharacterSerializableWeapon>();

            for (int i = 0; i < playerInventoryManager.itemsInInventory.Count; i++)
            {
                if (playerInventoryManager.itemsInInventory[i] == null)
                    continue;

                ItemRegular regularItemInInventory = playerInventoryManager.itemsInInventory[i] as ItemRegular;
                ItemVital vitalItemInInventory = playerInventoryManager.itemsInInventory[i] as ItemVital;
                ItemWeapon weaponInInventory = playerInventoryManager.itemsInInventory[i] as ItemWeapon;

                // Regular items
                if (regularItemInInventory != null)
                    currentCharacterData.regularItemsInInventory.Add(
                        CoreSaveGameManager.instance.GetSerializableRegularItemFromItem(regularItemInInventory));

                // Vital items
                if (vitalItemInInventory != null)
                    currentCharacterData.vitalItemsInInventory.Add(
                        CoreSaveGameManager.instance.GetSerializableVitalItemFromItem(vitalItemInInventory));

                // Weapons
                if (weaponInInventory != null)
                    currentCharacterData.weaponsInInventory.Add(
                        CoreSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(weaponInInventory));
            }
        }

        public void LoadGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            characterName = currentCharacterData.characterName;

            controller.enabled = false;
            transform.position = new Vector3(
                currentCharacterData.xPosition,
                currentCharacterData.yPosition,
                currentCharacterData.zPosition);
            controller.enabled = true;

            playerStatsManager.vitality = currentCharacterData.vitality;
            playerStatsManager.endurance = currentCharacterData.endurance;
            playerStatsManager.strength = currentCharacterData.strength;
            playerStatsManager.intelligence = currentCharacterData.intelligence;
            playerStatsManager.willpower = currentCharacterData.willpower;
            playerStatsManager.agility = currentCharacterData.agility;
            playerStatsManager.speed = currentCharacterData.speed; ;

            playerStatsManager.maxHealth =
                playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerStatsManager.vitality);
            playerStatsManager.maxStamina =
                playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerStatsManager.endurance);

            playerStatsManager.currentHealth = currentCharacterData.currentHealth;
            playerStatsManager.currentStamina = currentCharacterData.currentStamina;

            // TO-DO: Stats HUD UI
            //PlayerUIManager.instance.playerHUDManager.SetMaxStaminaValue(playerStatsManager.maxStamina);

            // Regular Items
            for (int i = 0; i < currentCharacterData.regularItemsInInventory.Count; i++)
            {
                ItemRegular itemRegular = currentCharacterData.regularItemsInInventory[i].GetRegularItem();
                playerInventoryManager.AddItemToInventory(itemRegular);
            }

            // Vital Items
            for (int i = 0; i < currentCharacterData.vitalItemsInInventory.Count; i++)
            {
                ItemVital itemVital = currentCharacterData.vitalItemsInInventory[i].GetVitalItem();
                playerInventoryManager.AddItemToInventory(itemVital);
            }

            // Weapons
            for (int i = 0; i < currentCharacterData.weaponsInInventory.Count; i++)
            {
                ItemWeapon weapon = currentCharacterData.weaponsInInventory[i].GetWeapon();
                playerInventoryManager.AddItemToInventory(weapon);
            }
        }
    }
}
