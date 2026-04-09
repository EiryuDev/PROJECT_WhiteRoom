using UnityEngine;

namespace WEV.WhiteRoom
{
    [CreateAssetMenu(menuName = "White Room/A.I/States/Boss Sleep State")]
    public class CoreAIBossSleepState : CoreAIState
    {
        public override CoreAIState Tick(AICharacterManager aiCharacter)
        {
            return base.Tick(aiCharacter);
        }
    }
}
