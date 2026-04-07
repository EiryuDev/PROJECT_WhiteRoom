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
        [HideInInspector] public PlayerInteractionManager playerInteractionManager;
        [HideInInspector] public PlayerUIManager playerUIManager;

        [Header("AREA")] 
        public WorldLocationSceneSet areaCurrentlyIn;

        protected override void Awake()
        {
            base.Awake();
            playerCameraManager = GetComponentInChildren<PlayerCameraManager>();
            playerInputManager = GetComponent<PlayerInputManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerInteractionManager = GetComponent<PlayerInteractionManager>();
            playerUIManager = FindAnyObjectByType<PlayerUIManager>();
        }
        public void Update()
        {
            playerInputManager.UseAllInputs();
            playerLocomotionManager.UseAllMovement();
            playerInventoryManager.HandleHeldObject();
        }
        private void LateUpdate()
        {
            playerCameraManager.UseAllCameraMovement();
        }
    }
}
