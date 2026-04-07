using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CharacterMeleeWeaponDamageHitbox : WorldDamageHitbox
    {
        [Header("ATTACKING CHARACTER")]
        public CharacterManager characterCausingDamage; // When calculating damage this is used to check for attacker damage modifiers

        [Header("WEAPON ATTACK MODIFIERS")]
        public float light_Attack_01_Modifier; // Base attack modifier for the first light attack
        public float light_Attack_02_Modifier; // Base attack modifier for the second light attack
        public float light_Attack_03_Modifier; // Base attack modifier for the third light attack
        public float heavy_Attack_01_Modifier; // Base attack modifier for the first heavy attack
        public float heavy_Attack_02_Modifier; // Base attack modifier for the second heavy attack
        public float heavy_Attack_03_Modifier; // Base attack modifier for the third heavy attack
        public float running_Attack_01_Modifier; // Base attack modifier for the first running attack
        public float charge_Attack_01_Modifier; // Base attack modifier for the first charge attack
        public float charge_Attack_02_Modifier; // Base attack modifier for the second charge attack
        public float charge_Attack_03_Modifier; // Base attack modifier for the third charge attack

        protected override void Awake()
        {
            base.Awake();  
            
            if(damageHitbox == null)
            {
                damageHitbox = GetComponent<Collider>();
            }

            damageHitbox.enabled = false; // Melee weapon colliders should be disabled at start, only enable when animation allows
        }

        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            // BELOW CODE: If we want to search on both the damageable character colliders & the character controller collider just check for null and do the following
            //if (damageTarget != null) 
            //{
            //    damageTarget = other.GetComponent<CharacterManager>();
            //}

            if (damageTarget != null)
            {
                // BELOW CODE: Player should not damage themselves
                if (damageTarget == characterCausingDamage)
                    return;

                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // BELOW CODE: Checks if we can damage this target based on friendly fire

                // BELOW CODE: Checks if target is blocking
                
                // BELOW CODE: Take damage
                DamageTarget(damageTarget);
            }
        }
        
        protected override void DamageTarget(CharacterManager damageTarget)
        {
            // BELOW CODE: Not damaging the same target more than once in a single attack, So add them to list that checks before applying damage
            if (charactersDamaged.Contains(damageTarget))
                return;

            charactersDamaged.Add(damageTarget);
        }
    }
}
