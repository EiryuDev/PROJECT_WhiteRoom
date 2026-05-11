namespace WEV.WhiteRoom
{
    public class NPCCharacterAnimatorManager : AICharacterAnimatorManager
    {
        private NPCCharacterManager npc;

        protected override void Awake()
        {
            base.Awake();

            npc = GetComponent<NPCCharacterManager>();
        }
    }
}