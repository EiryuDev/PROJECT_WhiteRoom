using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CharacterUIManager : MonoBehaviour
    {
        [Header("UI DATA")] 
        public bool hasFloatingHPBar = true; // Check if the character have the floating hp bar or not
        public UICharacterHPBar characterHPBar; // Reference to the UI_CharacterHPBar script

        public void OnHPChanged(int oldValue, int newValue)
        {
            characterHPBar.oldHealthValue = oldValue;
            characterHPBar.SetStat(newValue);
        }

        public void ResetCharacterHPBar()
        {
            if (characterHPBar == null)
                return;

            characterHPBar.currentDamageTaken = 0;
        }
    }
}
