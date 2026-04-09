namespace WEV.WhiteRoom
{
    public class AICharacterStatsManager : CharacterStatsManager
    {
        AICharacterManager aiCharacter;
        
        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}
