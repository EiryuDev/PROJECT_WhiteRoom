using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace WEV.WhiteRoom
{
    public class CoreItemDatabase : MonoBehaviour
    {
        public static CoreItemDatabase Instance; 
        public ItemWeapon unarmedWeapon;
        
        public GameObject pickUpItemPrefab;
        
        [Header("Weapon Settings")]
        [SerializeField] List<ItemWeapon> weapons = new List<ItemWeapon>(); 
        
        [Header("Item Settings")]
        [SerializeField] List<Item> items = new List<Item>();

        [Header("Keys Settings")]
        [SerializeField] List<ItemKey> keys = new List<ItemKey>();

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

            // BELOW CODE: Add all of our weapons to the list of items
            foreach(var weapon in weapons)
            {
                items.Add(weapon);
            }

            // BELOW CODE: Add all of our keys to the list of items
            foreach (var key in keys)
            {
                items.Add(key);
            }

            // BELOW CODE: Assign all of our items a unique item id
            for (int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }

        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public Item GetItemByID(int ID)
        {
            return items.FirstOrDefault(item => item.itemID == ID);
        }

        public ItemWeapon GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
        }

        public ItemKey GetKeyByID(int ID)
        {
            return keys.FirstOrDefault(key => key.itemID == ID);
        }

        public ItemWeapon GetWeaponFromSerializedData(CharacterSerializableWeapon serializableWeapon)
        {
            ItemWeapon weapon = null;
            
            if(GetWeaponByID(serializableWeapon.itemID))
                weapon = Instantiate(GetWeaponByID(serializableWeapon.itemID)); 

            if (weapon == null)
                return Instantiate(unarmedWeapon);
            
            return weapon;
        }
    }
}
