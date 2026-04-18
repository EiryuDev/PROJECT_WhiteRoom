using UnityEngine;

namespace WEV.WhiteRoom
{
    public class ItemInteractableElevatorButton : ItemInteractable
    {
        ItemInteractableElevator elevator;

        protected override void Awake()
        {
            base.Awake();
            elevator = GetComponentInParent<ItemInteractableElevator>();
        }

        public override void OnTriggerEnter(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if(character != null)
                elevator.AddCharacterToListOfCharactersOnElevator(character);

            if (elevator.elevatorIsRising || elevator.elevatorIsDescending)
                return;

            base.OnTriggerEnter(other);

        }

        public override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);

            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character != null)
                elevator.RemoveCharacterFromListOfCharactersOnElevator(character);
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            elevator.AttemptToActivateElevator();
        }
    }
}
