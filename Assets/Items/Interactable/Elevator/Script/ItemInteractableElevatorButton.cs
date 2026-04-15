using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace WEV.WhiteRoom
{
    public class ItemInteractableElevatorButton : ItemInteractable
    {
        ItemInteractableElevator elevator;
        private CharacterManager characterInLift;

        protected override void Awake()
        {
            base.Awake();
            elevator = GetComponentInParent<ItemInteractableElevator>();
        }

        protected override void Start()
        {
            base.Start();

            if (elevator.elevatorIsRising)
                ActivateElevator(true);

            if (elevator.elevatorIsDescending)
                ActivateElevator(false);
        }

        public override void OnTriggerEnter(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if(character != null)
                AddCharacterToListOfCharactersOnElevator(character);

            if (elevator.elevatorIsRising || elevator.elevatorIsDescending)
                return;

            base.OnTriggerEnter(other);

        }

        public override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);

            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character != null)
                RemoveCharacterFromListOfCharactersOnElevator(character);
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            AttemptToActivateElevator();
        }

        private IEnumerator MoveElevatorCoroutine(bool isRising)
        {
            // BELOW CODE: Close the elevator door
            elevator.animator.Play(elevator.closeElevatorDoorAnimation);
            elevator.audioSource.PlayOneShot(elevator.elevatorDoorClosingSFX);

            interactableCollider.enabled = false;

            // BELOW CODE: When the elevator starts, remove it as an interactable whilst it's going
            for(int i = 0; i < elevator.charactersOnElevator.Count; i++)
            {
                if (elevator.charactersOnElevator[i] == null)
                    continue;

                PlayerManager player = elevator.charactersOnElevator[i] as PlayerManager;

                if(player == null)
                    continue;

                player.playerInteractionManager.RemoveInteractionFromList(this);
            }

            // BELOW CODE: Sfx 
            elevator.audioSource.clip = elevator.elevatorMovingSFX;
            elevator.audioSource.Play();

            // BELOW CODE: Decide the destination based on whether the elevator is rising or descending
            Vector3 destination = elevator.destinationHigh;

            if(!isRising)
                destination = elevator.destinationLow;

            // BELOW CODE: Move the elevator to the destination
            while (elevator.transform.localPosition != destination)
            {
                elevator.transform.localPosition =
                    Vector3.MoveTowards(elevator.transform.localPosition,
                    destination, elevator.moveSpeed * Time.deltaTime);

                Vector3 velocityOfMovement = Vector3.MoveTowards(elevator.transform.position,
                    destination, elevator.moveSpeed * Time.deltaTime);

                elevator.position = elevator.transform.localPosition;

                for(int i = 0; i < elevator.charactersOnElevator.Count; i++)
                {
                    if (elevator.charactersOnElevator[i] == null)
                        continue;

                    if (!elevator.charactersOnElevator[i].gameObject.activeInHierarchy)
                        RemoveCharacterFromListOfCharactersOnElevator(elevator.charactersOnElevator[i]);

                    characterInLift = elevator.charactersOnElevator[i];

                    characterInLift.characterLocomotionManager.canMove = false;
                    characterInLift.characterLocomotionManager.canJump = false;
                    characterInLift.characterLocomotionManager.canRotate = false;
                    characterInLift.characterLocomotionManager.canCrouch = false;
                    characterInLift.characterLocomotionManager.canSlide = false;

                    // BELOW CODE: Move the characters on the elevator with the elevator
                    if (!elevator.charactersOnElevator[i].characterLocomotionManager.isJumping)
                        elevator.charactersOnElevator[i].transform.position =
                            new Vector3(elevator.charactersOnElevator[i].transform.position.x,
                            velocityOfMovement.y + elevator.yMovementOffset,
                            elevator.charactersOnElevator[i].transform.position.z);
                }

                yield return null;
            }

            // BELOW CODE: Stop the movement flags
            elevator.elevatorIsRising = false;
            elevator.elevatorIsDescending = false;

            // BELOW CODE: Stop the movement sfx
            elevator.audioSource.Stop();

            // BELOW CODE: Play the stopped sfx
            elevator.audioSource.PlayOneShot(CoreSoundFXManager.instance.
                ChooseRandomSFXFromArray(elevator.elevatorStoppingSFX));

            // BELOW CODE: If you are animating any part of the elevator, stop here
            if (isRising)
            {
                Debug.Log("Elevator reached HIGH destination");
                OnReachedHigh(characterInLift);
            }
            else
            {
                Debug.Log("Elevator reached LOW destination");
                OnReachedLow(characterInLift);
            }

            // BELOW CODE: Re-enable the interaction with the elevator
            interactableCollider.enabled = true;

            yield return null;  
        }

        private void ActivateElevator(bool isRising)
        {
            StartCoroutine(MoveElevatorCoroutine(isRising));
        }

        private void AttemptToActivateElevator()
        {
            if(elevator.transform.localPosition == elevator.destinationHigh)
            {
                elevator.elevatorIsDescending = true;

                ActivateElevator(false);
            }
            else if(elevator.transform.localPosition == elevator.destinationLow)
            {
                elevator.elevatorIsRising = true;

                ActivateElevator(true);
            }
        }

        private void AddCharacterToListOfCharactersOnElevator(CharacterManager character)
        {
            if (elevator.charactersOnElevator.Contains(character))
                return;

            elevator.charactersOnElevator.Add(character);
            character.characterLocomotionManager.isRidingLift = true;
        }

        private void RemoveCharacterFromListOfCharactersOnElevator(CharacterManager character)
        {
            if (!elevator.charactersOnElevator.Contains(character))
                return;

            elevator.charactersOnElevator.Remove(character);
            character.characterLocomotionManager.isRidingLift = false;
        }

        private void OnReachedHigh(CharacterManager character)
        {
            Debug.Log("TOP floor");

            elevator.animator.Play(elevator.openElevatorDoorAnimation);
            elevator.audioSource.PlayOneShot(elevator.elevatorDoorOpeningSFX);

            character.characterLocomotionManager.canMove = true;
            character.characterLocomotionManager.canJump = true;
            character.characterLocomotionManager.canRotate = true;
            character.characterLocomotionManager.canCrouch = true;
            character.characterLocomotionManager.canSlide = true;
        }

        private void OnReachedLow(CharacterManager character)
        {
            Debug.Log("BOTTOM floor");

            elevator.animator.Play(elevator.openElevatorDoorAnimation);
            elevator.audioSource.PlayOneShot(elevator.elevatorDoorOpeningSFX);

            character.characterLocomotionManager.canMove = true;
            character.characterLocomotionManager.canJump = true;
            character.characterLocomotionManager.canRotate = true;
            character.characterLocomotionManager.canCrouch = true;
            character.characterLocomotionManager.canSlide = true;
        }
    }
}
