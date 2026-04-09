using UnityEngine;

namespace WEV.WhiteRoom
{
    public class AICharacterInventoryManager : CharacterInventoryManager
    {
        AICharacterManager aiCharacter;
        
        [Header("Loot Chance")] 
        [Tooltip("Estimate Chance for dropping the item?")]
        public int dropItemChance = 10;
        [SerializeField] Item[] droppableItems;

        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }

        public void UseDropSystem()
        {
            // BELOW CODE: The status of this character will drop an item
            bool willDropItem = false;
            
            // BELOW CODE: Random number rolled from 0-100 to check the chance of dropping the item
            int itemChanceRoll = Random.Range(0, 100);
            
            // BELOW CODE: If the number is equal to or lower than the item drop chance, we pass the check and drop and item
            if(itemChanceRoll <= dropItemChance)
                willDropItem = true;

            if (!willDropItem)
                return;
            
            Item generatedItem = droppableItems[Random.Range(0, droppableItems.Length)];
            Debug.Log("Item ID: " + generatedItem.itemID);

            if (generatedItem == null)
                return;

            GameObject itemPickUpInteractableGameObject = Instantiate(CoreItemDatabase.Instance.pickUpItemPrefab);
        }
    }
}
