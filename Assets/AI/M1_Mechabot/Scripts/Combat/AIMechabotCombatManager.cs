using UnityEngine;

namespace WEV.WhiteRoom
{
    public class AIMechabotCombatManager : AICharacterCombatManager
    {
        [Header("ATTACK HAND")]
        public AttackHand attackHand; // Choose which hand enemy can attack

        [Header("DAMAGE HITBOX")]
        [SerializeField] AICharacterDamageHitbox rightHandDamageHitbox; // Reference to the Undead Sword Damage Hitbox Script for right hand
        [SerializeField] AICharacterDamageHitbox leftHandDamageHitbox; // Reference to the Undead Sword Damage Hitbox Script for left hand

        [Header("DAMAGE")]
        [SerializeField] int baseDamage = 25; // Value for the base damage of the AI
        [SerializeField] private int basePoiseDamage = 25; // Value for the base poise damage of the AI
        [SerializeField] float attack01DamageModifier = 1.0f; // Value for the first attack damage modifier of the AI 
        [SerializeField] float attack02DamageModifier = 1.4f; // Value for the second attack damage modifier of the AI 
        [SerializeField] float attack03DamageModifier = 1.8f; // Value for the third attack damage modifier of the AI
        
        public void SetAttack01Damage()
        {
            switch(attackHand)
            {
                case AttackHand.RightHand:
                    rightHandDamageHitbox.physicalDamage = baseDamage * attack01DamageModifier;
                    rightHandDamageHitbox.poiseDamage = basePoiseDamage * attack01DamageModifier;
                    break;
                case AttackHand.LeftHand:
                    leftHandDamageHitbox.physicalDamage = baseDamage * attack01DamageModifier;
                    leftHandDamageHitbox.poiseDamage = basePoiseDamage * attack01DamageModifier;
                    break;
                case AttackHand.BothHand:
                    rightHandDamageHitbox.physicalDamage = baseDamage * attack01DamageModifier;
                    leftHandDamageHitbox.physicalDamage = baseDamage * attack01DamageModifier;
                    rightHandDamageHitbox.poiseDamage = basePoiseDamage * attack01DamageModifier;
                    leftHandDamageHitbox.poiseDamage = basePoiseDamage * attack01DamageModifier;
                    break;
                default:
                    break;
            }
        }
        
        public void SetAttack02Damage()
        {
            switch (attackHand)
            {
                case AttackHand.RightHand:
                    rightHandDamageHitbox.physicalDamage = baseDamage * attack02DamageModifier;
                    rightHandDamageHitbox.poiseDamage = basePoiseDamage * attack02DamageModifier;
                    break;
                case AttackHand.LeftHand:
                    leftHandDamageHitbox.physicalDamage = baseDamage * attack02DamageModifier;
                    leftHandDamageHitbox.poiseDamage = basePoiseDamage * attack02DamageModifier;
                    break;
                case AttackHand.BothHand:
                    rightHandDamageHitbox.physicalDamage = baseDamage * attack02DamageModifier;
                    leftHandDamageHitbox.physicalDamage = baseDamage * attack02DamageModifier;
                    rightHandDamageHitbox.poiseDamage = basePoiseDamage * attack02DamageModifier;
                    leftHandDamageHitbox.poiseDamage = basePoiseDamage * attack02DamageModifier;
                    break;
                default:
                    break;
            }
        }
        
        public void SetAttack03Damage()
        {
            switch (attackHand)
            {
                case AttackHand.RightHand:
                    rightHandDamageHitbox.physicalDamage = baseDamage * attack03DamageModifier;
                    rightHandDamageHitbox.poiseDamage = basePoiseDamage * attack03DamageModifier;
                    break;
                case AttackHand.LeftHand:
                    leftHandDamageHitbox.physicalDamage = baseDamage * attack03DamageModifier;
                    leftHandDamageHitbox.poiseDamage = basePoiseDamage * attack03DamageModifier;
                    break;
                case AttackHand.BothHand:
                    rightHandDamageHitbox.physicalDamage = baseDamage * attack03DamageModifier;
                    leftHandDamageHitbox.physicalDamage = baseDamage * attack03DamageModifier;
                    rightHandDamageHitbox.poiseDamage = basePoiseDamage * attack03DamageModifier;
                    leftHandDamageHitbox.poiseDamage = basePoiseDamage * attack03DamageModifier;
                    break;
                default:
                    break;
            }
        }
        
        public void EnableRightHandDamageHitbox()
        {
            rightHandDamageHitbox.EnableDamageHitbox();
            aiCharacter.aiCharacterSoundFXManager.PlayAttackGruntSFX();
        }
        
        public void DisableRightHandDamageHitbox()
        {
            rightHandDamageHitbox.DisableDamageHitbox();
        }
        
        public void EnableLeftHandDamageHitbox()
        {
            leftHandDamageHitbox.EnableDamageHitbox();
            aiCharacter.aiCharacterSoundFXManager.PlayAttackGruntSFX();
        }
        
        public void DisableLeftHandDamageHitbox()
        {
            leftHandDamageHitbox.DisableDamageHitbox();
        }
    }
}
