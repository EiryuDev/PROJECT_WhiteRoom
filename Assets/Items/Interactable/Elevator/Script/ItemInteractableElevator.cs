using UnityEngine;
using System.Collections.Generic;

namespace WEV.WhiteRoom
{
    public class ItemInteractableElevator : ItemInteractable
    {
        [Header("Position Settings")]
        public Vector3 position;
        public bool elevatorIsRising = false;
        public bool elevatorIsDescending = false;
        public float positionSmoothTime = 0.1f;
        public float yMovementOffset = 0.3f;

        [Header("Destination Settings")]
        public float moveSpeed = 2;
        public Vector3 destinationHigh;
        public Vector3 destinationLow;

        [Header("Characters On Elevator Settings")]
        public List<CharacterManager> charactersOnElevator = new List<CharacterManager>();

        [Header("Animation Settings")]
        public Animator animator;
        public string openElevatorDoorAnimation;
        public string closeElevatorDoorAnimation;

        [Header("SFX Settings")]
        public AudioSource audioSource;
        public AudioClip elevatorDoorOpeningSFX;
        public AudioClip elevatorDoorClosingSFX;
        public AudioClip elevatorMovingSFX;
        public AudioClip[] elevatorStoppingSFX;
        
        protected override void Awake()
        {
            base.Awake();

            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            // BELOW CODE: Open the elevator door
            animator.Play(openElevatorDoorAnimation);
            audioSource.PlayOneShot(elevatorDoorOpeningSFX);
        }
    }
}
