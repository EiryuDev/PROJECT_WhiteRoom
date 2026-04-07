using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WEV.WhiteRoom
{
    public class CharacterManager : MonoBehaviour
    {
        [HideInInspector] public Animator animator;
        [HideInInspector] public Rigidbody rigidBody;
        [HideInInspector] public AudioSource audioSource;
        [HideInInspector] public CharacterController controller;
        [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
        [HideInInspector] public CharacterStatsManager characterStatsManager;
        [HideInInspector] public CharacterInventoryManager characterInventoryManager;
        [HideInInspector] public CharacterCombatManager characterCombatManager;

        [Header("FLAGS")]
        public bool isPerformingAction = false;
        public bool isInvulnerable = false;

        [Header("CHARACTER STATUS")]
        public bool isDead = false;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            rigidBody = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
            controller = GetComponent<CharacterController>();
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            characterInventoryManager = GetComponent<CharacterInventoryManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
        }
        
        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
            animator.SetBool("isMoving", characterLocomotionManager.isMoving);
        }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            characterStatsManager.currentHealth = 0;
            isDead = true;

            // BELOW CODE: Reset any flags here that need to reset

            // BELOW CODE: If we are not grounded play an aerial death animation

            if (!manuallySelectDeathAnimation)
            {
                characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
            }

            // BELOW CODE: Play some death sfx

            yield return new WaitForSeconds(5);

            // BELOW CODE: Award players with "something"

            // BELOW CODE: Disable character
        }

        protected virtual void IgnoreMyOwnColliders()
        {
            Collider characterControllerCollider = GetComponent<Collider>();
            Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();
            List<Collider> ignoreColliders = new List<Collider>();

            // BELOW CODE: Adds all the character's damageable colliders, to the list that will be used to ignore collisions
            foreach(var collider in damageableCharacterColliders)
            {
                ignoreColliders.Add(collider);
            }

            // BELOW CODE: Adds the character controller collider to the list that will be used to ignore collisions
            ignoreColliders.Add(characterControllerCollider);

            // BELOW CODE: Gets through every collider on the list, and ignore collisions with one another
            foreach(var collider in ignoreColliders)
            {
                foreach(var otherCollider in ignoreColliders)
                {
                    Physics.IgnoreCollision(collider, otherCollider, true);
                }
            }
        }
    }
}