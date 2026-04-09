namespace WEV.WhiteRoom
{
    public class AICharacterSoundFXManager : CharacterSoundFXManager
    {
        private AICharacterManager aiCharacter;
        
        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }
    }
}
