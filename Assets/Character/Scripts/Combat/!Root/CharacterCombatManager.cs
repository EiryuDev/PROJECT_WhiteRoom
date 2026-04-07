using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CharacterCombatManager : MonoBehaviour
    {
        protected CharacterManager character; // Reference to the Character Manager script

        [Header("FLAGS")]
        public bool isAttacking = false;
        
        [Header("LAST ATTACK ANIMATION PERFORMED")]
        public string lastAttackAnimationPerformed; // Reference to the last attack animation played/performed

        [Header("ATTACK TARGET")]
        public CharacterManager currentTarget; // Reference to the Character Manager script for the attack target

        [Header("ATTACK TYPE")]
        public AttackType currentAttackType; // Reference to the AttackType enum to get the current weapon attack type

        [Header("LOCK ON TRANSFORM")]
        public Transform lockOnTransform; // Reference to the Transform component to get the lock on transform for the character

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void SetTarget(CharacterManager newTarget)
        {
            if(newTarget != null)
            {
                currentTarget = newTarget;
            }
            else
            {
                currentTarget = null;
            }
        }

        public void EnableIsInvulnerable()
        {
            character.isInvulnerable = true;
        }

        public void DisableIsInvulnerable()
        {
            character.isInvulnerable = false;
        }
        public virtual void EnableCanDoCombo()
        {

        }
        public virtual void DisableCanDoCombo()
        {

        }
    }
}
