using UnityEngine;

namespace WEV.WhiteRoom
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerCameraManager playerCameraManager;
        [HideInInspector] public PlayerInputManager playerInputManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;

        protected override void Awake()
        {
            base.Awake();
            playerCameraManager = GetComponentInChildren<PlayerCameraManager>();
            playerInputManager = GetComponent<PlayerInputManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
        }
        public void Update()
        {
            playerInputManager.UseAllInputs();
            playerLocomotionManager.UseAllMovement();
        }
        private void LateUpdate()
        {
            playerCameraManager.UseAllCameraMovement();
        }
    }
}
