using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace WEV.WhiteRoom
{
    public class CoreHitboxDamage : MonoBehaviour
    {
        [Header("HITBOX")]
        [SerializeField] protected Collider damageHitbox; // Reference to the Collider component for the hitbox

        [Header("DAMAGE")]
        [Tooltip("Damage will be split into Standard, Strike, Slash and Pierce")]
        public float physicalDamage = 0; // Physical damage value for this state
        public float magicDamage = 0; // Magic damage value for this state
        public float fireDamage = 0; // Fire damage value for this state 
        public float waterDamage = 0; // Water damage value for this state
        public float iceDamage = 0; // Ice damage value for this state
        public float lightningDamage = 0; // Lightning damage value for this state
        public float holyDamage = 0; // Holy damage value for this state
        
        [Header("POISE")]
        public float poiseDamage = 0; // Poise damage value for this state

        [Header("CONTACT POINT")]
        protected Vector3 contactPoint; // Contact point value for the hitbox to take damage

        [Header("CHARACTER DAMAGED")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>(); // List of all the characters who are damaged
        
        [Header("BLOCK")]
        protected Vector3 directionFromAttackToDamageTarget; // Reference to the Vector3 for direction from attack to damage target
        protected float dotValueFromAttackToDamageTarget; // Reference to the float for the dot value from attack to damage target

        protected virtual void Awake()
        {
            
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();
            
            // BELOW CODE: If we want to search on both the damageable character colliders & the character controller collider just check for null and do the following
            //if (damageTarget != null) 
            //{
            //    damageTarget = other.GetComponent<CharacterManager>();
            //}

            if(damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // BELOW CODE: Checks if we can damage this target based on friendly fire

                // BELOW CODE: Take damage
                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            // BELOW CODE: Not damaging the same target more than once in a single attack, So add them to list that checks before applying damage
            if (charactersDamaged.Contains(damageTarget))
                return;

            charactersDamaged.Add(damageTarget);

        }
        
        public virtual void EnableDamageHitbox()
        {
            damageHitbox.enabled = true;
        }

        public virtual void DisableDamageHitbox() 
        {
            damageHitbox.enabled = false;
            charactersDamaged.Clear(); // We reset the characters that have been hit when we reset the hitbox, so they can be hit again
        }
    }
}
