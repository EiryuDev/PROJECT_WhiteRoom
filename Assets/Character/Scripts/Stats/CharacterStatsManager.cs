using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Health Settings")]
        public int currentHealth = 100;

        [Header("Stamina Settings")]
        public int currentStamina = 100;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
    }
}
