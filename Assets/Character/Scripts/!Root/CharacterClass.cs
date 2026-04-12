using UnityEngine;

namespace WEV.WhiteRoom
{
    [CreateAssetMenu(menuName = "White Room/Character Classes/Class")]
    public class CharacterClass : ScriptableObject
    {
        [Header("CLASS INFORMATION")] 
        [Tooltip("What is the name of the class?")]
        public string className;
        [Tooltip("What is the lore of the class?")]
        [TextArea]public string classLore;

        [Header("CLASS STATS")] 
        [Tooltip("Affects the amount of damage that you can take before you die.")] 
        public int vitality = 10;
        [Tooltip("Affects all actions performed by a character (melee, spells, lockpicking, speech, etc).")]
        public int endurance = 10;
        [Tooltip("Affects physical damage dealt with weapons, carrying capacity and starting Health. Also determines maximum Endurance.")] 
        public int strength = 10;
        [Tooltip("Affects maximum Magicka. Also influences the success rate and potency when making potions and the success rate when enchanting items.")]
        public int intelligence = 10;
        [Tooltip("Affects spellcasting success rate and resistance to Paralyze and Silence. Also determines maximum Endurance.")]
        public int willpower = 10;
        [Tooltip("Affects hit chance and dodge chance for physical attacks and resistance to knock down. Influences the success rate of sneaking, lockpicking and blocking. Also determines maximum Endurance.")]
        public int agility = 10;
        [Tooltip("Affects movement speed (i.e. walking, running, swimming, and levitating).")]
        public int speed = 10;

        [Header("CLASS WEAPONS")] 
        [Tooltip("What are the main hand weapons given to this class?")]
        public ItemWeapon[] mainHandWeapons = new ItemWeapon[3];
        [Tooltip("What are the off hand weapons given to this class?")]
        public ItemWeapon[] offHandWeapons = new ItemWeapon[3];

        public void SetClass(PlayerManager player)
        {
            UI_TitleScreenManager.Instance.SetCharacterClass(player, vitality, endurance,
                strength, intelligence, willpower, agility, speed, mainHandWeapons, offHandWeapons);
        }
    }
}
