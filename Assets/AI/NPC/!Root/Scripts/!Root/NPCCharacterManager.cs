using UnityEngine;

namespace WEV.WhiteRoom
{
    public class NPCCharacterManager : AICharacterManager
    {
        [HideInInspector] public NPCCharacterLocomotionManager npcCharacterLocomotionManager;
        [HideInInspector] public NPCCharacterAnimatorManager npcCharacterAnimatorManager;
        [HideInInspector] public NPCCharacterCombatManager npcCharacterCombatManager;
        [HideInInspector] public NPCCharacterSoundFXManager npcCharacterSoundFXManager;

        protected override void Awake()
        {
            base.Awake();

            npcCharacterAnimatorManager = GetComponent<NPCCharacterAnimatorManager>();
            npcCharacterLocomotionManager = GetComponent<NPCCharacterLocomotionManager>();
            npcCharacterSoundFXManager = GetComponent<NPCCharacterSoundFXManager>();
        }

        protected override void Start()
        {
            base.Awake();
        }
    }
}
