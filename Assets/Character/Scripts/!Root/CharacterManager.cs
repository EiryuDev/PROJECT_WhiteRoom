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
        [HideInInspector] public CharacterEquipmentManager characterEquipmentManager;
        [HideInInspector] public CharacterCombatManager characterCombatManager;
        [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;

        [Header("CHARACTER NAME")]
        public string characterName = "";

        [Header("CHARACTER GROUP")]
        public CharacterGroup characterGroup;

        [Header("FLAGS")]
        public bool isPerformingAction = false;
        public bool isInvulnerable = false;

        [Header("CHARACTER STATUS")]
        public bool isDead = false;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            animator = GetComponent<Animator>();
            rigidBody = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
            controller = GetComponent<CharacterController>();
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            characterInventoryManager = GetComponent<CharacterInventoryManager>();
            characterEquipmentManager = GetComponent<CharacterEquipmentManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
            characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
        }
        
        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
        }

        protected virtual void Update()
        {
            animator.SetBool("isGrounded", characterLocomotionManager.isGrounded);
            animator.SetBool("isMoving", characterLocomotionManager.isMoving);
        }

        protected virtual void FixedUpdate()
        {

        }

        protected virtual void LateUpdate()
        {

        }

        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {

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