using UnityEngine;

namespace WEV.WhiteRoom
{
    public class AICharacterUIManager : CharacterUIManager
    {
        [Header("Dialogue Data Settings")]
        public UI_CharacterDialogue uiDialogue;

        private void Awake()
        {
            uiDialogue = GetComponentInChildren<UI_CharacterDialogue>();
        }
    }
}
