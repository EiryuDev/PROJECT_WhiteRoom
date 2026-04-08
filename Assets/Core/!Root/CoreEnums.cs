using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CoreEnums : MonoBehaviour
    {
    }

    // Character
    public enum CharacterSlot
    {
        CharacterSlot_01, CharacterSlot_02, CharacterSlot_03,
        CharacterSlot_04, CharacterSlot_05, CharacterSlot_06,
        CharacterSlot_07, CharacterSlot_08, CharacterSlot_09,
        CharacterSlot_10, NO_SLOT
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
