namespace WEV.WhiteRoom
{
    public class NPCCharacterLocomotionManager : AICharacterLocomotionManager
    {
        NPCCharacterManager npc;

        protected override void Awake()
        {
            base.Awake();
            
            npc = GetComponent<NPCCharacterManager>();
        }
    }
}