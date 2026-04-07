using UnityEngine;
using System.Collections.Generic;

namespace WEV.WhiteRoom
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        [HideInInspector] public PlayerManager player;

        [Header("Player Settings")]
        public ItemPlayer currentPlayerDataBeingUsed;

        [Header("Inventory Settings")]
        [Tooltip("Which is the current item in the hand?")]
        public Item currentItemInHand;
        [Tooltip("Contains all the items in the player's inventory.")]
        public List<Item> itemsInInventory; 

        [Header("Weapon Settings")]
        public ItemWeapon currentRightHandWeapon; 
        public ItemWeapon currentLeftHandWeapon; 

        [Header("Quick Slot Settings")]
        public ItemWeapon[] weaponsInRightHandSlots = new ItemWeapon[3]; 
        public int rightHandWeaponIndex = 0; 
        public ItemWeapon[] weaponsInLeftHandSlots = new ItemWeapon[3];
        public int leftHandWeaponIndex = 0;

        public GameObject currentHeldObject;
        private Rigidbody heldObjRb;
        private int holdLayer;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            holdLayer = LayerMask.NameToLayer("Hold");
        }

        public void AddItemToInventory(Item item, int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                itemsInInventory.Add(item);
            }
        }

        public void RemoveItemFromInventory(Item item)
        {    
            if (item == null)
                  return;
  
              bool isStackable = item.maxItemAmount > 1;
  
              // BELOW CODE: Stackable items
              if (isStackable)
              {
                  int amountToRemove = item.currentItemAmount;
  
                  for (int i = itemsInInventory.Count - 1; i >= 0; i--)
                  {
                      if (itemsInInventory[i] == null)
                          continue;
  
                      if (itemsInInventory[i].itemID != item.itemID)
                          continue;
  
                      int available = itemsInInventory[i].currentItemAmount;
  
                      // BELOW CODE: remove only what we need
                      int removeAmount = Mathf.Min(available, amountToRemove);
                      
                      itemsInInventory[i].currentItemAmount -= removeAmount;
                      amountToRemove -= removeAmount;
  
                      // BELOW CODE: safety clamp (prevents negatives forever)
                      itemsInInventory[i].currentItemAmount = Mathf.Max(0, itemsInInventory[i].currentItemAmount);
  
                      // BELOW CODE: remove empty stacks
                      if (itemsInInventory[i].currentItemAmount <= 0)
                          itemsInInventory.RemoveAt(i);
  
                      // BELOW CODE: STOP when done
                      if (amountToRemove <= 0)
                          break;
                  }
              }
              // BELOW CODE: Non stackable item
              else
              {
                  itemsInInventory.Remove(item);
              }
  
              // BELOW CODE: Clean nulls
              for (int i = itemsInInventory.Count - 1; i >= 0; i--)
              {
                  if (itemsInInventory[i] == null)
                  {
                      itemsInInventory.RemoveAt(i);
                  }
              }
        }

        public void PickUpWorldObject(GameObject obj)
        {
            if (currentHeldObject != null) return;

            currentHeldObject = obj;
            heldObjRb = obj.GetComponent<Rigidbody>();

            if (heldObjRb != null)
            {
                heldObjRb.isKinematic = true;
            }

            obj.transform.SetParent(player.playerCameraManager.holdPos);
            obj.layer = holdLayer;

            Collider objCol = obj.GetComponent<Collider>();
            Collider playerCol = player.GetComponent<Collider>();

            if (objCol && playerCol)
            {
                Physics.IgnoreCollision(objCol, playerCol, true);
            }
        }

        public void DropObject()
        {
            if (currentHeldObject == null) return;

            Physics.IgnoreCollision(currentHeldObject.GetComponent<Collider>(), player.GetComponent<Collider>(), false);

            currentHeldObject.layer = 0;

            if (heldObjRb != null)
                heldObjRb.isKinematic = false;

            currentHeldObject.transform.SetParent(null);

            ItemInteractable interactable = currentHeldObject.GetComponent<ItemInteractable>();
            if (interactable != null)
            {
                interactable.EnableInteraction();
            }

            currentHeldObject = null;
        }

        public void ThrowObject()
        {
            if (currentHeldObject == null) return;

            Physics.IgnoreCollision(currentHeldObject.GetComponent<Collider>(), player.GetComponent<Collider>(), false);

            currentHeldObject.layer = 0;

            if (heldObjRb != null)
            {
                heldObjRb.isKinematic = false;
                heldObjRb.AddForce(player.transform.forward * currentPlayerDataBeingUsed.throwForce);
            }

            currentHeldObject.transform.SetParent(null);
            currentHeldObject = null;
        }

        public void HandleHeldObject()
        {
            if (currentHeldObject == null) return;

            // Follow hand
            currentHeldObject.transform.position = player.playerCameraManager.holdPos.position;

            player.playerCameraManager.RotateObject(currentHeldObject, true);

            // THROW
            if (player.playerInputManager.throwInput)
            {
                player.playerInputManager.throwInput = false;
                ThrowObject();
                return;
            }
        }
    }
}
