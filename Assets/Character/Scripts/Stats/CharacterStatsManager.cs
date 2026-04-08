using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Health Settings")]
        public int currentHealth = 100;
        public int maxHealth = 100;

        [Header("Stamina Settings")]
        public float currentStamina = 100;
        public int maxStamina = 100;

        [Header("Stats Settings")]
        public int vitality = 10;
        public int endurance = 10;
        public int strength = 10;
        public int intelligence = 10;
        public int willpower = 10;
        public int agility = 10;
        public int speed = 10;

        [Header("Stamina Regeneration Settings")]
        [SerializeField]
        private float staminaRegenerationAmount = 2; 
        private float staminaRegenerationTimer = 0; 
        private float staminaTickTimer = 0; 
        [SerializeField] private float staminaRegenerationDelay = 2;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public int CalculateHealthBasedOnVitalityLevel(int vitality)
        {
            float health = 0;

            // BELOW CODE: Equation for health calculations
            health = vitality * 15;

            return Mathf.RoundToInt(health);
        }

        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            // BELOW CODE: Equation for stamina calculations
            stamina = endurance * 10;

            return Mathf.RoundToInt(stamina);
        }

        public virtual void RegenerateStamina()
        {
            // BELOW CODE: Do not regenerate stamina if we using actions
            if (character.characterLocomotionManager.isSprinting)
                return;

            if (character.isPerformingAction)
                return;

            staminaRegenerationTimer += Time.deltaTime;

            if (staminaRegenerationTimer >= staminaRegenerationDelay)
            {
                if (currentStamina < maxStamina)
                {
                    staminaTickTimer += Time.deltaTime;

                    if (staminaTickTimer >= 0.1)
                    {
                        staminaTickTimer = 0;
                        currentStamina += staminaRegenerationAmount;
                    }
                }
            }
        }

        public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
        {
            // BELOW CODE: Only reset the regeneration if the action used stamina
            // BELOW CODE: We don't want to to reset the regeneration if we already regenerating stamina
            if (currentStaminaAmount < previousStaminaAmount)
            {
                staminaRegenerationTimer = staminaRegenerationDelay;
            }
        }
    }
}
