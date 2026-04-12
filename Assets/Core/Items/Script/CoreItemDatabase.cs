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
        [SerializeField] List<ItemRegular> regularItems = new List<ItemRegular>();
        [SerializeField] List<ItemVital> vitalItems = new List<ItemVital>();

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

            // BELOW CODE: Add all of our regular items to the list of items
            foreach (var regularItem in regularItems)
            {
                items.Add(regularItem);
            }

            // BELOW CODE: Add all of our vital items to the list of items
            foreach (var vitalItem in vitalItems)
            {
                items.Add(vitalItem);
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

        public ItemRegular GetRegularItemByID(int ID)
        {
            return regularItems.FirstOrDefault(regularItem => regularItem.itemID == ID);
        }

        public ItemVital GetVitalItemByID(int ID)
        {
            return vitalItems.FirstOrDefault(vitalItem => vitalItem.itemID == ID);
        }

        public ItemKey GetKeyByID(int ID)
        {
            return keys.FirstOrDefault(key => key.itemID == ID);
        }

        public ItemRegular GetRegularItemFromSerializedData(CharacterSerializableRegularItem serializableRegularItem)
        {
            ItemRegular itemRegular = null;

            if (GetRegularItemByID(serializableRegularItem.itemID))
                itemRegular = Instantiate(GetRegularItemByID(serializableRegularItem.itemID));

            //if (item == null)
            //    return Instantiate(unarmedWeapon);

            return itemRegular;
        }

        public ItemVital GetVitalItemFromSerializedData(CharacterSerializableVitalItem serializableVitalItem)
        {
            ItemVital itemVital = null;

            if (GetVitalItemByID(serializableVitalItem.itemID))
                itemVital = Instantiate(GetVitalItemByID(serializableVitalItem.itemID));

            //if (item == null)
            //    return Instantiate(unarmedWeapon);

            return itemVital;
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
