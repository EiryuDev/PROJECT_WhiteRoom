using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace WEV.WhiteRoom
{
    [System.Serializable]
    // LOGIC: We want to reference this data for every save file
    public class CharacterSaveData
    {
        [Header("SCENE INDEX Settings")] 
        public int sceneIndex = 1; // The index number of the scene

        [Header("CHARACTER NAME Settings")] 
        public string characterName = "Eiryu"; // Name of the character

        [Header("TIME PLAYED Settings")] 
        public float secondsPlayed; // Number of gameplay time displayed in seconds
         
        [Header("WORLD COORDINATES Settings")]
        public float xPosition; // X position of the player
        public float yPosition; // Y position of the player
        public float zPosition; // Z position of the player

        [Header("RESOURCES Settings")]
        public int currentHealth; // Current health of the character
        public float currentStamina; // Current stamina of the stamina

        [Header("STATS Settings")]
        public int vitality; // Shows the player resistance to death, basically health value
        public int endurance; // Shows the player endurance value, basically stamina value
        public int strength;
        public int intelligence;
        public int willpower;
        public int agility;
        public int speed;

        [Header("CORE ITEMS Settings")]
        public CharacterSerializableDictionary<int, bool> coreItemsLooted; // This int is the item I.D, the bool is looted status

        [Header("INVENTORY Settings")]
        public List<CharacterSerializableRegularItem> regularItemsInInventory;
        public List<CharacterSerializableVitalItem> vitalItemsInInventory;
        public List<CharacterSerializableWeapon> weaponsInInventory;

        [Header("Doors Settings")]
        public List<string> doorsOpened;

        [Header("Dialogue Settings")]
        // TO-DO: Use a serializable dictionary here to save values (ID and stages #) for many NPCs
        public int robiStageID = 0;

        public CharacterSaveData()
        { 
            coreItemsLooted = new CharacterSerializableDictionary<int, bool>();
            regularItemsInInventory = new List<CharacterSerializableRegularItem>();
            vitalItemsInInventory = new List<CharacterSerializableVitalItem>();
            weaponsInInventory = new List<CharacterSerializableWeapon>();
            doorsOpened = new List<string>();
        }
    }
}
