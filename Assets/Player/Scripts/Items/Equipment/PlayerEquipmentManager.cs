using UnityEngine;

namespace WEV.WhiteRoom
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager player; // Reference to the Player Manager script
        public CharacterWeaponHolderSlot rightHandSlot; // Reference to the Character Weapon Holder Slot script for right hand slot
        public CharacterWeaponHolderSlot leftHandWeaponSlot; // Reference to the Character Weapon Holder Slot script for left hand weapon slot

        public GameObject rightHandWeaponModel;  // Reference to the right hand weapon model
        public GameObject leftHandWeaponModel;  // Reference to the left hand weapon model
        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

            InitialiseWeaponSlot();
        }
        protected override void Start()
        {
            base.Start();

            LoadWeaponsOnBothHands();
        }
        private void InitialiseWeaponSlot()
        {
            CharacterWeaponHolderSlot[] weaponSlots = GetComponentsInChildren<CharacterWeaponHolderSlot>();

            foreach (var weaponSlot in weaponSlots)
            {
                if (weaponSlot.weaponSlot == WeaponModelSlot.RightHandWeaponSlot)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHandWeaponSlot)
                {
                    leftHandWeaponSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponsOnBothHands()
        {
            LoadRightWeapon();
            LoadLeftWeapon();
        } 

        // BELOW CODE: Right weapon
        public void LoadRightWeapon()
        {
            if (player.playerInventoryManager.currentRightHandWeapon != null)
            {
                // BELOW CODE: Remove the old weapon
                rightHandSlot.UnloadWeapon();

                // BELOW CODE: Load the new weapon
                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                rightHandWeaponManager = rightHandWeaponModel.GetComponentInChildren<CharacterWeaponManager>();
                rightHandWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
                player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);
            }
        }

        // LOGIC: Checks if we have another weapon besides our main weapon, if we do, NEVER swap to unarmed, rotate between weapon 1 & 2
        // LOGIC: If we don't, swap to unarmed, then SKIP the other empty slot and swap back. Do not process both empty slots befoer returning to main weapon.
        public void SwitchRightWeapon()
        {
            player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Hand_Weapon_01", false, false, true, true);

            ItemWeapon selectedWeapon = null;

            // Disable two handing if we are two handing 
            // BELOW CODE: Check our weapon index (we have 3 slots, so that's 3 possible numbers)
            // BELOW CODE: Add one to our index to switch to the next potential weapon
            player.playerInventoryManager.rightHandWeaponIndex += 1;

            //  BELOW CODE: If our index is out of bounds, reset it to the first position(0)
            if (player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
            {
                player.playerInventoryManager.rightHandWeaponIndex = 0;

                // BELOW CODE: We check if we are holding more than one weapon
                float weaponCount = 0;
                ItemWeapon firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != CoreItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;

                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.rightHandWeaponIndex = -1;
                    selectedWeapon = CoreItemDatabase.Instance.unarmedWeapon;
                }
                else
                {
                    // BELOW CODE: Jump back to the first weapon
                    player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                }

                return;
            }

            foreach(ItemWeapon weapon in player.playerInventoryManager.weaponsInRightHandSlots)
            {
                // BELOW CODE: Check to see if the next potential weapon does not equal to the "unarmed" weapon
                if (player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != CoreItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                    return;
                }
            }

            if(selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
            {
                SwitchRightWeapon();
            }
        }

        // Left weapon
        public void SwitchLeftWeapon()
        {
            player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Left_Hand_Weapon_01", false, false, true, true);

            ItemWeapon selectedWeapon = null;

            // Disable two handing if we are two handing 
            // BELOW CODE: Check our weapon index (we have 3 slots, so that's 3 possible numbers)
            // BELOW CODE: Add one to our index to switch to the next potential weapon
            player.playerInventoryManager.leftHandWeaponIndex += 1;

            //  BELOW CODE: If our index is out of bounds, reset it to the first position(0)
            if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
            {
                player.playerInventoryManager.leftHandWeaponIndex = 0;

                // BELOW CODE: We check if we are holding more than one weapon
                float weaponCount = 0;
                ItemWeapon firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < player.playerInventoryManager.weaponsInLeftHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInLeftHandSlots[i].itemID != CoreItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;

                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.leftHandWeaponIndex = -1;
                    selectedWeapon = CoreItemDatabase.Instance.unarmedWeapon;
                }
                else
                {
                    // BELOW CODE: Jump back to the first weapon
                    player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                }

                return;
            }

            foreach (ItemWeapon weapon in player.playerInventoryManager.weaponsInLeftHandSlots)
            {
                // BELOW CODE: Check to see if the next potential weapon does not equal to the "unarmed" weapon
                if (player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID != CoreItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex];
                    return;
                }
            }

            if (selectedWeapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2)
            {
                SwitchLeftWeapon();
            }
        }
        public void LoadLeftWeapon() 
        {
            if (player.playerInventoryManager.currentLeftHandWeapon != null)
            {
                // BELOW CODE: Remove the old weapon
                if(leftHandWeaponSlot.currentWeaponModel != null) 
                    leftHandWeaponSlot.UnloadWeapon();

                // BELOW CODE: Load the new weapon
                leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
                
                switch (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType)
                {
                    case WeaponModelType.Weapon:
                        leftHandWeaponSlot.LoadWeapon(leftHandWeaponModel);
                        break;
                    default:
                        break;
                }

                leftHandWeaponManager = leftHandWeaponModel.GetComponentInChildren<CharacterWeaponManager>();
                leftHandWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
            }
        }

        // Damage Hitboxes
        public void OpenDamageHitbox()
        {
            // BELOW CODE: Open right hand weapon damage hitbox
            if(player.playerCombatManager.isUsingRightHand)
            {
                // BELOW CODE: Open a trail
                rightHandWeaponManager.ToggleWeaponTrail(true);
                rightHandWeaponManager.meleeDamageHitbox.EnableDamageHitbox();
                player.playerSoundFXManager.PlaySoundFX(CoreSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentRightHandWeapon.whooshes));
            }
            // BELOW CODE: Open left hand weapon damage hitbox
            else if (player.playerCombatManager.isUsingLeftHand)
            {
                // BELOW CODE: Open a trail
                leftHandWeaponManager.ToggleWeaponTrail(true);
                leftHandWeaponManager.meleeDamageHitbox.EnableDamageHitbox();
                player.playerSoundFXManager.PlaySoundFX(CoreSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentLeftHandWeapon.whooshes));
            }

            // BELOW CODE: Play Weapon Whoosh sound effect
        }
        public void CloseDamageHitbox()
        {
            // BELOW CODE: Open right hand weapon damage hitbox
            if (player.playerCombatManager.isUsingRightHand)
            {
                // BELOW CODE: Close the trail
                rightHandWeaponManager.ToggleWeaponTrail(false);
                rightHandWeaponManager.meleeDamageHitbox.DisableDamageHitbox();
            }
            // BELOW CODE: Open left hand weapon damage hitbox
            else if (player.playerCombatManager.isUsingLeftHand)
            {
                // BELOW CODE: Close the trail
                leftHandWeaponManager.ToggleWeaponTrail(false);
                leftHandWeaponManager.meleeDamageHitbox.DisableDamageHitbox();
            }
        }
    }
}
