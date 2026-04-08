using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CoreEnums : MonoBehaviour
    {
    }

    public enum CharacterGroup 
    { 
	Player, 
	Enemy
    }
    
    // Location
    public enum WorldAreaLocation 
    { 
        Gameplay_01
    }

    // Item
    public enum ItemType 
    { 
        // Regular items are used as pickup, consumables
        RegularItem, 
        // Key item are specific quest item which can't be removed
        KeyItem 
    }

    public enum ItemPickUpType 
    { 
        World, 
        Drop 
    }

    public enum InteractableType 
    {
        Item, 
        NPC 
    }

    // Weapon
    public enum WeaponModelSlot
    {
        RightHandWeaponSlot, 
        LeftHandWeaponSlot
    }

    public enum WeaponModelType 
    { 
        Weapon,
        Gun 
    }

    // Attack
    public enum AttackType
    {
        LightAttack01,
        LightAttack02,
        LightAttack03,
        HeavyAttack01,
        HeavyAttack02,
        HeavyAttack03,
        ChargedAttack01,
        ChargedAttack02,
        ChargedAttack03,
        RunningAttack01
    }

    public enum AttackHand 
    { 
        RightHand, 
        LeftHand, 
        BothHand
    }
}
