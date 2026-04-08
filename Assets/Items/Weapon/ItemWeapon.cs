using UnityEngine;

namespace WEV.WhiteRoom
{
    public class ItemWeapon : Item
    {
        // Animator controller override (Change attack animation based on weapon currently using)

        [Header("ANIMATIONS")]
        [Tooltip("Animator Controller for the particular weapon")]
        public AnimatorOverrideController weaponAnimator;

        [Header("MODEL INSTANTIATION")] 
        [Tooltip("What is the type of this weapon?")]
        public WeaponModelType weaponModelType;
        
        [Header("WEAPON MODEL")]
        [Tooltip("Prefab for the weapon model")]
        public GameObject weaponModel; 

        [Header("WEAPON REQUIREMENTS")]
        [Tooltip("Strength level required for using the weapon")]
        public int strengthREQ = 0;
        [Tooltip("Dexterity level required for using the weapon")]
        public int dexREQ = 0; 
        [Tooltip("Intelligence level required for using the weapon")]
        public int intREQ = 0; 
        [Tooltip("Faith level required for using the weapon")]
        public int faithREQ = 0;

        [Header("WEAPON BASE DAMAGE")]
        [Tooltip("Physical damage of the weapon")]
        public int physicalDamage = 0; 
        [Tooltip("Magic damage of the weapon")]
        public int magicDamage = 0;
        [Tooltip("Fire damage of the weapon")]
        public int fireDamage = 0;
        [Tooltip("Holy damage of the weapon")]
        public int holyDamage = 0; 
        [Tooltip("Lightning damage of the weapon")]
        public int lightningDamage = 0; 

        // Weapon guard absorptions (blocking power)

        [Header("WEAPON POISE")]
        [Tooltip("The value of poise break for the weapon.")]
        public float poiseDamage;
        // Offensive poise bonus when attacking

        [Header("ATTACK MODIFIERS")]
        [Tooltip("Base attack modifier for the first light attack")]
        public float light_Attack_01_Modifier = 1.1f; 
        [Tooltip("Base attack modifier for the second light attack")]
        public float light_Attack_02_Modifier = 1.2f; 
        [Tooltip("Base attack modifier for the third light attack")]
        public float light_Attack_03_Modifier = 1.3f; 
        [Tooltip("Base attack modifier for the first heavy attack")]
        public float heavy_Attack_01_Modifier = 1.4f; 
        [Tooltip("Base attack modifier for the second heavy attack")]
        public float heavy_Attack_02_Modifier = 1.6f; 
        [Tooltip("Base attack modifier for the third heavy attack")]
        public float heavy_Attack_03_Modifier = 1.8f;
        [Tooltip("Base attack modifier for the first running attack")]
        public float running_Attack_01_Modifier = 1.1f; 
        [Tooltip("Base attack modifier for the first rolling attack")]
        public float rolling_Attack_01_Modifier = 1.1f; 
        [Tooltip("Base attack modifier for the first backstep attack")]
        public float backstep_Attack_01_Modifier = 1.1f; 
        [Tooltip("Base attack modifier for the first charge attack")]
        public float charge_Attack_01_Modifier = 2.0f; 
        [Tooltip("Base attack modifier for the second charge attack")]
        public float charge_Attack_02_Modifier = 2.2f; 
        [Tooltip("Base attack modifier for the third charge attack")]
        public float charge_Attack_03_Modifier = 2.4f; 

        [Header("WEAPON STAMINA COSTS MODIFIERS")]
        [Tooltip("Base stamina cost for the weapon")]
        public int baseStaminaCost = 20; 
        [Tooltip("Base stamina cost multiplier for light attack action")]
        public float lightAttackStaminaCostMultiplier = 0.9f; 
        [Tooltip("Base stamina cost multiplier for heavy attack action")]
        public float heavyAttackStaminaCostMultiplier = 1.3f; 
        [Tooltip("Base stamina cost multiplier for running attack action")]
        public float runningAttackStaminaCostMultiplier = 1.1f; 
        [Tooltip("Base stamina cost multiplier for rolling attack action")]
        public float rollingAttackStaminaCostMultiplier = 1.1f; 
        [Tooltip("Base stamina cost multiplier for backstep attack action")]
        public float backstepAttackStaminaCostMultiplier = 1.1f; 
        [Tooltip("Base stamina cost multiplier for charge attack action")]
        public float chargeAttackStaminaCostMultiplier = 2.0f; 

        // Item based actions (RB, RT, LB, LT)
        [Header("ACTIONS")]
        [Tooltip("Weapon action for one hand weapon (right bumper / Left Mouse Click)")]
        public CoreWeaponItemAction oh_RB_Action;
        [Tooltip("Weapon action for one hand weapon (right trigger / Right Mouse Click)")]
        public CoreWeaponItemAction oh_RT_Action;
        [Tooltip("Weapon action for one hand weapon (left bumper / G Key)")]
        public CoreWeaponItemAction oh_LB_Action;

        // Blocking sounds
        [Header("WEAPON SFX")]
        [Tooltip("List of audio clip containing weapon whooshes.")]
        public AudioClip[] whooshes;
    }
}
