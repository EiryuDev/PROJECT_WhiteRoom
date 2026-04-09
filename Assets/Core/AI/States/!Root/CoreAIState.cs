using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CoreAIState : ScriptableObject
    {
        public virtual CoreAIState Tick(AICharacterManager aiCharacter)
        {
            return this;
        }
        public virtual CoreAIState SwitchState(AICharacterManager aiCharacter, CoreAIState newState)
        {
            ResetStateFlags(aiCharacter);
            return newState;
        }
        protected virtual void ResetStateFlags(AICharacterManager aiCharacter)
        {
            // BELOW CODE: Reset any state flags here, so when AI return to the state, they are blank once again
        }
    }
}
