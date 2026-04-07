using UnityEngine;

namespace WEV.WhiteRoom
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        [Tooltip("The ID for the particular item")]
        public int itemID; 
        [Tooltip("The name of the particular item")]
        public string itemName; 
        [Tooltip("The sprite icon for the particular item")]
        public Sprite itemIcon; 
        [Tooltip("The lore of the particular item")]
        [TextArea] public string itemLore;
        
        // BELOW CODE: Decides if this item can have a stackable amount
        [Tooltip("What is the max amount of particular item?")]
        public int maxItemAmount = 1;
        [Tooltip("What is the current amount of particular item?")]
        public int currentItemAmount = 1;

        [Tooltip("The type of the particular item")]
        public ItemType itemType; 
    }
}
