namespace WEV.WhiteRoom
{
    public class AICharacterEquipmentManager : CharacterEquipmentManager
    {
        AICharacterManager aiCharacter;

        protected override void Start()
        {
            base.Start();

            aiCharacter = GetComponent<AICharacterManager>();
        }
    }
}
