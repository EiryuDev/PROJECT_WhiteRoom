using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private CharacterManager characterInLift;

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

        [Header("Auto Closer Settings")]
        public ItemInteractableElevatorCloser elevatorCloser;
        
        protected override void Awake()
        {
            base.Awake();

            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        protected override void Start()
        {
            if (elevatorIsRising)
                ActivateElevator(true);

            if (elevatorIsDescending)
                ActivateElevator(false);
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            // BELOW CODE: Open the elevator door
            animator.Play(openElevatorDoorAnimation);
            audioSource.PlayOneShot(elevatorDoorOpeningSFX);
        }

        private void ActivateElevator(bool isRising)
        {
            StartCoroutine(MoveElevatorCoroutine(isRising));
        }

        public void AttemptToActivateElevator()
        {
            if (transform.localPosition == destinationHigh)
            {
                elevatorIsDescending = true;

                ActivateElevator(false);
            }
            else if (transform.localPosition == destinationLow)
            {
                elevatorIsRising = true;

                ActivateElevator(true);
            }
        }

        private IEnumerator MoveElevatorCoroutine(bool isRising)
        {
            // BELOW CODE: Close the elevator door
            animator.Play(closeElevatorDoorAnimation);
            audioSource.PlayOneShot(elevatorDoorClosingSFX);

            interactableCollider.enabled = false;

            // BELOW CODE: When the elevator starts, remove it as an interactable whilst it's going
            for (int i = 0; i < charactersOnElevator.Count; i++)
            {
                if (charactersOnElevator[i] == null)
                    continue;

                PlayerManager player = charactersOnElevator[i] as PlayerManager;

                if (player == null)
                    continue;

                player.playerInteractionManager.RemoveInteractionFromList(this);
            }

            // BELOW CODE: Sfx 
            audioSource.clip = elevatorMovingSFX;
            audioSource.loop = true;
            audioSource.Play();

            // BELOW CODE: Decide the destination based on whether the elevator is rising or descending
            Vector3 destination = destinationHigh;

            if (!isRising)
                destination = destinationLow;

            // BELOW CODE: Move the elevator to the destination
            while (transform.localPosition != destination)
            {
                transform.localPosition =
                    Vector3.MoveTowards(transform.localPosition,
                    destination, moveSpeed * Time.deltaTime);

                Vector3 velocityOfMovement = Vector3.MoveTowards(transform.position,
                    destination, moveSpeed * Time.deltaTime);

                position = transform.localPosition;

                for (int i = 0; i < charactersOnElevator.Count; i++)
                {
                    if (charactersOnElevator[i] == null)
                        continue;

                    if (!charactersOnElevator[i].gameObject.activeInHierarchy)
                        RemoveCharacterFromListOfCharactersOnElevator(charactersOnElevator[i]);

                    characterInLift = charactersOnElevator[i];

                    characterInLift.characterLocomotionManager.canMove = false;
                    characterInLift.characterLocomotionManager.canJump = false;
                    characterInLift.characterLocomotionManager.canRotate = false;
                    characterInLift.characterLocomotionManager.canCrouch = false;
                    characterInLift.characterLocomotionManager.canSlide = false;

                    // BELOW CODE: Move the characters on the elevator with the elevator
                    if (!charactersOnElevator[i].characterLocomotionManager.isJumping)
                        charactersOnElevator[i].transform.position =
                            new Vector3(charactersOnElevator[i].transform.position.x,
                            velocityOfMovement.y + yMovementOffset,
                            charactersOnElevator[i].transform.position.z);
                }

                yield return null;
            }

            // BELOW CODE: Stop the movement flags
            elevatorIsRising = false;
            elevatorIsDescending = false;

            // BELOW CODE: Stop the movement sfx
            audioSource.loop = false;
            audioSource.Stop();

            // BELOW CODE: Play the stopped sfx
            audioSource.PlayOneShot(CoreSoundFXManager.instance.
                ChooseRandomSFXFromArray(elevatorStoppingSFX));

            // BELOW CODE: If you are animating any part of the elevator, stop here
            if (isRising)
            {
                Debug.Log("Elevator reached HIGH destination");
                OpenElevatorDoor(characterInLift);
            }
            else
            {
                Debug.Log("Elevator reached LOW destination");
                OpenElevatorDoor(characterInLift);
            }

            // BELOW CODE: Re-enable the interaction with the elevator
            interactableCollider.enabled = true;

            yield return null;
        }

        public void OpenElevatorDoor(CharacterManager character)
        {
            Debug.Log("TOP floor");

            animator.Play(openElevatorDoorAnimation);
            audioSource.PlayOneShot(elevatorDoorOpeningSFX);

            character.characterLocomotionManager.canMove = true;
            character.characterLocomotionManager.canJump = true;
            character.characterLocomotionManager.canRotate = true;
            character.characterLocomotionManager.canCrouch = true;
            character.characterLocomotionManager.canSlide = true;

            if(elevatorCloser != null)
            {
                elevatorCloser.interactableCollider.enabled = true;
            }
        }

        public void CloseElevatorDoor(CharacterManager character)
        {
            Debug.Log("TOP floor");

            interactableCollider.enabled = false;
            PlayerUIManager.instance.player.playerInteractionManager.RemoveInteractionFromList(this);

            animator.Play(closeElevatorDoorAnimation);
            audioSource.PlayOneShot(elevatorDoorClosingSFX);

            character.characterLocomotionManager.canMove = true;
            character.characterLocomotionManager.canJump = true;
            character.characterLocomotionManager.canRotate = true;
            character.characterLocomotionManager.canCrouch = true;
            character.characterLocomotionManager.canSlide = true;
        }

        public void AddCharacterToListOfCharactersOnElevator(CharacterManager character)
        {
            if (charactersOnElevator.Contains(character))
                return;

            charactersOnElevator.Add(character);
            character.characterLocomotionManager.isRidingLift = true;
        }

        public void RemoveCharacterFromListOfCharactersOnElevator(CharacterManager character)
        {
            if (!charactersOnElevator.Contains(character))
                return;

            charactersOnElevator.Remove(character);
            character.characterLocomotionManager.isRidingLift = false;
        }
    }
}
